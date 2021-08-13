using SignalRGameSetup.Models.Setup.Containers;
using System.Web.Mvc;

namespace SignalRGameSetup.Controllers
{
    public class SetupController : Controller
    {
        // GET: Game
        public ActionResult StartScreen()

        {
            return View();
        }

        public ActionResult GoToGame(string gameCode, string participantId)
        {
            // test this
            ViewBag.Test = $"Setup: {gameCode}; Id: {participantId}";

            GoToGamePage container = new GoToGamePage()
            {
                GameCode = gameCode,
                ParticipantId = participantId
            };

            TempData["GameCode"] = gameCode;
            TempData["ParticipantId"] = participantId;

            return RedirectToAction("New", "Game");
        }


    }
}