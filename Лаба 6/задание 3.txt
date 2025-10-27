using System;

class Laba6
{
    public static int[,] CreateMatr(int size)
    {
        int[,] G = new int[size, size];
        Random rnd = new Random();
        for (int i = 0; i < size; i++)
        {
            for (int j = i; j < size; j++)
            {
                if (i == j)
                {
                    G[i, j] = 0;
                }
                else
                {
                    int value = rnd.Next(0, 2);
                    G[i, j] = value;
                    G[j, i] = value;
                }
            }
        }

        Print(G);
        return G;
    }

    public static void Print(int[,] matr)
    {
        for (int i = 0; i < matr.GetLength(0); i++)
        {
            for (int j = 0; j < matr.GetLength(1); j++)
            {
                Console.Write(matr[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public static int[,] Matr1Matr2A(int[,] G1, int[,] G2)
    {
        Console.WriteLine("Объединение:");

        int maxSize = Math.Max(G1.GetLength(0), G2.GetLength(0));
        int minSize = Math.Min(G1.GetLength(0), G2.GetLength(0));

        int[,] resultMatrix = new int[maxSize, maxSize];

        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                if (i < minSize && j < minSize)
                {
                    resultMatrix[i, j] = (G1[i, j] == 1 || G2[i, j] == 1) ? 1 : 0;
                }
                else if (i < G1.GetLength(0) && j < G1.GetLength(0))
                {
                    resultMatrix[i, j] = G1[i, j];
                }
                else if (i < G2.GetLength(0) && j < G2.GetLength(0))
                {
                    resultMatrix[i, j] = G2[i, j];
                }
                else
                {
                    resultMatrix[i, j] = 0;
                }
            }
        }
        Print(resultMatrix);
        return resultMatrix;
    }

    public static int[,] Matr1Matr2B(int[,] G1, int[,] G2)
    {
        Console.WriteLine("Пересечение:");


        int minSize = Math.Min(G1.GetLength(0), G2.GetLength(0));

        int[,] resultMatrix = new int[minSize, minSize];

        for (int i = 0; i < minSize; i++)
        {
            for (int j = 0; j < minSize; j++)
            {
                if (G1[i, j] == 1 && G2[i, j] == 1)
                {
                    resultMatrix[i, j] = 1;
                }
                else
                {
                    resultMatrix[i, j] = 0;
                }
            }
        }
        Print(resultMatrix);
        return resultMatrix;
    }

    public static int[,] Matr1Matr2C(int[,] G1, int[,] G2)
    {
        Console.WriteLine("Кольцевая сумма:");

        int maxSize = Math.Max(G1.GetLength(0), G2.GetLength(0));
        int minSize = Math.Min(G1.GetLength(0), G2.GetLength(0));

        int[,] resultMatrix = new int[maxSize, maxSize];

        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                if (i < minSize && j < minSize)
                {
                    // Обе матрицы имеют этот элемент
                    resultMatrix[i, j] = (G1[i, j] != G2[i, j]) ? 1 : 0;
                }
                else if (i < G1.GetLength(0) && j < G1.GetLength(0))
                {
                    // Только первая матрица имеет этот элемент
                    resultMatrix[i, j] = G1[i, j];
                }
                else if (i < G2.GetLength(0) && j < G2.GetLength(0))
                {
                    // Только вторая матрица имеет этот элемент
                    resultMatrix[i, j] = G2[i, j];
                }
                else
                {
                    resultMatrix[i, j] = 0;
                }
            }
        }
        Print(resultMatrix);
        return resultMatrix;
    }

    public static void Main()
    {
        // Создаем первую матрицу
        Console.WriteLine("Введите размер первой матрицы: ");
        int size1 = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Матрица 1:");
        int[,] G1 = CreateMatr(size1);

        // Создаем вторую матрицу
        Console.WriteLine("\nВведите размер второй матрицы: ");
        int size2 = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Матрица 2:");
        int[,] G2 = CreateMatr(size2);

        // Выполняем операции
        Console.WriteLine("\nРезультаты операций:");
        int[,] G3 = Matr1Matr2A(G1, G2);
        G3 = Matr1Matr2B(G1, G2);
        G3 = Matr1Matr2C(G1, G2);
    }
}