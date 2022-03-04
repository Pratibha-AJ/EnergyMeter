using EnergyMeter.Controllers;
using EnergyMeter.Services;
using System;
using Xunit;
using NSubstitute;

namespace EnergyMeter.UnitTests
{
    public class MeterControllerTest
    {

        private readonly IMeterReadService _meterReadService;
        private readonly MeterController _meterController;


        public MeterControllerTest()
        {
            _meterReadService = Substitute.For<IMeterReadService>();
            _meterController = new MeterController(_meterReadService);

        }

        [Fact]
        public void Test1()
        {

        }

    }


      

}

