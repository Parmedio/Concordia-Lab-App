using ConnectionService.Abstract;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionService
{
    public class ConnectionService : IConnectionService
    {
        private readonly IWebApiCaller _webApiCaller;
        private readonly IConnectionChecker _connectionChecker;

        public ConnectionService(IWebApiCaller webApiCaller, IConnectionChecker connectionChecker)
        {
            _webApiCaller = webApiCaller;
            _connectionChecker = connectionChecker;
        }

        public async Task CheckConnectionAndCallApi()
        {
            // Verifica la connessione di rete
            Task<bool> isConnected = _connectionChecker.CheckConnection();

            if ()
            {
                // Effettua la chiamata alla web API
                await _webApiCaller.CallApi();
            }
        }


    }
}
