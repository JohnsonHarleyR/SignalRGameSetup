using SignalRGameSetup.Models.Game;
using System;
using System.Collections.Generic;

namespace SignalRGameSetup.Helpers.Game
{
    public class GameHelper
    {
        private static YahtzeeGame model;
        private static Random random = new Random();
        private static int? playerTurn = 0; // starts with first index, which is 0



        // NEW GAME

        public static void StartNewGame()
        {

            // create new model
            model = new YahtzeeGame();

            // Clear the players
            model.Players.Clear();

            // create list of all new dice
            model.Dice = new List<Die>()
           {
               new Die(0),
               new Die(1),
               new Die(2),
               new Die(3),
               new Die(4)
           };

            // roll all the dice for initial values
            RollAllDice();

        }

        // PLAYERS
        public void AddPlayer(string name)
        {
            YahtzeePlayer player = new YahtzeePlayer();

            player.Name = name;

            model.Players.Add(player);

            // Set the first player if there is only one player in the list so far
            if (model.Players.Count == 1)
            {
                SetFirstPlayer();
            }

        }

        // Set the first initial player
        public static void SetFirstPlayer()
        {
            if (model.Players.Count != 0 && model.CurrentPlayer == null)
            {
                model.CurrentPlayer = model.Players[0];
            }
        }


        // Change the current player after a turn is finished
        public static void RotatePlayer()
        {
            // if there is no current player, set the first player
            if (model.CurrentPlayer == null)
            {
                SetFirstPlayer();
            }
            else // otherwise, figure out the next player
            {
                // increment the playerTurn
                playerTurn++;

                // if the playerTurn is now greater than or equal to the Players count,
                // it's time to set the turn back to 0
                if (playerTurn >= model.Players.Count)
                {
                    playerTurn = 0;
                }
            }

        }

        // MODEL
        public YahtzeeGame GetModel()
        {
            return model;
        }

        public void SetModel(YahtzeeGame model)
        {
            GameHelper.model = model;
        }



        // DICE ROLLING

        // Set die to new value
        public static void SetDieValue(int dieIndex, int newValue)
        {
            model.Dice[dieIndex].Value = newValue;
        }

        public void SetAllDiceValues(int[] values) // values must be in correct order and be correct length
        {
            // check correct length or else log error
            if (values.Length == model.Dice.Count)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    SetDieValue(i, values[i]);
                }

            }
            else
            {
                throw new Exception("Error: value array does not match number of dice.");
            }
        }

        // Roll a single die
        public static void RollSingleDie(Die die)
        {
            die.Value = random.Next(1, 7);
        }

        // Roll a set group of dice
        public static void RollDiceGroup(List<Die> dice)
        {
            foreach (var die in dice)
            {
                RollSingleDie(die);
            }
        }

        // Roll all the dice
        public static void RollAllDice()
        {
            // Call the RollDiceGroup() method using the list of all dice
            RollDiceGroup(model.Dice);
        }
    }
}