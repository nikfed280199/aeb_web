using Microsoft.AspNetCore.Mvc;
using ClientHistory.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClientHistory.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://web-server/api/messages");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetMessages(DateTime? startDate, DateTime? endDate)
        {
            var messages = await GetMessagesByDateRange(startDate, endDate);
            return View("Index", messages);
        }

        private async Task<List<Message>> GetMessagesByDateRange(DateTime? startDate, DateTime? endDate)
        {
            string query = "?";
            if (startDate.HasValue)
            {
                query += "startDate=" + startDate.Value.ToString("o") + "&";
            }
            if (endDate.HasValue)
            {
                query += "endDate=" + endDate.Value.ToString("o") + "&";
            }
            query = query.TrimEnd('&');

            var response = await _httpClient.GetAsync(query);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Message>>();
        }
    }
}
