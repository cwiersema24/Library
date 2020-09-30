using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class DemoController : ControllerBase
        
    {
        IConfiguration _config;

        public DemoController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("employees/{employeeId:int}", Name ="employees#get")]
        public ActionResult LookupEmployee([FromRoute] int employeeId)
        {
            var response = new GetEmployeeDetailsResponse
            {
                Id = employeeId,
                Name = "Joe Schmidt",
                Department = "Dev",
                StartingSalary = 50000
            };
            return Ok(response);
        }
        [HttpGet("blogs/{year:int}/{month:int:range(1,12)}/{day:int:range(1,31)}")]
        public ActionResult GetBlogPosts([FromRoute] int year, [FromRoute] int month, [FromRoute] int day)
        {
            return Ok($" Getting the blogs for {year}/{month}/{day}");
        }

        [HttpGet("employees")]
        public ActionResult GetEmployees([FromQuery] string department ="All", [FromQuery] decimal minSalary = 0)
        {
            return Ok($"Returning all employees from {department} with a minimum salary of {minSalary:c}");
        }

        [HttpGet("whoami")]
        public ActionResult ShowUserAgent([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok($"I see your eunning {userAgent}");
        }

        [HttpPost("employees")]
        public ActionResult Hire([FromBody] PostEmployeesCreate employeeToHire)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GetEmployeeDetailsResponse
            {
                Id = new Random().Next(50, 10000),
                Name = employeeToHire.Name,
                Department = employeeToHire.Department,
                StartingSalary = employeeToHire.StartingSalary
            };
            return CreatedAtRoute("employees#get", new { employeeId = response.Id }, response);
        }
        [HttpGet("message")]
        public ActionResult GetMessage()
        {
            var msg = _config.GetValue<String>("message");
            return Ok($"The message is {msg}");
        }
    }

    public class PostEmployeesCreate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Department { get; set; }
        [Range(10000,200000)]
        public decimal StartingSalary { get; set; }
    }
    public class GetEmployeeDetailsResponse
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Department { get; set; }
        [Range(10000, 200000)]
        public decimal StartingSalary { get; set; }
    }
}
