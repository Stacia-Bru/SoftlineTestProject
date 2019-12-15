using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using SoftlineTestProject.Models;

namespace SoftlineTestProject.Business_Logics
{
    /// <summary>
    /// Реализация интерфейса управление списком, формирующимся из содержимого директорий
    /// </summary>
    public class DirectoryManager : IDirectoryManager
    {
        /// <summary>
        /// Реализация запроса содержимого директории
        /// </summary>
        /// <param name="DirectoryName">путь к директории, содержимое которой следует получить</param>
        /// <returns>List<Filename> список экземпляров класса FileName, в которых хранится информация о файлах и поддиректориях </returns>
        public List<Filename> getDirectoryListing(string DirectoryName)
        {
            DirectoryInfo dir = new DirectoryInfo(DirectoryName);

            List<Filename> resultList = new List<Filename> { new Filename("..", dir.FullName, true, new DateTime()) };  //подъем по файловой системе

            resultList.AddRange(dir.GetDirectories().Select(f => new Filename(f.Name, dir.FullName, true, f.CreationTime)).ToList());  //Возвращаем список директорий
            resultList.AddRange(dir.GetFiles().Select(f => new Filename(f.Name, dir.FullName, false, f.CreationTime)).ToList());  //Добавляем в конец список файлов
            return resultList;
        }
    }
}


