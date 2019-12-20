using Serilog;
using System;
using System.Web.Http;
using System.Web.SessionState;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using ToysStore.ActionFilters;

namespace ToysStore
{

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            GlobalConfiguration.Configure(WebApiConfig.Register);
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DependencyInjection());
            var container = builder.Build();
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
            var logger = container.Resolve<ILogger>();
            RegisterWebApiFilters(GlobalConfiguration.Configuration.Filters, logger);


            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile<AutoMapperProfile>());

            logger.Debug("App started");
        }

        public static void RegisterWebApiFilters(System.Web.Http.Filters.HttpFilterCollection filters, ILogger logger)
        {
            filters.Add(new ActionWebApiCacheFilter(logger));
            filters.Add(new ActionWebApiLoggingFilter(logger));

        }


        public override void Init()
        {
            this.PostAuthenticateRequest += _PostAuthenticateRequest;
            base.Init();
        }

        private void _PostAuthenticateRequest(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }
    }
}
