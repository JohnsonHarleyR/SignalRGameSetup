using System.Web.Mvc;

namespace SignalRGameSetup.Controllers
{
    public class GameController : Controller
    {

        public ActionResult New()
        {

            string gameCode = TempData["GameCode"].ToString();
            string participantId = TempData["ParticipantId"].ToString();

            ViewBag.GameCode = gameCode;
            ViewBag.ParticipantId = participantId;

            return View();
        }
    }
}