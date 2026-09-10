using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using System.Security.Claims;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceRecordController : BasicController
    {
        private readonly IAttendanceRecordService _attendanceRecordService;
        public AttendanceRecordController(IAttendanceRecordService attendanceRecordService)
        {
            _attendanceRecordService = attendanceRecordService;
        }
        [HttpPost("AddAttendanceRecord")]
        [Authorize]
        public async Task<IActionResult> AddAttendanceRecord()
        {
            var employeeIdValue = User.FindFirstValue("EmployeeId");
            var result = await _attendanceRecordService.CheckInAsync(new CheckInDtos
            {
                EmployeeId = int.Parse(employeeIdValue)
            });
            return HandleResult(result);
        }
       
    }
}
