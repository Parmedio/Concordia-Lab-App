using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionService
{
    public class WebApiCaller : IWebApiCaller
    {
        private readonly HttpClient _httpClient;

        public WebApiCaller(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CallApi()
        {
            try
            {
                // Effettua la chiamata HTTP alla tua Web API
                HttpResponseMessage response = await _httpClient.GetAsync("https://example.com/api/sync");

                // Verifica lo stato della risposta
                if (response.IsSuccessStatusCode)
                {
                    // La chiamata è stata eseguita con successo
                    string responseContent = await response.Content.ReadAsStringAsync();
                    // Puoi elaborare la risposta qui, se necessario
                }
                else
                {
                    // La chiamata ha restituito un errore
                    string errorMessage = $"Errore durante la sincronizzazione: {response.StatusCode}";
                    // Puoi gestire l'errore qui, se necessario
                }
            }
            catch (Exception ex)
            {
                // Si è verificato un errore durante la chiamata HTTP
                string errorMessage = $"Errore durante la sincronizzazione: {ex.Message}";
                // Puoi gestire l'errore qui, se necessario
            }
        }
    }
}
