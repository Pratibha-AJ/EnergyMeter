using System;
using Microsoft.AspNetCore.Http;
using EnergyMeter.DTO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using EnergyMeter.Repository;

namespace EnergyMeter.Services
{
    public class MeterReadService : IMeterReadService
    {
        private static List<MeterRead> meterReads;
        private static List<MeterReadUploadStatus> meterReadsStatus;
       
        private IUploadRepository _uploadRepository;
        public MeterReadService(IUploadRepository uploadRepository)
        {
            _uploadRepository = uploadRepository;

        }

        public List<MeterReadUploadStatus> UploadMeterRead(IFormFile meterReadFile)
        {
            meterReads = new List<MeterRead>();
            meterReadsStatus = new List<MeterReadUploadStatus>();
            MeterReadToList(meterReadFile);
            UploadMeterReadtoDB();
            return meterReadsStatus;
        }

        public List<MeterRead> GetALLMeterReads()
        {
          return  _uploadRepository.GetALLMeterReads();
        
        }

        private void MeterReadToList(IFormFile meterReadFile)
        {
           
            var result = new StringBuilder();
            using (var reader = new StreamReader(meterReadFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }

            Chilkat.Csv csv = new Chilkat.Csv();
            csv.LoadFromString(result.ToString());
         
            int row;
            int n = csv.NumRows;         

            for (row = 1; row <= n - 1; row++)
            {               
                meterReads.Add(new MeterRead
                {
                    RowNo = row
                    ,AccountId = csv.GetCell(row, 0)              
                    ,ReadDateTime = csv.GetCell(row, 1)
                    ,  ReadValue = csv.GetCell(row, 2)
                });
            }

        }

        private void UploadMeterReadtoDB()
        {          
            DateTime dateTime = DateTime.Now;
            UInt16 readvalue = 0;
            UInt32 Account = 0;

            meterReads.ForEach(i =>
            {
                if (ValidateMeterRead(i))
                {
                    if (CheckAccountMeterReadExist(i))
                    {
                        meterReadsStatus.Add(new MeterReadUploadStatus
                        {
                            RowNo = i.RowNo,
                            AccountId = i.AccountId,
                            Status = "Error",
                            ErrorMessage = "Account Meter read exist or Less than existing meter read"

                        });

                    }
                    else {
                        _uploadRepository.InsertMeterRead(i);
                        meterReadsStatus.Add(new MeterReadUploadStatus
                        {
                            RowNo = i.RowNo,
                            AccountId = i.AccountId,
                            Status = "Success"
                           

                        });


                    }

                }
            });          

        }

        private bool ValidateMeterRead(MeterRead meterRead)
        {
            DateTime dateTime = DateTime.Now;
            UInt16 readvalue = 0;
            UInt32 Account = 0;
            bool IsDataValid = true;

            if (!UInt32.TryParse(meterRead.AccountId, out Account))
            {
                IsDataValid = false;

                if (meterReadsStatus.Where(c => c.RowNo == meterRead.RowNo).Count() == 0)
                    meterReadsStatus.Add(new MeterReadUploadStatus
                    {
                        RowNo = meterRead.RowNo,
                        AccountId = meterRead.AccountId,
                        Status = "Error",
                        ErrorMessage = "Invalid Account."

                    });
                else
                    meterReadsStatus.Find(c => c.RowNo == meterRead.RowNo).ErrorMessage = meterReadsStatus.Find(c => c.RowNo == meterRead.RowNo).ErrorMessage + "Invalid Account";
            }

            if (!DateTime.TryParse(meterRead.ReadDateTime, out dateTime))
            {
                IsDataValid = false;
                if (meterReadsStatus.Where(c => c.RowNo == meterRead.RowNo).Count() == 0)
                    meterReadsStatus.Add(new MeterReadUploadStatus
                    {
                        RowNo = meterRead.RowNo,
                        AccountId = meterRead.AccountId,
                        Status = "Error",
                        ErrorMessage = "Invalid Meter Reading Date time."

                    });
                else
                    meterReadsStatus.Find(c => c.RowNo == meterRead.RowNo).ErrorMessage = meterReadsStatus.Find(c => c.RowNo == meterRead.RowNo).ErrorMessage + "Invalid Read Date";
            }

            if (!UInt16.TryParse(meterRead.ReadValue, out readvalue))
            {
                IsDataValid = false;
                if (meterReadsStatus.Where(c => c.RowNo == meterRead.RowNo).Count() == 0)
                    meterReadsStatus.Add(new MeterReadUploadStatus
                    {
                        RowNo = meterRead.RowNo,
                        AccountId = meterRead.AccountId,
                        Status = "Error",
                        ErrorMessage = "Invalid Read Meter Value."

                    });
                else
                    meterReadsStatus.Find(c => c.RowNo == meterRead.RowNo).ErrorMessage = meterReadsStatus.Find(c => c.RowNo == meterRead.RowNo).ErrorMessage + ",Invalid Read";
            }

            return IsDataValid ;






        }

        private bool CheckAccountMeterReadExist(MeterRead meterRead)
        {
            return _uploadRepository.CheckAccountMeterReadExist(meterRead);        
        }


    }
}
