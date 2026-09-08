using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos.Identity.loginDtos
{
    public class LoginInPutDtos
    {
        public string NationalID { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
