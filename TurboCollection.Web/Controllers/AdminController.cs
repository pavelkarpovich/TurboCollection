using Microsoft.AspNetCore.Mvc;

namespace TurboCollection.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
