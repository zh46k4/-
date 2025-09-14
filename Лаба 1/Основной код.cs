
using System;
using System.Data.Common;
using System.Xml.Linq;

namespace FirstProgram
{

    class Program
    {
        
        static int[,] cratematrix(int cols ,int rows, int [,]matrix) {

            Random rnd = new Random();
            

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = rnd.Next(0, 10);
                }
            }
            return matrix;

        }

        static void PrintMatrix(int[,] matrix, int rows, int cols)
        {
            Console.WriteLine("Двумерный массив:");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static void Differenceofelements(int[,] matrix) {
            int max = int.MinValue, min = int.MaxValue;
            foreach (int i in matrix) {
                if (i > max) max = i;
                if (i < min) min = i;
            }
            int differense = max - min;
            Console.WriteLine(differense);
            
        }

        static void countbychoice(int[,] matrix , int cols, int rows) {

            Console.WriteLine("Выберите что считать, для ситолбцов введите j, для строк i");
            string choice = Console.ReadLine();
            Console.WriteLine("Выберите номер (строки или столбца)");
            int num = Convert.ToInt32(Console.ReadLine());

            int sum = 0;

            switch (choice) {
                case "i":
                        if (num < rows)
                        {
                            for (int i = 0; i < matrix.GetLength(0); i++)
                            {
                                for (int j = 0; j < matrix.GetLength(1); j++)
                                {
                                    if (i == num) sum += matrix[i, j];
                                }
                            }
                            Console.WriteLine(sum);
                        }
                        else Console.WriteLine("Неверный ввод номера столбца");
                        break;
                case "j":
                        if (num < cols)
                        {
                            for (int i = 0; i < matrix.GetLength(0); i++)
                            {
                                for (int j = 0; j < matrix.GetLength(1); j++)
                                {
                                    if (j == num) sum += matrix[i, j];
                                }
                            }
                            Console.WriteLine(sum);
                        }
                        else Console.WriteLine("Неверный ввод номера столбца");
                    break;
                default:
                    Console.WriteLine("Вы ввели явно не i и не j");
                    break;
            }   
            
        }
            static void Main()
        {
            

            Console.WriteLine("Введите размер массива (столбцы)");
            int cols = Convert.ToInt32(Console.ReadLine());
            
            Console.WriteLine("Введите размер массива (строки)");
            int rows = Convert.ToInt32(Console.ReadLine());
            

            int[,] matrix = new int[rows, cols];

            cratematrix(cols, rows, matrix);
            PrintMatrix(matrix, rows, cols);

            Differenceofelements(matrix);

            countbychoice(matrix , cols, rows);



            Console.ReadKey();
        }
    }
}