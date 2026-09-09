using Microsoft.AspNetCore.Identity;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos.Identity.loginDtos;
using SRSProject.Infrastructure.DataContext.IDentityEntity;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.Repository
{
    public class IdentityRepo : IIdentityService
    {
        private const string DefaultPassword = "123";
        private readonly UserManager<UserEntity> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IJwtCreator _jwtCreator;
        public IdentityRepo(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager, IJwtCreator jwtCreator)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._jwtCreator = jwtCreator;
        }

        public async Task<Result<bool>> ChangePasswordAsync(string NationalID, string newPassword, CancellationToken cancellationToken = default)
        {
            var userIsExit = await userManager.FindByNameAsync(NationalID);

            if (userIsExit is null)
            {
                return Result<bool>.Failure(
                      Error.NotFound(
                          "ErrorType.UserNotFound",
                          "User Not Exist"
                      )
                    );

            }


            var passwordChanged = await userManager.ChangePasswordAsync(userIsExit, "123", newPassword);
            if (!passwordChanged.Succeeded)
            {
                return Result<bool>.Failure(
                    Error.Failure(
                        "Identity.PasswordChangeFailed",
                        "Failed to change password"
                    )
                );
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<string>> CreateEmployeeAccountAsync(string Role, string name, int employeeId, string nationalId, CancellationToken cancellationToken = default)
        {

            cancellationToken.ThrowIfCancellationRequested();

            var user = new UserEntity
            {
                DisplayName = name,
                UserName = nationalId,
                EmployeeId = employeeId,
                MustChangePassword = true
            };

            var creationResult =
                await userManager.CreateAsync(
                    user,
                    DefaultPassword);

            if (!creationResult.Succeeded)
            {
                return Result<string>.Failure(
                    Error.Failure(
                        "Identity.CreateFailed",
                        "The User Result Not Created "));
            }
            var roleIsExits = await roleManager.RoleExistsAsync(Role);
            if (!roleIsExits)
            {
                await roleManager.CreateAsync(new IdentityRole(Role));
            }
            var roleResult =
                await userManager.AddToRoleAsync(
                    user,
                    Role);

            if (!roleResult.Succeeded)
            {
                return Result<string>.Failure(
                    Error.Failure(
                        "Identity.RoleAssignmentFailed",
                        "Role Assignment Failed "));
            }

            return Result<string>.Success(user.Id);
        }

        public async Task<Result<PresentationLoginDtos>> LoginAsync(LoginInPutDtos loginDto, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(loginDto.NationalID);

            if (user is null)
            {
                return Result<PresentationLoginDtos>.Failure(
                    Error.Failure(
                        "Identity.LoginFailed",
                        "Invalid National ID "));
            }



            var passwordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!passwordValid)
            {
                return Result<PresentationLoginDtos>.Failure(
                    Error.Failure(
                        "Identity.LoginFailed",
                        "Password is not correct "));
            }
            var roles = await userManager.GetRolesAsync(user);
            var token = _jwtCreator.CreateToken(user.Email, user.DisplayName, user.Id, roles ?? Array.Empty<string>(), cancellationToken);

            var result = new PresentationLoginDtos
            {
                ID = user.Id,
                NationalID = user.UserName,
                DisplayName = user.DisplayName,
                Token = token
            };


            return Result<PresentationLoginDtos>.Success(result);

        }
    }
}
