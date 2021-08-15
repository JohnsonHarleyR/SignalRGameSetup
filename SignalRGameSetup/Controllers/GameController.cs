using System.Web.Mvc;

namespace SignalRGameSetup.Controllers
{
    public class GameController : Controller
    {

        public ActionResult New(string reference)
        {
            // Put important participant info on the page
            var gameCodeData = TempData[$"GameCode{reference}"];
            var participantIdData = TempData[$"ParticipantId{reference}"];
            string gameCode = null;
            string participantId = null;
            if (gameCodeData == null || participantIdData == null)
            {
                return RedirectToAction("StartScreen", "Setup");
            }
            else
            {
                gameCode = TempData[$"GameCode{reference}"].ToString();
                participantId = TempData[$"ParticipantId{reference}"].ToString();
            }

            ViewBag.GameCode = gameCode;
            ViewBag.ParticipantId = participantId;

            return View();

        }
    }
}