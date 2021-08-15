using SignalRGameSetup.Helpers.Setup;
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

            GoToGamePage container = new GoToGamePage()
            {
                GameCode = gameCode,
                ParticipantId = participantId
            };

            // generate a second code to help with participant identity not getting lost
            string callCode = SetupHelper.GenerateCode(5);

            TempData[$"GameCode{callCode}"] = gameCode;
            TempData[$"ParticipantId{callCode}"] = participantId;

            return RedirectToAction("New", "Game", new { reference = callCode });
        }


    }
}