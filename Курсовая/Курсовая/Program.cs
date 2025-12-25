using System;
using System.IO;

namespace IndependentSetsFinder
{
    class Program
    {
        private static int[,] graph;
        private static int numVertices;
        private const int MAX_VERTICES = 100;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Программа поиска независимых множеств в графе");
            Console.WriteLine("=============================================\n");

            bool running = true;

            while (running)
            {
                Console.WriteLine("\nГлавное меню:");
                Console.WriteLine("1. Считать граф из файла");
                Console.WriteLine("2. Сгенерировать случайный граф");
                Console.WriteLine("3. Найти независимые множества");
                Console.WriteLine("4. Выход");

                Console.Write("\nВыберите действие: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        LoadGraphFromFile();
                        break;
                    case "2":
                        GenerateRandomGraph();
                        break;
                    case "3":
                        if (graph == null)
                        {
                            Console.WriteLine("\nОшибка: граф не загружен!");
                            Console.WriteLine("Сначала загрузите или сгенерируйте граф.");
                        }
                        else
                        {
                            FindIndependentSets();
                        }
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("\nЗавершение работы программы.");
                        break;
                    default:
                        Console.WriteLine("\nНеверный выбор. Пожалуйста, выберите 1-4.");
                        break;
                }
            }
        }

        static void LoadGraphFromFile()
        {
            Console.Clear();
            Console.WriteLine("Загрузка графа из файла");
            Console.WriteLine("=======================\n");

            Console.Write("Введите имя файла: ");
            string filename = Console.ReadLine();

            if (ReadGraphFromFile(filename))
            {
                Console.WriteLine("\nГраф успешно загружен.");
                ShowGraphMatrix();
            }
        }

        static bool ReadGraphFromFile(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine("Файл не найден!");
                    return false;
                }

                using (StreamReader reader = new StreamReader(filename))
                {
                    string firstLine = reader.ReadLine();
                    if (firstLine == null)
                    {
                        Console.WriteLine("Файл пустой!");
                        return false;
                    }

                    if (!int.TryParse(firstLine.Trim(), out numVertices))
                    {
                        Console.WriteLine("Некорректный формат количества вершин!");
                        return false;
                    }

                    if (numVertices <= 0 || numVertices > MAX_VERTICES)
                    {
                        Console.WriteLine("Количество вершин должно быть от 1 до " + MAX_VERTICES);
                        return false;
                    }

                    graph = new int[numVertices, numVertices];

                    for (int i = 0; i < numVertices; i++)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            Console.WriteLine("Недостаточно строк в файле! Ожидалось " + numVertices);
                            return false;
                        }

                        string[] values = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        if (values.Length != numVertices)
                        {
                            Console.WriteLine("Ошибка в строке " + (i + 2) + ": ожидалось " + numVertices + " чисел");
                            return false;
                        }

                        for (int j = 0; j < numVertices; j++)
                        {
                            if (!int.TryParse(values[j], out int value))
                            {
                                Console.WriteLine("Некорректное число: " + values[j]);
                                return false;
                            }

                            if (value != 0 && value != 1)
                            {
                                Console.WriteLine("Значение должно быть 0 или 1: " + value);
                                return false;
                            }

                            graph[i, j] = value;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при чтении файла: " + ex.Message);
                return false;
            }
        }

        static void GenerateRandomGraph()
        {
            Console.Clear();
            Console.WriteLine("Генерация случайного графа");
            Console.WriteLine("===========================\n");

            Console.Write("Введите количество вершин: ");
            if (!int.TryParse(Console.ReadLine(), out numVertices))
            {
                Console.WriteLine("Неверное количество вершин!");
                return;
            }

            if (numVertices <= 0 || numVertices > MAX_VERTICES)
            {
                Console.WriteLine("Количество вершин должно быть от 1 до " + MAX_VERTICES);
                return;
            }

            Console.Write("Введите имя файла для сохранения графа: ");
            string filename = Console.ReadLine();

            if (GenerateRandomGraphFile(numVertices, filename))
            {
                Console.WriteLine("\nСлучайный граф успешно сгенерирован.");
                ShowGraphMatrix();
            }
        }

        static bool GenerateRandomGraphFile(int vertices, string filename)
        {
            try
            {
                numVertices = vertices;
                graph = new int[numVertices, numVertices];
                Random rand = new Random();

                string directory = Path.GetDirectoryName(filename);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine(numVertices);

                    int edgeCount = 0;
                    for (int i = 0; i < numVertices; i++)
                    {
                        for (int j = 0; j < numVertices; j++)
                        {
                            if (i == j)
                            {
                                graph[i, j] = 0;
                            }
                            else if (j > i)
                            {
                                int value = rand.Next(100) < 50 ? 1 : 0;
                                graph[i, j] = value;
                                graph[j, i] = value;

                                if (value == 1) edgeCount++;
                            }

                            writer.Write(graph[i, j]);
                            if (j < numVertices - 1) writer.Write(" ");
                        }
                        writer.WriteLine();
                    }

                    Console.WriteLine("Вершин: " + numVertices + ", Ребер: " + edgeCount);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при генерации графа: " + ex.Message);
                return false;
            }
        }

        static void FindIndependentSets()
        {
            Console.Clear();
            Console.WriteLine("Поиск независимых множеств");
            Console.WriteLine("===========================\n");

            ShowGraphMatrix();

            Console.Write("\nВведите имя файла для сохранения результатов: ");
            string outputFile = Console.ReadLine();

            int[] subset = new int[MAX_VERTICES];
            int totalFound = 0;

            Console.WriteLine("\nНайденные независимые множества:");
            FindIndependentSetsRecursive(subset, 0, 0, outputFile, ref totalFound);

            Console.WriteLine("Найдено множеств: " + totalFound);

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void FindIndependentSetsRecursive(int[] subset, int index, int count,
                                                string outputFile, ref int totalFound)
        {
            if (index == numVertices)
            {
                bool independent = true;

                for (int i = 0; i < count && independent; i++)
                {
                    for (int j = i + 1; j < count; j++)
                    {
                        if (graph[subset[i], subset[j]] == 1)
                        {
                            independent = false;
                            break;
                        }
                    }
                }

                if (independent && count > 0)
                {
                    totalFound++;

                    Console.Write("{ ");
                    for (int i = 0; i < count; i++)
                    {
                        Console.Write(subset[i]);
                        if (i != count - 1) Console.Write(", ");
                    }
                    Console.WriteLine(" }");

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputFile, true))
                        {
                            for (int i = 0; i < count; i++)
                            {
                                writer.Write(subset[i]);
                                if (i != count - 1) writer.Write(" ");
                            }
                            writer.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при записи в файл: " + ex.Message);
                    }
                }
                return;
            }

            subset[count] = index;
            FindIndependentSetsRecursive(subset, index + 1, count + 1, outputFile, ref totalFound);
            FindIndependentSetsRecursive(subset, index + 1, count, outputFile, ref totalFound);
        }

        static void ShowGraphMatrix()
        {
            if (graph == null)
            {
                Console.WriteLine("Граф не загружен.");
                return;
            }

            Console.WriteLine("Матрица смежности графа (" + numVertices + " вершин):");
            Console.Write("   ");
            for (int j = 0; j < numVertices; j++)
            {
                Console.Write(j.ToString().PadLeft(3));
            }
            Console.WriteLine();

            Console.Write("   ");
            for (int j = 0; j < numVertices; j++)
            {
                Console.Write("---");
            }
            Console.WriteLine();

            for (int i = 0; i < numVertices; i++)
            {
                Console.Write(i.ToString().PadLeft(2) + "|");
                for (int j = 0; j < numVertices; j++)
                {
                    Console.Write(graph[i, j].ToString().PadLeft(3));
                }
                Console.WriteLine();
            }
        }
    }
}