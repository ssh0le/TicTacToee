using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndNoughts
{
    class GameField
    {
        private int currentTopPos;
        private int currentLeftPos;
        readonly private int size;
        public Cell[,] CellMatrix;

        public enum SignMode
        {
            Cross,
            Nought
        }

        public enum Direction
        {
            ToTop,
            ToBottom,
            ToLeft,
            ToRight
        }

        public GameField(int size, int topPos = 0, int leftPos = 0)
        {
            this.size = size;
            currentLeftPos = leftPos;
            currentTopPos = topPos;
            FillCellMatrix();
            CombinationCounter.SetCellMatrix(CellMatrix);
        }

        public void SwitchSelectedCell(Direction direction)
        {
            if (IsOutOfBounds(direction))
            {
                return;
            }
            
            CellMatrix[currentLeftPos, currentTopPos].Print(false);
            switch (direction) 
            {
                case Direction.ToBottom: currentTopPos++; break;
                case Direction.ToTop: currentTopPos--; break;
                case Direction.ToLeft: currentLeftPos--; break;
                case Direction.ToRight: currentLeftPos++; break;
            }

            CellMatrix[currentLeftPos, currentTopPos].Print(true);
        }

        public void SwitchSelectedCell(int leftPos, int topPos)
        {
            CellMatrix[currentLeftPos, currentTopPos].Print(false);
            if (leftPos >= size || topPos >= size || leftPos < 0 || topPos < 0)
            {
                return;
            }

            currentLeftPos = leftPos;
            currentTopPos = topPos;
            CellMatrix[currentLeftPos, currentTopPos].Print(true);
        }

        private bool IsOutOfBounds(Direction direction) => direction switch
        {
            Direction.ToBottom => currentTopPos + 1 >= size,
            Direction.ToTop => currentTopPos - 1 < 0,
            Direction.ToLeft => currentLeftPos - 1 < 0,
            Direction.ToRight => currentLeftPos + 1 >= size
        };

        private void FillCellMatrix()
        {
            CellMatrix = new Cell[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    CellMatrix[i, j] = new Cell(i, j);
                }
            }
        }

        public void SetSign(SignMode sign, out bool isSuccessful)
        {
            CellMatrix[currentLeftPos, currentTopPos].SwitchType(ConvertToCellType(sign), out isSuccessful);
        }

        private void ClearCellCombinations()
        {
            foreach (Cell cell in CellMatrix)
            {
                cell.ClearCombinations();
            }
        }

        public int CountCombinations(SignMode sign)
        {
            ClearCellCombinations();
            int count = 0;
            for (int j = 0; j < size - 2; j++)
            {
                for (int i = 0; i < size - 2; i++)
                {
                    count += CountHorizontalCombinationIn3x3Matrix(i, j, ConvertToCellType(sign));
                    count += CountVerticalCombinationIn3x3Matrix(i, j, ConvertToCellType(sign));
                    count += CountDiagonalCombinationIn3x3Matrix(i, j, ConvertToCellType(sign));
                    count += CountReverseDiagonalCombinationIn3x3Matrix(i, j, ConvertToCellType(sign));
                }
            }
            return count;
        }

        private int CountVerticalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
        {
            int count = 0;
            for (int j = 0; j < 3; j++)
            {
                bool isCombination = true;
                for (int i = 0; i < 3; i++)
                {
                    if (CellMatrix[leftPos + i, topPos + j].type != type || CellMatrix[leftPos + i, topPos + j].IsInVerticalCombination)
                    {
                        isCombination = false;
                        break;
                    }
                }
                if (isCombination)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        CellMatrix[leftPos + i, topPos + j].IsInVerticalCombination = true;
                    }
                    count++;
                }
            }
            return count;
        }

        private int CountHorizontalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                bool isCombination = true;
                for (int j = 0; j < 3; j++)
                {
                    if (CellMatrix[leftPos + i, topPos + j].type != type || CellMatrix[leftPos + i, topPos + j].IsInHorizontalCombination)
                    {
                        isCombination = false;
                        break;
                    }

                }
                if (isCombination)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        CellMatrix[leftPos + i, topPos + j].IsInHorizontalCombination = true;
                    }
                    count++;
                }
            }
            return count;
        }

        private int CountDiagonalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
        {
            int count = 0;
            bool isCombination = true;
            for (int i = 0; i < 3; i++)
            {
                if (CellMatrix[leftPos + i, topPos + i].type != type || CellMatrix[leftPos + i, topPos + i].IsInDiagonalCombination)
                {
                    isCombination = false;
                    break;
                }
            }
            if (isCombination)
            {
                for (int i = 0; i < 3; i++)
                {
                    CellMatrix[leftPos + i, topPos + i].IsInDiagonalCombination = true;
                }
                count++;
            }
            return count;
        }

        private int CountReverseDiagonalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
        {
            int count = 0;
            bool isCombination = true;
            for (int i = 2; i >= 0; i--)
            {
                if (CellMatrix[leftPos + i, topPos + 2 - i].type != type || CellMatrix[leftPos + i, topPos + i].IsInReverseDiagonalCombination)
                {
                    isCombination = false;
                    break;
                }

            }
            if (isCombination)
            {
                for (int i = 2; i >= 0; i--)
                {
                    CellMatrix[leftPos + i, topPos + i].IsInReverseDiagonalCombination = true;
                }
                count++;
            }
            return count;
        }

        private Cell.CellType ConvertToCellType(SignMode sign)
        {
            if (sign == SignMode.Cross)
            {
                return Cell.CellType.WithCross;
            }
            else
            {
                return Cell.CellType.WithNought;
            }
        }

        public bool CheckEmptyCells()
        {
            foreach (Cell cell in CellMatrix)
            {
                if (cell.type == Cell.CellType.Empty)
                {
                    return true;
                }
            }
            return false;
        }

        public void Print()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    CellMatrix[i, j].Print(false);
                }
            }

            CellMatrix[currentLeftPos, currentTopPos].Print(true);
        }
    }

    class Cell
    {
        public const int CellHeight = 3;
        public const int CellWeight = 5;
        const string Border = "+ - +";
        const string Middle = "|   |";
        const string MiddleWithCross = "| X |";
        const string MiddleWithNought = "| O |";
        private const ConsoleColor SelectedCellColor = ConsoleColor.DarkGreen;
        public CellType type;
        public int LeftPos;
        public int TopPos;
        public bool IsInVerticalCombination = false;
        public bool IsInHorizontalCombination = false;
        public bool IsInDiagonalCombination = false;
        public bool IsInReverseDiagonalCombination = false;
        public Cell(int leftPos, int topPos)
        {
            LeftPos = leftPos;
            TopPos = topPos;
            type = CellType.Empty;
        }

        public void ClearCombinations()
        {
            IsInVerticalCombination = false;
            IsInHorizontalCombination = false;
            IsInDiagonalCombination = false;
            IsInReverseDiagonalCombination = false;
        }

        public enum CellType
        {
            Empty,
            WithCross,
            WithNought
        }

        public void SwitchType(CellType type, out bool isSuccess)
        {
            isSuccess = false;
            if (this.type != CellType.Empty)
            {
                return;
            }
            this.type = type;
            isSuccess = true;
            Print(true);
        }

        public void Print(bool isSelected)
        {
            if (isSelected)
            {
                Console.ForegroundColor = SelectedCellColor;
            }
            Console.SetCursorPosition(LeftPos * (Cell.CellWeight - 1), TopPos * (Cell.CellHeight - 1));
            Console.Write(Border);
            Console.SetCursorPosition(LeftPos * (Cell.CellWeight - 1), TopPos * (Cell.CellHeight - 1) + 1);
            switch (type)
            {
                case CellType.Empty: Console.Write(Middle); break;
                case CellType.WithCross: Console.Write(MiddleWithCross); break;
                case CellType.WithNought: Console.Write(MiddleWithNought); break;
            }

            Console.SetCursorPosition(LeftPos * (Cell.CellWeight - 1), TopPos * (Cell.CellHeight - 1) + 2);
            Console.Write(Border);
            Console.ResetColor();
        }
    }
}
