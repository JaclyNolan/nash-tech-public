using ClosedXML.Excel;
using MVCDotNetAssignment.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDotNetAssignment.BusinessLogics.Services
{
    public class ExcelService
    {
        public static void CreateExcelFile(List<Person> people, string filePath)
        {
            var workbook = new XLWorkbook();
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
            
            workbook.SaveAs(filePath);
        }
    }
}
