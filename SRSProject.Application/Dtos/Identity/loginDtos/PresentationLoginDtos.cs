using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos.Identity.loginDtos
{
    public class PresentationLoginDtos
    {
        public string ID { get; set; } = default!;
        public string NationalID { get; set; } = default!;

        public string DisplayName { get; set; } = default!;

        public string Token { get; set; } = default!;
    }
}
