using SignalRGameSetup.Helpers.Chat;
using SignalRGameSetup.Models.Setup;
using SignalRGameSetup.Models.Setup.Containers;
using System.Web.Mvc;

namespace SignalRGameSetup.Controllers
{
    public class GameController : Controller
    {
        // TODO Move unit tests from old project into this one

        // TODO Add button on main page for players to start a new game - add a "are you sure?" message
        // TODO store all game information into a table that can be accessed later
        // TODO  when page first loads (session?) ask user if they want to start a new game or load the previous game
        // TODO Add a high score table
        // TODO Add a page once somebody wins to say the winner
        // TODO Allow user to log in and track their stats? (maybe not, idk - long term maybe)

        // TODO solve issue with maximum request length being exceeded (?)

        // TODO remove all unnecessary code and commented out stuff and also references at top

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


    }
}