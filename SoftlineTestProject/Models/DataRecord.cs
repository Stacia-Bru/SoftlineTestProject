using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftlineTestProject.Models
{
    /// <summary>
    /// строка таблицы с данными
    /// </summary>
    public class DataRecord
    {
        /// <summary>
        /// Список ячеек строки
        /// </summary>
        public List<string> CellData { get; set; }

        /// <summary>
        /// признак заголовка таблицы
        /// </summary>
        public bool isHeader { get; set; }

        /// <summary>
        /// Конструктор, инициализирующий поля класса
        /// </summary>
        /// <param name="rows">массив значений ячеек строки</param>
        /// <param name="isHeader">признак заголовка таблицы</param>
        public DataRecord(string[] rows, bool isHeader)
        {
            this.CellData = new List<string>();
            foreach (string row in rows)
            {
                this.CellData.Add(row);
            }
            this.isHeader = isHeader;

        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DataRecord()
        {
            this.CellData = new List<string>();
            this.isHeader = false;
        }
    }

    /// <summary>
    /// Данные таблицы
    /// </summary>
    public class DataFile
    {
        /// <summary>
        /// Список строк таблицы
        /// </summary>
        public List<DataRecord> ListOfRecords { get; set; }

        /// <summary>
        /// Путь к источнику данных
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Конструктор, инициализирующий поля класса
        /// </summary>
        /// <param name="RecordList">Список строк таблицы</param>
        /// <param name="path">Путь к источнику данных</param>
        public DataFile(List<DataRecord> RecordList, string path)
        {
            this.ListOfRecords = RecordList;
            this.Path = path;
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DataFile()
        {
            this.ListOfRecords = new List<DataRecord>();
            this.Path = "";
        }

    }
}
 
 
 