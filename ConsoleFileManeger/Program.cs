using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ConsoleFileManeger
{
    class Program
    {
        const int MAX_COUNT_ITEM_IN_PAGE = 15; 

        static List<string> files = new List<string>();
        static List<string> directories = new List<string>();
        static List<string> all = new List<string>();
        static string currentPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        static int currentCursorIndex = 0;
        static int pageCount = 0;
        static int currentPage = 1;

        static void Main(string[] args)
        {
            ConsoleSettings();
            ReadCurrentDir();
            PrintFiles();
            PrintPaging();
            string newPath = null;

            try
            {
                while (true)
                {
                    ConsoleKeyInfo info = Console.ReadKey();
                    switch (info.Key)
                    {
                        case ConsoleKey.F1:   //Открытие указанного каталога
                            {
                                Console.Write("\nВведите путь: ");
                                currentPath = Console.ReadLine();
                                ReadCurrentDir();
                                PrintFiles();
                                PrintPaging();
                            }
                            break;
                        case ConsoleKey.F2: //Копирование каталога
                            {
                                if (Directory.Exists(currentPath))
                                {
                                    newPath = Directory.GetDirectories(currentPath)[currentCursorIndex];
                                    Directory.CreateDirectory($"{newPath}_Copy");
                                    ReadCurrentDir();
                                    PrintFiles();
                                    PrintPaging();
                                }
                            }
                            break;
                        case ConsoleKey.F3: //Удаление каталога
                            {
                                newPath = Directory.GetDirectories(currentPath)[currentCursorIndex];
                                Directory.Delete(newPath);
                                ReadCurrentDir();
                                PrintFiles();
                                PrintPaging();
                            }
                            break;
                        case ConsoleKey.F4:
                            {

                            }
                            break;
                        case ConsoleKey.UpArrow:  //идём вверх по катологу
                            {
                                if (currentCursorIndex > 0)
                                {
                                    currentCursorIndex--;
                                    if (currentCursorIndex < currentPage * MAX_COUNT_ITEM_IN_PAGE - MAX_COUNT_ITEM_IN_PAGE)
                                    {
                                        currentPage--;
                                    }
                                    PrintFiles();
                                    PrintPaging();
                                }
                            }
                            break;
                        case ConsoleKey.DownArrow: //идём вниз по катологу
                            {
                                if (currentCursorIndex < all.Count - 2)
                                {
                                    currentCursorIndex++;
                                    if (currentCursorIndex > currentPage * MAX_COUNT_ITEM_IN_PAGE - 1)
                                    {
                                        currentPage++;
                                    }
                                    PrintFiles();
                                    PrintPaging();
                                }

                            }
                            break;
                        case ConsoleKey.LeftArrow:  //пейджинг страниц влево
                            {
                                if (currentPage != 1)
                                {
                                    currentPage--;
                                    currentCursorIndex = currentPage * MAX_COUNT_ITEM_IN_PAGE - MAX_COUNT_ITEM_IN_PAGE;
                                    PrintFiles();
                                    PrintPaging();
                                }
                            }
                            break;
                        case ConsoleKey.RightArrow: //пейджинг страниц вправо
                            {
                                if (currentPage < pageCount)
                                {
                                    currentPage++;
                                    currentCursorIndex = currentPage * MAX_COUNT_ITEM_IN_PAGE - MAX_COUNT_ITEM_IN_PAGE;
                                    PrintFiles();
                                    PrintPaging();
                                }

                            }
                            break;
                        case ConsoleKey.Enter:    //Открытие католога на котором находиться курсор
                            {
                                currentPath = Directory.GetDirectories(currentPath)[currentCursorIndex];
                                ReadCurrentDir();
                                PrintFiles();
                                PrintPaging();
                            }
                            break;
                        case ConsoleKey.Backspace:
                            {

                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }

        static void PrintMainMenu()  //Вывод меню консоли
        {
            Console.WriteLine("\t\t\t\t\t\t\tFILE MANAGER\n" +
                "══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗" +
                "\n [F1 = Open Path]   [F2 = Copy]   [F3 = Delete]   [Enter = Open Directory]   [Navigation: UpArrow, DownArrow, LeftArrow, RightArrow]\n" +
                "══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
        }

        static void PrintFiles() //Вывод списка катологов и файлов на косноль
        {
            Console.Clear();
            PrintMainMenu();

            var firstIndex = MAX_COUNT_ITEM_IN_PAGE * currentPage - MAX_COUNT_ITEM_IN_PAGE;

            var lastIndex = (all.Count - (firstIndex + 1)) < MAX_COUNT_ITEM_IN_PAGE ? all.Count - (firstIndex + 1) : MAX_COUNT_ITEM_IN_PAGE;
            if (lastIndex < 0)
            {
                lastIndex++;
            }
            
            var currentPageItems = all.GetRange(firstIndex, lastIndex);

            foreach (var item in currentPageItems)
            {
                if (item.Equals(all[currentCursorIndex]))
                {
                    ConsoleColor current = Console.BackgroundColor;

                    Console.BackgroundColor = ConsoleColor.DarkGray;

                    Console.WriteLine(item);

                    Console.BackgroundColor = current;

                }
                else
                {
                    Console.WriteLine(item);
                }
            }
        }

        public static void PrintPaging() //Вывод текущей страницы
        {
            Console.WriteLine($"\nСтраница {currentPage} из {pageCount}\n" +
                $"══════════════════════════════════════════════════════════" +
                $"══════════════════════════════════════════════════════════" +
                $"══════════════════╝");
        }
     
        //public static bool IsDirectory(string path)
        //{
        //    FileAttributes attr = File.GetAttributes(path);

        //    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public static void ReadCurrentDir() //Объединяет список каталогов и список файлов в общий список
        {
            files = Directory.GetFiles(currentPath).ToList();
            directories = Directory.GetDirectories(currentPath).ToList();
            all = directories.Concat(files).ToList();
            pageCount = all.Count / MAX_COUNT_ITEM_IN_PAGE;
            if (all.Count % MAX_COUNT_ITEM_IN_PAGE > 0)
            {
                pageCount += 1;
            }
        }

        public static void ConsoleSettings() //Размер окна и цвет текста 
        {
            Console.CursorVisible = false;
            Console.SetBufferSize(140, 41);
            Console.SetWindowSize(140, 41);
            Console.ForegroundColor = ConsoleColor.Green;
        }

    }
}
