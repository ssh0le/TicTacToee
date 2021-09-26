using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndNoughts
{
    class Game
    {
        readonly private Menu menu;
        readonly private GameMode gameMode;
        bool startFlag = true;
        private bool isHostMove;
        private bool isOver;
        private GameField.SignMode hostSign;
        private GameField.SignMode currentSign;
        private GameField gameField;
        private const int defaultFieldSize = 3;
        private int fieldSize = defaultFieldSize;
        readonly string[] rivalModes = { "Rival mode: < vs AI >", "Rival mode: < vs another player >" };
        readonly string[] signModes = { "Sign mode: < X >", "Sign mode: < O >", "Sign mode: < random >" };
        readonly string[] firstMoveModes = { "First move: < you >", "First move: < not you >", "First move: < random >" };
        const string startGameLine = "Start game";
        const string changeSizeLine = "Change field size";

        public Game()
        {
            MenuOption startOption = new MenuOption(startGameLine);
            startOption.ShiftToNext = delegate()
            {
                startFlag = true;
            };

            MenuOption changeFieldOption = new MenuOption(changeSizeLine);
            changeFieldOption.ShiftToNext = delegate ()
            {
                SetNewFieldSize();
                DisplayMenu();
            };
            MenuOption rivalOption = new MenuOption(rivalModes);
            rivalOption.ShiftToNext = delegate()
            {
                gameMode.ShiftRivalMode();
            };

            MenuOption signOption = new MenuOption(signModes);
            signOption.ShiftToNext = delegate()
            {
                gameMode.ShiftSignMode();
            };

            MenuOption firstMoveOption = new MenuOption(firstMoveModes);
            firstMoveOption.ShiftToNext = delegate ()
            {
                gameMode.ShiftFirstMoveMode();
            };

            MenuOption[] options = 
            {
                changeFieldOption, 
                rivalOption,
                signOption,
                firstMoveOption,
                startOption,
            };

            menu = new Menu(options, 0, 1);
            gameMode = new GameMode();
            startFlag = false;
        }

        public string GetFieldSizeLine()
        {
            return string.Concat("Field size: ", fieldSize);
        }

        private void SetNewFieldSize()
        {
            Console.Clear();
            Console.WriteLine($"The field size must be odd and be greater or equal 3\nCurrent size: {fieldSize}");
            do
            {
                Console.Write("Enter new field size: ");
                string strSize = Console.ReadLine();
                if (!int.TryParse(strSize, out int newSize))
                {
                    Console.WriteLine("\nWrong format!");
                    continue;
                }
                if (newSize % 2 != 1)
                {
                    Console.WriteLine("\nThis size is not odd!");
                    continue;
                }
                if (newSize < 3)
                {
                    Console.WriteLine("\nThis size is less than 3!");
                    continue;
                }
                fieldSize = newSize;
                break;
            }
            while (true);
        }

        public void Launch()
        {
            DisplayMenu();
            PickOptions();
        }

        public void DisplayMenu()
        {
            Console.Clear();
            Console.Write(GetFieldSizeLine());
            menu.Display();
        }

        public void PickOptions()
        {
            while (true)
            {
                if (startFlag)
                {
                    Start();
                    return;
                }
                switch (GetMenuNavigationKey())
                {
                    case ConsoleKey.UpArrow: menu.GoToPrevious(); break;
                    case ConsoleKey.DownArrow: menu.GoToNext(); break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar: menu.ShiftOption(); break;
                }
            }
        }

        private void Start()
        {
            SetParametrs();
            Console.Clear();
            gameField.Print();
            isOver = false;
            if (!isHostMove && gameMode.RivalMode == GameMode.RivalType.AI)
            {
                SetSignAtRandomCell();
                SwitchCurrentSign();
                SwitchHostMove();
            }

            PrintCurrentResults();
            do
            {
                switch (GetFieldNavigationKey())
                {
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter: ConfirmMove(); break;
                    case ConsoleKey.RightArrow: gameField.SwitchSelectedCell(GameField.Direction.ToRight); break;
                    case ConsoleKey.LeftArrow: gameField.SwitchSelectedCell(GameField.Direction.ToLeft); break;
                    case ConsoleKey.UpArrow: gameField.SwitchSelectedCell(GameField.Direction.ToTop); break;
                    case ConsoleKey.DownArrow: gameField.SwitchSelectedCell(GameField.Direction.ToBottom); break;
                }
            }
            while (!isOver && gameField.CheckEmptyCells() && CheckComeBackCombination() && CheckPotentialCombination());
            PrintFinalResults();
        }

        private void SetParametrs()
        {
            gameField = new GameField(fieldSize, 0, 0);
            SetSign();
            SetFirstMove();
        }

        private int GetRandomNumber(int max)
        {
            Random rnd = new Random();
            return rnd.Next(0, max + 1);
        }

        private void SetSign()
        {
            switch (gameMode.SignMode)
            {
                case GameMode.SignType.Cross: hostSign = GameField.SignMode.Cross; break;
                case GameMode.SignType.Nought: hostSign = GameField.SignMode.Nought; break;
                default: hostSign = (GameField.SignMode)GetRandomNumber(1); break;
            }

            currentSign = hostSign;
        }

        private void SetFirstMove()
        {
            switch (gameMode.FirstMoveMode)
            {
                case GameMode.FirstMoveType.Host: isHostMove = true; break;
                case GameMode.FirstMoveType.NotHost: isHostMove = false; SwitchCurrentSign(); break;
                default: isHostMove = Convert.ToBoolean(GetRandomNumber(1)); break;
            }

            if (!isHostMove)
            {
                SwitchCurrentSign();
            }
        }

        private void SwitchCurrentSign()
        {
            currentSign = currentSign == GameField.SignMode.Cross ? GameField.SignMode.Nought : GameField.SignMode.Cross;
        }

        private void PrintCurrentResults()
        {
            Console.SetCursorPosition(0, fieldSize * (Cell.CellHeight - 1) + 2);
            string player1Sign = ConvertToString(hostSign);
            string player2Sign = ConvertToString(ReverseSign(hostSign));
            Console.WriteLine(string.Concat("Current move: ", isHostMove ? player1Sign : player2Sign));
            int player1Score = gameField.CountCombinations(hostSign);
            int player2Score = gameField.CountCombinations(ReverseSign(hostSign));
            Console.WriteLine(string.Concat("Player 1 (", ConvertToString(hostSign), ") score: ", player1Score));
            Console.WriteLine(string.Concat("Player 2 (", ConvertToString(ReverseSign(hostSign)), ") score: ", player2Score));
        }

        private void PrintFinalResults()
        {
            Console.Clear();
            gameField.Print();
            Console.SetCursorPosition(0, fieldSize * (Cell.CellHeight - 1) + 2);
            int player1Score = gameField.CountCombinations(hostSign);
            int player2Score = gameField.CountCombinations(ReverseSign(hostSign));
            Console.WriteLine(string.Concat("Player 1 (", ConvertToString(hostSign), ") score: ", player1Score));
            Console.WriteLine(string.Concat("Player 2 (", ConvertToString(ReverseSign(hostSign)), ") score: ", player2Score));
            switch (player1Score - player2Score)
            {
                case > 0: Console.WriteLine("\nPlayer 1 won!\n"); break;
                case < 0: Console.WriteLine("\nPlayer 2 won!\n"); break;
                default: Console.WriteLine("\nDraw!\n"); break;
            }
        }

        private bool CheckPotentialCombination()
        {
            if (CombinationCounter.CountPotentialCombinations(hostSign) == 0 || CombinationCounter.CountPotentialCombinations(ReverseSign(hostSign)) == 0)
            {
                return false;
            }

            return true;
        }

        private bool CheckComeBackCombination()
        {
            if (gameField.CountCombinations(hostSign) - gameField.CountCombinations(ReverseSign(hostSign)) > CombinationCounter.CountPotentialCombinations(ReverseSign(hostSign)))
            {
                return false;
            }
            if (gameField.CountCombinations(ReverseSign(hostSign)) - gameField.CountCombinations(hostSign) > CombinationCounter.CountPotentialCombinations(hostSign))
            {
                return false;
            }
            return true;
        }

        private string ConvertToString(GameField.SignMode sign) => sign == GameField.SignMode.Cross ? "X" : "O";

        private GameField.SignMode ReverseSign(GameField.SignMode sign) => sign == GameField.SignMode.Cross ? GameField.SignMode.Nought : GameField.SignMode.Cross;

        private void SwitchHostMove() => isHostMove = !isHostMove;

        private void ConfirmMove()
        {
            gameField.SetSign(currentSign, out bool isSuccess);
            if (isSuccess && gameField.CheckEmptyCells())
            {
                SwitchCurrentSign();
            }
            else
            {
                return;
            }

            if (!CheckComeBackCombination() || !CheckPotentialCombination())
            {
                isOver = true;
                return;
            }
            
            if (gameMode.RivalMode == GameMode.RivalType.AI)
            {
                SetSignAtRandomCell();
                SwitchCurrentSign();
            }

            SwitchHostMove();
            PrintCurrentResults();
        }

        private void SetSignAtRandomCell()
        {
            do
            {
                int leftPos = GetRandomNumber(fieldSize - 1);
                int topPos = GetRandomNumber(fieldSize - 1);
                for (int i = leftPos; i < fieldSize; i++)
                {
                    for (int j = topPos; j < fieldSize; j++)
                    {
                        if (gameField.CellMatrix[i, j].type == Cell.CellType.Empty)
                        {
                            gameField.SwitchSelectedCell(i, j);
                            gameField.SetSign(currentSign, out bool isSuccessful);
                            if (isSuccessful)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            while (true);
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

        public static ConsoleKey GetFieldNavigationKey()
        {
            do
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow: return pressedKey.Key;
                }
            }
            while (true);
        }
    }
}
