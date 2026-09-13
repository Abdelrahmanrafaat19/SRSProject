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
        [HttpPost("AddAttendanceRecordCheckIn")]
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


        [HttpPost("AddAttendanceRecordCheckOut")]
        [Authorize]
        public async Task<IActionResult> AddAttendanceRecordCheckOut()
        {
            var employeeIdValue = User.FindFirstValue("EmployeeId");
            var result = await _attendanceRecordService.CheckOutAsync(new CheckOutDtos
            {
                EmployeeId = int.Parse(employeeIdValue)
            });
            return HandleResult(result);
        }


        [HttpGet("GetAttendanceReportForEmployee")]
        [Authorize]
        public async Task<IActionResult> GetAttendanceReport(DateOnly? from, DateOnly? to)
        {
            var employeeIdValue = User.FindFirstValue("EmployeeId");
            var result = await _attendanceRecordService.GetReportForEmployeeAsync(int.Parse(employeeIdValue), from, to);
            return HandleResult(result);
        }

        [HttpGet("GetAttendanceReportForAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAttendanceReportForAdmin(DateOnly? from, DateOnly? to)
        {
            var result = await _attendanceRecordService.GetReportForAdminAsync(null, from, to);
            return HandleResult(result);
        }

    }
}
