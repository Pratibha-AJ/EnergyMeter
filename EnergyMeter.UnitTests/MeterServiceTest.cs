using EnergyMeter.Controllers;
using EnergyMeter.Services;
using System;
using Xunit;
using NSubstitute;
using EnergyMeter.Repository;

namespace EnergyMeter.UnitTests
{
   public class MeterServiceTest
    {
        private readonly MeterReadService _meterReadService;
        private readonly IUploadRepository _meterRepository;
        public MeterServiceTest()
        {
            _meterRepository = Substitute.For<IUploadRepository>();
            _meterReadService = new MeterReadService(_meterRepository);

        }

    }
}
