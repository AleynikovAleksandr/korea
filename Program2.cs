using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal class Program
    {
        static void Main()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Пути к файлам, куда сохранится информация
            string filePath1 = Path.Combine(desktopPath, "Текст1.txt");
            string filePath2 = Path.Combine(desktopPath, "Текст2.txt");
            string mergedFilePath = Path.Combine(desktopPath, "Объединить информацию.txt");

            // Удаляем существующие файлы, если они существуют, и создаем новые
            if (File.Exists(filePath1))
            {
                File.Delete(filePath1);
            }
            if (File.Exists(filePath2))
            {
                File.Delete(filePath2);
            }
            if (File.Exists(mergedFilePath))
            {
                File.Delete(mergedFilePath);
            }

            // Ввод текста для первого файла
            Console.WriteLine("Введите текст для первого файла (для завершения ввода введите 'конец1'):");
            InputTextToFile(filePath1);

            // Ввод текста для второго файла
            Console.WriteLine("Введите текст для второго файла (для завершения ввода введите 'конец2'):");
            InputTextToFile(filePath2);

            // Объединение информации из двух файлов
            string mergedInfo = MergeFiles(filePath1, filePath2);

            // Вывод результата на консоль
            Console.WriteLine("\nОбъединение файлов завершено.");
            Console.WriteLine("Объединенная информация:");
            Console.WriteLine(mergedInfo);

            // Записываем объединенную информацию в файл, удаляем существующее содержимое
            WriteToFile(mergedFilePath, mergedInfo);

            Console.ReadLine();
        }

        static void InputTextToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string line;
                while ((line = Console.ReadLine()) != "конец1" && line != "конец2")
                {
                    writer.WriteLine(line);
                }
            }
        }

        static string MergeFiles(string filePath1, string filePath2)
        {
            // Создаем HashSet для хранения уникальных строк
            HashSet<string> uniqueLines = new HashSet<string>();

            // Читаем и обрабатываем первый файл
            ProcessFile(filePath1, uniqueLines);

            // Читаем и обрабатываем второй файл
            ProcessFile(filePath2, uniqueLines);

            // Удаляем пустые строки
            uniqueLines.RemoveWhere(string.IsNullOrWhiteSpace);

            // Объединяем уникальные строки
            string mergedInfo = string.Join("\n", uniqueLines);

            return mergedInfo;
        }

        static void ProcessFile(string filePath, HashSet<string> uniqueLines)
        {
            // Читаем содержимое файла построчно и добавляем уникальные строки в HashSet
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    uniqueLines.Add(line);
                }
            }
        }

        static void WriteToFile(string filePath, string mergedInfo)
        {
            // Записываем объединенную информацию в файл, удаляем существующее содержимое
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string line in mergedInfo.Split(new[] { "\n" }, StringSplitOptions.None))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }

    }


}
            
 


    



    


// Изменения для iss60
// Изменения для iss60
