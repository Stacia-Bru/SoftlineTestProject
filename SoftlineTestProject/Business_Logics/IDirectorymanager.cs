using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SoftlineTestProject.Models;

namespace SoftlineTestProject.Business_Logics
{
    /// <summary>
    /// Интерфес управления списком, формирующимся из содержимого директорий
    /// </summary>
    public interface IDirectoryManager
    {
        /// <summary>
        /// Запрос содержимого директории
        /// </summary>
        /// <param name="DirectoryName">путь к директории, содержимое которой следует получить</param>
        /// <returns>List<Filename> список экземпляров класса FileName, в которых хранится информация о файлах и поддиректориях </returns>
        List<Filename> getDirectoryListing(string DirectoryName);
    }
}