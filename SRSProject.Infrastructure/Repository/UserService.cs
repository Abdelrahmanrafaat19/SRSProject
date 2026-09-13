using Microsoft.AspNetCore.Identity;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using SRSProject.Infrastructure.DataContext.IDentityEntity;
using StockManagment.Application.common;


namespace SRSProject.Infrastructure.Repository
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<string>> CreateAdminAsync(CreateAdminDto data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var existing = await _userManager.FindByNameAsync(data.NationalId);
            if (existing != null)
                return Result<string>.Failure(Error.Validation("User.Create", "National ID already exists"));

            var user = new UserEntity
            {
                DisplayName = data.DisplayName,
                UserName = data.NationalId,
                Email = data.Email ?? string.Empty,
                EmployeeId = 0,
                MustChangePassword = true
            };

            var createResult = await _userManager.CreateAsync(user, "123");
            if (!createResult.Succeeded)
                return Result<string>.Failure(Error.Failure("Identity.CreateFailed", "Failed to create admin user"));

            var role = "Admin";
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, role);
            if (!addRoleResult.Succeeded)
                return Result<string>.Failure(Error.Failure("Identity.RoleAssignmentFailed", "Failed to assign admin role"));

            return Result<string>.Success(user.Id);
        }

        public async Task<Result<bool>> UpdateUserAsync(UpdateUserDto data, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(data.NationalId);
            if (user == null)
                return Result<bool>.Failure(Error.NotFound("User.Update", "User not found"));

            if (!string.IsNullOrWhiteSpace(data.DisplayName))
                user.DisplayName = data.DisplayName;
            if (!string.IsNullOrWhiteSpace(data.Email))
                user.Email = data.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result<bool>.Failure(Error.Failure("User.Update", "Failed to update user"));

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> SetActiveAsync(string nationalId, bool isActive, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(nationalId);
            if (user == null)
                return Result<bool>.Failure(Error.NotFound("User.SetActive", "User not found"));

            if (isActive)
            {
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnd = System.DateTimeOffset.MaxValue;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result<bool>.Failure(Error.Failure("User.SetActive", "Failed to update user active state"));

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> AssignRolesAsync(string nationalId, IEnumerable<string> roles, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(nationalId);
            if (user == null)
                return Result<bool>.Failure(Error.NotFound("User.AssignRoles", "User not found"));

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return Result<bool>.Failure(Error.Failure("User.AssignRoles", "Failed to remove existing roles"));

            foreach (var role in roles.Distinct())
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var addResult = await _userManager.AddToRolesAsync(user, roles.Distinct());
            if (!addResult.Succeeded)
                return Result<bool>.Failure(Error.Failure("User.AssignRoles", "Failed to add roles"));

            return Result<bool>.Success(true);
        }

        public async Task<Result<IReadOnlyList<UserDto>>> GetUsersAsync(UserQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters?.Search))
            {
                var s = parameters.Search.Trim();
                query = query.Where(u => u.UserName.Contains(s) || u.DisplayName.Contains(s) || u.Email.Contains(s));
            }

            if (parameters?.Page.HasValue == true && parameters.PageSize.HasValue)
            {
                var skip = (parameters.Page.Value - 1) * parameters.PageSize.Value;
                query = query.Skip(skip).Take(parameters.PageSize.Value);
            }

            var users = query.ToList();
            var result = new List<UserDto>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(new UserDto
                {
                    Id = u.Id,
                    NationalId = u.UserName,
                    DisplayName = u.DisplayName,
                    Email = u.Email,
                    IsActive = !(u.LockoutEnd.HasValue && u.LockoutEnd > System.DateTimeOffset.UtcNow),
                    EmployeeId = u.EmployeeId,
                    Roles = roles.ToList()
                });
            }

            return Result<IReadOnlyList<UserDto>>.Success(result);
        }
    }
}
