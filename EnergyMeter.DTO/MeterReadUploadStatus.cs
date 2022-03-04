using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyMeter.DTO
{
    public class MeterReadUploadStatus
    {
        public int RowNo { get; set; }

        public string AccountId { get; set; }

        public string Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}
