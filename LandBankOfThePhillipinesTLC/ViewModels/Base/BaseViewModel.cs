using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace LandBankOfThePhillipinesTLC.ViewModels.Base
{
    public class BaseViewModel : BindableBase, INavigationAware, IDestructible
    {
        private INavigationService navigationService;

        public virtual void Destroy()
        {
            
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }
        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
            
        }
    }
}
