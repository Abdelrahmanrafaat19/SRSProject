using System.Collections.Generic;

namespace SRSProject.Application.Dtos
{
    public sealed class CreateAdminDto
    {
        public string DisplayName { get; set; } = default!;
        public string NationalId { get; set; } = default!;
        public string? Email { get; set; }
    }

    public sealed class UpdateUserDto
    {
        public string NationalId { get; set; } = default!;
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
    }

    public sealed class UserDto
    {
        public string Id { get; set; } = default!;
        public string NationalId { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public int EmployeeId { get; set; }
        public IReadOnlyList<string> Roles { get; set; } = new List<string>();
    }

    public sealed class UserQueryParameters
    {
        public string? Search { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
