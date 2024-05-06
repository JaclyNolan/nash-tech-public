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

            int column = 1;
            worksheet.Cell(1, column++).Value = "First Name";
            worksheet.Cell(1, column++).Value = "Last Name";
            worksheet.Cell(1, column++).Value = "Gender";
            worksheet.Cell(1, column++).Value = "Date of Birth";
            worksheet.Cell(1, column++).Value = "Birthplace";
            worksheet.Cell(1, column++).Value = "Phone Number";
            worksheet.Cell(1, column++).Value = "Age";
            worksheet.Cell(1, column++).Value = "Is Graduated";

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
