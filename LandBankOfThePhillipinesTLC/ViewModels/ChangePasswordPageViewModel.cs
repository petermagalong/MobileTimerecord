using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using LandBankOfThePhillipinesTLC.Constants;
using LandBankOfThePhillipinesTLC.Models;
using LandBankOfThePhillipinesTLC.ViewModels.Base;
using LandBankOfThePhillipinesTLC.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace LandBankOfThePhillipinesTLC.ViewModels
{
    public class ChangePasswordPageViewModel : BaseNavigationViewModel
    {
        private readonly IUserDialogs _userDialogs;
        public ChangePasswordPageViewModel(INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService)
        {
            _userDialogs = userDialogs;

            ChangePassCommand = new DelegateCommand(changepass);
        }

        #region bindable command
        public ICommand ChangePassCommand { get; set; }
        #endregion

        #region Bindable Properties
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        private string _oldPass;
        public string OldPass
        {
            get => _oldPass;
            set => SetProperty(ref _oldPass, value);
        }
        private string _newPass;
        public string NewPass
        {
            get => _newPass;
            set => SetProperty(ref _newPass, value);
        }
        private string _confirmPass;
        public string ConfirmPass
        {
            get => _confirmPass;
            set => SetProperty(ref _confirmPass, value);
        }
        #endregion

        #region method
        private async void changepass()
        {
            if(NewPass != ConfirmPass)
            {
               await _userDialogs.AlertAsync("Password not match");
                return;
            }
            var loading = _userDialogs.Loading("Please wait...");
            try
            {
                ChangepassDto changepassDto = new ChangepassDto();
                changepassDto.username = UserName;
                changepassDto.oldpassword = OldPass;
                changepassDto.password = NewPass;

                string result = await UserTransaction(changepassDto);
                TimelogResult response = ParseResponce(result);

                if (response.Message.Contains("password does not match."))
                {
                    loading.Hide();
                    await _userDialogs.AlertAsync("old password not match");
                    return;
                }

                if(!response.Message.Contains("successfully updated"))
                {
                    loading.Hide();
                    await _userDialogs.AlertAsync("invalid , cant process data!");
                    return;
                }

                loading.Hide();
                await _userDialogs.AlertAsync("Succesfully Change.");
                await NavigationService.NavigateAsync(nameof(LoginPage));
                return;

            }
            catch(Exception)
            {

            }
            

        }

        private async Task<string> UserTransaction(ChangepassDto changepassDto)
        {
            HttpClient httpClient = new HttpClient();
            string serializedObject = JsonConvert.SerializeObject(changepassDto);
            HttpContent httpContent = new StringContent(serializedObject);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage result = await httpClient.PostAsync($"{ApiConstants.BaseUrl}changePass", httpContent);
            string response = await result.Content.ReadAsStringAsync();

            return response;
        }

        private TimelogResult ParseResponce(string response)
        {
            return JsonConvert.DeserializeObject<TimelogResult>(response);
        }
        #endregion
    }
}
