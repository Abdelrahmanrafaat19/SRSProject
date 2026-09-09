using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos.Identity
{
    public class ChangePasswordDto
    {
        public string NationID { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
