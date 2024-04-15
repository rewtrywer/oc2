﻿
namespace MyProgram
{
    class Program
    {
        public static void Main()
        {
            string eof = "eof";
            string bad = "bad";
            string empty = "empty";

            List<Tuple<object, object>> A = new List<Tuple<object, object>>()
            {
                mT(26, 27),
                mT(27, 28),
                mT(28, 29),
                mT(29, eof)
            };

            List<Tuple<object, object>> B = new List<Tuple<object, object>>()
            {
                mT(16, 17),
                mT(17, 12),
                mT(12, 13),
                mT(13, eof)
            };

            List<Tuple<object, object>> C = new List<Tuple<object, object>>()
            {
                mT(2, 15),
                mT(15, 9),
                mT(9, 19),
                mT(19, eof)
            };

            List<Tuple<object, object>> D = new List<Tuple<object, object>>()
            {
                mT(23, 24),
                mT(24, 12),
                mT(12, 13),
                mT(13, eof)
            };

            List<Tuple<object, object>> FAT = new List<Tuple<object, object>>()
            {
                mT(0, empty),
                mT(1, empty),
                mT(2, 15),
                mT(3, empty),
                mT(4, 5),
                mT(5, 6),
                mT(6, 7),
                mT(7, eof),
                mT(8, bad),
                mT(9, 19),
                mT(10, 11),
                mT(11, 12),
                mT(12, 13),
                mT(13, eof),
                mT(14, empty),
                mT(15, 9),
                mT(16, 17),
                mT(17, 12),
                mT(18, bad),
                mT(19, eof),
                mT(20, empty),
                mT(21, empty),
                mT(22, empty),
                mT(23, 24),
                mT(24, 12),
                mT(25, empty),
                mT(26, 27),
                mT(27, 28),
                mT(28, 29),
                mT(29, eof),
                mT(30, empty),
                mT(31, empty)
            };

            crossFiles(A, B, C, D, FAT);
            badFiles(A, B, C, D, FAT);
            lostFiles(A, B, C, D, FAT);
        }

        private static Tuple<object, object> mT(object a, object b)
        {
            Tuple<object, object> t = new Tuple<object, object>(a, b);
            return t;
        }

        private static void lostFiles(List<Tuple<object, object>> A, List<Tuple<object, object>> B, List<Tuple<object, object>> C,
                                      List<Tuple<object, object>> D, List<Tuple<object, object>> FAT)
        {
            // Определение значений для проверки
            //string eof = "eof";
            string empty = "empty";
            string bad = "bad";

            // Инициализация переменных для подсчета и хранения потерянных файлов

            int countFree = 0;
            List<Tuple<object, object>> lost = new List<Tuple<object, object>>();

            try
            {
                // Поиск потерянных файлов в FAT
                lost = FAT.Where(x => !(A.Contains(x) || B.Contains(x) || C.Contains(x) || D.Contains(x)) &&
                    !(x.Item2.ToString() == empty || x.Item2.ToString() == bad)).ToList();

                // Подсчет количества свободных кластеров

                countFree = FAT.Count(x => x.Item2.ToString() == empty);

                // Формирование информационного сообщения о потерянных файлах
                string info = "\nПотерянные кластеры: \n";
                if (lost.Count > 0)
                {
                    info += string.Join("\n", lost.Select(x => $"{x.Item1} {x.Item2}"));

                }
                else
                {
                    info = "Потерянных кластеров не найдено.";
                }
                info += $"\n\nКоличество свободных кластеров: {countFree}";

                // Вывод информационного сообщения
                Console.WriteLine(info);
            }
            catch
            {
                // Обработка ошибки
                Console.WriteLine("Fault");
            }
        }

        private static void crossFiles(List<Tuple<object, object>> A, List<Tuple<object, object>> B, List<Tuple<object, object>> C,
                                       List<Tuple<object, object>> D, List<Tuple<object, object>> FAT)
        {
            // Инициализация списка для хранения пересекающихся файлов
            List<Tuple<Tuple<object, object>, string>> cross = new List<Tuple<Tuple<object, object>, string>>();

            // Поиск пересекающихся файлов в FAT
            foreach (var fat in FAT)
            {
                // Формирование строки с указанием кластеров, в которых находится файл
                string crossFat = (A.Contains(fat) ? "A " : "") +
                                  (B.Contains(fat) ? "B " : "") +
                                  (C.Contains(fat) ? "C " : "") +
                                  (D.Contains(fat) ? "D " : "");

                if (crossFat.Length > 2)
                {
                    cross.Add(Tuple.Create(fat, crossFat));
                }
            }

            // Формирование информационного сообщения о пересекающихся файлах
            string info = cross.Count == 0 ? "Пересекающихся кластеров нет" :
                string.Join("", cross.OrderByDescending(x => x.Item2.Length)
                .Select((x, i) => $"{(i == 0 ? $"Файлы {x.Item2} пересекаются в кластерах:" : "")}\n{x.Item1.Item1}   {x.Item1.Item2}"));

            Console.WriteLine(info);
        }

        private static void badFiles(List<Tuple<object, object>> A, List<Tuple<object, object>> B, List<Tuple<object, object>> C,
                                     List<Tuple<object, object>> D, List<Tuple<object, object>> FAT)
        {
            // Инициализация переменных для формирования информационного сообщения
            string info = "";
            string bad = "bad";

            // Формирование информационного сообщения о поврежденных файлах
            List<Tuple<object, object>> badFiles = FAT.Where(x => x.Item2.ToString() == bad).ToList();
            if (badFiles.Count == 0)
            {
                info = "Поврежденных кластеров нет\n";
            }
            else
            {
                info = $"\nНайдено {badFiles.Count} поврежденных кластеров:\n";
                info += string.Join("\n", badFiles.Select(x => $"{x.Item1}  {x.Item2}"));
            }
            Console.WriteLine(info);
        }
    }
}