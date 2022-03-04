using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using EnergyMeter.DTO;

namespace EnergyMeter.Services
{
    public interface IMeterReadService
    {
        public List<MeterReadUploadStatus> UploadMeterRead(IFormFile meterReadFile);
        public List<MeterRead> GetALLMeterReads();

    }
}
