using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using MVCDotNetAssignment.BusinessLogics.Services;
using MVCDotNetAssignment.Models.DTOs;
using MVCDotNetAssignment.Models.Entities;
using ClosedXML.Excel;

namespace MVCDotNetAssignment.WebApp.Controllers
{
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        private readonly IPeopleBusinessLogics _peopleBusinessLogics;
        private readonly IWebHostEnvironment _webHostEnvironment;
          public PeopleController(IPeopleBusinessLogics peopleBusinessLogics, IWebHostEnvironment webHostEnvironment)
        {
            _peopleBusinessLogics = peopleBusinessLogics;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            List<Person> people = await _peopleBusinessLogics.GetPeopleAsync();

            switch (sortOrder)
            {
                case "name_desc":
                    people = people.OrderByDescending(s => s.FirstName).ToList();
                    break;
                case "Date":
                    people = people.OrderBy(s => s.DoB).ToList();
                    break;
                case "date_desc":
                    people = people.OrderByDescending(s => s.DoB).ToList();
                    break;
                default:
                    people = people.OrderBy(s => s.FirstName).ToList();
                    break;
            }

            return View("List", people);
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(
            "FirstName,LastName,Gender,DoB,Birthplace,PhoneNumber,IsGraduated")] Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _peopleBusinessLogics.CreatePersonAsync(person);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(person);
        }

        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(string id)
        {
            Person? person = await _peopleBusinessLogics.GetPersonAsync(id);
            if (person == null) return NotFound();
            return View(person);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind(
            "FirstName,LastName,Gender,DoB,Birthplace,PhoneNumber,IsGraduated")] Person person)
        {
            Person? personExist = await _peopleBusinessLogics.GetPersonAsync(id);
            if (personExist == null) return NotFound();
            try
            {
                if (ModelState.IsValid)
                {

                    await _peopleBusinessLogics.UpdatePersonAsync(id, person);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(person);
        }

        [HttpGet("Details/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            Person? person = await _peopleBusinessLogics.GetPersonAsync(id);
            if (person == null) return NotFound();
            return View(person);
        }

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                Person? person = await _peopleBusinessLogics.GetPersonAsync(id);
                if (person == null) return NotFound();
                await _peopleBusinessLogics.DeletePersonAsync(id);
            } catch
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            
            return View();
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

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("People");

                var person1 = new Person();

                int column = 1;
                worksheet.Cell(1, column++).Value = nameof(person1.FirstName);
                worksheet.Cell(1, column++).Value = nameof(person1.LastName);
                worksheet.Cell(1, column++).Value = nameof(person1.Gender);
                worksheet.Cell(1, column++).Value = nameof(person1.DoB);
                worksheet.Cell(1, column++).Value = nameof(person1.Birthplace);
                worksheet.Cell(1, column++).Value = nameof(person1.PhoneNumber);
                worksheet.Cell(1, column++).Value = nameof(person1.Age);
                worksheet.Cell(1, column++).Value = nameof(person1.IsGraduated);

                int row = 2;
                foreach (Person person in people)
                {
                    column = 1;
                    worksheet.Cell(row, column++).Value = person.FirstName;
                    worksheet.Cell(row, column++).Value = person.LastName;
                    worksheet.Cell(row, column++).Value = person.Gender.ToString();
                    worksheet.Cell(row, column++).Value = person.DoB.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, column++).Value = person.Birthplace;
                    worksheet.Cell(row, column++).Value = person.PhoneNumber;
                    worksheet.Cell(row, column++).Value = person.Age;
                    worksheet.Cell(row, column++).Value = person.IsGraduated.ToString();

                    row++;
                }

                byte[] excelData;
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    excelData = memoryStream.ToArray();
                }

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}