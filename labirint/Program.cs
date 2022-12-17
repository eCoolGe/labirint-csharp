using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace labirint
{
    class Program
    {
        protected static void WriteAt(string s1, ConsoleColor color1, int x1, int y1)
        {
            try
            {

                Console.SetCursorPosition(x1, y1);
                Console.ForegroundColor = color1;
                Console.Write(s1);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(x1, y1);
            }
            catch (ArgumentOutOfRangeException e)
            {
                WriteError(e.Message);
            }
        }
        protected static void WriteError(string s1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(s1);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadKey();
            Environment.Exit(322);
        }


        static void Main(string[] args)
        {
            Console.Title = "Игра Лабиринт | Версия: 1.0 | (с) eCoolGe CopyRight 2020";
            // Начало кода
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.WriteLine("  Пожалуйста, разверните окно на весь экран для нормальной работы!");
            Console.WriteLine("  Внимание! Диапазон строк от 3 до 30 | Диапазон столбцов от 6 до 60.");
            Console.WriteLine("  Нажмите ESC, чтобы пропустить уровень.");
            Console.WriteLine("  Спасибо за внимание! с:");
            Console.WriteLine("  Нажмите, чтобы продолжить...");
            Console.ReadKey();
            Console.Clear();

            Console.Write("  Число строк лабиринта? : "); 
            int length = Convert.ToInt32(Console.ReadLine());
            Console.Write("  Число столбцов лабиринта? [x2 строк]: "); 
            int width = Convert.ToInt32(Console.ReadLine());
            Console.Write("  Включить увеличение сложности? [1/0]: "); 
            int hard = Convert.ToInt32(Console.ReadLine());
            
            if (width <= 5 || length <= 2 || width >= 61 || length >= 31) // Проверка введенных переменных на возможную на ошибку
                WriteError("  Критическая ошибка: строки(столбцы) должны находится в диапазоне от 3(6) до 30(60)");
            else if (hard < 0 || hard > 1)
                WriteError("  Критическая ошибка: параметр может принимать значения 0 или 1");

            do
            {
                Random rnd = new Random();
                // Функция увеличения сложности
                if (hard == 2 && width <= 59 && length <= 29)
                {
                    width = ((width-2) * 2) + 2; 
                    length = ((length-1) * 2) + 1;
                } 
                else if (hard == 1)
                {
                    width = (width * 2) + 2; 
                    length = (length * 2) + 1;
                    hard = 2;
                }
                else if (hard == 0)
                {
                    width = (width * 2) + 2; 
                    length = (length * 2) + 1;
                    hard = -1;
                }
                else if (width >= 61 || length >= 31)
                {
                    WriteAt("Извините! Достигнуты максимальные размеры лабиринта.",ConsoleColor.Red, width+2, 1);
                    WriteAt("Генератор лабиринта просто создаст новый. Ожидайте...", ConsoleColor.Red, width + 2, 2);
                    System.Threading.Thread.Sleep(3000);
                }

                // Функция генератора лабиринтов
                string[,] maze = new string[length, width];
                Console.Clear();
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    for (int j = 0; j < maze.GetLength(1) - 1; j++)
                    {

                        if ((j % 2) == 0)
                        {
                            maze[i, j] = "+"; 
                        } 
                        else maze[i, j] = "~";
                    }
                }

                for (int i = 1; i < maze.GetLength(0) - 1; i += 2)
                {
                    for (int j = 0; j < maze.GetLength(1); j++)
                    {
                        if ((j % 2) == 0)
                        {
                            maze[i, j] = "|";
                        }
                        else maze[i, j] = " ";
                    }
                }

                for (int j = 2; j < width - 2; j += 2)
                {
                    maze[1, j] = " ";
                }

                for (int i = 0; i < length; i++)
                {
                    maze[i, width - 1] = "#";
                }

                for (int i = 3; i < maze.GetLength(0) - 1; i += 2)
                {
                    int k1 = 1;
                    for (int j = 1; j < width - 1; j += 2)
                    {
                        int rnd1 = rnd.Next(0, 2);
                        if (maze[i, j + 2] == "#") rnd1 = 0;

                        if (rnd1 == 1) //Идем вправо
                        {
                            maze[i, j + 1] = " ";
                            k1 += 2;

                        }
                        else if (rnd1 == 0) // Идем вверх
                        {
                            if (k1 == 1)
                            {
                                if (maze[i, j] == " ") maze[i - 1, j] = " ";

                            }
                            else if (k1 > 1)
                            {
                                int rnd2 = rnd.Next(j + 1 - k1, j + 1);
                                if (maze[i - 1, rnd2] == "~")
                                {
                                    maze[i - 1, rnd2] = " ";
                                }
                                else if (maze[i - 1, rnd2] == "+")
                                {
                                    maze[i - 1, rnd2 + 1] = " ";
                                }
                            }
                            k1 = 1;
                        }
                    }
                }


                int x = 1, y = 1; // Координаты игрока
                Console.Clear();
                // Рисуем лабиринт
                Console.BackgroundColor = ConsoleColor.Black;
                for (int i = 0; i < maze.GetLength(0); i++)
                {
                    for (int j = 0; j < maze.GetLength(1) - 1; j++)
                    {
                        Console.Write(maze[i, j]);
                    }
                    Console.WriteLine();
                }
                WriteAt("о", ConsoleColor.Red, width - 3, length - 2);

                while (true) 
                {
                    Console.CursorVisible = false;
                    
                    WriteAt("о", ConsoleColor.DarkYellow, x, y);
                    
                    // обработка ввода
                    ConsoleKeyInfo ki = Console.ReadKey();

                    if (ki.Key == ConsoleKey.Escape) break;
                    if (ki.Key == ConsoleKey.LeftArrow && maze[y, x - 1] == " ") //влево
                    {
                        WriteAt(" ", ConsoleColor.Gray, x, y);
                        x--;
                    }
                    if (ki.Key == ConsoleKey.RightArrow && maze[y, x + 1] == " ") //вправо
                    {
                        WriteAt(" ", ConsoleColor.Gray, x, y);
                        x++;
                    }
                    if (ki.Key == ConsoleKey.UpArrow && maze[y - 1, x] == " ") //вверх
                    {
                        WriteAt(" ", ConsoleColor.Gray, x, y);
                        y--;
                    }
                    if (ki.Key == ConsoleKey.DownArrow && maze[y + 1, x] == " ") //вниз
                    {
                        WriteAt(" ", ConsoleColor.Gray, x, y);
                        y++;
                    }

                    if (y == length - 2 && x == width - 3)
                    {
                        Console.Clear();
                        Console.WriteLine("/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\");
                        Console.WriteLine("  Поздравляю! Вы прошли данный лабиринт.");
                        Console.WriteLine("  Игра начнется заново через 3 секунды.");
                        Console.WriteLine("  Пожалуйста, никуда не уходите! с:");
                        Console.WriteLine("\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/");
                        System.Threading.Thread.Sleep(3000);
                        break;
                    }
                }
            } while (true);
        }
    }
}