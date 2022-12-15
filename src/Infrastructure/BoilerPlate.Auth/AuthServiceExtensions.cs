using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using BoilerPlate.Application.Models.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BoilerPlate.Auth.Models;
using BoilerPlate.Application.Responses;

namespace BoilerPlate.Auth
{

    public static class AuthServiceExtensions
    {
        public static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<JwtPermissions>(configuration.GetSection("JwtClaim"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.Authority = configuration["JwtSettings:Issuer"];
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"]

                        //For testing (if Issuer is not available)
                        //SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                        //{
                        //    var jwt = new JwtSecurityToken(token);

                        //    return jwt;
                        //}
                    };

                    o.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = async c =>
                        {
                            var newClaims = c.Principal.Claims.ToList();
                            var Permissions = configuration.GetSection("JwtClaim:Permission").Get<List<Permission>>();
                            newClaims.Add(new Claim("permission", JsonConvert.SerializeObject(Permissions)));
                            c.Principal.AddIdentity(new ClaimsIdentity(newClaims));
                        },
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 498;
                            c.Response.ContentType = "text/plain";
                            if (c.Exception.Message.StartsWith("IDX10223"))
                            {
                                var response = new Response<string>();
                                response.Succeeded = false;
                                response.Errors = new List<string>();
                                response.Errors.Add("Token not valid");
                                response.Message = "Token not valid";
                                return c.Response.WriteAsync(JsonConvert.SerializeObject(response));
                            }
                                
                            return c.Response.WriteAsync(c.Exception.Message);
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("401 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("403 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }

    }
}
