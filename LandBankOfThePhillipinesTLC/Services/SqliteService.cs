using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandBankOfThePhillipinesTLC.Constants;
using LandBankOfThePhillipinesTLC.Models;
using LandBankOfThePhillipinesTLC.Services.Base;
using SQLite;
using Xamarin.Forms;

namespace LandBankOfThePhillipinesTLC.Services
{
    public class SqliteService
    {
        private readonly SQLiteAsyncConnection _sqliteConnection;

        public SqliteService()
        {
            string databasePath = DependencyService.Get<IFileHelper>().GetLocalFilePath(AppConstants.DatabaseFileName);

            _sqliteConnection = new SQLiteAsyncConnection(databasePath, AppConstants.Flags);
            _sqliteConnection.CreateTableAsync<TimelogSqlData>();
        }

        public async Task<int> SaveData(TimelogSqlData transactionItem)
        {
            return await _sqliteConnection.InsertAsync(transactionItem);
        }
        public async Task<int> Counter()
        {
            return await _sqliteConnection.ExecuteScalarAsync<int>("select count(*) from TimelogSqlData");
        }
        public async Task<List<UserTimelogData>> listSqldata()
        {
            List<UserTimelogData> userlogdata = new List<UserTimelogData>();
            var query = _sqliteConnection.Table<TimelogSqlData>().Where(s => s.source_device.Equals("mobile"));

            var result = await query.ToListAsync();
            //return result;
            //var a = "";
            
            foreach (var s in result)
            {

                userlogdata.Add
                (new UserTimelogData
                {
                    scanning_number = s.scanning_number,
                    transaction_date = s.transaction_date,
                    transaction_time = s.transaction_time,
                    transaction_type = s.transaction_type,
                    source_location = s.source_location,
                    source_device = s.source_device
                });
            }
            return userlogdata;    

        }
        public async Task<string> DeleteAllWordsAsync()
        {
            await _sqliteConnection.DeleteAllAsync<TimelogSqlData>();
            return "success";
        }

    }
}
