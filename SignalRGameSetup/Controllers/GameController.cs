using System.Web.Mvc;

namespace SignalRGameSetup.Controllers
{
    public class GameController : Controller
    {

        public ActionResult New()
        {
            // Put important participant info on the page
            var gameCodeData = TempData["GameCode"];
            var participantIdData = TempData["ParticipantId"];
            string gameCode = null;
            string participantId = null;
            if (gameCodeData == null || participantIdData == null)
            {
                return RedirectToAction("StartScreen", "Setup");
            }
            else
            {
                gameCode = TempData["GameCode"].ToString();
                participantId = TempData["ParticipantId"].ToString();
            }

            ViewBag.GameCode = gameCode;
            ViewBag.ParticipantId = participantId;

            return View();

        }
    }
}