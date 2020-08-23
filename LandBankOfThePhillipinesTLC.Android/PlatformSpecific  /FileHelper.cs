using System;
using System.IO;
using LandBankOfThePhillipinesTLC.Droid.PlatformSpecific;
using LandBankOfThePhillipinesTLC.Services.Base;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace LandBankOfThePhillipinesTLC.Droid.PlatformSpecific
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string databaseName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
        }
    }
}
