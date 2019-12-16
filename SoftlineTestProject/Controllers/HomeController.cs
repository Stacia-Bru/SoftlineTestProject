using System.Web.Mvc;
using System.IO;
using SoftlineTestProject.Models;
using SoftlineTestProject.DAL;
using SoftlineTestProject.Business_Logics;

namespace SoftlineTestProject.Controllers
{
    /// <summary>
    /// Основной контроллер проекта
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Экземпляр менеджера директорий
        /// </summary>
        private IDirectoryManager directoryManager;

        /// <summary>
        /// Экземпляр менеджера источника данных
        /// </summary>
        private IDataFileManager dataFileManager;

        /// <summary>
        /// Конструктор, инициализирующий поля класса
        /// </summary>
        /// <param name="directoryManager">Экземпляр менеджера директорий</param>
        /// <param name="dataFileManager">Экземпляр менеджера источника данных</param>
        public HomeController(IDirectoryManager directoryManager, IDataFileManager dataFileManager)
        {
            this.directoryManager = directoryManager;
            this.dataFileManager = dataFileManager;
        }

        /// <summary>
        /// Метод доступа к начальной странице
        /// </summary>
        /// <returns>Основное представление</returns>
        public ActionResult Index()
        {
            return View();  //основное представление, начальная страница
        }

        /// <summary>
        /// Метод доступа к списку содержимого директории
        /// </summary>
        /// <param name="DirectoryName">путь к директории</param>
        /// <param name="FileName">имя запрашиваемой директории</param>
        /// <returns>частичное представление со списком содержимого директории или код ошибки 404</returns>
        public ActionResult DirectoryListing(string DirectoryName, string FileName)
        {
            // контролер для частичного представления выдачи директории или файла
            ViewBag.Message = "Это частичное представление со всплывающим диалоговым окном.";
            string path = DirectoryName + FileName;

            //Валидация полученных данных
            if (path == null)
                path = Server.MapPath("~/Files/");  //если ничего не передали -  в корень
            if (path.Length <= 0)
                path = Server.MapPath("~/Files/");  //если передали параметр, но пустой - в корень

            if (Directory.Exists(path))
            {
                var directoryListing = directoryManager.getDirectoryListing(path);
                //Если директория пуста - ее все равно надо отобразить, пустой, как минимум чтобы была строка с подъемом наверх по дереву директорий
                return PartialView(directoryListing);  //передаем список директорий и файлов с путями до _директории_
            }
            else return HttpNotFound();

        }

        /// <summary>
        /// Метод доступа к содержимому файла
        /// </summary>
        /// <param name="DirectoryName">путь к директории, содержащей файл</param>
        /// <param name="FileName">имя файла</param>
        /// <param name="separator">строка, содержащая разделитель столбцов таблицы</param>
        /// <param name="isHeader">признак наличия заголовка таблицы</param>
        /// <returns>Частичное представление с экземпляром класса DataFile или null</returns>
        public ActionResult ParseFile(string DirectoryName, string FileName, string separator, bool isHeader)
        {
            // контролер для частичного представления выдачи директории или файла
            ViewBag.Message = "Это частичное представление с таблицей данных.";
            string path = DirectoryName + FileName;

            DataFile resultDataFile = dataFileManager.readFile(path, separator, isHeader);

           if (resultDataFile != null)
                return PartialView(resultDataFile);  //возвращаем в частичное представление
            else
            {
                //Откроем пустой файл, если он реально пуст или не мог быть прочитан по какой-то причине
                return PartialView(); 
            }
        }

        /// <summary>
        /// Метод сохранения данных в файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="separator">строка, содержащая разделитель столбцов таблицы</param>
        /// <param name="recordSet">экземпляр класса DataFile</param>
        /// <returns>Частичное представление с HTTP кодом результата 200 в случае успеха и 422 в случае ошибки</returns>
        public ActionResult SaveFile(string path, string separator, DataFile recordSet)  //string recordSet
        {
             // контролер для частичного представления выдачи директории или файла
            ViewBag.Message = "Это частичное представление с одной строкой об успехе записи.";

            int result = dataFileManager.writeFile(recordSet, path, separator);  //если найден файл или найдена директория и имя файла есть - работаем

            if (result == 0)
            {
                Response.StatusCode = 200;
                return PartialView();  //получено и сохранено
            } 
            else
            {
                Response.StatusCode = 422;
                return PartialView();  //ухитрились не смочь перезаписать файл.. ну может он был занят
            }  
        }
    }


}