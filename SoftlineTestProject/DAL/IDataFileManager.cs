using System.IO;
using SoftlineTestProject.Models;


namespace SoftlineTestProject.DAL
{
    /// <summary>
    /// Интерфейс управления источником данных
    /// </summary>
    public interface IDataFileManager
    {
        /// <summary>
        /// Получение ридера потока данных
        /// </summary>
        /// <param name="path">путь к источнику данных</param>
        /// <returns>ридер потока данных или null в случае ошибки</returns>
        StreamReader getFileStream(string path);

        /// <summary>
        /// Чтение из потока и помещение полученных данных в экземпляр объекта DataFile
        /// </summary>
        /// <param name="path">путь к потоку данных</param>
        /// <param name="separator">строка, содержащая разделитель столбцов таблицы</param>
        /// <param name="isHeader">признак заголовка таблицы</param>
        /// <returns>заполненный экземпляр объекта DataFile или null в случае ошибки</returns>
        DataFile readFile(string path, string separator, bool isHeader);

        /// <summary>
        /// Сохранение данных из экземпляра объекта DataFile в поток
        /// </summary>
        /// <param name="dataFile">экземпляр объекта DataFile с данными, которые надо сохранить</param>
        /// <param name="path">путь к потоку</param>
        /// <param name="separator">строка, содержащая разделитель столбцов таблицы</param>
        /// <returns>0 в случае успеха, -1 в случае ошибки</returns>
        int writeFile(DataFile dataFile, string path, string separator);
    }
}