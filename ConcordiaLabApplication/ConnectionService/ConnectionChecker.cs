using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionService.Abstract;

namespace ConnectionService
{
    public class ConnectionChecker : IConnectionChecker
    {

        public async Task<bool> CheckConnection()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Effettua una richiesta HTTP ad un endpoint della Web API
                    HttpResponseMessage response = await httpClient.GetAsync("https://api.trello.com");

                    // Verifica lo stato della risposta
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    // Gestisci eventuali eccezioni
                    Console.WriteLine($"Si è verificato un errore durante la verifica della connessione: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
