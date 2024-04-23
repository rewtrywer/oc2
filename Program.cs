
using System;
using System.Collections.Specialized;
using System.Text;

namespace MyProgram
{
    class Program
    {
       public static List<Tuple<object, object>> lost = new List<Tuple<object, object>>();
       private static List<Tuple<object, object>> freeClusters = new List<Tuple<object, object>>();

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
                //mT(22, empty),
                //mT(23, 24),
                //mT(24, 12),
                //mT(25, empty),
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
                mT(11, 14),
                mT(12, 13),
                mT(13, eof),
                mT(14, eof),
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

            string resultA = CheckEndOfFileMarker(A);
            string resultB = CheckEndOfFileMarker(B);
            string resultC = CheckEndOfFileMarker(C);
            string resultD = CheckEndOfFileMarker(D);

            Console.WriteLine(resultA + " A");
            Console.WriteLine(resultB + " B");
            Console.WriteLine(resultC + " C");
            Console.WriteLine(resultD + " D");

           
            CreateAndPrintList(lost);
           

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
            

            try
            {
                // Поиск потерянных файлов в FAT
                lost = FAT.Where(x => !(A.Contains(x) || B.Contains(x) || C.Contains(x) || D.Contains(x)) &&
                    !(x.Item2.ToString() == empty || x.Item2.ToString() == bad)).ToList();

                // Подсчет количества свободных кластеров
                freeClusters = FAT.Where(x => x.Item2.ToString() == empty).ToList();

                countFree = freeClusters.Count;

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
                info += $"\n\nКоличество свободных кластеров: {countFree}\n";

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
            string info = "";

            if (cross.Count == 0)
            {
                info = "Пересекающихся кластеров нет";
            }
            else
            {
                info = string.Join("", cross.Select((x, i) => $"{(i == 0 ? $"Файлы {x.Item2} пересекаются в кластерах:" : "")}\n{x.Item1.Item1}  {x.Item1.Item2}"));
            }
            
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

        public static string CheckEndOfFileMarker(List<Tuple<object, object>> records)
        {
            if (records.Count == 0)
            {
                return "Список записей пуст";
            }

            var lastRecord = records.Last();
            if (lastRecord.Item2.ToString() == "eof")
            {
                return "Запись 'eof' найдена в конце файла";
            }
            else
            {
                return "Запись 'eof' не найдена в конце файла";
            }
        }

        private static void CreateAndPrintList(List<Tuple<object, object>> lostClusters)
        {
            List<Tuple<object, object>> C = new List<Tuple<object, object>>();

            int numOfEofMarkers = lostClusters.Count(t => t.Item2.ToString() == "eof");

            for (int j = 0; j < numOfEofMarkers; j++)
            {
                List<Tuple<object, object>> tempC = new List<Tuple<object, object>>();

                for (int i = 0; i < lostClusters.Count; i++)
                {
                    if (lostClusters[i].Item2.ToString() == "eof")
                    {
                        tempC.Add(mT(lostClusters[i].Item1, lostClusters[i].Item2));
                        break;
                    }
                    else
                    {
                        tempC.Add(mT(lostClusters[i].Item1, lostClusters[i].Item2)); 
                    } 
                }

                C.AddRange(tempC);
                
                Console.WriteLine($"\nНовый файл {j +1}:") ;
                foreach (var tuple in C)
                {
                    Console.WriteLine($"mT({tuple.Item1}, {tuple.Item2});");
                }
                C.Clear();
                foreach (var tuple in tempC)
                {
                    lostClusters.Remove(tuple);
                }
            }  
        }

        
    }
}