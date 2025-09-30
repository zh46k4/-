using System.Collections.Generic;
using System.Security.Cryptography;

namespace Laba3_ALVT
{
    public class Node
    {
        public string Data { get; set; }      // Полезная информация
        public int Priority { get; set; }     // Приоритет
        public Node Next { get; set; }        // Ссылка на следующий узел

        public Node(string data, int priority)
        {
            Data = data;
            Priority = priority;
            Next = null;
        }
    }

    public class Prioretet
    {

        private Node head;

        public Prioretet()
        {
            head = null;
        }

        public void Enqueue(string data, int prioretet)
        {

            Node newNode = new Node(data, prioretet);
            if (head == null)
            {
                head = newNode;
                Console.WriteLine($"Добавлен довый элемент: {data}");
            }

            if (prioretet > head.Priority)
            {
                newNode.Next = head;
                head = newNode;
                return;
            }

            Node pervious = null;
            Node current = head;

            while (current != null && current.Priority >= prioretet)
            {
                pervious = current;
                current = current.Next;
            }
            pervious.Next = newNode;
            newNode.Next = current;

        }

        public void Dispaly()
        {
            if (head == null)
            {
                Console.WriteLine("Очередь пуста!");
            }
            Node current = head;
            int position = 1;
            Console.WriteLine("\n--- Содержимое очереди ---");
            while (current != null)
            {
                Console.WriteLine($"{position}. Данные: '{current.Data}', Приоритет: {current.Priority}");
                current = current.Next;
                position++;
            }
        }
    }
    class Program()
    {

        static void AddElement(Prioretet queue)
        {

            Console.Write("Введите данные: ");
            string data = Console.ReadLine();
            Console.Write("Введите приоритет (число): ");
            if (int.TryParse(Console.ReadLine(), out int priority))
            {
                queue.Enqueue(data, priority);
                Console.WriteLine("Элемент добавлен!");
            }
            else
            {
                Console.WriteLine("Ошибка: приоритет должен быть числом!");
            }

        }

        static void Main()
        {

            //Prioretet testQueue = new Prioretet();
            //testQueue.Enqueue("Обычная задача", 3);
            //testQueue.Enqueue("Аварийная", 10);
            //testQueue.Enqueue("Срочная", 7);
            //testQueue.Enqueue("Не важно", 1);
            //testQueue.Dispaly();
            //Console.WriteLine();

            Prioretet queue = new Prioretet();
            bool runing = true;


            Console.WriteLine("=== ПРИОРИТЕТНАЯ ОЧЕРЕДЬ ===");
            while (true)
            {
                Console.WriteLine("\n1. Добавить элемент");
                Console.WriteLine("2. Просмотреть очередь");
                Console.WriteLine("3. Выйти");
                Console.Write("Выберите действие: ");

                string choise = Console.ReadLine();

                switch (choise)
                {
                    case "1":
                        AddElement(queue);
                        break;
                    case "2":
                        queue.Dispaly();
                        break;
                    case "3":
                        runing = false;
                        break;
                    default:
                        Console.WriteLine("Неверный вобор!");
                        break;
                }

            }

        }

    }

}