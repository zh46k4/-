using System.Diagnostics.Contracts;

namespace Laba3_ALVT
{
    public class Node
    {
        public string Data { get; set; }      // Полезная информация
        public int Priority { get; set; }     // Приоритет
        public Node Next { get; set; }        // Ссылка на следующий узел

        public Node(string data)
        {
            Data = data;
            Next = null;
        }
    }

    class Queue
    {

        private Node head;
        private Node tail;

        public Queue()
        {
            head = null;
            tail = null;
        }

        public void Enqueue(string data)
        {

            Node newNode = new Node(data);
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }
        }

        public string DeQueue()
        {
            if (head == null)
            {
                Console.WriteLine("Очередь пуста");
                return null;
            }
            string data = head.Data;
            head = head.Next;

            if (head == null)
            {
                tail = null;
            }

            Console.WriteLine($"Извлеченный элемент {data}");
            return data;

        }

        public string First()
        {
            if (head == null)
            {
                Console.WriteLine("Очередь пуста");
                return null;
            }

            string data = head.Data;
            Console.WriteLine($"Первый элемент: {data}");
            return data;

        }

        public void Display()
        {

            if (head == null)
            {
                Console.WriteLine("Очередь пуста");
            }

            Node current = head;
            int plase = 1;


            Console.WriteLine("\n--- Содержимое очереди ---");

            while (current != null)
            {
                Console.WriteLine($"Номер: {plase}, Эллемент: {current.Data}");
                current = current.Next;
                plase++;
            }

        }

    }
    class Prog
    {

        static void Main()
        {
            Queue queue = new Queue();

            queue.Enqueue("Первый");
            queue.Enqueue("Второй");
            queue.Enqueue("Третий");

            queue.Display();
            // Вывод:
            // 1. Первый
            // 2. Второй  
            // 3. Третий
            Console.WriteLine();
            Console.WriteLine(queue.First());


            queue.DeQueue(); // Уйдет "Первый"
            queue.DeQueue(); // Уйдет "Второй"

            queue.Display();
            // Вывод:
            // 1. Третий

            queue.DeQueue(); // Уйдет "Третий"
            queue.DeQueue(); // Очередь пуста!


        }
    }

}