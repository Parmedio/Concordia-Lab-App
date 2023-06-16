using BackgroundServices.Abstract;
using BackgroundServices.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerTests
{
    public class ConnectionCheckerTests
    {
        private readonly IConnectionChecker _sut;
        public ConnectionCheckerTests()
        {
            _sut = new ConnectionChecker();
        }

        [Fact]
        public async Task CheckConnection_Should_Return_True_With_Connection()
        {
            var isConnected = _sut.CheckConnection();
            Assert.True(await isConnected);
        }

        [Fact]
        public async Task CheckConnection_Should_Return_False_With_No_Connection()
        {
            var connectionChecker = new Mock<IConnectionChecker>();
            connectionChecker.Setup(c => c.CheckConnection()).ReturnsAsync(false);

            var isConnected = await connectionChecker.Object.CheckConnection();
            Assert.False(isConnected);
        }
    }
}
