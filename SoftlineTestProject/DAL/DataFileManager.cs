using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using SoftlineTestProject.Models;
using SoftlineTestProject.Helpers;

namespace SoftlineTestProject.DAL
{
    /// <summary>
    /// Реализация интерфейса управления источником данных
    /// </summary>
    public class DataFileManager : IDataFileManager
    {
        /// <summary>
        /// Получение ридера файлового потока
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <returns>ридер файлового потока или null в случае ошибки</returns>
        public StreamReader getFileStream(string path)
        {
            try
            {
                StreamReader fileStream = new StreamReader(path, Encoding.UTF8);
                if (System.IO.File.Exists(path)) return fileStream;
                else return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Чтение из файлового потока и помещение полученных данных в экземпляр объекта DataFile
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="separator">строка, содержащая разделитель столбцов таблицы</param>
        /// <param name="isHeader">признак заголовка таблицы</param>
        /// <returns>заполненный экземпляр объекта DataFile или null в случае ошибки</returns>
        public DataFile readFile(string path, string separator, bool isHeader)
        {
            if (File.Exists(path))
            {
                if (separator.Length <= 0) separator = "\u0009";  //по-умолчанию таб
                string[] preparedSeparator = { ConversionHelper.decodeEscapedUnicode(separator) };

                DataFile ResultFile = new DataFile(new List<DataRecord>(), path);

                StreamReader fileStream = getFileStream(path);
                if (fileStream == null) return null;
                try
                {
                    string currentLine = "";

                    if (isHeader)
                    {
                        currentLine = fileStream.ReadLine(); //считываем первую строку
                        string[] stringArray = currentLine.Split(preparedSeparator, StringSplitOptions.RemoveEmptyEntries);
                        DataRecord currentRecord = new DataRecord(stringArray, true);
                        ResultFile.ListOfRecords.Add(currentRecord);
                    }

                    while ((currentLine = fileStream.ReadLine()) != null)
                    {
                        // currentLine = fileStream.ReadLine();
                        string[] stringArray = currentLine.Split(preparedSeparator, StringSplitOptions.RemoveEmptyEntries);
                        DataRecord currentRecord = new DataRecord(stringArray, false);
                        ResultFile.ListOfRecords.Add(currentRecord);
                    }
                    return ResultFile;
                }
                catch
                {
                    return null;
                }
                finally {
                    fileStream.Close();
                }
                
            }
            else
                return null;
        }

        /// <summary>
        /// Сохранение данных из экземпляра объекта DataFile в файл
        /// </summary>
        /// <param name="dataFile">экземпляр объекта DataFile с данными, которые надо сохранить</param>
        /// <param name="path">путь к файлу</param>
        /// <param name="separator">строка, содержащая разделитель столбцов таблицы</param>
        /// <returns>0 в случае успеха, -1 в случае ошибки</returns>
        public int writeFile(DataFile dataFile, string path, string separator)   
        {
            if (separator.Length <= 0) separator = "\u0009";  //по-умолчанию ставим табы

            var preparedSeparator = ConversionHelper.decodeEscapedUnicode(separator);

            StreamWriter fileStream = File.CreateText(path);
            try
            {
                foreach(DataRecord line in dataFile.ListOfRecords)
                {
                    if (line.CellData.Select(cell => cell.Length).Max() > 0)
                        fileStream.WriteLine(String.Join(preparedSeparator, line.CellData));
                }
                return 0;
            }
            catch
            {
                return -1;
            }
            finally
            {
                fileStream.Close();
            }
        }
    }
}