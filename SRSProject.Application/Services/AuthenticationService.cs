using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos.Identity.loginDtos;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;
        public AuthenticationService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public Task<Result<bool>> ChangePasswordAsync(string NationalID, string newPassword, CancellationToken cancellationToken = default)
        {
            return _identityService.ChangePasswordAsync(NationalID, newPassword, cancellationToken);
        }

        public Task<Result<PresentationLoginDtos>> LoginAsync(LoginInPutDtos loginDto, CancellationToken cancellationToken)
        {

            return _identityService.LoginAsync(loginDto, cancellationToken);
        }
    }
}
