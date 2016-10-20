[assembly: Microsoft.Owin.OwinStartup(typeof(AS.CMS.Startup))]

namespace AS.CMS
{
    using System.Configuration;

    using CKSource.CKFinder.Connector.Config;
    using CKSource.CKFinder.Connector.Core.Builders;
    using CKSource.CKFinder.Connector.Host.Owin;
    using CKSource.FileSystem.Local;

    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;

    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            RegisterFileSystems();

            builder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                AuthenticationMode = AuthenticationMode.Active
            });

            var route = ConfigurationManager.AppSettings["ckfinderRoute"];
            builder.Map(route, SetupConnector);
        }

        private static void RegisterFileSystems()
        {
            FileSystemFactory.RegisterFileSystem<LocalStorage>();
        }

        private static void SetupConnector(IAppBuilder builder)
        {

            var allowedRoleMatcherTemplate = ConfigurationManager.AppSettings["ckfinderAllowedRole"];
            var authenticator = new RoleBasedAuthenticator(allowedRoleMatcherTemplate);
            
            var connectorFactory = new OwinConnectorFactory();
            var connectorBuilder = new ConnectorBuilder();
            var connector = connectorBuilder
                .LoadConfig()
                .SetAuthenticator(authenticator)
                .SetRequestConfiguration(
                    (request, config) =>
                    {
                        config.LoadConfig();
                    })
                .Build(connectorFactory);

            builder.UseConnector(connector);
        }
    }
}
