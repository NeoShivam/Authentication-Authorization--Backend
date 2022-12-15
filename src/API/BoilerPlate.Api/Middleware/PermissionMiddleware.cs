using BoilerPlate.Auth.Models;
using Newtonsoft.Json;

namespace BoilerPlate.Api.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionMiddleware> _logger;

        public PermissionMiddleware(RequestDelegate next, ILogger<PermissionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
                if (!context.Response.HasStarted)
                {
                    string controller = context.Request.RouteValues["controller"].ToString().ToLower();

                    var claim = context.User.Claims.Where(x => x.Type == "permission").FirstOrDefault();
                    if (claim != null && claim.Value != "")
                    {
                        List<Permission> permissions = JsonConvert.DeserializeObject<List<Permission>>(claim.Value);
                        var resource = permissions.Where(a => a.resource == controller).FirstOrDefault();
                    if (resource != null && resource.scopes != null)
                    {
                        List<string> scopes = resource.scopes.ToList();
                        string requiredPermission = "";
                        switch (context.Request.Method)
                        {
                            case "GET":
                                requiredPermission = "view";
                                break;
                            case "POST":
                                requiredPermission = "create";
                                break;
                            case "PUT":
                                requiredPermission = "edit";
                                break;
                            case "DELETE":
                                requiredPermission = "delete";
                                break;
                            default:
                                break;
                        }
                        if (scopes.Contains(requiredPermission))
                            await _next(context);
                        else
                            await BlockRequest(context, 403);
                    }
                    else
                        await BlockRequest(context, 403);
                    }
                    else
                        await BlockRequest(context, 401);
                }
                else
                    _logger.LogError(context.Response.StatusCode+" :: Unvalidated Token");

                async Task BlockRequest(HttpContext context, int statusCode)
                {
                    _logger.LogError(statusCode + "Request Blocked");
                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(statusCode + " Not Allowed");
                }
            }
        
        }

    }

