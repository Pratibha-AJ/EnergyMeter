using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using EnergyMeter.Services;


namespace EnergyMeter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterController : ControllerBase
    {
        private readonly IMeterReadService _meterReadService;
        public MeterController(IMeterReadService meterReadService)
        {
            _meterReadService = meterReadService;
        
        }

        [HttpPost]
        [Route("MeterReadUpload")]
        public IActionResult MeterReadingUpload([Required]IFormFile meterReadFile)
        {
            try
            {
                if (meterReadFile.ContentType == "text/plain")
                {
                    var status = _meterReadService.UploadMeterRead(meterReadFile);
                    return Ok(status);
                }

                return BadRequest("Invalid File");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("MeterRead")]
        public IActionResult GetALLMeterRead()
        {
            try
            {           
                return Ok(_meterReadService.GetALLMeterReads());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }




    }
}
