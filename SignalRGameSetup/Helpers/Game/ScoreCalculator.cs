using SignalRGameSetup.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalRGameSetup.Helpers.Game
{
    // TODO refactor methods to use LINQ and lambdas

    // Calculate all the scores based on a given set of dice
    public static class ScoreCalculator
    {

        // Other important methods for game


        // Take a list of dice from the yahtzee model and return an array of integer values for the dice
        public static int[] TurnDiceToArray(List<Die> dice)
        {
            int[] diceArray = dice.Select(die => die.Value).ToArray();

            return diceArray;

        }

        // method to calculate all theoretical scores with a set of dice
        // this should create a new sheet, copy any values from the sheet passed in, then
        // calculate all missing values (besides totals and bonuses) based on the given dice
        // then it should return the sheet (this can be used on the view to show user theoretical calculations)
        public static ScoreSheet GetTheoreticalScores(ScoreSheet originalSheet, int[] dice)
        {
            // create new
            ScoreSheet newSheet = new ScoreSheet();

            // copy values into new sheet
            foreach (var property in originalSheet.GetType().GetProperties())
            {

                // Make sure the parameter isn't IsComplete as that's the only one that can't be passed
                if (property.Name != "IsComplete" && property.GetValue(originalSheet) != null)
                {
                    SetPropertyToValue(newSheet, property.Name, (int?)property.GetValue(originalSheet));
                }

            }

            if (originalSheet.Aces == null)
            {
                CalculateAces(newSheet, dice);
            }
            if (originalSheet.Twos == null)
            {
                CalculateTwos(newSheet, dice);
            }
            if (originalSheet.Threes == null)
            {
                CalculateThrees(newSheet, dice);
            }
            if (originalSheet.Fours == null)
            {
                CalculateFours(newSheet, dice);
            }
            if (originalSheet.Fives == null)
            {
                CalculateFives(newSheet, dice);
            }
            if (originalSheet.Sixes == null)
            {
                CalculateSixes(newSheet, dice);
            }

            if (originalSheet.ThreeOfAKind == null)
            {
                CalculateThreeOfAKind(newSheet, dice);
            }
            if (originalSheet.FourOfAKind == null)
            {
                CalculateFourOfAKind(newSheet, dice);
            }
            if (originalSheet.FullHouse == null)
            {
                CalculateFullHouse(newSheet, dice);
            }
            if (originalSheet.SmallStraight == null)
            {
                CalculateSmallStraight(newSheet, dice);
            }
            if (originalSheet.LargeStraight == null)
            {
                CalculateLargeStraight(newSheet, dice);
            }
            if (originalSheet.Chance == null)
            {
                CalculateChance(newSheet, dice);
            }
            if (originalSheet.Yahtzee == null)
            {
                CalculateYahtzee(newSheet, dice);
            }



            //// go through properties on new sheet, calculate anything null besides the bonuses and totals
            //foreach (var property in newSheet.GetType().GetProperties())
            //{
            //    // if it's null, calculate that property with CalculateChosen
            //    if (property.GetValue(newSheet) == null)
            //    {
            //        CalculateChosenNoCheck(newSheet, property.Name, dice);
            //    }
            //}

            // return new sheet
            return newSheet;
        }


        // method to calculate chosen score based on property name
        public static void CalculateChosen(ScoreSheet scoresheet, string propertyName, int[] dice)
        {
            // figure out which method to call
            switch (propertyName)
            {
                case "Aces":
                    CalculateAces(scoresheet, dice);
                    break;
                case "Twos":
                    CalculateTwos(scoresheet, dice);
                    break;
                case "Threes":
                    CalculateThrees(scoresheet, dice);
                    break;
                case "Fours":
                    CalculateFours(scoresheet, dice);
                    break;
                case "Fives":
                    CalculateFives(scoresheet, dice);
                    break;
                case "Sixes":
                    CalculateSixes(scoresheet, dice);
                    break;
                case "ThreeOfAKind":
                    CalculateThreeOfAKind(scoresheet, dice);
                    break;
                case "FourOfAKind":
                    CalculateFourOfAKind(scoresheet, dice);
                    break;
                case "FullHouse":
                    CalculateFullHouse(scoresheet, dice);
                    break;
                case "SmallStraight":
                    CalculateSmallStraight(scoresheet, dice);
                    break;
                case "LargeStraight":
                    CalculateLargeStraight(scoresheet, dice);
                    break;
                case "Yahtzee":
                    CalculateYahtzee(scoresheet, dice);
                    break;
                case "YahtzeeBonus":
                    AddYahtzeeBonus(scoresheet, dice);
                    break;
                case "Chance":
                    CalculateChance(scoresheet, dice);
                    break;
                default:
                    Console.WriteLine("Error: scorecard property not found.");
                    break;
            }

            // do checks after to see if any bonuses or total scores need to be calculated
            // also see if the player is finished with their scoresheet - everything is filled out
            CheckComplete(scoresheet);

        }

        // method to calculate chosen score based on property name - this one does not do a score check - relevant for theoretical calculations
        private static void CalculateChosenNoCheck(ScoreSheet scoresheet, string propertyName, int[] dice)
        {
            // figure out which method to call
            switch (propertyName)
            {
                case "Aces":
                    CalculateAces(scoresheet, dice);
                    break;
                case "Twos":
                    CalculateTwos(scoresheet, dice);
                    break;
                case "Threes":
                    CalculateThrees(scoresheet, dice);
                    break;
                case "Fours":
                    CalculateFours(scoresheet, dice);
                    break;
                case "Fives":
                    CalculateFives(scoresheet, dice);
                    break;
                case "Sixes":
                    CalculateSixes(scoresheet, dice);
                    break;
                case "ThreeOfAKind":
                    CalculateThreeOfAKind(scoresheet, dice);
                    break;
                case "FourOfAKind":
                    CalculateFourOfAKind(scoresheet, dice);
                    break;
                case "FullHouse":
                    CalculateFullHouse(scoresheet, dice);
                    break;
                case "SmallStraight":
                    CalculateSmallStraight(scoresheet, dice);
                    break;
                case "LargeStraight":
                    CalculateLargeStraight(scoresheet, dice);
                    break;
                case "Yahtzee":
                    CalculateYahtzee(scoresheet, dice);
                    break;
                case "YahtzeeBonus":
                    AddYahtzeeBonus(scoresheet, dice);
                    break;
                case "Chance":
                    CalculateChance(scoresheet, dice);
                    break;
                default:
                    break;
            }

        }

        // method to calculate chosen score based on property name - this one does not do a score check - relevant for theoretical calculations
        private static void SetPropertyToValue(ScoreSheet scoresheet, string propertyName, int? value)
        {
            // figure out which method to call
            switch (propertyName)
            {
                case "Aces":
                    scoresheet.Aces = value;
                    break;
                case "Twos":
                    scoresheet.Twos = value;
                    break;
                case "Threes":
                    scoresheet.Threes = value;
                    break;
                case "Fours":
                    scoresheet.Fours = value;
                    break;
                case "Fives":
                    scoresheet.Fives = value;
                    break;
                case "Sixes":
                    scoresheet.Sixes = value;
                    break;
                case "ThreeOfAKind":
                    scoresheet.ThreeOfAKind = value;
                    break;
                case "FourOfAKind":
                    scoresheet.FourOfAKind = value;
                    break;
                case "FullHouse":
                    scoresheet.FullHouse = value;
                    break;
                case "SmallStraight":
                    scoresheet.SmallStraight = value;
                    break;
                case "LargeStraight":
                    scoresheet.LargeStraight = value;
                    break;
                case "Yahtzee":
                    scoresheet.Yahtzee = value;
                    break;
                case "YahtzeeBonus":
                    scoresheet.YahtzeeBonusCount = value;
                    break;
                case "Chance":
                    scoresheet.Chance = value;
                    break;
                default:
                    Console.WriteLine("Error: scorecard property not found.");
                    break;
            }

        }



        // Start and return a new score sheet
        public static ScoreSheet GetEmptySheet()
        {
            // create sheet
            ScoreSheet scoresheet = new ScoreSheet();

            // populate sheet with initial values
            foreach (var property in scoresheet.GetType().GetProperties())
            {
                // Make sure it's not the Yahtzee Bonus
                if (property.Name != "YahtzeeBonusCount")
                {
                    property.SetValue(scoresheet, null);
                }

            }

            // return new scoresheet
            return scoresheet;


        }

        // UPPER SECTION METHODS

        public static void CalculateAces(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.Aces == null)
            {
                int? score = CountHowMany(1, dice) * 1;

                // set the value to the new score
                scoresheet.Aces = score;
            }


        }

        public static void CalculateTwos(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.Twos == null)
            {
                int? score = CountHowMany(2, dice) * 2;

                // set the value to the new score
                scoresheet.Twos = score;
            }
        }

        public static void CalculateThrees(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.Threes == null)
            {
                int? score = CountHowMany(3, dice) * 3;


                // set the value to the new score
                scoresheet.Threes = score;

            }
        }

        public static void CalculateFours(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.Fours == null)
            {
                int? score = CountHowMany(4, dice) * 4;

                // set the value to the new score
                scoresheet.Fours = score;

            }
        }

        public static void CalculateFives(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.Fives == null)
            {
                int? score = CountHowMany(5, dice) * 5;


                // set the value to the new score
                scoresheet.Fives = score;

            }
        }

        public static void CalculateSixes(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.Sixes == null)
            {
                int? score = CountHowMany(6, dice) * 6;

                // set the value to the new score
                scoresheet.Sixes = score;

            }
        }









        // LOWER SECTION METHODS

        public static void CalculateThreeOfAKind(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.ThreeOfAKind == null)
            {
                int? score = 0;

                // Calculate
                // make sure there is a three of a kind. If so, add all dice together

                // first, get the most common value - this will be the 3 of a kind if it has at least 3
                int mostCommon = GetMostCommonValue(dice);

                // now get how many there are
                int howMany = CountHowMany(mostCommon, dice);

                // if there are 3 or more, add all the dice together
                if (howMany >= 3)
                {
                    score = AddDiceTogether(dice);
                }

                // set the value to the new score
                scoresheet.ThreeOfAKind = score;

            }
        }

        public static void CalculateFourOfAKind(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.FourOfAKind == null)
            {
                int? score = 0;

                // Calculate
                // make sure there is a four of a kind. If so, add all dice together

                // first, get the most common value - this will be the 3 of a kind if it has at least 3
                int mostCommon = GetMostCommonValue(dice);

                // now get how many there are
                int howMany = CountHowMany(mostCommon, dice);

                // if there are 3 or more, add all the dice together
                if (howMany >= 4)
                {
                    score = AddDiceTogether(dice);
                }

                // set the value to the new score
                scoresheet.FourOfAKind = score;

            }
        }

        public static void CalculateFullHouse(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.FullHouse == null)
            {
                int? score = 0;

                // Calculate
                // get a list of values in the dice - if there are two, continue
                List<int> values = GetOriginalValues(dice);
                if (values.Count == 2)
                {
                    // find out which value is most common and least common
                    int mostCommon;
                    int leastCommon;
                    if (GetMostCommonValue(dice) == values[0])
                    {
                        mostCommon = values[0];
                        leastCommon = values[1];
                    }
                    else
                    {
                        mostCommon = values[1];
                        leastCommon = values[0];
                    }
                    // now check that the most common has a count of 3 and the other has a count of two
                    if (CountHowMany(mostCommon, dice) == 3 && CountHowMany(leastCommon, dice) == 2)
                    {
                        // if the conditions match, add 25 to the score
                        score = 25;
                    }
                }

                // set the value to the new score
                scoresheet.FullHouse = score;

            }
        }

        public static void CalculateSmallStraight(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.SmallStraight == null)
            {
                int? score = 0;

                // Calculate
                // get a list of values
                List<int> values = GetOriginalValues(dice);

                // see how many numbers are in a row
                int howManyInRow = GetHowManyInRow(values);

                // if there are four numbers in a row, set the score to 30
                if (howManyInRow >= 4)
                {
                    score = 30;
                }

                scoresheet.SmallStraight = score;
            }
        }

        public static void CalculateLargeStraight(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.LargeStraight == null)
            {
                int? score = 0;

                // Calculate
                // get a list of values
                List<int> values = GetOriginalValues(dice);

                // see how many numbers are in a row
                int howManyInRow = GetHowManyInRow(values);

                // if there are four numbers in a row, set the score to 30
                if (howManyInRow == 5)
                {
                    score = 40;
                }

                scoresheet.LargeStraight = score;
            }
        }

        public static void CalculateYahtzee(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.Yahtzee == null)
            {
                int? score = 0;

                // Calculate
                // take the first die value and see how many there are
                int howMany = CountHowMany(dice[0], dice);

                // if there are 5, there is a yahtzee so change the score to 50
                if (howMany == 5)
                {
                    score = 50;
                }

                // set the value to the new score
                scoresheet.Yahtzee = score;

            }
        }

        public static void CalculateChance(ScoreSheet scoresheet, int[] dice)
        {
            // Only do it if property on scoresheet is still null
            if (scoresheet.Chance == null)
            {
                int? score = 0;

                // Calculate
                //add all the dice together
                score = AddDiceTogether(dice);

                // set the value to the new score
                scoresheet.Chance = score;

            }
        }

        // Add to the count of how may yahtzee bonuses there are
        public static void AddYahtzeeBonus(ScoreSheet scoresheet, int[] dice)
        {
            // make sure first that a yahtzee has been already been filled in and the current
            // set of dice is a yahtzee. If so, add to the count
            if (scoresheet.Yahtzee == 50 && IsYahtzee(dice))
            {
                scoresheet.YahtzeeBonusCount++;
            }
        }


        // METHODS TO CHECK IF A SECTION IS COMPLETE - IF IT IS THEN CALCULATE BONUSES AND TOTALS


        // METHODS TO CALCULATE BONUSES

        // This one should only be called after the uppertotalbeforebonus is calculated
        private static void CalculateUpperBonus(ScoreSheet scoresheet)
        {
            int? bonus = 0;

            // if the upper total before the bonus is at least 63, add the bonus
            if (scoresheet.UpperTotalBeforeBonus >= 63)
            {
                bonus = 35;
            }

            // set the upper bonus
            scoresheet.UpperBonus = bonus;

        }

        private static void CalculateYahtzeeBonus(ScoreSheet scoresheet)
        {

            // set the bonus to 100 * the number of yahtzee bonuses
            int? bonus = 100 * scoresheet.YahtzeeBonusCount;

            // set that property to total
            scoresheet.YahtzeeBonus = bonus;

        }


        // METHODS TO CALCULATE TOTALS

        // This method checks the upper and lower sections and then calls the calculations if they are needed
        public static void CheckComplete(ScoreSheet scoresheet)
        {

            // check the upper section
            if (scoresheet.UpperTotalFinal == null && IsUpperComplete(scoresheet))
            {
                // First calculate total without bonus
                CalculateUpperTotalBeforeBonus(scoresheet);

                // Now calculate the bonus
                CalculateUpperBonus(scoresheet);

                // Now calculate final total
                CalculateUpperTotalFinal(scoresheet);

            }

            // check the lower section
            if (scoresheet.LowerTotal == null & IsLowerComplete(scoresheet))
            {
                // Calculate lower total - this will also calculate the yahtzee bonus
                CalculateLowerTotal(scoresheet);
            }

            // check the grand total - if it's null and both sections are complete
            if (scoresheet.GrandTotal == null && scoresheet.UpperTotalFinal != null &&
                scoresheet.LowerTotal != null)
            {
                // Calculate the grand total
                CalculateGrandTotal(scoresheet);

                // once grand total has been calculated, set IsComplete to true on the spreadsheet
                scoresheet.IsComplete = true;
            }

        }

        private static void CalculateUpperTotalBeforeBonus(ScoreSheet scoresheet)
        {


            // Get the list of upper properties
            List<int?> properties = GetUpperProperties(scoresheet);

            // total up all the values
            int? total = 0;
            foreach (var p in properties)
            {
                total += p;
            }

            // set the total before bonus
            scoresheet.UpperTotalBeforeBonus = total;


        }

        // NOTE this one will calculate everything for the upper section
        private static void CalculateUpperTotalFinal(ScoreSheet scoresheet)
        {


            // Now calculate final total
            scoresheet.UpperTotalFinal = scoresheet.UpperTotalBeforeBonus + scoresheet.UpperBonus;


        }

        private static void CalculateLowerTotal(ScoreSheet scoresheet)
        {

            // only allow this method if the upper section is complete without nulls
            if (IsLowerComplete(scoresheet))
            {
                // Get the list of upper properties
                List<int?> properties = GetLowerProperties(scoresheet);

                // total up all the values
                int? total = 0;
                foreach (var p in properties)
                {
                    total += p;
                }

                // calculate the yahtzee bonus and add it too
                CalculateYahtzeeBonus(scoresheet);
                total += scoresheet.YahtzeeBonus;

                // set the total before bonus
                scoresheet.LowerTotal = total;

            }


        }

        // Only if everything else has been calculated
        private static void CalculateGrandTotal(ScoreSheet scoresheet)
        {
            scoresheet.GrandTotal = scoresheet.UpperTotalFinal + scoresheet.LowerTotal;
        }



        // PRIVATE METHODS TO HELP WITH CALCULATIONS

        private static List<int?> GetUpperProperties(ScoreSheet scoresheet)
        {
            // make a list of the upper section values
            List<int?> properties = new List<int?>
            {
                scoresheet.Aces, scoresheet.Twos, scoresheet.Threes,
                scoresheet.Fours, scoresheet.Fives, scoresheet.Sixes
            };

            return properties;
        }

        private static List<int?> GetLowerProperties(ScoreSheet scoresheet)
        {
            // make a list of the lower section values
            List<int?> properties = new List<int?>
            {
                scoresheet.ThreeOfAKind, scoresheet.FourOfAKind, scoresheet.FullHouse,
                scoresheet.SmallStraight, scoresheet.LargeStraight, scoresheet.Yahtzee, scoresheet.Chance
            };

            return properties;
        }

        // determine if the upper section is all filled out - good before calculating totals and bonuses
        private static bool IsUpperComplete(ScoreSheet scoresheet)
        {

            // make a list of the upper section values
            List<int?> properties = GetUpperProperties(scoresheet);

            // now loop through properties - if one of them is null, return false
            foreach (var p in properties)
            {
                if (p == null)
                {
                    return false;
                }
            }

            // if nothing was false, return true
            return true;

        }


        // determine if the lower section is all filled out - good before calculating totals and bonuses
        private static bool IsLowerComplete(ScoreSheet scoresheet)
        {
            // make a list of the upper section values
            List<int?> properties = GetLowerProperties(scoresheet);

            // now loop through properties - if one of them is null, return false
            foreach (var p in properties)
            {
                if (p == null)
                {
                    return false;
                }
            }

            // if nothing was false, return true
            return true;
        }


        // count how many dice of a particular number there are
        private static int CountHowMany(int number, int[] dice)
        {
            int count = 0;

            // loop through and count how many there are in the dice
            for (int i = 0; i < dice.Length; i++)
            {
                if (dice[i] == number)
                {
                    count++;
                }
            }

            // return the count
            return count;

        }

        // add all dice togther
        private static int? AddDiceTogether(int[] dice)
        {
            int total = 0;

            // loop through and count how many there are in the dice
            for (int i = 0; i < dice.Length; i++)
            {
                total += dice[i];
            }

            // return the count
            return total;

        }

        // check if there is a yahtzee or not and return true or false
        private static bool IsYahtzee(int[] dice)
        {
            // grab the first die and see if there are 5 of them. If so, return true.
            int howMany = CountHowMany(dice[0], dice);

            if (howMany == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // get a list of all the original numbers in a dice array
        private static List<int> GetOriginalValues(int[] dice)
        {
            List<int> values = new List<int>();

            // loop through and add any original values to the list
            for (int i = 0; i < dice.Length; i++)
            {
                if (!values.Contains(dice[i]))
                {
                    values.Add(dice[i]);
                }
            }

            // return list
            return values;

        }

        // get the die value with the highest count
        private static int GetMostCommonValue(int[] dice)
        {
            // first get a list of original values
            List<int> values = GetOriginalValues(dice);

            // prepare variables to find highest count
            int commonValue = 0;
            int howMany = 0;

            // loop through values list - if a value has a higher
            // count than the howMany variable, set that to the new common value
            foreach (var value in values)
            {
                int count = CountHowMany(value, dice);
                if (count > howMany)
                {
                    commonValue = value;
                    howMany = count;
                }
            }

            // NOTE: It is ok for the purposes of this if two values have the same count and only 1 is returned -
            // this method is used for 3 and 4 of a kind and such.

            // return the most common value
            return commonValue;

        }

        // get the lowest die value - for the sake of calculating straights
        private static int GetLowestValue(List<int> values)
        {
            int lowest = 7; // there should definitely be one lower than 7 lol

            // loop through values, store lowest
            foreach (var value in values)
            {
                if (value < lowest)
                {
                    lowest = value;
                }
            }

            // return lowest value
            return lowest;

        }

        // get the highest die value - for the sake of calculating straights
        private static int GetHighestValue(List<int> values)
        {
            int highest = 0; // there should definitely be one higher than 0

            // loop through values, store lowest
            foreach (var value in values)
            {
                if (value > highest)
                {
                    highest = value;
                }
            }

            // return lowest value
            return highest;

        }


        // get how many values in a list are in a row
        private static int GetHowManyInRow(List<int> values)
        {
            // two variables, one counting from lowest, the other counting from highest
            int countFromLowest = 1; // tthere will always be at least one, either the lowest or highest
            int countFromHighest = 1;

            // get highest and lowest values in list
            int lowest = GetLowestValue(values);
            int highest = GetHighestValue(values);

            // count values in a row from lowest
            foreach (var value in values)
            {
                // increment lowest value
                lowest++;
                // if it's contained in value list, add to the count
                if (values.Contains(lowest)) // don't worry about going over 6 because it will be caught
                {
                    countFromLowest++;
                }
                else // otherwise, break the loop because the values are no longer in a row
                {
                    break;
                }
            }

            // count values in a row from highest
            foreach (var value in values)
            {
                // decrement highest value
                highest--;
                // if it's contained in value list, add to the count
                if (values.Contains(highest)) // don't worry about going under 1 because it will be caught
                {
                    countFromHighest++;
                }
                else // otherwise, break the loop because the values are no longer in a row
                {
                    break;
                }
            }

            // return the count that is higher - if neither is then it doesn't matter which one
            if (countFromLowest > countFromHighest)
            {
                return countFromLowest;
            }
            else
            {
                return countFromHighest;
            }


        }


    }
}