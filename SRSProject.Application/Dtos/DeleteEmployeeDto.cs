using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos
{
    public class DeleteEmployeeDto
    {
        public required string NationalID { get; set; }
        public required string Role { get; set; }

    }
}
