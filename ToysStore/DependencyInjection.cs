using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using ToysStore.Managers;
using Serilog;
using System.Web;
using AutoMapper;


namespace ToysStore
{
    public class DependencyInjection : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<AuthenticationManager>()
                  .As<IAuthenticationManager>()
                  .SingleInstance();

            builder.RegisterType<SessionStateManager>()
                 .As<ISessionStateManager>()
                 .SingleInstance();

            builder.RegisterType<ToysManager>()
                  .As<IToysManager>()
                  .SingleInstance();

            builder.RegisterType<BacketManager>()
                  .As<IBacketManager>()
                  .SingleInstance();

            builder.RegisterType<DataBaseManager>()
                  .As<IDataBaseManager>()
                  .SingleInstance();

            builder.Register<ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                            .Enrich.WithHttpRequestType()
                            .Enrich.WithUserName()
                            .Enrich.FromLogContext()
                            .MinimumLevel.Debug()
                            .WriteTo.File(HttpRuntime.AppDomainAppPath + "/logs/log.txt",
                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                            .CreateLogger();
            }).SingleInstance();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).InstancePerLifetimeScope();


        }

    }
}