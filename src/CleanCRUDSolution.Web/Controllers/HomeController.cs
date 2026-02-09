using Microsoft.AspNetCore.Mvc;

namespace CleanCRUDSolution.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
