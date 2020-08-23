using System;
namespace LandBankOfThePhillipinesTLC.Services.Base
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string databaseName);
    }
}
