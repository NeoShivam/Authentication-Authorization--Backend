namespace BoilerPlate.Api.Middleware
{
    public class Permission
    {
        public string resource { get; set; }
        public List<string> scope { get; set; }
    }
}
