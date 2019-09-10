using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace _1_BaseFunctionMassive
{
    class Program
    {
        static void Main(string[] args)
        {
            var ex = new Extension();
            var arrayNumForOnlyNum = new HashSet<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', };
            var q = new Questions();
            WriteLine("С# - Уровень 1. Задание 4.1");
            WriteLine("Кузнецов");
            WriteLine("Дан целочисленный массив из 20 элементов. Элементы массива могут принимать целые значения от –10 000 до 10 000 включительно. " + Environment.NewLine +
                      "Написать программу, позволяющую найти и вывести количество пар элементов массива, в которых хотя бы одно число делится на 3. " + Environment.NewLine +
                      "В данной задаче под парой подразумевается два подряд идущих элемента массива. Например, для массива из пяти элементов: 6; 2; 9; –3; 6 – ответ: 4.");
            var t = new RArray<int>
                (int.Parse(q.Question<int>("Укажите размер массива: ", arrayNumForOnlyNum)));
            ex.Print(t.NextRandomArrayValues().ToString(), PositionForRow.LeftEdge, Console.CursorTop + 1);
            ex.Print($"Количество пар - {t.GetPair().Count.ToString()}", PositionForRow.LeftEdge, Console.CursorTop + 1);

            var t2 = new RArray<int>(5);
            t2[0] = 6;
            t2[1] = 2;
            t2[2] = 9;
            t2[3] = -3;
            t2[4] = 6;

            ex.Print(t2.ToString(), PositionForRow.LeftEdge, Console.CursorTop + 1);
            ex.Print($"Количество пар - {t2.GetPair().Count.ToString()}", PositionForRow.LeftEdge, Console.CursorTop + 1);

            ex.Pause();
        }
    }

    struct RArray<T> where T: struct, IComparable<T>
    {
        T[] array;  // он приватный
        Dictionary<int, Tuple<T, T>> listPair;


        public RArray(int n)
        {
            array = new T[(dynamic)n];
            listPair = new Dictionary<int, Tuple<T, T>>();
        }

        public RArray<T> NextRandomArrayValues()
        {
            var oldValue = new T();
            var nextValue = new T();
            var r = new Random();
            for (var i = 0; i < array.Length; ++i)
            {
                oldValue = nextValue;
                nextValue = (dynamic)r.Next(-10000, 10000);
                array[i] = nextValue;

                if (i != 0 && (oldValue % (dynamic)3 == 0 || nextValue % (dynamic)3 == 0))
                    listPair.Add(i - 1, Tuple.Create(oldValue, nextValue));                
            }
            return this;
        }
        
        public Dictionary<int, Tuple<T, T>> GetPair() => listPair;

        // либо создаем индексируемое свойство
        public T this[int i]
        {
            get { return array[i]; }
            set
            {
                if(-10000 <= (dynamic)value && (dynamic)value <= 10000)
                    array[i] = value;                
                else if (-10000 > (dynamic)value)
                    array[i] = (dynamic)(-10000);
                else if ((dynamic)value > 10000)
                    array[i] = (dynamic)(-10000);

                var b1 = listPair.TryGetValue(i, out var t1);
                if (b1)
                {
                    if (value % (dynamic)3 == 0 || t1.Item2 % (dynamic)3 == 0)
                        listPair[i] = Tuple.Create(value, t1.Item2);
                    else
                        listPair.Remove(i);
                }
                else if (array.Length > i + 1 && (value % (dynamic)3 == 0 || array[i + 1] % (dynamic)3 == 0))
                        listPair.Add(i, Tuple.Create(value, array[i + 1]));
                

                if(i > 0)
                {
                    var b2 = listPair.TryGetValue(i - 1, out var t2);
                    if (b2)
                    {
                        if (t2.Item1 % (dynamic)3 == 0 || value % (dynamic)3 == 0)
                            listPair[i - 1] = Tuple.Create(t2.Item1, value);
                        else
                            listPair.Remove(i - 1);
                    }
                    else if (i - 1 >= 0 && (array[i - 1] % (dynamic)3 == 0 || value % (dynamic)3 == 0))
                        listPair.Add(i - 1, Tuple.Create(array[i], value));
                }
            }
        }

        public override string ToString()
        {
            var text = new StringBuilder();
            text.Append("Array: " + Environment.NewLine);
            for(var i = 0; i < array.Length; ++i)
            {
                text.Append($"{i}({array[i]})");
                text.Append(", ");
            }
            text.Remove(text.Length - 1, 1);
            text.Append(Environment.NewLine);
            text.Append("RArray: "+ Environment.NewLine);
            foreach(var e in listPair)
            {
                text.Append($"{e.Key}-{e.Key+1}({e.Value.Item1}:{e.Value.Item2})");
                text.Append(", ");
            }
            text.Remove(text.Length - 1, 1);
            return text.ToString();
        }
    }

    /// <summary>
    /// Класс запроса данных у пользователя
    /// </summary>
    class Questions
    {
        /// <summary>
        /// Перевести первый символ в заглавный
        /// </summary>
        /// <param name="text">Корректируемый текст</param>
        /// <returns></returns>
        public string FirstUpper(string text) => text.Substring(0, 1).ToUpper() + (text.Length > 1 ? text.Substring(1) : "");

        /// <summary>
        /// Запрос данных у пользователя
        /// </summary>
        /// <typeparam name="T">Тип выводимых значений</typeparam>
        /// <param name="text">Текст запроса значения у пользователя</param>
        /// <param name="arraySym">Массив допустимых вводимых символов пользователем</param>
        /// <returns></returns>
        public string Question<T>(string text, HashSet<char> arraySym)
        {
            Console.Write(text);
            var textAnswer = new StringBuilder();
            while (true)
            {
                var symbol = Console.ReadKey(true);
                if (arraySym.Contains(symbol.KeyChar))
                {
                    textAnswer.Append(symbol.KeyChar.ToString());
                    Console.Write(symbol.KeyChar.ToString());
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
            Console.Write("");
            return textAnswer.ToString();
        }
    }

    /// <summary>
    /// Класс расширения возможности консольного приложения
    /// </summary>
    public class Extension
    {
        /// <summary>
        /// Вывести текст на экран
        /// </summary>
        /// <param name="text">Текст выводимый на экран пользователя</param>
        /// <param name="x">Начальная позиция X для выводимого текста</param>
        /// <param name="y">Позиция Y (начиная от верха экрана) для выводимого текста</param>
        public void Print(string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        /// <summary>
        /// Вывести текст на экран
        /// </summary>
        /// <param name="text">Текст выводимый на экран пользователя</param>
        /// <param name="position">Расположение на экране</param>
        /// <param name="y">Позиция Y (начиная от верха экрана) для выводимого текста</param>
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

        /// <summary>
        /// Пауза приложения
        /// </summary>
        /// <param name="millisec">Продолжительность паузы в миллисекундах</param>
        public void Pause(int millisec) => System.Threading.Thread.Sleep(millisec);
        /// <summary>
        /// Пауза приложения до нажатия любой клавиши пользователем
        /// </summary>
        public void Pause() => ReadKey(true);

    }

    /// <summary>
    /// Позиция на экране
    /// </summary>
    public enum PositionForRow
    {
        Center,
        LeftEdge,
        RightEdge
    }
}
