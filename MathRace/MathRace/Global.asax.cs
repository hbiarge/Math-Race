namespace MathRace
{
    using System;

    using MathRace.App_Start;
    using MathRace.Infrastructure.Resolvers;

    using Microsoft.AspNet.SignalR;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            IoCConfig.Configure();

            GlobalHost.DependencyResolver = new SignalRNinjectDependencyResolver(IoCConfig.Container);
        }
    }
}