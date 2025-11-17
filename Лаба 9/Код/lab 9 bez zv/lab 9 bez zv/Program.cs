using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА №9 - ЗАДАНИЕ 1");
        Console.WriteLine("==================================\n");
        Console.WriteLine("Введите кол-во вершин в графее");
        int vertices = 8;
        try
        {
            vertices = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            Console.WriteLine(ex.GetType());
            Console.WriteLine("Вершин будет 8");
        }

        // ЗАДАНИЕ 1.1 - Генерация матрицы смежности
        Console.WriteLine("1.1 - Генерация матрицы смежности для неориентированного графа");
        Console.WriteLine("-------------------------------------------------------------");
        var matrix = GenerateAdjacencyMatrix(vertices, 0.3);
        PrintMatrix(matrix);

        // Выбор начальной вершины для обхода
        Console.WriteLine("\nВыбор начальной вершины для обхода");
        Console.WriteLine("----------------------------------");
        int startVertex = GetStartVertex(vertices);

        // ЗАДАНИЕ 1.2 - Поиск расстояний BFS
        Console.WriteLine($"\n1.2 - Поиск расстояний BFS (от вершины {startVertex})");
        Console.WriteLine("----------------------------------------");
        var distances = BFSFindDistances(matrix, startVertex);
        PrintDistances(distances, startVertex);

        // ЗАДАНИЕ 1.3* - Работа со списками смежности
        Console.WriteLine("\n1.3* - Работа со списками смежности");
        Console.WriteLine("----------------------------------");
        var adjList = MatrixToAdjacencyList(matrix);
        PrintAdjacencyList(adjList);

        var listDistances = BFSFindDistancesWithAdjacencyList(adjList, startVertex);
        PrintDistances(listDistances, startVertex);

        // Проверка совпадения результатов
        bool resultsMatch = Enumerable.SequenceEqual(distances, listDistances);
        Console.WriteLine($"\nРезультаты BFS с матрицей и списками совпадают: {resultsMatch}");

        // Матрица расстояний между всеми вершинами
        Console.WriteLine("\nМатрица расстояний между всеми вершинами");
        Console.WriteLine("--------------------------------------");
        var distanceMatrix = BuildDistanceMatrix(matrix);
        PrintDistanceMatrix(distanceMatrix);

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    // 1.1 - Генерация матрицы смежности для неориентированного графа
    static int[,] GenerateAdjacencyMatrix(int vertices, double density = 0.3)
    {
        Random rand = new Random();
        int[,] matrix = new int[vertices, vertices];

        for (int i = 0; i < vertices; i++)
        {
            for (int j = i + 1; j < vertices; j++)
            {
                if (rand.NextDouble() < density)
                {
                    matrix[i, j] = 1;
                    matrix[j, i] = 1;
                }
            }
        }

        return matrix;
    }

    // Выбор начальной вершины
    static int GetStartVertex(int maxVertex)
    {
        while (true)
        {
            Console.Write($"Введите начальную вершину (0-{maxVertex - 1}): ");
            if (int.TryParse(Console.ReadLine(), out int vertex) && vertex >= 0 && vertex < maxVertex)
            {
                return vertex;
            }
            Console.WriteLine($"Ошибка! Введите число от 0 до {maxVertex - 1}");
        }
    }

    // Вывод матрицы смежности на экран
    static void PrintMatrix(int[,] matrix)
    {
        int n = matrix.GetLength(0);
        Console.Write("   ");
        for (int i = 0; i < n; i++)
            Console.Write($"{i,2} ");
        Console.WriteLine();

        for (int i = 0; i < n; i++)
        {
            Console.Write($"{i,2} ");
            for (int j = 0; j < n; j++)
            {
                Console.Write($"{matrix[i, j],2} ");
            }
            Console.WriteLine();
        }
    }

    // 1.2 - Поиск расстояний BFS с матрицей смежности
    static int[] BFSFindDistances(int[,] G, int v)
    {
        int size_G = G.GetLength(0);
        int[] DIST = new int[size_G];

        // П.1.1 - для всех i положим DIST[i] = -1
        for (int i = 0; i < size_G; i++)
        {
            DIST[i] = -1;
        }

        // П.1.2 - ВЫПОЛНЯТЬ BFSD(v)
        BFSD(G, v, DIST);

        return DIST;
    }

    // Алгоритм BFSD(v) из методички
    static void BFSD(int[,] G, int v, int[] DIST)
    {
        // П.2.1 - Создать пустую очередь
        Queue<int> Q = new Queue<int>();

        // П.2.2 - Поместить v в очередь
        Q.Enqueue(v);

        // П.2.3 - Обновить вектор расстояний DIST[x] = 0
        DIST[v] = 0;

        // П.2.4 - ПОКА Q != ∅ ВЫПОЛНЯТЬ
        while (Q.Count > 0)
        {
            // П.2.5 - v = Q.front()
            int currentV = Q.Dequeue();

            // П.2.8 - ДЛЯ i = 1 ДО size_G ВЫПОЛНЯТЬ
            for (int i = 0; i < G.GetLength(0); i++)
            {
                // П.2.9 - ЕСЛИ G(v,i) == 1 И DIST == -1
                if (G[currentV, i] == 1 && DIST[i] == -1)
                {
                    // П.2.11 - Поместить i в очередь
                    Q.Enqueue(i);
                    // П.2.12 - Обновить вектор расстояний DIST[i] = DIST[v] + 1
                    DIST[i] = DIST[currentV] + 1;
                }
            }
        }
    }

    // 1.3* - Преобразование матрицы смежности в списки смежности
    static Dictionary<int, List<int>> MatrixToAdjacencyList(int[,] matrix)
    {
        int n = matrix.GetLength(0);
        Dictionary<int, List<int>> adjList = new Dictionary<int, List<int>>();

        for (int i = 0; i < n; i++)
        {
            adjList[i] = new List<int>();
            for (int j = 0; j < n; j++)
            {
                if (matrix[i, j] == 1)
                {
                    adjList[i].Add(j);
                }
            }
        }

        return adjList;
    }

    // 1.3* - Поиск расстояний BFS со списками смежности
    static int[] BFSFindDistancesWithAdjacencyList(Dictionary<int, List<int>> adjList, int v)
    {
        int size_G = adjList.Count;
        int[] DIST = new int[size_G];

        for (int i = 0; i < size_G; i++)
        {
            DIST[i] = -1;
        }

        Queue<int> Q = new Queue<int>();
        Q.Enqueue(v);
        DIST[v] = 0;

        while (Q.Count > 0)
        {
            int currentV = Q.Dequeue();

            foreach (int neighbor in adjList[currentV])
            {
                if (DIST[neighbor] == -1)
                {
                    Q.Enqueue(neighbor);
                    DIST[neighbor] = DIST[currentV] + 1;
                }
            }
        }

        return DIST;
    }

    // Построение матрицы расстояний между всеми вершинами
    static int[,] BuildDistanceMatrix(int[,] matrix)
    {
        int n = matrix.GetLength(0);
        int[,] distanceMatrix = new int[n, n];

        for (int i = 0; i < n; i++)
        {
            int[] distances = BFSFindDistances(matrix, i);
            for (int j = 0; j < n; j++)
            {
                distanceMatrix[i, j] = distances[j];
            }
        }

        return distanceMatrix;
    }

    // Вывод матрицы расстояний
    static void PrintDistanceMatrix(int[,] distanceMatrix)
    {
        int n = distanceMatrix.GetLength(0);
        Console.Write("   ");
        for (int i = 0; i < n; i++)
            Console.Write($"{i,3} ");
        Console.WriteLine();

        for (int i = 0; i < n; i++)
        {
            Console.Write($"{i,2} ");
            for (int j = 0; j < n; j++)
            {
                if (distanceMatrix[i, j] == -1)
                    Console.Write("  - ");
                else
                    Console.Write($"{distanceMatrix[i, j],3} ");
            }
            Console.WriteLine();
        }
    }

    // Вспомогательные функции
    static void PrintDistances(int[] distances, int startVertex)
    {
        Console.WriteLine($"Вектор расстояний от вершины {startVertex}:");
        for (int i = 0; i < distances.Length; i++)
        {
            string status = distances[i] == -1 ? "недостижима" : distances[i].ToString();
            Console.WriteLine($"  Вершина {i}: расстояние = {status}");
        }
    }

    static void PrintAdjacencyList(Dictionary<int, List<int>> adjList)
    {
        Console.WriteLine("Списки смежности:");
        foreach (var vertex in adjList.Keys.OrderBy(k => k))
        {
            Console.WriteLine($"  Вершина {vertex}: [{string.Join(", ", adjList[vertex])}]");
        }
    }
}