namespace BoilerPlate.Auth.Models
{
    public class Permission
    {
        public string resource { get; set; }
        public List<string> scopes { get; set; }
    }
    public class JwtPermissions
    {
        public List<Permission> Permissions { get; set; }
    }
}
