using Microsoft.AspNetCore.Mvc;
using MVCDotNetAssignment.BusinessLogics.Services;
using MVCDotNetAssignment.Models.DTOs;
using MVCDotNetAssignment.Models.Entities;

namespace MVCDotNetAssignment.WebApp.Controllers
{
    [Route("people")]
    public class PeopleController : Controller
    {
        private readonly IPeopleBusinessLogics _peopleBusinessLogics;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PeopleController(IPeopleBusinessLogics peopleBusinessLogics, IWebHostEnvironment webHostEnvironment)
        {
            _peopleBusinessLogics = peopleBusinessLogics;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("gender/{gender}")]
        public async Task<IActionResult> GetPeopleByGenderAsync(int gender)
        {
            //To-do: implement if List is null or !any
            List<Person> people = await _peopleBusinessLogics.GetPeopleByGenderAsync(gender);
            return View("ViewJson", people);
        }
        [HttpGet("oldest")]
        public async Task<IActionResult> GetOldestPersonAsync(int gender)
        {
            Person person = await _peopleBusinessLogics.GetOldestPersonAsync();
            return View("ViewObject", person);
        }
        [HttpGet("fullnames")]
        public async Task<IActionResult> GetFullNamesAsync()
        {
            List<FullNameViewModel> fullNames = await _peopleBusinessLogics.GetFullNameAsync();
            return View("ViewJson", fullNames);

        }
        //To-do: add try catch to catch bad request error for bad query string
        //Do it in middleware, check for Task<IActionResult> Method(int number) -> do tryparse number else return bad request
        //Easier way is to just try catch in the controller action itself. Better for small projects
        [HttpGet("age/above/{year}")]
        public async Task<IActionResult> GetPeopleAboveYearAsync(int year)
        {
            List<Person> people = await _peopleBusinessLogics.GetPeopleBirthYearAboveAsync(year);
            return View("ViewJson", people);
        }
        [HttpGet("age/is/{year}")]
        public async Task<IActionResult> GetPeopleIsYearAsync(int year)
        {
            List<Person> people = await _peopleBusinessLogics.GetPeopleBirthYearIsAsync(year);
            return View("ViewJson", people);
        }
        [HttpGet("age/less/{year}")]
        public async Task<IActionResult> GetPeopleLessYearAsync(int year)
        {
            List<Person> people = await _peopleBusinessLogics.GetPeopleBirthYearLessAsync(year);
            return View("ViewJson", people);
        }
        [HttpGet("download/excel")]
        public async Task<IActionResult> GetExcelFile()
        {
            List<Person> people = await _peopleBusinessLogics.GetPeopleAsync();
            string fileName = "People.xlsx";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "ExportedFiles", fileName);

            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            ExcelService.CreateExcelFile(people, filePath);
            return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
