using StockManagment.Application.common;
using SRSProject.Application.Dtos;
using System.Collections.Generic;

namespace SRSProject.Application.Contracts
{
    public interface IUserService
    {
        Task<Result<string>> CreateAdminAsync(CreateAdminDto data, CancellationToken cancellationToken = default);

        Task<Result<bool>> UpdateUserAsync(UpdateUserDto data, CancellationToken cancellationToken = default);

        Task<Result<bool>> SetActiveAsync(string nationalId, bool isActive, CancellationToken cancellationToken = default);

        Task<Result<bool>> AssignRolesAsync(string nationalId, IEnumerable<string> roles, CancellationToken cancellationToken = default);

        Task<Result<IReadOnlyList<UserDto>>> GetUsersAsync(UserQueryParameters parameters, CancellationToken cancellationToken = default);
    }
}
