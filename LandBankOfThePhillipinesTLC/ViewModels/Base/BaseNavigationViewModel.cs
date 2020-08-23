using System;
using Prism.Navigation;
using System.Text;
using System.Collections.Generic;

namespace LandBankOfThePhillipinesTLC.ViewModels.Base
{
    public class BaseNavigationViewModel : BaseViewModel
    {
        protected INavigationService NavigationService;
        public BaseNavigationViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
