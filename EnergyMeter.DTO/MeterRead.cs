using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyMeter.DTO
{
    public class MeterRead
    {
        public int RowNo { get; set; }
        public string AccountId { get; set; }

        public string ReadDateTime { get; set; }

        public string ReadValue { get; set; }
    }
}
