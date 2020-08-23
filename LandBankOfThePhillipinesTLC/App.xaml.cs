using System;
using Acr.UserDialogs;
using LandBankOfThePhillipinesTLC.Views;
using Plugin.Settings;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LandBankOfThePhillipinesTLC
{
    public partial class App : PrismApplication
    {
        public static string FilePath;
        public App():this(null){}

        public App(IPlatformInitializer platformInitializer) : base(platformInitializer){}

        public T Resolve<T>() => Container.Resolve<T>();
        public INavigationService NavigationSvc => NavigationService;
        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationSvc.NavigateAsync($"app:///{nameof(LoginPage)}");
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(UserDialogs.Instance);
            containerRegistry.RegisterInstance(CrossSettings.Current);

            containerRegistry.RegisterForNavigation<LoginPage>();
            containerRegistry.RegisterForNavigation<HomePage>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage>();
        }

        
    }
}
