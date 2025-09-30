using System.Globalization;

namespace Laba3_ALVT
{
    public class Node
    {
        public string Data { get; set; }      // Полезная информация
        public Node Next { get; set; }        // Ссылка на следующий узел

        public Node(string data)
        {
            Data = data;
            Next = null;
        }
    }

    class Steck
    {

        private Node top;
        private Node tail;

        public Steck()
        {
            top = null;
            tail = null;

        }

        public string Peek()
        {

            if (top == null)
            {
                Console.WriteLine("Стек пуст");
                return null;
            }
            else
            {
                Console.WriteLine($"Первый элемент: {top.Data}");
                return top.Data;
            }
        }

        public void Add(string Data)
        {
            Node NewNode = new Node(Data);
            NewNode.Next = top;
            top = NewNode;
        }

        public string Pop()
        {
            if (top == null)
            {
                Console.WriteLine("Стек пуст");
                return null;
            }
            else
            {
                string data = top.Data;
                top = top.Next;
                Console.WriteLine($"Удален элемент: {data}");
                return data;
            }

        }
        public void Display()
        {
            if (top == null)
            {
                Console.WriteLine("Стек пуст");
                return;
            }

            Node current = top;
            while (current != null)
            {
                Console.WriteLine(current.Data);
                current = current.Next;
            }
        }
    }
    class Prog
    {

        static void Main()
        {
            Steck st = new Steck();
            st.Add("Первый");
            st.Add("Второй");
            st.Add("Третий");
            st.Add("Четвертый");

            st.Display();
            Console.WriteLine();
            st.Peek();

            st.Pop();
            Console.WriteLine();
            st.Display();


            Console.ReadKey();
        }

    }


}