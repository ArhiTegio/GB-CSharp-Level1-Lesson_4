using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Console;

namespace _4_DoubleMassive
{
    class Program
    {
        static void Main(string[] args)
        {
            var ex = new Extension();
            var q = new Questions();
            WriteLine("С# - Уровень 1. Задание 4.4");
            WriteLine("Кузнецов");
            WriteLine("4. *а) Реализовать класс для работы с двумерным массивом. Реализовать конструктор, заполняющий массив случайными числами. Создать методы, которые возвращают сумму всех элементов массива, сумму всех элементов массива больше заданного, свойство, возвращающее минимальный элемент массива, свойство, возвращающее максимальный элемент массива, метод, возвращающий номер максимального элемента массива (через параметры, используя модификатор ref или out)" + Environment.NewLine +
                      "   *б) Добавить конструктор и методы, которые загружают данные из файла и записывают данные в файл." + Environment.NewLine +
                      "Дополнительные задачи" + Environment.NewLine +
                      "    в) Обработать возможные исключительные ситуации при работе с файлами.");

        }
    }

    struct DoubleClass<T>
    {
        T[,] array;  // он приватный

        public T Min { get
            {
                T min = array[0, 0];
                for (var i = 0; i < array.Length; i++)
                    for (var j = 0; j < array.Length; j++)
                        if ((dynamic)array[i, j] < min)
                            min = (dynamic)array[i, j];
                return min;

            }
        }

        public T Max
        {
            get
            {
                T max = array[0, 0];
                for (var i = 0; i < array.Length; i++)
                    for (var j = 0; j < array.Length; j++)
                        if ((dynamic)array[i, j] > max)
                            max = (dynamic)array[i, j];
                return max;
            }
        }

        public DoubleClass(int x, int y, bool random = false)
        {
            array = new T[(dynamic)x, (dynamic)y];

            if (random)
                NextRandomArrayValues();
        }



        public DoubleClass(string filename)
        {
            //@"data.txt"
            array = new T[1, 1];
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename);
                try
                {
                    var s = new List<string>();
                    while(sr.EndOfStream)
                        s.Add(sr.ReadLine());

                    array = new T[s[0].Length,s.Count];
                    for (var i = 0; i < array.Length; i++)
                    {
                        var l = s[i].Split(' ');
                        for (var j = 0; j < array.Length; j++)
                            array[i, j] = (dynamic)double.Parse(l[j]);
                    }

                    sr.Close();
                }
                catch (Exception e)
                {
                    sr.Close();
                }
            }
        }

        public DoubleClass<T> WriteMassiveInFaile(string filename)
        {
            StreamWriter sr = new StreamWriter(filename);
            try
            {
                var t = new List<string>();
                var l = new StringBuilder();
                for (var i = 0; i < array.Length; i++)
                {
                    for (var j = 0; j < array.Length; j++)
                        l.Append(array[i, j]);
                    l.Append(Environment.NewLine);
                }

                sr.Write(l.ToString());
                sr.Close();
            }
            catch (Exception e)
            {
                MessageBox();
                sr.Close();
            }
            return this;
        }

        void NextRandomArrayValues(int step = 1)
        {
            var r = new Random();
            for (var i = 0; i < array.Length; i += step)
                for (var j = 0; j < array.Length; j += step)
                    array[i, j] = (dynamic)r.Next(-10000, 10000);
        }

        public void PositionMaxElement(ref int x, ref int y)
        {
            T max = array[0, 0];
            for (var i = 0; i < array.Length; i++)
                for (var j = 0; j < array.Length; j++)
                    if ((dynamic)array[i, j] > max)
                    {
                        max = (dynamic)array[i, j];
                        x = i;
                        y = j;
                    }
        }

        public T Sum()
        {
            T sum = array[0,0];
            for (var i = 0; i < array.Length; i++)
                for (var j = 0; j < array.Length; j++)
                    sum += (dynamic)array[i, j];
            return sum;
        }

        public T Sum(T n)
        {
            T sum = array[0, 0];
            for (var i = 0; i < array.Length; i++)
                for (var j = 0; j < array.Length; j++)
                    if((dynamic)array[i, j] > n)
                        sum += (dynamic)array[i, j];
            return sum;
        }






    }

    class Questions
    {
        public string FirstUpper(string text) => text.Substring(0, 1).ToUpper() + (text.Length > 1 ? text.Substring(1) : "");

        public string Question<T>(string text, HashSet<char> arraySym, bool show)
        {
            Console.WriteLine(text);
            var textAnswer = new StringBuilder();
            while (true)
            {
                var symbol = Console.ReadKey(true);
                if (arraySym.Contains(symbol.KeyChar))
                {
                    textAnswer.Append(symbol.KeyChar.ToString());
                    if (show)
                        Console.Write(symbol.KeyChar.ToString());
                    else
                        Console.Write('*');
                }

                if (symbol.Key == ConsoleKey.Backspace && textAnswer.Length > 0)
                {
                    textAnswer.Remove(textAnswer.Length - 1, 1);
                    Console.Write(symbol.KeyChar.ToString());
                    Console.Write(" ");
                    Console.Write(symbol.KeyChar.ToString());
                }

                if (typeof(T) == typeof(string))
                {
                    if (symbol.Key == ConsoleKey.Enter && textAnswer.Length > 0)
                        break;
                }
                else
                    if (symbol.Key == ConsoleKey.Enter &&
                        double.TryParse(textAnswer.ToString()
                            .Replace(".", ","),
                            out var number))
                    break;
            }
            Console.WriteLine("");
            return textAnswer.ToString();
        }
    }

    public class Extension
    {
        public void Print(string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        public void Print(string text, PositionForRow position, int y)
        {
            if (position == PositionForRow.Center)
            {
                var n = (WindowWidth - text.Length) / 2;
                if (n >= 0)
                    Console.SetCursorPosition(n, y);
                else
                    Console.SetCursorPosition(0, y);
                Console.Write(text);
            }

            if (position == PositionForRow.LeftEdge)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(text);
            }

            if (position == PositionForRow.RightEdge)
            {
                var n = (WindowWidth - text.Length);
                if (n >= 0)
                    Console.SetCursorPosition(n, y);
                else
                    Console.SetCursorPosition(0, y);
                Console.Write(text);
            }
        }

        public void Pause(int millisec) => System.Threading.Thread.Sleep(millisec);
        public void Pause() => ReadKey(true);

    }

    public enum PositionForRow
    {
        Center,
        LeftEdge,
        RightEdge
    }
}
