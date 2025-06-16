using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;
using System.Threading;

namespace API
{
    public class DynamicProxyConfigProvider : IProxyConfigProvider
    {
        private volatile ProxyConfig _config;

        public DynamicProxyConfigProvider(IConfiguration configuration)
        {
            var settings = configuration.GetReverseProxySettings();

            var (routes, clusters) = BuildRoutesAndClusters(settings);
            _config = new ProxyConfig(routes, clusters);
        }

        public IProxyConfig GetConfig() => _config;

        public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            var oldConfig = _config;
            _config = new ProxyConfig(routes, clusters);
            oldConfig.SignalChange();
            Console.WriteLine("Proxy config updated.");
        }

        private static (List<RouteConfig>, List<ClusterConfig>) BuildRoutesAndClusters(ReverseProxySettings settings)
        {
            var routes = new List<RouteConfig>();
            var clusters = new List<ClusterConfig>();

            foreach (var service in settings.Services)
            {
                string serviceName = service.Key;
                string serviceAddress = service.Value;

                string clusterId = $"{serviceName}-cluster";

                var cluster = new ClusterConfig()
                {
                    ClusterId = clusterId,
                    Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "destination1", new DestinationConfig { Address = serviceAddress } }
                    }
                };
                clusters.Add(cluster);

                foreach (var routeTemplatePair in settings.RoutesTemplate)
                {
                    string templateName = routeTemplatePair.Key;
                    var template = routeTemplatePair.Value;

                    var route = new RouteConfig()
                    {
                        RouteId = $"{serviceName}-{templateName}-route",
                        ClusterId = clusterId,
                        AuthorizationPolicy = template.AuthorizationPolicy,
                        Match = new RouteMatch
                        {
                            Path = template.MatchPathTemplate.Replace("{service}", serviceName)
                        },
                        Transforms = new List<IReadOnlyDictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                ["PathPattern"] = template.TransformPathTemplate.Replace("{service}", serviceName)
                            }
                        }
                    };
                    routes.Add(route);
                }
            }

            return (routes, clusters);
        }

        private class ProxyConfig : IProxyConfig
        {
            private readonly CancellationTokenSource _cts = new();

            public ProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
            {
                Routes = routes;
                Clusters = clusters;
                ChangeToken = new CancellationChangeToken(_cts.Token);
            }

            public IReadOnlyList<RouteConfig> Routes { get; }
            public IReadOnlyList<ClusterConfig> Clusters { get; }
            public IChangeToken ChangeToken { get; }

            public void SignalChange()
            {
                if (!_cts.IsCancellationRequested)
                {
                    _cts.Cancel();
                }
            }
        }
    }

}
