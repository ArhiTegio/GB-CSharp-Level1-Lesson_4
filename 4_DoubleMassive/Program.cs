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


            var dM = new DoubleMassive<int>(10, 10);

            ex.Print($"Инициализация массива: {dM}", PositionForRow.LeftEdge, CursorTop + 1);
            ex.Print($"Заполнение случайными числами: {dM.NextRandomArrayValues()}", PositionForRow.LeftEdge, CursorTop + 1);
            ex.Print($"Сумма всех чисел: {dM.Sum()}", PositionForRow.LeftEdge, CursorTop + 1);
            ex.Print($"Сумма всех чисел больше 1000: {dM.Sum(1000)}", PositionForRow.LeftEdge, CursorTop + 1);
            ex.Print($"Минимальное значение: {dM.Min}", PositionForRow.LeftEdge, CursorTop + 1);
            ex.Print($"Максимальное значение: {dM.Max}", PositionForRow.LeftEdge, CursorTop + 1);
            var t1 = 0;
            var t2 = 0;
            dM.PositionMaxElement(ref t1, ref t2);
            ex.Print($"Позицыя максимального значения: {t1} {t2}", PositionForRow.LeftEdge, CursorTop + 1);
            ex.Print($"Записываем в файл log.txt: ", PositionForRow.LeftEdge, CursorTop + 1);
            dM.WriteMassiveInFile(@"log.txt");
            dM.NextRandomArrayValues();
            ex.Print($"Повторно заполняем случайными числами: {dM}", PositionForRow.LeftEdge, CursorTop + 1);
            dM = new DoubleMassive<int>(@"log.txt");
            ex.Print($"Загружаем из файла log.txt: {dM}", PositionForRow.LeftEdge, CursorTop + 1);


            ex.Pause();
        }
    }

    /// <summary>
    /// Двумерный массив
    /// </summary>
    /// <typeparam name="T"></typeparam>
    struct DoubleMassive<T>
    {
        T[,] array;  // он приватный


        /// <summary>
        /// Получить максимальное значение из массива
        /// </summary>
        public T Min { get
            {
                T min = array[0, 0];
                for (var i = 0; i < array.GetLength(0); i++)
                    for (var j = 0; j < array.GetLength(1); j++)
                        if ((dynamic)array[i, j] < min)
                            min = (dynamic)array[i, j];
                return min;
            }
        }

        /// <summary>
        /// Получить минимальное значение из массива
        /// </summary>
        public T Max
        {
            get
            {
                T max = array[0, 0];
                for (var i = 0; i < array.GetLength(0); i++)
                    for (var j = 0; j < array.GetLength(1); j++)
                        if ((dynamic)array[i, j] > max)
                            max = (dynamic)array[i, j];
                return max;
            }
        }

        /// <summary>
        /// Инициализация массива определенных размеров и при необходимости заполнение случайными числами
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="random"></param>
        public DoubleMassive(int x, int y, bool random = false)
        {
            array = new T[(dynamic)x, (dynamic)y];

            if (random)
                NextRandomArrayValues();
        }


        /// <summary>
        /// Считать массив из файла
        /// </summary>
        /// <param name="filename"></param>
        public DoubleMassive(string filename)
        {
            //@"data.txt"
            array = new T[1, 1];
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename);
                try
                {
                    var s = new List<string>();
                    while (true)
                    {
                        var t = sr.ReadLine();
                        if (t != null)                        
                            s.Add(t);                        
                        else
                            break;
                    }

                    array = new T[s[0].Split(' ').Length - 1, s.Count];
                    for (var i = 0; i < array.GetLength(0); i++)
                    {
                        var l = s[i].Split(' ');
                        for (var j = 0; j < array.GetLength(1); j++)
                            array[i, j] = (T)((dynamic)double.Parse(l[j]));
                    }

                    sr.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка при загрузки файла");
                    sr.Close();
                }
            }
        }

        /// <summary>
        /// Записать массив в файл
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public DoubleMassive<T> WriteMassiveInFile(string filename)
        {
            StreamWriter sr = new StreamWriter(filename);
            try
            {
                var t = new List<string>();
                var l = new StringBuilder();
                for (var i = 0; i < array.GetLength(0); i++)
                {
                    for (var j = 0; j < array.GetLength(1); j++)
                        l.Append(array[i, j] + " ");
                    l.Append(Environment.NewLine);
                }

                sr.Write(l.ToString());
                sr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка при сохранении файла");
                sr.Close();
            }
            return this;
        }


        /// <summary>
        /// Заполнение массива случайными числами
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public DoubleMassive<T> NextRandomArrayValues(int step = 1)
        {
            var r = new Random();
            for (var i = 0; i < array.GetLength(0); i += step)
                for (var j = 0; j < array.GetLength(1); j += step)
                    array[i, j] = (dynamic)r.Next(-10000, 10000);
            return this;
        }

        /// <summary>
        /// Позиция максимального значения в массиве
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void PositionMaxElement(ref int x, ref int y)
        {
            T max = (new T[1])[0];
            for (var i = 0; i < array.GetLength(0); i++)
                for (var j = 0; j < array.GetLength(1); j++)
                    if ((dynamic)array[i, j] > max)
                    {
                        max = (dynamic)array[i, j];
                        x = i;
                        y = j;
                    }
        }

        /// <summary>
        /// Сумма массива
        /// </summary>
        /// <returns></returns>
        public T Sum()
        {
            T sum = (new T[1])[0];
            for (var i = 0; i < array.GetLength(0); i++)
                for (var j = 0; j < array.GetLength(1); j++)
                    sum += (dynamic)array[i, j];
            return sum;
        }

        /// <summary>
        /// Сумма массива если значения выше заданного значения
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public T Sum(T n)
        {

            T sum = (new T[1])[0];
            for (var i = 0; i < array.GetLength(0); i++)
                for (var j = 0; j < array.GetLength(1); j++)
                    if((dynamic)array[i, j] > n)
                        sum += (dynamic)array[i, j];
            return sum;
        }

              
        public override string ToString()
        {
            var text = new StringBuilder();
            text.Append(Environment.NewLine + "Array: " + Environment.NewLine);
            for (var i = 0; i < array.GetLength(0); ++i)
            {
                for (var j = 0; j < array.GetLength(1); ++j)
                {
                    text.Append($"{array[i, j].ToString()}");
                    text.Append(", ");
                }
                text.Remove(text.Length - 1, 1);
                text.Append(Environment.NewLine);
            }
            return text.ToString();
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
