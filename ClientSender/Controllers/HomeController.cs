using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using ClientSender.Models;

namespace ClientSender.Controllers;

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
    public async Task<IActionResult> SendMessage(string text, int sequenceNumber)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var message = new Message
            {
                Text = text,
                SequenceNumber = sequenceNumber
            };

            var response = await _httpClient.PostAsJsonAsync("", message);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.MessageStatus = "Message sent!";
            }
            else
            {
                ViewBag.MessageStatus = "Error sending message.";
            }
        }
        else
        {
            ViewBag.MessageStatus = "Message cannot be empty.";
        }
        return View("Index");
    }
}
