using System;
using System.Collections.Generic;
using System.Linq;

public class GraphOperations
{
    //МАТРИЦ

    public static int[,] CreateMatrix()
    {
        Console.WriteLine("Введите размер матрицы: ");
        int size = Convert.ToInt32(Console.ReadLine());
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
        PrintMatrix(G);
        return G;
    }

    public static void PrintMatrix(int[,] matr)
    {
        Console.WriteLine("Матрица смежности:");
        for (int i = 0; i < matr.GetLength(0); i++)
        {
            for (int j = 0; j < matr.GetLength(1); j++)
            {
                Console.Write(matr[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public static int[,] MatrixIdentification(int[,] matr, int first, int second)
    {
        Console.WriteLine($"Отождествление вершин {first} и {second}:");

        int size = matr.GetLength(0);
        if (first < 0 || first >= size || second < 0 || second >= size || first == second)
        {
            Console.WriteLine("Ошибка: неверные номера вершин!");
            return matr;
        }

        int newSize = size - 1;
        int[,] resMatr = new int[newSize, newSize];

        int mergedVertex = Math.Min(first, second);
        int removedVertex = Math.Max(first, second);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == removedVertex || j == removedVertex)
                    continue;

                int newI = i;
                int newJ = j;
                if (i > removedVertex) newI = i - 1;
                if (j > removedVertex) newJ = j - 1;

                if (i == mergedVertex || j == mergedVertex)
                {
                    int value1 = matr[i, j];
                    int value2;

                    if (i == mergedVertex)
                    {
                        value2 = matr[removedVertex, j];
                    }
                    else
                    {
                        value2 = matr[i, removedVertex];
                    }


                    if (i == mergedVertex && j == mergedVertex)
                    {

                        bool hasLoop = matr[mergedVertex, mergedVertex] == 1 ||
                                      matr[removedVertex, removedVertex] == 1 ||
                                      matr[mergedVertex, removedVertex] == 1;
                        resMatr[newI, newJ] = hasLoop ? 1 : 0;
                    }
                    else
                    {
                        resMatr[newI, newJ] = (value1 == 1 || value2 == 1) ? 1 : 0;
                    }
                }
                else
                {
                    resMatr[newI, newJ] = matr[i, j];
                }
            }
        }
        PrintMatrix(resMatr);
        return resMatr;
    }


    public static int[,] MatrixContraction(int[,] matr, int first, int second)
    {
        Console.WriteLine($"Стягивание вершин {first} и {second}:");

        if (matr[first, second] == 0)
        {
            Console.WriteLine("Ошибка: между вершинами " + first + " и " + second + " нет ребра!");
            return matr;
        }

        int[,] resMatr = new int[matr.GetLength(0) - 1, matr.GetLength(0) - 1];

        int remains = Math.Min(first, second);
        int goOut = Math.Max(first, second);

        for (int i = 0; i < matr.GetLength(0); i++)
        {
            for (int j = 0; j < matr.GetLength(1); j++)
            {
                if (i == goOut || j == goOut)
                    continue;

                int newI = i;
                int newJ = j;
                if (i > goOut) newI = i - 1;
                if (j > goOut) newJ = j - 1;

                if (i == remains || j == remains)
                {
                    int value1 = matr[i, j];
                    int value2;

                    if (i == remains)
                    {
                        value2 = matr[goOut, j];
                    }
                    else
                    {
                        value2 = matr[i, goOut];
                    }

                    if ((i == remains && j == remains) ||
                        (i == remains && j == goOut) ||
                        (i == goOut && j == remains))
                    {

                        bool hasLoop = matr[remains, remains] == 1 ||
                                      matr[goOut, goOut] == 1 ||
                                      HasCommonNeighbors(matr, remains, goOut);
                        resMatr[newI, newJ] = hasLoop ? 1 : 0;
                    }
                    else
                    {
                        resMatr[newI, newJ] = (value1 == 1 || value2 == 1) ? 1 : 0;
                    }
                }
                else
                {
                    resMatr[newI, newJ] = matr[i, j];
                }
            }
        }
        PrintMatrix(resMatr);
        return resMatr;
    }

    // Проверка общих соседей для создания петель
    private static bool HasCommonNeighbors(int[,] matr, int v1, int v2)
    {
        int size = matr.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            if (i != v1 && i != v2 && matr[v1, i] == 1 && matr[v2, i] == 1)
            {
                return true;
            }
        }
        return false;
    }


    public static int[,] MatrixSplitting(int[,] matr, int vertex)
    {
        Console.WriteLine($"Расщепление вершины {vertex}:");

        int newSize = matr.GetLength(0) + 1;
        int[,] resMatr = new int[newSize, newSize];

        int newVertex = newSize - 1;


        for (int i = 0; i < matr.GetLength(0); i++)
        {
            for (int j = 0; j < matr.GetLength(0); j++)
            {
                resMatr[i, j] = matr[i, j];
            }
        }

        Random rnd = new Random();


        List<int> adjacentVertices = new List<int>();
        for (int j = 0; j < matr.GetLength(0); j++)
        {
            if (matr[vertex, j] == 1 && j != vertex)
            {
                adjacentVertices.Add(j);
            }
        }

        if (adjacentVertices.Count < 2)
        {
            Console.WriteLine("Нельзя расщепить вершину с менее чем 2 смежными вершинами!");
            return matr;
        }

        int edgesToMove = rnd.Next(1, adjacentVertices.Count);
        List<int> movedEdges = new List<int>();

        while (movedEdges.Count < edgesToMove && adjacentVertices.Count > 0)
        {
            int randomIndex = rnd.Next(0, adjacentVertices.Count);
            int targetVertex = adjacentVertices[randomIndex];
            movedEdges.Add(targetVertex);
            adjacentVertices.RemoveAt(randomIndex);
        }


        foreach (int targetVertex in movedEdges)
        {
            resMatr[vertex, targetVertex] = 0;
            resMatr[targetVertex, vertex] = 0;

            resMatr[newVertex, targetVertex] = 1;
            resMatr[targetVertex, newVertex] = 1;
        }

        resMatr[vertex, newVertex] = 1;
        resMatr[newVertex, vertex] = 1;


        if (matr[vertex, vertex] == 1)
        {
            resMatr[vertex, vertex] = 1;
            resMatr[newVertex, newVertex] = 1;
        }

        PrintMatrix(resMatr);
        return resMatr;
    }

    // СПИСК
    public static List<List<int>> CreateAdjacencyList(int[,] matr)
    {
        List<List<int>> adjacencyList = new List<List<int>>();

        for (int i = 0; i < matr.GetLength(0); i++)
        {
            List<int> neighbors = new List<int>();
            for (int j = 0; j < matr.GetLength(0); j++)
            {
                if (matr[i, j] == 1)
                {
                    neighbors.Add(j);
                }
            }
            adjacencyList.Add(neighbors);
        }

        PrintAdjacencyList(adjacencyList);
        return adjacencyList;
    }

    public static void PrintAdjacencyList(List<List<int>> adjacencyList)
    {
        Console.WriteLine("Список смежности:");
        for (int i = 0; i < adjacencyList.Count; i++)
        {
            string str = "[";
            List<int> vertices = adjacencyList[i];
            for (int j = 0; j < vertices.Count; j++)
            {
                if (j != vertices.Count - 1)
                    str += vertices[j] + ",";
                else
                    str += vertices[j];
            }
            str += "]";
            Console.WriteLine($"Вершина {i}: {str}");
        }
    }

    public static List<List<int>> ListIdentification(List<List<int>> adjacencyList, int first, int second)
    {
        Console.WriteLine($"Отождествление вершин {first} и {second}:");

        int remains = Math.Min(first, second);
        int deleted = Math.Max(first, second);

        List<List<int>> result = new List<List<int>>();

        for (int i = 0; i < adjacencyList.Count; i++)
        {
            List<int> currentList = new List<int>(adjacencyList[i]);

            if (i == remains)
            {
                List<int> list1 = new List<int>(adjacencyList[remains]);
                List<int> list2 = new List<int>(adjacencyList[deleted]);


                list1.Remove(deleted);
                list2.Remove(remains);


                list1.AddRange(list2);


                if (adjacencyList[remains].Contains(deleted) || adjacencyList[deleted].Contains(remains))
                {
                    if (!list1.Contains(remains))
                        list1.Add(remains);
                }


                if (adjacencyList[remains].Contains(remains) && !list1.Contains(remains))
                    list1.Add(remains);
                if (adjacencyList[deleted].Contains(deleted))
                {
                    if (!list1.Contains(remains))
                        list1.Add(remains);
                }


                for (int j = 0; j < list1.Count; j++)
                {
                    if (list1[j] > deleted)
                        list1[j] -= 1;
                }

                list1 = new List<int>(new HashSet<int>(list1));
                result.Add(list1);
            }
            else if (i == deleted)
            {
                continue;
            }
            else
            {
                List<int> newList = new List<int>(currentList);
                for (int j = 0; j < newList.Count; j++)
                {
                    if (newList[j] == deleted)
                        newList[j] = remains;
                    if (newList[j] > deleted)
                        newList[j] -= 1;
                }
                newList = new List<int>(new HashSet<int>(newList));
                result.Add(newList);
            }
        }

        PrintAdjacencyList(result);
        return result;
    }

    public static List<List<int>> ListContraction(List<List<int>> adjacencyList, int first, int second)
    {
        Console.WriteLine($"Стягивание ребра между {first} и {second}:");

        if (!adjacencyList[first].Contains(second) || !adjacencyList[second].Contains(first))
        {
            Console.WriteLine("У вершин нет общего ребра!");
            return new List<List<int>>();
        }

        int remains = Math.Min(first, second);
        int deleted = Math.Max(first, second);

        List<List<int>> result = new List<List<int>>();

        for (int i = 0; i < adjacencyList.Count; i++)
        {
            List<int> currentList = new List<int>(adjacencyList[i]);

            if (i == remains)
            {
                currentList.Remove(deleted);
                List<int> addList = new List<int>(adjacencyList[deleted]);
                addList.Remove(remains);

                List<int> commonNeighbors = new List<int>();
                foreach (int neighbor in adjacencyList[remains])
                {
                    if (neighbor != deleted && adjacencyList[deleted].Contains(neighbor) && neighbor != remains)
                    {
                        commonNeighbors.Add(neighbor);
                    }
                }


                if (commonNeighbors.Count > 0 ||
                    adjacencyList[remains].Contains(remains) ||
                    adjacencyList[deleted].Contains(deleted))
                {
                    if (!currentList.Contains(remains))
                        currentList.Add(remains);
                }

                for (int j = 0; j < addList.Count; j++)
                {
                    if (addList[j] > deleted)
                        addList[j] -= 1;
                }

                for (int j = 0; j < currentList.Count; j++)
                {
                    if (currentList[j] > deleted)
                        currentList[j] -= 1;
                }

                currentList.AddRange(addList);
                currentList = new List<int>(new HashSet<int>(currentList));
                result.Add(currentList);
            }
            else if (i == deleted)
            {
                continue;
            }
            else
            {
                if (currentList.Contains(deleted))
                {
                    int index = currentList.IndexOf(deleted);
                    currentList[index] = remains;
                }

                for (int j = 0; j < currentList.Count; j++)
                {
                    if (currentList[j] > deleted)
                        currentList[j] -= 1;
                }

                currentList = new List<int>(new HashSet<int>(currentList));
                result.Add(currentList);
            }
        }

        PrintAdjacencyList(result);
        return result;
    }

    public static List<List<int>> ListSplitting(List<List<int>> adjacencyList, int vertex)
    {
        Console.WriteLine($"Расщепление вершины {vertex}:");

        if (adjacencyList[vertex].Count < 2)
        {
            Console.WriteLine("Нельзя расщепить вершину с менее чем 2 смежными вершинами!");
            return new List<List<int>>();
        }

        List<List<int>> result = new List<List<int>>();
        Random rnd = new Random();

        for (int i = 0; i < adjacencyList.Count; i++)
        {
            if (i == vertex)
            {
                int splitIndex = rnd.Next(1, adjacencyList[vertex].Count);

                List<int> list1 = adjacencyList[vertex].Take(splitIndex).ToList();
                List<int> list2 = adjacencyList[vertex].Skip(splitIndex).ToList();


                list2.Add(vertex);


                if (adjacencyList[vertex].Contains(vertex))
                {
                    list1.Add(vertex);
                    list2.Add(adjacencyList.Count);
                }

                result.Add(list1);
                result.Add(list2);
            }
            else
            {
                result.Add(new List<int>(adjacencyList[i]));
            }
        }

        PrintAdjacencyList(result);
        return result;
    }


    public static void Main()
    {
        Console.WriteLine("=== РАБОТА С ГРАФАМИ ===");

        int[,] matrix = null;
        List<List<int>> adjacencyList = null;

        while (true)
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Создать новый граф (матрица)");
            Console.WriteLine("2. Преобразовать матрицу в список смежности");
            Console.WriteLine("3. Операции с матрицей");
            Console.WriteLine("4. Операции со списком смежности");
            Console.WriteLine("5. Выход");

            Console.Write("Выберите действие: ");
            int mainChoice = Convert.ToInt32(Console.ReadLine());

            switch (mainChoice)
            {
                case 1:
                    matrix = CreateMatrix();
                    break;

                case 2:
                    if (matrix != null)
                    {
                        adjacencyList = CreateAdjacencyList(matrix);
                    }
                    else
                    {
                        Console.WriteLine("Сначала создайте матрицу!");
                    }
                    break;

                case 3:
                    if (matrix != null)
                    {
                        MatrixOperationsMenu(ref matrix);
                    }
                    else
                    {
                        Console.WriteLine("Сначала создайте матрицу!");
                    }
                    break;

                case 4:
                    if (adjacencyList != null)
                    {
                        ListOperationsMenu(ref adjacencyList);
                    }
                    else
                    {
                        Console.WriteLine("Сначала создайте список смежности!");
                    }
                    break;

                case 5:
                    Console.WriteLine("Выход из программы...");
                    return;

                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    public static void MatrixOperationsMenu(ref int[,] matrix)
    {
        while (true)
        {
            Console.WriteLine("\n=== ОПЕРАЦИИ С МАТРИЦЕЙ ===");
            Console.WriteLine("1. Показать матрицу");
            Console.WriteLine("2. Отождествление вершин");
            Console.WriteLine("3. Стягивание вершин");
            Console.WriteLine("4. Расщепление вершины");
            Console.WriteLine("5. Назад в главное меню");

            Console.Write("Выберите операцию: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    PrintMatrix(matrix);
                    break;

                case 2:
                    Console.Write("Введите первую вершину: ");
                    int v1 = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Введите вторую вершину: ");
                    int v2 = Convert.ToInt32(Console.ReadLine());
                    matrix = MatrixIdentification(matrix, v1, v2);
                    break;

                case 3:
                    Console.Write("Введите первую вершину: ");
                    v1 = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Введите вторую вершину: ");
                    v2 = Convert.ToInt32(Console.ReadLine());
                    matrix = MatrixContraction(matrix, v1, v2);
                    break;

                case 4:
                    Console.Write("Введите вершину для расщепления: ");
                    int vertex = Convert.ToInt32(Console.ReadLine());
                    matrix = MatrixSplitting(matrix, vertex);
                    break;

                case 5:
                    return;

                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    public static void ListOperationsMenu(ref List<List<int>> adjacencyList)
    {
        while (true)
        {
            Console.WriteLine("\n=== ОПЕРАЦИИ СО СПИСКОМ СМЕЖНОСТИ ===");
            Console.WriteLine("1. Показать список смежности");
            Console.WriteLine("2. Отождествление вершин");
            Console.WriteLine("3. Стягивание ребра");
            Console.WriteLine("4. Расщепление вершины");
            Console.WriteLine("5. Назад в главное меню");

            Console.Write("Выберите операцию: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    PrintAdjacencyList(adjacencyList);
                    break;

                case 2:
                    Console.Write("Введите первую вершину: ");
                    int v1 = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Введите вторую вершину: ");
                    int v2 = Convert.ToInt32(Console.ReadLine());
                    adjacencyList = ListIdentification(adjacencyList, v1, v2);
                    break;

                case 3:
                    Console.Write("Введите первую вершину: ");
                    v1 = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Введите вторую вершину: ");
                    v2 = Convert.ToInt32(Console.ReadLine());
                    adjacencyList = ListContraction(adjacencyList, v1, v2);
                    break;

                case 4:
                    Console.Write("Введите вершину для расщепления: ");
                    int vertex = Convert.ToInt32(Console.ReadLine());
                    adjacencyList = ListSplitting(adjacencyList, vertex);
                    break;

                case 5:
                    return;

                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }
}