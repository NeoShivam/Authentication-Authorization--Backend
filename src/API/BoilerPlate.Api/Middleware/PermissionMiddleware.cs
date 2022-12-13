using Newtonsoft.Json;

namespace BoilerPlate.Api.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string controller = context.Request.RouteValues["controller"].ToString().ToLower();

            var claim = context.User.Claims.Where(x => x.Type == "permission").FirstOrDefault();
            if (claim != null && claim.Value != "")
            {
                List<Permission> permissions = JsonConvert.DeserializeObject<List<Permission>>(claim.Value);
                var resource = permissions.Where(a => a.resource == controller).FirstOrDefault();
                if (resource != null)
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
                    {
                        await _next(context);
                    }
                    else
                    {
                        await Unauthorized(context);
                    }

                }
            }
            else
            {
                await Unauthorized(context);
            }
            async Task Unauthorized(HttpContext context)
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject("403 Not authorized");
                await context.Response.WriteAsync(result);
            }

        }
    }
}
