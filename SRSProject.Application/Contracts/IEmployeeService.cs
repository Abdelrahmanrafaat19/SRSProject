using SRSProject.Application.Dtos;
using SRSProject.Domain.Entities;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Contracts
{
    public interface IEmployeeService
    {
        Task<Result<CreateEmployeeDtos>> CreateEmployee(CreateEmployeeDtos data, CancellationToken cancellationToken);
        public  Task<Result<IReadOnlyList<EmployeeEntity>>> GetAllAsync(EmployeeSpecificationParameters parameters,CancellationToken cancellationToken = default);
        public Task<Result<bool>> DeleteEmployeeAsync(DeleteEmployeeDto data);

        Task<Result<EmployeeEntity>> GetEmployeeByIdForAdminAsync(string NationalID);
        Task<Result<EmployeeEntity>> GetEmployeeByIdForEmployeeAsync(int id);
        Task<Result<bool>> UpdateEmployeeAsync(UpdateEmployeeDto data, string role, CancellationToken cancellationToken = default);
    }
}
