using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndNoughts
{
    internal class Menu
    {
        private readonly MenuOption[] menuOptions;
        private const int DefaultPosition = 0;
        private int currentPosition;
        private readonly int leftPos = 0;
        private readonly int topPos = 0;

        private const string SelectedPrefix = " >> ";
        private const string UnselectedPrefix = "    ";

        public enum ShiftDirection
        {
            ToNext,
            ToPrevious,
        }

        public Menu(MenuOption[] options, int leftPos, int topPos)
        {
            menuOptions = options;
            this.leftPos = leftPos;
            this.topPos = topPos;
            currentPosition = DefaultPosition;
        }

        public void Display()
        {
            Console.SetCursorPosition(leftPos, topPos);
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.Write(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(leftPos, topPos);
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.WriteLine(string.Concat(UnselectedPrefix, menuOptions[i].GetOption()));
            }
            currentPosition = DefaultPosition;
            PrintOption(true, menuOptions[currentPosition].GetOption());
        }

        public void GoToNext()
        {
            PrintOption(false, menuOptions[currentPosition].GetOption());
            if (currentPosition + 1 != menuOptions.Length)
            {   
                currentPosition++;
            }
            else
            {
                currentPosition = DefaultPosition;
            }
            PrintOption(true, menuOptions[currentPosition].GetOption());
        }

        public void GoToPrevious()
        {
            PrintOption(false, menuOptions[currentPosition].GetOption());
            if (currentPosition == DefaultPosition)
            {
                currentPosition = menuOptions.Length - 1;
            }
            else
            {
                currentPosition--;
            }
            PrintOption(true, menuOptions[currentPosition].GetOption());
        }

        public void ShiftOption()
        {
            menuOptions[currentPosition].GoToNext();
            PrintOption(true, menuOptions[currentPosition].GetOption());
        }

        private void PrintOption(bool isSelected, string option)
        {
            Console.SetCursorPosition(leftPos, topPos + currentPosition);
            if (isSelected)
            {
                Console.Write(new string(' ', Console.BufferWidth));
                Console.SetCursorPosition(leftPos, topPos + currentPosition);
                Console.Write(string.Concat(SelectedPrefix, option, '\n'));
            }
            else
            {
                Console.Write(string.Concat(UnselectedPrefix, option, '\n'));
            }
        }
    }

    internal class MenuOption
    {
        private string[] options;
        private int currentPosition;
        private const int defaultPosition = 0;
        public delegate void ShiftHandler();
        public ShiftHandler ShiftToNext;

        public MenuOption(params string[] options)
        {
            this.options = options;
            currentPosition = defaultPosition;
        }

        public void SetNewOption(params string[] options)
        {
            this.options = options;
            currentPosition = defaultPosition;
        }

        public string GetOption()
        {
            return options[currentPosition];
        }

        public void GoToNext()
        {
            if (options.Length == 1)
            {
                ShiftToNext?.Invoke();
                return;
            }

            if (currentPosition + 1 != options.Length)
            {
                currentPosition++;
            }
            else
            {
                currentPosition = defaultPosition;
            }
            ShiftToNext?.Invoke();
        }
    }
}
