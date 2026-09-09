using SRSProject.Application.Dtos.Identity.loginDtos;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Contracts
{
    public interface IAuthenticationService
    {
        public Task<Result<PresentationLoginDtos>> LoginAsync(LoginInPutDtos loginDto, CancellationToken cancellationToken =default);
        public  Task<Result<bool>> ChangePasswordAsync(string NationalID, string newPassword, CancellationToken cancellationToken = default);
    }
}
