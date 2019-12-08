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
            var guessMessage = new GuessMessageCreator()
                .Create(candidate, count);

            _printAction(guessMessage);
        }
    }

    public class GuessMessageCreator
    {
        private string _number;
        private string _verb;
        private string _pluralModifier;

        public string Create(
            string itemName,
            int itemCount)
        {
            if (itemCount == 0)
                SetupNoItemsMessage();
            else if (itemCount == 1)
                SetupSingleItemMessage();
            else
                SetupManyItemsMessage(itemCount);

            return GetMessage(itemName);
        }

        private string GetMessage(string itemName)
        {
            return $"There {_verb} {_number} {itemName}{_pluralModifier}";
        }

        private void SetupManyItemsMessage(
            int itemCount)
        {
            _number = itemCount.ToString();
            _verb = "are";
            _pluralModifier = "s";
        }

        private void SetupSingleItemMessage()
        {
            _number = "1";
            _verb = "is";
            _pluralModifier = "";
        }

        private void SetupNoItemsMessage()
        {
            _number = "no";
            _verb = "are";
            _pluralModifier = "s";
        }
    }
}