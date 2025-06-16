public static class ReverseProxyConfigExtensions
{
    public static ReverseProxySettings GetReverseProxySettings(this IConfiguration config)
    {
        return config.GetSection("ReverseProxy").Get<ReverseProxySettings>();
    }
}
