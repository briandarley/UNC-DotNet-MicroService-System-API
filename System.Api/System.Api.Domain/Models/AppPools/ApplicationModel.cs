namespace System.Api.Domain.Models.AppPools
{
    public class ApplicationModel
    {
        public string Path { get; set; }
        public AppPoolStateModel AppPool { get; set; }
        public string SwaggerPath { get; set; }
    }
}
