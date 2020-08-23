using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using LandBankOfThePhillipinesTLC.Constants;
using LandBankOfThePhillipinesTLC.Helpers;
using LandBankOfThePhillipinesTLC.Models;
using LandBankOfThePhillipinesTLC.ViewModels.Base;
using LandBankOfThePhillipinesTLC.Views;
using Newtonsoft.Json;
using Plugin.Fingerprint;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using LandBankOfThePhillipinesTLC.Services;
using static Xamarin.Essentials.Permissions;

namespace LandBankOfThePhillipinesTLC.ViewModels
{
    public class LoginPageViewModel : BaseNavigationViewModel
    {
        private readonly IUserDialogs _userDialogs;

        public LoginPageViewModel(INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService)
        {
            _userDialogs = userDialogs;
            LoginCommand = new DelegateCommand(Login);
        }

        #region Bindable Commands
        public ICommand LoginCommand { get; set; }
        public ICommand FGCommand { get; set; }
        #endregion

        #region Bindable Properties
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        #endregion

        #region method

        private async void getAuthAsync()
        {
            
            var cancelationToken = new System.Threading.CancellationToken();
            var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!", cancelationToken);
            if (result.Authenticated)
            {
                if (Helpers.Settings.LastUsedEmail != string.Empty && Helpers.Settings.LastPassword != string.Empty)
                {
                    
                  await  NavigationService.NavigateAsync(nameof(HomePage));
                }
                else
                {
                    _userDialogs.Alert("Auth", "User success but no user logined !", "ok");
                    return;
                }

            }
            else
            {
                _userDialogs.Alert("Auth", "fail", "ok");
                return;
            }

        }
        private async void Login()
        {
            var loading = _userDialogs.Loading("Please wait");
            // this is nothing
            //if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            //{
            //    _userDialogs.Alert("No Internet Access!");
            //    loading.Hide();
            //    return;
            //}

            try
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    _userDialogs.Alert("Email is Required!");
                    loading.Hide();
                    return;
                }

                if (string.IsNullOrEmpty(Password))
                {
                    _userDialogs.Alert("password is required");
                    loading.Hide();
                    return;
                }


                if (Settings.LastUsedEmail == UserName && Settings.LastPassword == Password)
                {
                    loading.Hide();
                    await NavigationService.NavigateAsync(nameof(HomePage));
                    return;
                }

                LoginDto loginDto = new LoginDto();
                loginDto.Username = UserName;
                loginDto.Password = Password;

                string response = await Authenticate(loginDto);
                UserData userData = GetUserData(response);

                //IList<Datum> detail = userData.Data;

                if (!userData.Message.Contains("Successfully fetched data.") || userData.Message.Contains("Username and password does not match."))
                {
                    loading.Hide();
                    _userDialogs.Alert("Incorrect username or password");
                    return;
                }
                if(userData.Data[0].isfirstlogin=="1")
                {
                    loading.Hide();
                    await NavigationService.NavigateAsync(nameof(ChangePasswordPage));
                    return;
                }
               
                loading.Hide();
                Settings.FullName = userData.Data[0].full_name;
                Settings.ScanningNumber = userData.Data[0].scanning_number;
                Settings.LastUsedEmail = UserName;
                Settings.LastPassword = Password;
                Settings.Lattitude1 = userData.Data[0].lat1;
                Settings.Lattitude2 = userData.Data[0].lat2;
                Settings.Longhitude1 = userData.Data[0].long1;
                Settings.Longhitude2 = userData.Data[0].long2;
                await NavigationService.NavigateAsync(nameof(HomePage));
                return;
            }
            catch (Exception)
            {
                loading.Hide();
                await _userDialogs.AlertAsync("Could not process request");
                return;
                //await _userDialogs.AlertAsync(e.ToString());
            }
        }

        private async Task<string> Authenticate(LoginDto loginDto)
        {
            HttpClient httpClient = new HttpClient();
            string serializeObject = JsonConvert.SerializeObject(loginDto);
            HttpContent httpContent = new StringContent(serializeObject);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage result = await httpClient.PostAsync($"{ApiConstants.BaseUrl}checkUser", httpContent);
            string response = await result.Content.ReadAsStringAsync();

            return response;
        }
        private UserData GetUserData(string response)
        {
            return JsonConvert.DeserializeObject<UserData>(response);
        }
        //public async Task GetLocationAsync()
        //{
        //    var status = await CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
        //    if (status != PermissionStatus.Granted)
        //    {
        //        await _userDialogs.AlertAsync("Notify user permission was denied");
        //        // Notify user permission was denied
        //        return;
        //    }

        //    var location = await Geolocation.GetLocationAsync();
        //}
        
        #endregion
    }
}
