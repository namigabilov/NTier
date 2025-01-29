namespace WebApi.Services.Swagger
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SwaggerMultiGroupAttribute : Attribute
    {
        public string[] GroupNames { get; }

        public SwaggerMultiGroupAttribute(params string[] groupNames)
        {
            GroupNames = groupNames;
        }
    }
}