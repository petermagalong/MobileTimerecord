using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;

namespace LandBankOfThePhillipinesTLC.Views
{
    public partial class ChangePasswordPage : ContentPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await DisplayAlert("", "Would you like to exit from application?", "Yes", "No");
                if (result)
                {
                    Thread.CurrentThread.Abort();
                }
            });
            return true;
        }
    }
}
