using Microsoft.AspNetCore.Mvc;
using ClientViewer.Services;

namespace ClientViewer.Controllers;

public class HomeController : Controller
{
    private readonly WebSocketService _webSocketService;

    public HomeController(WebSocketService webSocketService)
    {
        _webSocketService = webSocketService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public JsonResult GetMessages()
    {
        return Json(_webSocketService.Messages);
    }
}
