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
    }
}
