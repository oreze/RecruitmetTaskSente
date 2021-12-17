using System;
using System.IO;
using System.Xml;

namespace RecruitmentTask.Services
{
    public class XmlService
    {
        public static XmlDocument ReadXmlFrom(string path)
        {
            var doc = new XmlDocument();
            var file = ReadFile(path);
            doc.LoadXml(file);
            return doc;
        }

        private static string ReadFile(string path)
        {
            try
            {
                if (File.Exists(path))
                    return File.ReadAllText(path);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Entered path is invalid. {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An I/O error occurred while opening the file. {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Error during accessing the file. {ex.Message}");
            }

            return "";
        }
    }
}