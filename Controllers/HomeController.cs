using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieStoreWeb.Models;

namespace MovieStoreWeb.Controllers;

public class HomeController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

}
