using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndNoughts
{
    static class CombinationCounter
    {
        public static Cell[,] CellMatrix;
        private static int size;

        public static void SetCellMatrix(Cell[,] cellMatrix)
        {
            CellMatrix = cellMatrix;
            size = cellMatrix.GetLength(0);
        }

        public static int CountCombinations(GameField.SignMode sign)
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

        private static void ClearCellCombinations()
        {
            foreach (Cell cell in CellMatrix)
            {
                cell.ClearCombinations();
            }
        }

        private static int CountVerticalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
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

        private static int CountHorizontalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
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

        private static int CountDiagonalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
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

        private static int CountReverseDiagonalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
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

        private static Cell.CellType ConvertToCellType(GameField.SignMode sign)
        {
            if (sign == GameField.SignMode.Cross)
            {
                return Cell.CellType.WithCross;
            }
            else
            {
                return Cell.CellType.WithNought;
            }
        }
        private static int CountPotentialVerticalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
        {
            int count = 0;
            for (int j = 0; j < 3; j++)
            {
                bool isCombination = true;
                for (int i = 0; i < 3; i++)
                {
                    if (CellMatrix[leftPos + i, topPos + j].IsInVerticalCombination)
                    {
                        isCombination = false;
                        break;
                    }
                    if (CellMatrix[leftPos + i, topPos + j].type == type || CellMatrix[leftPos + i, topPos + j].type == Cell.CellType.Empty)
                    {
                        continue;
                    }
                    isCombination = false;
                    break;
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

        private static int CountPotentialHorizontalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                bool isCombination = true;
                for (int j = 0; j < 3; j++)
                {
                    if (CellMatrix[leftPos + i, topPos + j].IsInHorizontalCombination)
                    {
                        isCombination = false;
                        break;
                    }
                    if (CellMatrix[leftPos + i, topPos + j].type == type || CellMatrix[leftPos + i, topPos + j].type == Cell.CellType.Empty)
                    {
                        continue;
                    }
                    isCombination = false;
                    break;

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

        private static int CountPotentialDiagonalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
        {
            int count = 0;
            bool isCombination = true;
            for (int i = 0; i < 3; i++)
            {
                if (CellMatrix[leftPos + i, topPos + i].IsInDiagonalCombination)
                {
                    isCombination = false;
                    break;
                }
                if (CellMatrix[leftPos + i, topPos + i].type == type || CellMatrix[leftPos + i, topPos + i].type == Cell.CellType.Empty)
                {
                    continue;
                }
                isCombination = false;
                break;
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

        private static int CountPotentialReverseDiagonalCombinationIn3x3Matrix(int leftPos, int topPos, Cell.CellType type)
        {
            int count = 0;
            bool isCombination = true;
            for (int i = 2; i >= 0; i--)
            {
                if (CellMatrix[leftPos + i, topPos + 2 - i].IsInReverseDiagonalCombination)
                {
                    isCombination = false;
                    break;
                }
                if (CellMatrix[leftPos + i, topPos + 2 - i].type == type || CellMatrix[leftPos + i, topPos + 2 - i].type == Cell.CellType.Empty)
                {
                    continue;
                }
                isCombination = false;
                break;
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

        public static int CountPotentialCombinations(GameField.SignMode sign)
        {
            CountCombinations(sign);
            int count = 0;
            for (int j = 0; j < size - 2; j++)
            {
                for (int i = 0; i < size - 2; i++)
                {
                    count += CountPotentialHorizontalCombinationIn3x3Matrix(i, j, ConvertToCellType(sign));
                    count += CountPotentialVerticalCombinationIn3x3Matrix(i, j, ConvertToCellType(sign));
                    count += CountPotentialDiagonalCombinationIn3x3Matrix(i, j, ConvertToCellType(sign));
                    count += CountPotentialReverseDiagonalCombinationIn3x3Matrix(i, j, ConvertToCellType(sign));
                }
            }
            return count;
        }
    }
}
