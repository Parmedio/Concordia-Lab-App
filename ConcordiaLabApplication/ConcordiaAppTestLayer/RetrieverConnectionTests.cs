using BackgroundServices;

using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Moq;

namespace ConcordiaAppTestLayer
{
    public class RetrieverConnectionTests
    {
        private readonly IConfiguration _configuration;
        private readonly IRetrieveConnectionTimeInterval _sut;

        public RetrieverConnectionTests()
        {
            _configuration = Mock.Of<IConfiguration>();
            Mock.Get(_configuration).Setup(p => p.GetSection("ConnectionCheckerInfo:ConnectionIntervalInfo:initialDate").Value).Returns("2023-1-1T00:00:00.000Z");
            Mock.Get(_configuration).Setup(p => p.GetSection("ConnectionCheckerInfo:ConnectionIntervalInfo:offset").Value).Returns("00.00:8:00");
            Mock.Get(_configuration).Setup(p => p.GetSection("ConnectionCheckerInfo:ConnectionIntervalInfo:duration").Value).Returns("00:4:00");
            _sut = new RetrieveConnectionTimeInterval(_configuration);
        }


        [Fact]
        public void DateInIntervalCheck()
        {
            _sut.IsTimeInInterval(DateTime.Parse("2023-1-1T00:00:00.000Z")).Should().Be((true, TimeSpan.Zero));
            _sut.IsTimeInInterval(DateTime.Parse("2023-1-1T00:04:00.000Z")).Should().Be((true, TimeSpan.Zero));
            _sut.IsTimeInInterval(DateTime.Parse("2023-1-1T00:04:01.000Z")).Should().Be((false, TimeSpan.Parse("00:03:59")));
            _sut.IsTimeInInterval(DateTime.Parse("2023-1-1T00:05:00.000Z")).Should().Be((false, TimeSpan.Parse("00:03:00")));
            _sut.IsTimeInInterval(DateTime.Parse("2023-1-1T00:08:00.000Z")).Should().Be((true, TimeSpan.Zero));
            _sut.IsTimeInInterval(DateTime.Parse("2023-1-1T00:08:01.000Z")).Should().Be((true, TimeSpan.Zero));
            _sut.IsTimeInInterval(DateTime.Parse("2023-1-1T00:12:01.000Z")).Should().Be((false, TimeSpan.Parse("00:03:59")));
        }

    }
}