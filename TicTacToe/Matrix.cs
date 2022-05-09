using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    static class Matrix
    {
        private static readonly Random _rand;

        static Matrix()
        {
            _rand = new Random();
        }

        public static double Randn()
        {
            double u1 = 1.0 - _rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - _rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

            return randStdNormal;
        }

        public static double[] Randn(int length)
        {
            double[] r = new double[length];
            for (int i = 0; i < length; i++)
            {
                r[i] = Randn();
            }

            return r;
        }

        public static double[,] Randn(int length, int length2)
        {
            double[,] r = new double[length, length2];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length2; j++)
                {
                    r[i, j] = Randn();
                }
            }

            return r;
        }

        public static double Mean(double[] matrix)
        {
            return matrix.Sum() / matrix.Length;
        }

        public static double Std(double[] matrix)
        {
            var mean = Mean(matrix);

            return Math.Sqrt(matrix.Select(i => Math.Pow(i - mean, 2)).Sum() / matrix.Length);
        }

        public static double[] Minus(double[] matrix, double value)
        {
            return matrix.Select(v => v - value).ToArray();
        }

        public static double[] Plus(double[] matrix, double value)
        {
            return matrix.Select(v => v + value).ToArray();
        }
        public static double[,] Plus(double[,] matrix1, double[,] matrix2)
        {
            if(matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
            {
                throw new ArgumentException("matrices are not same size");
            }

            var result = new double[matrix1.GetLength(0),matrix2.GetLength(1)];

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    result[i,j] = matrix1[i,j] + matrix2[i,j];
                }
            }

            return result;
        }


        public static double[] Divide(double[] matrix, double value)
        {
            return matrix.Select(v => v / value).ToArray();
        }

        public static double[] Dot(double[] matrix, double value)
        {
            return matrix.Select(v => v * value).ToArray();
        }

        public static double[,] Dot(double[,] matrix, double value)
        {
            var result = new double[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i,j] = matrix[i, j] * value;
                }
            }

            return result;
        }

        public static double[,] Divide(double[,] matrix, double value)
        {
            var result = new double[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = matrix[i, j] / value;
                }
            }

            return result;
        }

        public static double[,] Transpose(double[,] matrix)
        {
            var result = new double[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }
    }
}
