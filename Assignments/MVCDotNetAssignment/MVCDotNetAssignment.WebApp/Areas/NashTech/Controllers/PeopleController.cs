using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MVCDotNetAssignment.Application.Services;
using MVCDotNetAssignment.Application.DTOs;
using MVCDotNetAssignment.Domain.Entities;

namespace MVCDotNetAssignment.WebApp.Areas.NashTech.Controllers
{
    [Area("NashTech")]
    [Route("[area]/people")]
    public class PeopleController : Controller
    {
        private readonly IPeopleService _peopleBusinessLogics;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PeopleController(IPeopleService peopleBusinessLogics, IWebHostEnvironment webHostEnvironment)
        {
            _peopleBusinessLogics = peopleBusinessLogics;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("gender/{gender}")]
        public async Task<IActionResult> GetPeopleByGenderAsync(int gender)
        {
            List<Person> people = await _peopleBusinessLogics.GetPeopleByGenderAsync(gender);
            return Json(people);
        }
        [HttpGet("oldest")]
        public async Task<IActionResult> GetOldestPersonAsync(int gender)
        {
            Person person = await _peopleBusinessLogics.GetOldestPersonAsync();
            return Json(person);
        }
        [HttpGet("fullnames")]
        public async Task<IActionResult> GetFullNamesAsync()
        {
            List<FullNameViewModel> fullNames = await _peopleBusinessLogics.GetFullNameAsync();
            return Json(fullNames);
        }
        //To-do: add try catch to catch bad request error for bad query string
        //Do it in middleware, check for Task<IActionResult> Method(int number) -> do tryparse number else return bad request
        //Easier way is to just try catch in the controller action itself. Better for small projects
        [HttpGet("age")]
        public async Task<IActionResult> GetPeopleByYearAsync([FromQuery] string operation, [FromQuery] int year)
        {
            List<Person> people = await _peopleBusinessLogics.GetPeopleByBirthYearAsync(operation, year);
            return Json(people);
        }
    }
}
