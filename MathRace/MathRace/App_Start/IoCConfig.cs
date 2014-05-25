namespace MathRace.App_Start
{
    using MathRace.Model;

    using Ninject;

    public static class IoCConfig
    {
        public static IKernel Container { get; private set; }

        public static void Configure()
        {
            Container = new StandardKernel();

            RegisterTypes();
        }

        private static void RegisterTypes()
        {
            Container.Bind<RaceManager>().ToSelf().InSingletonScope();
        }
    }
}