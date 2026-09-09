using SRSProject.Application.Dtos.Identity.loginDtos;
using StockManagment.Application.common;


namespace SRSProject.Application.Contracts
{
    public interface IIdentityService
    {
        public Task<Result<PresentationLoginDtos>> LoginAsync(LoginInPutDtos loginDto, CancellationToken cancellationToken);
        Task<Result<string>> CreateEmployeeAccountAsync(
            string Role,
            string name,
           int employeeId,
           string nationalId,
           CancellationToken cancellationToken = default);


        public Task<Result<bool>> ChangePasswordAsync(
            string NationaID,
            string newPassword,
            CancellationToken cancellationToken = default);
    }
}
