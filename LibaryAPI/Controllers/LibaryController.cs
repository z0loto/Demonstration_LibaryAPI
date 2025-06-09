using LibaryAPI.Models;
using LibaryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LibaryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibaryController : ControllerBase
    {
        List<LibraryModel>? result;

        private readonly ILogger<LibaryController> _logger;
        private readonly IMemoryCache _cache;
        private readonly LibaryService _service;

        public LibaryController(ILogger<LibaryController> logger, IMemoryCache cache,LibaryService service)
        {
            _logger = logger;
            _cache = cache;
            _service = service;
        }
        /// <summary>
        /// //вывод всех данных
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            result = _service.GetContent();
            _logger.LogInformation($"Длина массива{result.Count}");
            if (result.Count == 0)
            {
                _logger.LogError("Список пуст(контроллер)");
                return NotFound();
            }
            _logger.LogInformation($"[{result.Count}]");
            return Ok(result);
        }
        /// <summary>
        /// вывод отсортрованных данных по жанру или автору без учета регистра(поиск по части слова)
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        [HttpGet("GetSort/{find}")]
        public ActionResult GetSort(string find)

        {
            result =_service.GetSort(find);
            
            if (result.Count == 0)
            {
                _logger.LogError("Данные не найдены");
                return NotFound();
            }
            _logger.LogInformation($"{result.Count} items");
            return Ok(result);
        }
        /// <summary>
        /// поиск книг по названию без учета регистра(поиск по части слова)
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        [HttpGet("FindBook/{find}")]
        public ActionResult FindBook(string find)
        {
            result= _service.FindBook(find);
            
            if (result.Count == 0)
            {
                _logger.LogError("Ничего не найдено");
                return NotFound();
            }
            _logger.LogInformation("Успех");
            return Ok(result);
        }
        /// <summary>
        /// сортировка по убыванию id
        /// </summary>
        /// <returns></returns>
        [HttpGet("SortDecrease")]
        public ActionResult SortDecrease()
        {
            IEnumerable <LibraryModel> result1 = _service.SortDecrease();
            if (!result1.Any())
            {
                _logger.LogInformation("Массив пуст");
                return BadRequest();
            }
            return Ok(result1);
        }
        /// <summary>
        ///добавление книги
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("PostBook")]
        public IActionResult PutBook([FromBody]LibraryModel book)
        {
            bool success = _service.PutBook(book);
            if (!success)
            {
                _logger.LogInformation("Ничего не получено");
                return BadRequest();
            }
            return Ok();
        }
        /// <summary>
        ///удаление книги
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{name}")]
        public IActionResult DeleteBook(string name) {
        bool success = _service.DeleteBook(name);
            if (!success)
            {
                return BadRequest();
            }
            return Ok();
        }

        //Несколько доп запросов GET
        [HttpGet("GENREplusPAGES")]
        public ActionResult AUTORplusPAGES([FromQuery] string genre, [FromQuery] int pages)//вывод книг с указанным жанром без учета регистра,
         //но название должно быть написано целиком + кол-во страниц должно быть больше указанных
        {
            var result=_service.GenreplusPages(genre, pages);
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }
        /// <summary>
        /// вывод всех авторов у кого есть книги со страницами больше указанного кол-ва
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        [HttpGet("AUTORplusPAGES")]
        public ActionResult AUTORplusPAGES([FromQuery] int pages)

        {
            var result = _service.AutorsPages(pages);
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }
        /// <summary>
        /// сортровка по убыванию по страницам(вывод 3 самых больших книг)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Sort")]
        public ActionResult AUTORplusPAGES()
        {
            var result = _service.SortDesceding();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
