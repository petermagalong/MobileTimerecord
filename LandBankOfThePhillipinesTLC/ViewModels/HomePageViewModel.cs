using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using LandBankOfThePhillipinesTLC.Constants;
using LandBankOfThePhillipinesTLC.Helpers;
using LandBankOfThePhillipinesTLC.Models;
using LandBankOfThePhillipinesTLC.Services;
using LandBankOfThePhillipinesTLC.ViewModels.Base;
using LandBankOfThePhillipinesTLC.Views;
using Newtonsoft.Json;
using Plugin.Fingerprint;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using permission = Plugin.Permissions.Abstractions;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace LandBankOfThePhillipinesTLC.ViewModels
{

    public class HomePageViewModel : BaseNavigationViewModel
    {


        private Location mylocation;
        private readonly IUserDialogs _userDialogs;

        public HomePageViewModel(INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService)
        {
            _userDialogs = userDialogs;


            Geoloc();
           //placemark();

            daytodayDisplay = DateTime.Now.ToString("dddd") + " " + DateTime.Now.ToString("MM-dd-yyyy");
            displayTime = DateTime.Now;
            
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                displayTime = DateTime.Now;
                return true;
            });
            //Geoloc();
            //permition();
            ScanNumber = Settings.ScanningNumber;
            FnameProp = Settings.FullName;
            Lat1 = Convert.ToDouble(Settings.Lattitude1);
            Lat2 = Convert.ToDouble(Settings.Lattitude2);
            Long1 = Convert.ToDouble(Settings.Longhitude1);
            Long2 = Convert.ToDouble(Settings.Longhitude2);

            SyncData();
            //locChecker();
            CheckInCommand = new DelegateCommand(Checkin);
            LunchOutCommand = new DelegateCommand(Breakout);
            LunchInCommand = new DelegateCommand(Breakin);
            CheckOutCommand = new DelegateCommand(Checkout);
            OTinCommand = new DelegateCommand(OTin);
            OTOutCommand = new DelegateCommand(OTout);
            LogoutImageCommand = new DelegateCommand(Logout);
            
        }

        #region bindable command
        public ICommand CheckInCommand { get; set; }
        public ICommand LunchOutCommand { get; set; }
        public ICommand LunchInCommand { get; set; }
        public ICommand CheckOutCommand { get; set; }
        public ICommand OTinCommand { get; set; }
        public ICommand OTOutCommand { get; set; }
        public ICommand LogoutImageCommand { get; set; }
        #endregion

        #region Bindable Properties
        private Double _lat1;
        public Double Lat1
        {
            get { return _lat1; }

            set { SetProperty(ref _lat1, value); }
        }
        private Double _lat2;
        public Double Lat2
        {
            get { return _lat2; }

            set { SetProperty(ref _lat2, value); }
        }
        private Double _long1;
        public Double Long1
        {
            get { return _long1; }

            set { SetProperty(ref _long1, value); }
        }
        private Double _long2;
        public Double Long2
        {
            get { return _long2; }

            set { SetProperty(ref _long2, value); }
        }
        private DateTime _displayTime;
        public DateTime displayTime
        {
            get { return _displayTime; }

            set { SetProperty(ref _displayTime, value); }
        }

        private string _daytodayDisplay;
        public string daytodayDisplay
        {
            get => _daytodayDisplay;
            set => SetProperty(ref _daytodayDisplay, value);
        }

        private string _geoLocator;
        public string Geolocator
        {
            get => _geoLocator;
            set => SetProperty(ref _geoLocator, value);
        }

        private UserTimelogData retuser;
        public UserTimelogData RetUser
        {
            get => retuser;
            set => SetProperty(ref retuser, value);
        }

        private string scanNumber;
        public string ScanNumber 
        {
            get => scanNumber;
            set => SetProperty(ref scanNumber, value);
        }

        private string dateProp;
        public string DateProp
        {
            get => dateProp;
            set => SetProperty(ref dateProp, value);
        }

        private string timeProp;
        public string TimeProp
        {
            get => timeProp;
            set => SetProperty(ref timeProp, value);
        }

        private string fnameProp;
        public string FnameProp
        {
            get => fnameProp;
            set => SetProperty(ref fnameProp, value);
        }
        #endregion

        #region method
        public async void Logout()
        {
            await NavigationService.NavigateAsync(nameof(LoginPage));
            return;
        }
        public async void Checkin()
        {
            var cancelationToken = new System.Threading.CancellationToken();
            var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!", cancelationToken);
            if (result.Authenticated)
            {
                await Transact("0", "Time in");

            }
            else
            {
                _userDialogs.Alert("Auth", "fail", "ok");
                return;
            }
            
            
        }
        public async void Checkout()
        {
            var cancelationToken = new System.Threading.CancellationToken();
            var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!", cancelationToken);
            if (result.Authenticated)
            {
                 await Transact("1", "Time out");
            }
            else
            {
                 _userDialogs.Alert("Auth", "fail", "ok");
                return;
            }
        }
        public async void Breakout()
        {
            var cancelationToken = new System.Threading.CancellationToken();
            var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!", cancelationToken);
            if (result.Authenticated)
            {
                  await Transact("2", "Break out");
            }
            else
            {
                _userDialogs.Alert("Auth", "fail", "ok");
                return;
            }
         }
        public async void Breakin()
        {
            var cancelationToken = new System.Threading.CancellationToken();
            var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!", cancelationToken);
            if (result.Authenticated)
            {
                await Transact("3", "Break In");
                
            }
            else
            {
                _userDialogs.Alert("Auth", "fail", "ok");
                return;
            }
        }
        public async void OTin()
        {
            var cancelationToken = new System.Threading.CancellationToken();
            var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!", cancelationToken);
            if (result.Authenticated)
            {
                await Transact("4", "OTin");
            }
            else
            {
                _userDialogs.Alert("Auth", "fail", "ok");
                return;
            }
        }
        public async void OTout()
        {
            var cancelationToken = new System.Threading.CancellationToken();
            var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!", cancelationToken);
            if (result.Authenticated)
            {
                await Transact("5", "OTout");
            }
            else
            {
                _userDialogs.Alert("Auth", "fail", "ok");
                return;
            }
        }

        //-------------- Main function inserting data with offline and ------------//
        //-------------- Offline features       -----------------------------------//

        private async Task Transact(string TrnType,string type)
        {
            List<UserTimelogData> timeloguser = new List<UserTimelogData>();
            SqliteService sql = new SqliteService();
            
            
            if (Geolocator == "")
            {
                _userDialogs.Alert("invalid location");
                return;
            }
            //back when done
            if (((Lat1 < mylocation.Latitude) && (mylocation.Latitude < Lat2) && (Long1 < mylocation.Longitude) && (mylocation.Longitude < Long2)) != true)
            {
                _userDialogs.Alert(" your location is invalid");
                return;
            }

            var loadingConnect = _userDialogs.Loading("Please wait...");
            try
            {
                UserTimelogData userTimelogData = new UserTimelogData();
                userTimelogData.scanning_number = Settings.ScanningNumber;
                userTimelogData.transaction_date = DateTime.Now.ToString("yyyy-MM-dd");
                userTimelogData.transaction_time = DateTime.Now.ToString("HH:mm");
                userTimelogData.transaction_type = TrnType;
                userTimelogData.source_location = Geolocator;
                userTimelogData.source_device = "mobile";
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    //_userDialogs.Alert("No Internet Access!");
                    //var loading = _userDialogs.Loading("Please wait...");


                    TimelogSqlData Sqldata = new TimelogSqlData();
                    var stock = new TimelogSqlData()
                    {
                        scanning_number = Settings.ScanningNumber,
                        transaction_date = DateTime.Now.ToString("yyyy-MM-dd"),
                        transaction_time = DateTime.Now.ToString("HH:mm"),
                        transaction_type = TrnType,
                        source_location = Geolocator,
                        source_device = "mobile"
                    };
                    var result = await sql.SaveData(stock);
                    var count = await sql.Counter();
                    if (result != 1)
                    {
                        _userDialogs.Alert("error to save in local file.");
                        loadingConnect.Hide();
                        return;
                    }

                    //_userDialogs.Alert(listahan.ToString());
                    _userDialogs.Alert("successfully save in local, pls connect to internet and login your account again.");
                    loadingConnect.Hide();
                    return;
                }
                else
                { 
                        string result = await UserTransaction(userTimelogData);
                        TimelogResult response = ParseResponce(result);
                        if (!response.Message.Contains("Successfully"))
                        {
                        loadingConnect.Hide();
                            await _userDialogs.AlertAsync("Failed to " + type + " !");
                            return;
                        }
                    

                    
                    loadingConnect.Hide();
                    await _userDialogs.AlertAsync("Succesfully " + type + " !");
                }
                
                DateProp = userTimelogData.transaction_date;
                TimeProp = userTimelogData.transaction_time;
                return;
                

            }
            catch(Exception)
            {
                loadingConnect.Hide();
                _userDialogs.Alert("Could not process request");
                return;
            }
        }
        private async void SyncData()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return;
            }
            
            List<UserTimelogData> timeloguser = new List<UserTimelogData>();
            SqliteService sql = new SqliteService();
            var sqlcount = await sql.Counter();
            int lastval = sqlcount;
            
            if (sqlcount != 0)
            {
                var loading = _userDialogs.Loading("Syncing Please wait...");
                try
                {
                    
                    timeloguser = await sql.listSqldata();
                    int cnt = 0;
                    for (int i = 0; i <= timeloguser.Count - 1; i++)
                    {
                        string result = await UserTransaction(timeloguser[i]);
                        TimelogResult response = ParseResponce(result);
                        if (!response.Message.Contains("Successfully"))
                        {
                            loading.Hide();
                            await _userDialogs.AlertAsync("Failed to sync !");
                            return;
                        }
                        else cnt += 1;
                    }
                    if (lastval == cnt)
                    {
                        await sql.DeleteAllWordsAsync();
                        loading.Hide();
                        await _userDialogs.AlertAsync("Syncing completed ! " + lastval + "Data sync");
                        return;
                    }
                    int total = cnt + 1;
                    loading.Hide();
                    await _userDialogs.AlertAsync("Syncing not completed ! " + total + "Data sync");
                    return;
                    //UserTransaction(timeloguser);
                }
                catch (Exception)
                {
                    // Unable to get location
                    loading.Hide();
                    _userDialogs.Alert("Could not process request");
                    return;
                }
            }
        }
        private async Task<string> UserTransaction(UserTimelogData userTimelogData)
        {
            HttpClient httpClient = new HttpClient();
            string serializedObject = JsonConvert.SerializeObject(userTimelogData);
            HttpContent httpContent = new StringContent(serializedObject);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage result = await httpClient.PostAsync($"{ApiConstants.BaseUrl}addEmployeeDTR", httpContent);
            string response = await result.Content.ReadAsStringAsync();

            return response;
        }

        private TimelogResult ParseResponce(string response)
        {
            return JsonConvert.DeserializeObject<TimelogResult>(response);
        }

        public async void Geoloc()
        {
            try
            {
                //var a = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                var b = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);

                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse))
                    {
                        await _userDialogs.AlertAsync("We need location permision");
                    }
                    var result = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    if (result.ContainsKey(Permission.Location))
                    {
                        status = result[Permission.Location];
                    }

                }

                if (status == PermissionStatus.Granted)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.High);
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    if (location == null)
                    {
                        location = await Geolocation.GetLocationAsync(new GeolocationRequest
                        {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(5)
                        });

                    }

                    if(location == null)
                    {
                        await NavigationService.NavigateAsync(nameof(LoginPage));
                        _userDialogs.Alert("invalid location pls turn on your location.");
                        return;
                    }
                    else 
                    {

                        Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                        Geolocator = location.Latitude + ", " + location.Longitude;
                        mylocation = location;
                        return;
                    }
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await NavigationService.NavigateAsync(nameof(LoginPage));
                    await _userDialogs.AlertAsync("permission denied, location is a must on this app pls try again.");
                    return;
                }

            }
            catch (FeatureNotEnabledException)
            {
                await NavigationService.NavigateAsync(nameof(LoginPage));
                await _userDialogs.AlertAsync("location is a must on this app, pls turn on your location and try again.");
                return;
                // Handle not enabled on device exception
            }
            catch (PermissionException)
            {
                await NavigationService.NavigateAsync(nameof(LoginPage));
                await _userDialogs.AlertAsync("permission denied, location is a must on this app pls try again.");
                return;
                // Handle permission exception
            }
            catch (Exception ex)
            {
                await _userDialogs.AlertAsync("error", ex.ToString() , "ok");
            }











            //try
            //{
            //    //var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationWhenInUsePermission>();
            //    //if (status != permission.PermissionStatus.Granted || status == permission.PermissionStatus.Unknown)
            //    //{
            //    //    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse))
            //    //    {
            //    //        status = await CrossPermissions.Current.RequestPermissionAsync<LocationWhenInUsePermission>();
            //    //        //Query permission
            //    //    }
            //    //    return;
            //    //}
            //    var request = new GeolocationRequest(GeolocationAccuracy.High);
            //    var location = await Geolocation.GetLastKnownLocationAsync();
            //    if (location != null)
            //    {

            //        Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

            //        Geolocator = location.Latitude + ", " + location.Longitude;
            //        mylocation = location;
            //        return;
            //    }
            //    if (location == null)
            //    {

            //        //await NavigationService.NavigateAsync(nameof(LoginPage));
            //        await _userDialogs.AlertAsync("location is a must on this app, pls open your gps location and login again. ");
            //        //await NavigationService.NavigateAsync(nameof(LoginPage));
            //        Thread.CurrentThread.Abort();
            //        return;
            //    }
            //}
            //catch (FeatureNotSupportedException )
            //{
            //    await _userDialogs.AlertAsync("Handle not supported on device");
            //    return;
            //    // Handle not supported on device exception
            //}
            //catch (FeatureNotEnabledException )
            //{
            //    await _userDialogs.AlertAsync("Not enabled on device");
            //    return;
            //    // Handle not enabled on device exception
            //}
            //catch (PermissionException )
            //{
            //    // Handle permission exception
            //}
            //catch (Exception )
            //{
            //    // Unable to get location
            //    await _userDialogs.AlertAsync("error location on device");
            //    return;
            //}

        }
       
        #endregion
    }
}
