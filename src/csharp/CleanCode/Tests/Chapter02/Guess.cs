using System;

namespace Tests.Chapter02
{
    public class Guess
    {
        private readonly Action<string> _printAction;

        public Guess(
            Action<string> printAction)
        {
            _printAction = printAction;
        }

        public void PrintGuessStatistics(
            string candidate,
            int count)
        {
            string number;
            string verb;
            string pluralModifier;
            if (count == 0)
            {
                number = "no";
                verb = "are";
                pluralModifier = "s";
            }
            else if (count == 1)
            {
                number = "1";
                verb = "is";
                pluralModifier = "";
            }
            else
            {
                number = count.ToString();
                verb = "are";
                pluralModifier = "s";
            }

            string guessMessage = string.Format("There {0} {1} {2}{3}", verb, number, candidate, pluralModifier);

            _printAction(guessMessage);
        }
    }
}