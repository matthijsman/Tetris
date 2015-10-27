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

        public bool IsValid { get; set; }

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
            Add(blockMatrix, xpos, ypos);
        }

        public Matrix(string fieldString)
        {
            IsValid = true;
            var fieldArray = fieldString.Split(';');
            Height = fieldArray.Length;
            Width = fieldArray[0].Split(',').Length;
            _matrix = new int[Width, Height];
            for (int y = 0; y < Height; y++)
            {
                var lineArray = fieldArray[y].Split(',');
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

        public void Add(Matrix matrix, int xPos, int yPos)
        {
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
                    
                        if (block._matrix[i, j] != 0 && ((y+j) > 19 || (x+i) > 9 ||_matrix[x + i, y + j] != 0))
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

        internal void RemoveFullLines()
        {
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
                    for (int suby = y; suby > 0; suby--)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            _matrix[x, suby] = _matrix[x, suby - 1];
                        }
                    }
                }
            }
        }
    }
}
