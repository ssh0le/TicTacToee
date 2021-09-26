using System;

namespace CrossesAndNoughts
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            bool isContinue = true;
            do
            {
                Game game = new Game();
                game.Launch();
                bool menuExit = false;
                MenuOption yesAnswer = new MenuOption("Yes");
                yesAnswer.ShiftToNext = delegate ()
                {
                    menuExit = true;
                };

                MenuOption noAnswer = new MenuOption("No");
                noAnswer.ShiftToNext = delegate ()
                {
                    isContinue = false;
                    menuExit = true;
                };

                MenuOption[] answers =
                {
                    yesAnswer,
                    noAnswer
                };
                Console.WriteLine("One more game?");
                Menu restartMenu = new Menu(answers, Console.CursorLeft, Console.CursorTop);
                restartMenu.Display();
                while (!menuExit)
                {
                    switch (GetMenuNavigationKey())
                    {
                        case ConsoleKey.UpArrow: restartMenu.GoToPrevious(); break;
                        case ConsoleKey.DownArrow: restartMenu.GoToNext(); break;
                        case ConsoleKey.Enter:
                        case ConsoleKey.Spacebar: restartMenu.ShiftOption(); break;
                    }
                }
            }
            while (isContinue);
        }

        public static ConsoleKey GetMenuNavigationKey()
        {
            do
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.Enter:
                    case ConsoleKey.DownArrow: return pressedKey.Key;
                }
            }
            while (true);
        }
    }
}
