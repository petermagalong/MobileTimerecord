using System;
using System.Threading.Tasks;

namespace LandBankOfThePhillipinesTLC.Services.Base
{
    public interface ISqliteService
    {
        Task<int> SaveData();
    }
}
