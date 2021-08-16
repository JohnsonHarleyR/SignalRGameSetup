using System;

namespace SignalRGameSetup.Models.Game
{
    public class Die
    {
        public int ID { get; set; }
        public string GameCode { get; set; }

        private int _value = 1; // default is 1
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                // only allow the value to be set from 1 to 6
                if (value >= 1 && value <= 6)
                {
                    _value = value;

                    //switch (_value) // set the value of the class according to the value
                    //{
                    //    default:
                    //    case 1:
                    //        ClassValue = "die-value-1";
                    //        break;
                    //    case 2:
                    //        ClassValue = "die-value-2";
                    //        break;
                    //    case 3:
                    //        ClassValue = "die-value-3";
                    //        break;
                    //    case 4:
                    //        ClassValue = "die-value-4";
                    //        break;
                    //    case 5:
                    //        ClassValue = "die-value-5";
                    //        break;
                    //    case 6:
                    //        ClassValue = "die-value-6";
                    //        break;
                    //}

                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }


            }
        }

        //public string ClassValue { get; set; }

        public bool Selected { get; set; } = false;
        public bool Animate { get; set; } = false;

        public Die() { }

        public Die(int dieNumber) // get which die it is - 1 through 5 - to creat the ID
        {
            ID = dieNumber;
        }

        public Die(int dieNumber, int value) // get which die it is - 1 through 5 - to creat the ID
        {
            ID = dieNumber;
            Value = value;
        }
    }
}