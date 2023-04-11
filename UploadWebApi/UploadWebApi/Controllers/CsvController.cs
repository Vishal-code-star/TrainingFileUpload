using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YourNamespace
{
    [ApiController]
    [Route("[controller]")]
    public class CsvController : ControllerBase
    {

        
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            Console.WriteLine("hi");
            if (file == null || file.Length <= 0)
            {
                return BadRequest("File is empty.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            var filePath = Path.Combine(uploadsFolder, file.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<MyCsvModelMap>();
            var records = csv.GetRecords<MyCsvModel>().ToList();

            if (records == null || records.Count == 0)
            {
                return BadRequest("CSV file is empty.");
            }

            var json = JsonSerializer.Serialize(records);

            // Save the file in the "uploads" folder
            var outputFile = Path.Combine(uploadsFolder, "output.json");
            await System.IO.File.WriteAllTextAsync(outputFile, json);

            /*return Content(json, "application/json");*/
            return Content("File uploaded succesfully");
        
    }


        /* [HttpPost("upload")]
         public async Task<IActionResult> Upload(IFormFile file)
         {
             if (file == null || file.Length <= 0)
             {
                 return BadRequest("File is empty.");
             }

             using var reader = new StreamReader(file.OpenReadStream());
             using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
             csv.Context.RegisterClassMap<MyCsvModelMap>();
             var records = csv.GetRecords<MyCsvModel>().ToList();
             var json = JsonSerializer.Serialize(records);
             return Content(json, "application/json");
         }*/

        [HttpGet("output")]
        public IActionResult GetOutputFile()
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var outputFile = Path.Combine(uploadsFolder, "output.json");

            if (!System.IO.File.Exists(outputFile))
            {
                return NotFound();
            }

            var json = System.IO.File.ReadAllText(outputFile);
            return Content(json, "application/json");
        }
    }

    public class MyCsvModel
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
       /* public string Country { get; set; }
        public string MaritalStatus { get;set; }

        public string City { get; set; }*/
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Course { get; set; }
        public int TenthPercent { get; set; }
        public int TwelfthPercent { get; set; }
        /*public int DropYears { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }*/
        /*public string GuardianPhoneNUmebr { get; set; }*/

    }

    public sealed class MyCsvModelMap : ClassMap<MyCsvModel>
    {
        public MyCsvModelMap()
        {

            Map(m => m.Id).Name("ID");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Email).Name("Email");
            Map(m => m.DOB).Name("Date of Birth");
            Map(m => m.Gender).Name("Gender");
           /* Map(m => m.Country).Name("Country");
            Map(m => m.MaritalStatus).Name("Marital Status");
            Map(m => m.City).Name("City");*/
            Map(m => m.State).Name("State");
            Map(m => m.Pincode).Name("Pincode");
            Map(m => m.Address).Name("Address");
            Map(m => m.PhoneNumber).Name("Phone Number");
            Map(m => m.Course).Name("Course");
            Map(m => m.TenthPercent).Name("10th Percentage");
            Map(m => m.TwelfthPercent).Name("12th Percentage");
            /*Map(m => m.DropYears).Name("Drop Years");
            Map(m => m.FatherName).Name("Father's Name");
            Map(m => m.MotherName).Name("Mother's Name");*/
            /*Map(m => m.GuardianPhoneNUmebr).Name("Guardian's Phone Number");*/
        }
    }
}

