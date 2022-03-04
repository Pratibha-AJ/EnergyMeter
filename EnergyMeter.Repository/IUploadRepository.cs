using System;
using System.Collections.Generic;
using System.Text;
using EnergyMeter.DTO;
namespace EnergyMeter.Repository
{
    public interface IUploadRepository
    {
        public bool CheckAccountMeterReadExist(MeterRead meterRead);
        public bool InsertMeterRead(MeterRead meterReads);
        public List<MeterRead> GetALLMeterReads();

        public void CreateDatabase();

        public void CreateTable();

        public void InsertAccountsDB(List<Account> accounts);
    }
}
