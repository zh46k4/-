using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА №9 - ЗАДАНИЕ 2*");
        Console.WriteLine("===================================\n");

        int vertices = 8;

        // Генерация тестового графа
        var matrix = GenerateAdjacencyMatrix(vertices, 0.3);
        var adjList = MatrixToAdjacencyList(matrix);

        Console.WriteLine("Тестовый граф:");
        PrintMatrix(matrix);

        // ЗАДАНИЕ 2.1* - Поиск расстояний DFS с матрицей
        Console.WriteLine("\n2.1* - Поиск расстояний DFS с матрицей смежности (от вершины 0)");
        Console.WriteLine("-------------------------------------------------------------");
        var dfsMatrixDistances = DFSFindDistances(matrix, 0);
        PrintDistances(dfsMatrixDistances);

        // ЗАДАНИЕ 2.2* - Поиск расстояний DFS со списками
        Console.WriteLine("\n2.2* - Поиск расстояний DFS со списками смежности (от вершины 0)");
        Console.WriteLine("---------------------------------------------------------------");
        var dfsListDistances = DFSFindDistancesWithAdjacencyList(adjList, 0);
        PrintDistances(dfsListDistances);

        // Сравнение с BFS для проверки корректности
        Console.WriteLine("\nСравнение с BFS для проверки:");
        Console.WriteLine("-----------------------------");
        var bfsDistances = BFSFindDistances(matrix, 0);
        Console.WriteLine("BFS расстояния:");
        PrintDistances(bfsDistances);

        bool dfsResultsMatch = Enumerable.SequenceEqual(dfsMatrixDistances, dfsListDistances);
        Console.WriteLine($"Результаты DFS с матрицей и списками совпадают: {dfsResultsMatch}");

        // ЗАДАНИЕ 2.3* - Сравнение производительности
        Console.WriteLine("\n2.3* - Сравнение производительности алгоритмов");
        Console.WriteLine("-----------------------------------------------");
        CompareAlgorithms();

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    // Генерация матрицы смежности
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

    // 2.1* - Поиск расстояний на основе обхода в глубину (матрица)
    static int[] DFSFindDistances(int[,] G, int v)
    {
        int size_G = G.GetLength(0);
        int[] DIST = new int[size_G];
        bool[] visited = new bool[size_G];

        for (int i = 0; i < size_G; i++)
        {
            DIST[i] = -1;
            visited[i] = false;
        }

        DFSMatrix(G, v, 0, DIST, visited);

        return DIST;
    }

    static void DFSMatrix(int[,] G, int current, int depth, int[] DIST, bool[] visited)
    {
        visited[current] = true;
        DIST[current] = depth;

        for (int i = 0; i < G.GetLength(0); i++)
        {
            if (G[current, i] == 1 && !visited[i])
            {
                DFSMatrix(G, i, depth + 1, DIST, visited);
            }
        }
    }

    // 2.2* - Поиск расстояний DFS со списками смежности
    static int[] DFSFindDistancesWithAdjacencyList(Dictionary<int, List<int>> adjList, int v)
    {
        int size_G = adjList.Count;
        int[] DIST = new int[size_G];
        bool[] visited = new bool[size_G];

        for (int i = 0; i < size_G; i++)
        {
            DIST[i] = -1;
            visited[i] = false;
        }

        DFSList(adjList, v, 0, DIST, visited);

        return DIST;
    }

    static void DFSList(Dictionary<int, List<int>> adjList, int current, int depth, int[] DIST, bool[] visited)
    {
        visited[current] = true;
        DIST[current] = depth;

        foreach (int neighbor in adjList[current])
        {
            if (!visited[neighbor])
            {
                DFSList(adjList, neighbor, depth + 1, DIST, visited);
            }
        }
    }

    // 2.3* - Сравнение производительности алгоритмов
    static void CompareAlgorithms()
    {
        int[] graphSizes = { 100, 500, 1000 };
        double[] densities = { 0.1, 0.3, 0.5 };

        Console.WriteLine("Сравнение времени выполнения (в тиках):");
        Console.WriteLine("======================================");

        foreach (int size in graphSizes)
        {
            foreach (double density in densities)
            {
                Console.WriteLine($"\nГраф: {size} вершин, плотность {density:P0}");

                // Генерация тестовых данных
                var matrix = GenerateAdjacencyMatrix(size, density);
                var adjList = MatrixToAdjacencyList(matrix);

                // Измерение времени выполнения
                long bfsMatrixTime = MeasureTime(() => BFSFindDistances(matrix, 0));
                long bfsListTime = MeasureTime(() => BFSFindDistancesWithAdjacencyList(adjList, 0));
                long dfsMatrixTime = MeasureTime(() => DFSFindDistances(matrix, 0));
                long dfsListTime = MeasureTime(() => DFSFindDistancesWithAdjacencyList(adjList, 0));

                Console.WriteLine($"BFS с матрицей:    {bfsMatrixTime,8} тиков");
                Console.WriteLine($"BFS со списками:   {bfsListTime,8} тиков");
                Console.WriteLine($"DFS с матрицей:    {dfsMatrixTime,8} тиков");
                Console.WriteLine($"DFS со списками:   {dfsListTime,8} тиков");

                // Анализ эффективности
                Console.WriteLine($"Эффективность списков над матрицей:");
                Console.WriteLine($"  BFS: {((double)bfsMatrixTime / bfsListTime):F2}x");
                Console.WriteLine($"  DFS: {((double)dfsMatrixTime / dfsListTime):F2}x");
            }
        }
    }

    // Вспомогательные функции

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

    static int[] BFSFindDistances(int[,] G, int v)
    {
        int size_G = G.GetLength(0);
        int[] DIST = new int[size_G];

        for (int i = 0; i < size_G; i++)
            DIST[i] = -1;

        Queue<int> Q = new Queue<int>();
        Q.Enqueue(v);
        DIST[v] = 0;

        while (Q.Count > 0)
        {
            int currentV = Q.Dequeue();

            for (int i = 0; i < G.GetLength(0); i++)
            {
                if (G[currentV, i] == 1 && DIST[i] == -1)
                {
                    Q.Enqueue(i);
                    DIST[i] = DIST[currentV] + 1;
                }
            }
        }

        return DIST;
    }

    static int[] BFSFindDistancesWithAdjacencyList(Dictionary<int, List<int>> adjList, int v)
    {
        int size_G = adjList.Count;
        int[] DIST = new int[size_G];

        for (int i = 0; i < size_G; i++)
            DIST[i] = -1;

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

    static long MeasureTime(Action algorithm)
    {
        var watch = Stopwatch.StartNew();
        algorithm();
        watch.Stop();
        return watch.ElapsedTicks;
    }

    static void PrintMatrix(int[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"{matrix[i, j]} ");
            }
            Console.WriteLine();
        }
    }

    static void PrintDistances(int[] distances)
    {
        for (int i = 0; i < distances.Length; i++)
        {
            string status = distances[i] == -1 ? "недостижима" : distances[i].ToString();
            Console.WriteLine($"  Вершина {i}: расстояние = {status}");
        }
    }
}