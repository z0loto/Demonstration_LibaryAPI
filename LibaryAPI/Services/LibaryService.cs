using System.Linq;
using LibaryAPI.Controllers;
using LibaryAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LibaryAPI.Services
{
    public class LibaryService
    {
        private readonly IMemoryCache _cache;
        List<LibraryModel> content = new List<LibraryModel>
      {
          new LibraryModel()
          {
              Name="Война и мир",
              Autor="Лев Николаевич Толстой",
              Genre="Роман",
              id=1,
              Pages=100

          },
           new LibraryModel()
          {
              Name="Преступление и наказание",
              Autor="Фёдор Михайлович Достоевский",
              Genre="Роман",
              id=2,
              Pages=300

          },
            new LibraryModel()
          {
              Name="Мастер и Маргарита",
              Autor="Михаил Афанасьевич Булгаков",
              Genre="Роман",
              id = 3,
              Pages=600

          },
                new LibraryModel()
          {
              Name="Белоснежка",
              Autor="Братья Гримм",
              Genre="Сказка",
              id=4,
              Pages=800

          },
                new LibraryModel()
          {
              Name="Красная шапочка",
              Autor="Братья Гримм",
              Genre="Сказка",
              id=5,
              Pages=200

          },
                      new LibraryModel()
          {
              Name="Записки юного врача",
              Autor="Михаил Афанасьевич Булгаков",
              Genre="Роман",
              id=6,
              Pages=100

          }
      };
        private readonly ILogger<LibaryController> _logger;
        public LibaryService(ILogger<LibaryController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        /// <summary>
        /// вывод всех данных
        /// </summary>
        /// <returns></returns>
        public List<LibraryModel> GetContent()
        {
            if (content.Count == 0)
            {
                _logger.LogInformation("Список пуст(сервис)");
            }
            return content;
        }
        /// <summary>
        /// вывод отсортрованных данных по жанру или автору без учета регистра(поиск по части слова)
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        public List<LibraryModel> GetSort(string find)
        {
            var result = content.Where(x => x.Genre.Contains(find, StringComparison.OrdinalIgnoreCase) || x.Autor.Contains(find, StringComparison.OrdinalIgnoreCase)).ToList();
            return result;
        }
        /// <summary>
        /// поиск книг по названию без учета регистра(поиск по части слова) контроллер
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        public List<LibraryModel> FindBook(string find)
        {
            var result = content.Where(item => item.Name.Contains(find, StringComparison.OrdinalIgnoreCase)).ToList();
            return result;
        }
        /// <summary>
        /// сортировка по убыванию id
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LibraryModel> SortDecrease()
        {
            if (content.Count == 0)
            {
                return Enumerable.Empty<LibraryModel>();
            }
            if (_cache.TryGetValue("sorted", out IEnumerable<LibraryModel>? cached))
            {
                
                if (cached != null && cached.Any())
                {
                    _logger.LogInformation("Выведено из кэша");
                    return cached;
                }
            }
            var result = content.OrderByDescending(x => x.id);
            if (result.Any())
            {
                _cache.Set("sorted", result);
            }
            return result;

        }
        /// <summary>
        /// добавление книги
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool PutBook(LibraryModel model)
        {
            if (model == null)
            {
                return false;
            }
            content.Add(model);
            return true;
        }
        /// <summary>
        /// удаление книги
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteBook(string name)
        {
            var delete = content.FirstOrDefault(x => x.Name == name);
            if (delete == default)
            {
                _logger.LogError("Такой книги нет в базе");
                return false;
            }
            _logger.LogInformation($"Книга {name} успешно удалена");
            content.Remove(delete);
            return true;
        }
        //Несколько доп запросов GET
        public IEnumerable<LibraryModel> GenreplusPages(string genre,int pages){//вывод книг с указанным жанром без учета регистра,
            //но название должно быть написано целиком + кол-во страниц должно быть больше указанных
            var result=content.Where(x=>string.Equals(x.Genre,genre,StringComparison.OrdinalIgnoreCase)&&x.Pages>pages).ToList();
            if (result.Count==0)
            {
                _logger.LogInformation("Таких книг нет");
                return result;
            }
            _logger.LogInformation($"Выведены книги жанра: {genre} и со страницами > {pages}");
            return result;
            }
        /// <summary>
        /// вывод всех авторов у кого есть книги со страницами больше указанного кол-ва
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public IEnumerable<string> AutorsPages(int pages)
        {
            var result = content.Where(x=>x.Pages > pages).Select(x=>x.Autor).Distinct().ToList();
            return result;
        }
        /// <summary>
        /// сортровка по убыванию по страницам(вывод 3 самых больших книг)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LibraryModel> SortDesceding()
        {
            var result=content.OrderByDescending(x => x.Pages).Take(3).ToList();
            return result;

        }

    }
}
