using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoftlineTestProject.Models
{
    /// <summary>
    /// Информация о файле или директории
    /// </summary>
    public class Filename
    {
        /// <summary>
        /// Имя описываемого файла или директории
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Путь до родительской директории
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// Признак директории
        /// </summary>
        public bool isDirectory { get; }

        /// <summary>
        /// Дата и время создания
        /// </summary>
        public DateTime CreationTime { get; }


        /// <summary>
        /// Конструктор, инициализирующий поля класса
        /// </summary>
        /// <param name="filename">Имя описываемого файла или директории</param>
        /// <param name="Directory">Путь до родительской директории</param>
        /// <param name="isDirectory">Признак директории</param>
        /// <param name="date">Дата и время создания</param>
        public Filename(string filename, string Directory, bool isDirectory, DateTime date)
        {
            this.Name = filename;
            this.Directory = Directory;
            this.isDirectory = isDirectory;
            this.CreationTime = date;
        }

        /// <summary>
        /// Возращает признак директории
        /// </summary>
        /// <returns>1 - для директории, 0 - для файла</returns>
        public int isDirectoryAsInt()
        {
            return (isDirectory) ? 1 : 0;
        }
    }
}