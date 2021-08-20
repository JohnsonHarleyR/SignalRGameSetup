using SignalRGameSetup.Helpers.Chat;
using SignalRGameSetup.Models.Game;
using SignalRGameSetup.Models.Setup;
using SignalRGameSetup.Models.Setup.Containers;
using SignalRGameSetup.Py;
using System.Web.Mvc;

namespace SignalRGameSetup.Controllers
{
    public class GameController : Controller
    {

        //[HttpPost]
        public ActionResult New(GoToGamePage container)
        // TODO figure out a way to make it hide the info in the url
        {
            if (container == null || container.GameCode == null || container.ParticipantId == null)
            {
                return RedirectToAction("StartScreen", "Setup");
            }

            // HACK putting all the chat info in the model so that nothing is lost due to threading in the hub
            NewGameViewModel model = new NewGameViewModel()
            {
                GameCode = container.GameCode,
                ParticipantId = container.ParticipantId,
                Chat = ChatHelper.GetChatByGameCode(container.GameCode)
            };

            return View(model);

        }

        public ActionResult GoToGame(string gameCode, string participantId)
        {

            GoToGamePage container = new GoToGamePage()
            {
                GameCode = gameCode,
                ParticipantId = participantId
            };

            return RedirectToAction("New", "Game",
                container);
        }



        public ActionResult Test()
        {
            // create fake game
            TestModel model = new TestModel();
            model.Game = new BattleShipsGame("TEST");
            model.ParticipantId = "LALA";

            // Test python creator
            PythonEngine engine = new PythonEngine();
            var testCalculator = engine.CreateInstance("testCalculator");

            var testAddition = testCalculator.add(3, 7);

            return View(model);
        }
    }
}