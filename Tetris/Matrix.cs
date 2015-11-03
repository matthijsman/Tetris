using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Matrix
    {
        private int[,] _matrix;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public double Score { get; set; }
        public bool IsValid { get; set; }
        public int RemovedLines { get; set; }

        public int SolidLines{get; set;}

        public Matrix(int width, int height)
        {
            IsValid = true;
            _matrix = new int[width, height];
            Width = width;
            Height = height;
        }

        public Matrix(Matrix matrix)
        {
            IsValid = true;
            CopyMatrix(matrix);
        }

        public Matrix(Matrix baseMatrix, Matrix blockMatrix, int xpos, int ypos)
        {
            IsValid = true;
            CopyMatrix(baseMatrix);
            //Normalize();
            Add(blockMatrix, xpos, ypos, baseMatrix.RemovedLines);
        }

        public Matrix(string fieldString)
        {
            SolidLines = 0;
            IsValid = true;
            var fieldArray = fieldString.Split(';');
            Height = fieldArray.Length;
            Width = fieldArray[0].Split(',').Length;
            _matrix = new int[Width, Height];
            
            for (int y = 0; y < Height; y++)
            {
                var lineArray = fieldArray[y].Split(',');
                if(int.Parse(lineArray[0].ToString()) == 3)
                {
                    SolidLines++;
                }

                for (int x = 0; x < Width; x++)
                {
                    _matrix[x, y] = int.Parse(lineArray[x].ToString()) > 1 ? 1 : 0;
                }

            }
        }

        private void CopyMatrix(Matrix matrix)
        {
            _matrix = new int[matrix.Width, matrix.Height];
            var height = matrix.Height;
            var width = matrix.Width;
            Height = height;
            Width = width;
            SolidLines = matrix.SolidLines;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _matrix[x, y] = matrix[x, y] != 0 ? 1 : 0;
                }

            }
        }



        public void Normalize()
        {
            var height = Height;
            var width = Width;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _matrix[x, y] = _matrix[x, y] == 0 ? 0 : 1;
                }
            }
        }

        public int this[int x, int y]
        {
            get
            {
                return _matrix[x, y];
            }
        }

        public void Add(Matrix matrix, int xPos, int yPos, int baseRemovedLines)
        {
            var landingHeight = Height - yPos + (matrix.Height / 2);
            for (int x = 0; x < matrix.Width; x++)
            {
                for (int y = 0; y < matrix.Height; y++)
                {
                    var finalX = x + xPos;
                    var finalY = y + yPos;
                    if (finalX < 0 && matrix[x, y] != 0)
                    {
                        IsValid = false;
                        return;
                    }
                    if (finalX >= Width && matrix[x, y] != 0)
                    {
                        IsValid = false;
                        return;
                    }
                    if (finalY >= Height && matrix[x, y] != 0)
                    {
                        IsValid = false;
                        return;
                    }

                    if (finalY < 0 && matrix[x, y] != 0)
                    {
                        IsValid = false;
                        return;
                    }

                    if (matrix[x, y] != 0 && finalX >= 0 && finalX < Width && finalY >= 0 && finalY < Height)
                    {
                        if (_matrix[finalX, finalY] != 0)
                        {
                            IsValid = false;
                            return;
                        }
                        _matrix[finalX, finalY]++;
                    }

                }
            }
            CalculateScore(landingHeight, baseRemovedLines);
        }

        private void CalculateScore(int landingHeight, int baseRemovedLines)
        {
            int rowTransitions = 0;
            int columnTransitions = 0;
            int numberOfHoles = 0;

            int removedLines = RemoveFullLines();
            RemovedLines = removedLines;
            int wellSums = GetWellSums();


            var height = Height;
            var width = Width;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var curval = _matrix[x, y];
                    if (y > 0 && curval == 0 && _matrix[x, y - 1] != 0)
                    {
                        numberOfHoles++;
                    }
                    if (x > 0 && curval != _matrix[x - 1, y])
                    {
                        rowTransitions++;
                    }
                    if (y > 0 && curval != _matrix[x, y - 1])
                    {
                        columnTransitions++;
                    }
                }
            }

            Score = landingHeight * -4.500158825082766 +
                GetFactor((baseRemovedLines!=0?baseRemovedLines:SolidLines)-SolidLines) * 3.4181268101392694 +
                GetFactor(removedLines-SolidLines) * 3.4181268101392694 +
                rowTransitions * -3.2178882868487753 +
                columnTransitions * -9.348695305445199 +
                numberOfHoles * -7.899265427351652 +
                wellSums * -3.3855972247263626;
        }

        private int GetWellSums()
        {
            var sum = 0;
            var height = Height;
            var width = Width;
            int wellDepth = 0;
            for (int x = 1; x < width - 1; x++)
            {
                wellDepth = 0;
                for (int y = 1; y < height; y++)
                {
                    if (_matrix[x, y] == 0 && _matrix[x - 1, y] == 1 && _matrix[x + 1, y] == 1)
                    {
                        wellDepth++;
                        sum += wellDepth;
                    }
                    if (_matrix[x, y] == 1)
                    {
                        wellDepth = 0;
                    }
                }
            }

            wellDepth = 0;
            for (int y = 1; y < height; y++)
            {
                if (_matrix[0, y] == 0 && _matrix[1, y] == 1)
                {
                    wellDepth++;
                    sum += wellDepth;
                }
                if (_matrix[0, y] == 1)
                {
                    wellDepth = 0;
                }
            }

            wellDepth = 0;
            for (int y = 1; y < height; y++)
            {
                if (_matrix[width - 1, y] == 0 && _matrix[width - 2, y] == 1)
                {
                    wellDepth++;
                    sum += wellDepth;
                }
                if (_matrix[width - 1, y] == 1)
                {
                    wellDepth = 0;
                }
            }

            return sum;
        }

        public bool CanAdd(Matrix block, int x, int y)
        {
            var height = block.Height;
            var width = block.Width;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {

                    //if ((x + i) >= 0 && (x + i) < 10 && (y + j) >= 0 && (y + j) < 20)
                    //{

                    if (block._matrix[i, j] != 0 && ((y + j) > 19 || (x + i) > 9 || _matrix[x + i, y + j] != 0))
                    {
                        return false;
                    }
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
            }
            return true;
        }

        public override string ToString()
        {
            var height = Height;
            var width = Width;
            string returnString = "";
            for (int y = 0; y < height; y++)
            {
                returnString += "|";
                for (int x = 0; x < width; x++)
                {
                    returnString += _matrix[x, y] == 0 ? (y == height - 1 ? "___" : "   ") : "[X]";
                }
                returnString += "|" + y + Environment.NewLine;
            }
            return returnString;
        }

        internal int RemoveFullLines()
        {
            int clearedLines = 0;
            var height = Height;
            var width = Width;
            for (int y = 0; y < height; y++)
            {
                bool lineFull = true;
                for (int x = 0; x < width; x++)
                {
                    if (_matrix[x, y] == 0)
                    {
                        lineFull = false;
                        break;
                    }
                }
                if (lineFull)
                {
                    clearedLines++;
                    for (int suby = y; suby > 0; suby--)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            _matrix[x, suby] = _matrix[x, suby - 1];
                        }
                    }
                }
            }
            return clearedLines;
        }

        internal int GetFactor(int clearedLines)
        {
            switch (clearedLines)
            {
                case 0:
                    return 0;
                default:
                    return (int)Math.Pow(2.0, (double)clearedLines-1);
            }
        }
    }
}
