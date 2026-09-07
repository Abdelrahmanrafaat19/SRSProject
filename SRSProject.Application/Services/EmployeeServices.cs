using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using SRSProject.Application.Specification;
using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Services
{
    public class EmployeeServices : IEmployeeService
    {
        private readonly IUnitOfWork _uniteOfWork;

        public EmployeeServices( IUnitOfWork unitOfWork)
        {
            _uniteOfWork = unitOfWork;
            
        }
        public async Task<Result<CreateEmployeeDtos>> CreateEmployee(CreateEmployeeDtos data, CancellationToken cancellationToken)
        {
              if(data.NationalID is null)
              {
                    return Result<CreateEmployeeDtos>.Failure(Error.Failure("Employee.NationalID" , "National ID Is Required"));
              }

                if (data.FullName is null)
                {
                    return Result<CreateEmployeeDtos>.Failure(Error.Failure("Employee.FullName", "FullName Is Required"));
                }

            if (data.BirthDate is null)
            {
                return Result<CreateEmployeeDtos>.Failure(Error.Failure("Employee.BirthDate", "BirthDate Is Required"));
            }

            if (data.BasicSalary is null)
            {
                return Result<CreateEmployeeDtos>.Failure(Error.Failure("Employee.BasicSalary", "BasicSalary Is Required"));
            }
            else if (data.BasicSalary <= 0) 
            {
                return Result<CreateEmployeeDtos>.Failure(Error.Failure("Employee.BasicSalary", "BasicSalary Must Be Greater than Zero"));
            }


            var newEmployee = new EmployeeEntity
            {
                FullName = data.FullName,
                NationalId = data.NationalID,
                BirthDate = data.BirthDate.Value,
                BasicSalary = data.BasicSalary.Value,
                ExpectedCheckInTime =data.ExpectedCheckInTime,
                ExpectedCheckOutTime = data.ExpectedCheckOutTime,
                IsActive = true
            };

            await _uniteOfWork.Repository<int , EmployeeEntity>().AddAsync(newEmployee, cancellationToken);

            var result = await _uniteOfWork.SaveChangesAsync();
            if(result > 0)
            {
                return Result<CreateEmployeeDtos>.Success(data);
            }
            else
            {
                return Result<CreateEmployeeDtos>.Failure(Error.Failure("Employee.Create", "Failed to create employee"));
            }
        }

        public async Task<Result<IReadOnlyList<EmployeeEntity>>> GetAllAsync(EmployeeSpecificationParameters parameters, CancellationToken cancellationToken = default)
        {
            var specification =
        new EmployeeSpecification(parameters);

            var employees = await _uniteOfWork.Repository<int, EmployeeEntity>().GetAllAsyncWithSpecification(
                    specification,
                    cancellationToken);

            return Result<IReadOnlyList<EmployeeEntity>>
                .Success(employees);
        }
    }
}
