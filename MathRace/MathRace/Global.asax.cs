namespace MathRace
{
    using System;
    using System.Web.Routing;

    using Microsoft.AspNet.SignalR;

    using Ninject;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Creamos el contenedor y mapeamos el tipo RaceManager como un  Singleton
            IKernel container = new StandardKernel();
            container.Bind<RaceManager>().ToSelf().InSingletonScope();

            // Redefinimos el DependencyResolver de signalR para que use el contenedor
            // El juego se iniciará con la primera petición
            GlobalHost.DependencyResolver = new NinjectDependencyResolver(container);

            // Volvemos a registrar las rutas de los hubs
            RouteTable.Routes.MapHubs();
        }
    }
}