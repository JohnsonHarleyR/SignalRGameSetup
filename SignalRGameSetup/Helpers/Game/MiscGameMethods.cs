using SignalRGameSetup.Models.Game;
using System.Collections.Generic;
using System.Linq;

namespace SignalRGameSetup.Helpers.Game
{
    public static class MiscGameMethods
    {
        // this class will do things like starting a new game and changing the player's turn


        // Start a new game - reset everything


        // Check if the game is complete
        public static bool GameIsComplete(YahtzeeGame model)
        {
            // determine how many players have completed their scoresheet
            int countFinished = model.Players.Count(p => p.Scoresheet.IsComplete);

            // see if the number of finished players is the same as the number of players in the game
            if (countFinished == model.Players.Count())
            {
                // return true if so
                return true;
            }
            // otherwise return false
            return false;
        }

        // Determine the winner - if the game is complete. Returns null otherwise.
        // Returns a list of winners just in case there is a tie
        public static List<YahtzeePlayer> GetWinnerList(YahtzeeGame model)
        {
            // create winner and set it to null
            List<YahtzeePlayer> winners = null;

            // make sure the game is complete to determine a winner - otherwise the method
            // will end up returning null
            if (GameIsComplete(model))
            {
                // go through players to determine who has the highest score
                int? highestScore = 0;
                for (int i = 0; i < model.Players.Count; i++)
                {
                    // if it's the first player, set them to the winner and highest score first
                    if (i == 0 && model.Players[i].Scoresheet.GrandTotal != null)
                    {
                        // initialize the list
                        winners = new List<YahtzeePlayer>();
                        winners.Add(model.Players[i]);
                        highestScore = winners[0].Scoresheet.GrandTotal;
                    }
                    // otherwise, see if the player beats the highest score
                    // if it's the SAME as the highest score, simply add them to the winner list
                    else if (model.Players[i].Scoresheet.GrandTotal == highestScore &&
                        model.Players[i].Scoresheet.GrandTotal != null)
                    {
                        winners.Add(model.Players[i]);
                    }
                    // otherwise, if they beat the highest score, set their score to highestscore
                    // and clear the winners list before adding the player to it
                    else if (model.Players[i].Scoresheet.GrandTotal > highestScore &&
                        model.Players[i].Scoresheet.GrandTotal != null)
                    {
                        winners.Clear();
                        winners.Add(model.Players[i]);
                        highestScore = winners[0].Scoresheet.GrandTotal;
                    }
                }
            }

            return winners;

        }


        // Change the turn to the next player - reset current player's information too (turns left, theor, Score to change)
        public static YahtzeeGame NextTurn(YahtzeeGame model)
        {

            // calculate the current player's scores based on their ScoreToChange in model
            ScoreCalculator.CalculateChosen(model.CurrentPlayer.Scoresheet, model.CurrentPlayer.ScoreToChange,
                ScoreCalculator.TurnDiceToArray(model.Dice));

            // reset player's rollsLeft, theoreticalScores, ScoreToChange
            model.CurrentPlayer.TheoreticalScores = null;
            model.CurrentPlayer.RollsLeft = 3;
            model.CurrentPlayer.ScoreToChange = null;

            // set the game's next turn to the next Player in the list
            if (model.Players.Count != 0)
            {
                for (int i = 0; i < model.Players.Count; i++)
                {
                    if (model.Players[i].Name == model.CurrentPlayer.Name)
                    {
                        model.Players[i] = model.CurrentPlayer;

                        // if it's the last item in the Players list, set the current player to the first in the list
                        if (i == model.Players.Count - 1)
                        {
                            model.CurrentPlayer = model.Players[0];
                        }
                        else
                        {
                            model.CurrentPlayer = model.Players[i + 1];
                        }
                        break;
                    }
                }
            }

            // Now check if the new current player's scoresheet is already complete.
            // If it is, use recursion by calling this method again - skip the player, basically
            if (model.CurrentPlayer.Scoresheet.IsComplete)
            {
                NextTurn(model);
            }

            return model;

        }


    }
}