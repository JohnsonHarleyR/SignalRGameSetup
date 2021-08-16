﻿namespace SignalRGameSetup.Models.Game
{
    public class ScoreSheet
    {
        public int? Aces { get; set; }
        public int? Twos { get; set; }
        public int? Threes { get; set; }
        public int? Fours { get; set; }
        public int? Fives { get; set; }
        public int? Sixes { get; set; }

        public int? UpperTotalBeforeBonus { get; set; }
        public int? UpperBonus { get; set; }
        public int? UpperTotalFinal { get; set; }

        public int? ThreeOfAKind { get; set; }
        public int? FourOfAKind { get; set; }
        public int? FullHouse { get; set; }
        public int? SmallStraight { get; set; }
        public int? LargeStraight { get; set; }
        public int? Yahtzee { get; set; }
        public int? Chance { get; set; }
        public int? YahtzeeBonusCount { get; set; } = 0;
        public int? YahtzeeBonus { get; set; }

        public int? LowerTotal { get; set; }

        public int? GrandTotal { get; set; }

        public bool IsComplete { get; set; } = false;
    }
}