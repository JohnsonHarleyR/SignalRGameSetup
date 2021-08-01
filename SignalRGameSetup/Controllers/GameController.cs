using System.Web.Mvc;

namespace SignalRGameSetup.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult StartScreen()
        {
            return View();
        }
    }
}