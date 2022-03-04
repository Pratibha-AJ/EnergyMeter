using System;
using System.Data.SQLite;
using EnergyMeter.DTO;
using System.Collections.Generic;
  

namespace EnergyMeter.Repository
{
    public class UploadRepository : IUploadRepository
    {
       private static readonly SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");

        public UploadRepository()
        {
          //  m_dbConnection.Open();

        }

        public bool CheckAccountMeterReadExist(MeterRead meterRead)
        {
            m_dbConnection.Open();
            bool isExist = false;
            try
            {

                SQLiteCommand command = new SQLiteCommand(m_dbConnection);

                string stm = @"SELECT * FROM MeterReads WHERE AccountId = @account AND  
                             MeterReadingDateTime = @MeterReadingDateTime  ";
                command.CommandText = stm;
                command.Parameters.AddWithValue("@account", meterRead.AccountId);
                command.Parameters.AddWithValue("@MeterReadingDateTime", meterRead.ReadDateTime);
                command.Prepare();
                var acc = command.ExecuteScalar();
                if(acc != null)
                    isExist = true;
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                m_dbConnection.Close();

            }
           
            return isExist;


        }

        public bool InsertMeterRead(MeterRead meterReads)
        {
            m_dbConnection.Open();
            bool success = false;
            try
            {

               SQLiteCommand command = new SQLiteCommand(m_dbConnection);             
              
                command.CommandText = "INSERT INTO MeterReads(AccountId,MeterReadingDateTime, MeterReadValue) VALUES(@account,@MeterReadingDateTime,@MeterReadValue)";
                command.Parameters.AddWithValue("@account", meterReads.AccountId);
                command.Parameters.AddWithValue("@MeterReadingDateTime", meterReads.ReadDateTime);
                command.Parameters.AddWithValue("@MeterReadValue", meterReads.ReadValue);
                command.Prepare();
                command.ExecuteNonQuery();
                success = true;


            } catch (Exception ex)
            {
                throw ex;
            
            }
            finally
            {
                m_dbConnection.Close();
            
            }
            return success;
        }

        public List<MeterRead> GetALLMeterReads()
        {
            List<MeterRead> meterReads = new List<MeterRead>();
            try 
            { 

                m_dbConnection.Open();
                string stm = @"SELECT * FROM MeterReads ";
                SQLiteCommand command = new SQLiteCommand(stm,m_dbConnection);             
                SQLiteDataReader dr = command.ExecuteReader();

                while(dr.Read())
                {
                    meterReads.Add(new MeterRead
                    {
                        AccountId = dr.GetInt32(0).ToString()
                    ,
                        ReadDateTime = dr.GetString(1)
                    ,
                        ReadValue = dr.GetInt32(2).ToString()
                    }

                    ); ;


                }

             


             }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                m_dbConnection.Close();
            
            }
            return meterReads;

        }

        public void CreateDatabase()
        {            

            SQLiteConnection.CreateFile("MyDatabase.sqlite");
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

    }


        public void CreateTable()
        {
            string createAccount = @"CREATE TABLE Accounts(AccountId INTEGER PRIMARY KEY,
            FirstName TEXT, LastName Text)";
           

            string createMeterReads = @"CREATE TABLE MeterReads( AccountId INTEGER,   MeterReadingDateTime Text
                    ,  MeterReadValue INTEGER  ,FOREIGN KEY(AccountId) REFERENCES Accounts(AccountId)  ,PRIMARY KEY(AccountId, MeterReadingDateTime)   )";


        }

        public void InsertAccountsDB(List<Account> accounts)
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            accounts.ForEach(i =>
            {

                command.CommandText = "INSERT INTO Accounts(AccountId,FirstName, LastName) VALUES(@account,@firdtname,@lastname)";
                command.Parameters.AddWithValue("@account", i.AccountId);
                command.Parameters.AddWithValue("@firdtname", i.FirstName);
                command.Parameters.AddWithValue("@lastname", i.LastName);
                command.Prepare();
                command.ExecuteNonQuery();
            }
            );

        }

    }
}
