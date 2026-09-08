using Microsoft.AspNetCore.Identity;
using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SRSProject.Infrastructure.DataContext.IDentityEntity
{
    public class UserEntity : IdentityUser
    {
        public EmployeeEntity? Employee { get; set; }
        public int? EmployeeId { get; set; }
        public bool MustChangePassword { get; set; } = true;
    }
}
