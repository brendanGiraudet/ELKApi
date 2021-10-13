namespace ELKApi.Config
{
    public class ElasticConfiguration
    {
        public string Uri { get; set; }
        public string GetLogUrl(string application) => string.Format(Uri.ToLower(), application.ToLower());

    }
}
