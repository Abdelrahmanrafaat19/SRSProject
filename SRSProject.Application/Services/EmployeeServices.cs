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
        private readonly IIdentityService _userManager;

        public EmployeeServices(IUnitOfWork unitOfWork, IIdentityService userManager)
        {
            _uniteOfWork = unitOfWork;
            _userManager = userManager;

        }
        public async Task<Result<CreateEmployeeDtos>> CreateEmployee(CreateEmployeeDtos data, CancellationToken cancellationToken)
        {
            await _uniteOfWork.BeginTransactionAsync(cancellationToken);
            var userExist = await _uniteOfWork.Repository<int, EmployeeEntity>().GetAllAsyncWithSpecification(
                new EmployeeSpecification(data.NationalID),
                cancellationToken);

            if (userExist.Count > 0)
            {
                return Result<CreateEmployeeDtos>.Failure(Error.Failure("Employee.NationalID", "National ID Already Exist"));
            }
            if (data.NationalID is null)
            {
                return Result<CreateEmployeeDtos>.Failure(Error.Failure("Employee.NationalID", "National ID Is Required"));
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

            try
            {
                var employee = new EmployeeEntity
                {
                    FullName = data.FullName,
                    NationalId = data.NationalID,
                    BirthDate = data.BirthDate.Value,
                    BasicSalary = data.BasicSalary.Value,
                    ExpectedCheckInTime =
                        data.ExpectedCheckInTime,
                    ExpectedCheckOutTime =
                        data.ExpectedCheckOutTime,
                    IsActive = true
                };

                await _uniteOfWork.Repository<int, EmployeeEntity>().AddAsync(
                    employee,
                    cancellationToken);

                await _uniteOfWork.SaveChangesAsync(
                    cancellationToken);

                var createUserResult =
                    await _userManager.CreateEmployeeAccountAsync(
                        data.Role,
                        data.FullName,
                        employee.Id,
                        data.NationalID);

                if (!createUserResult.IsSuccess)
                {
                    await _uniteOfWork.RollbackTransactionAsync(
                        cancellationToken);

                   

                    return Result<CreateEmployeeDtos>.Failure( createUserResult.Error);
                }

               

                await _uniteOfWork.CommitTransactionAsync(
                    cancellationToken);

                return Result<CreateEmployeeDtos>.Success(data);
            }
            catch
            {
                await _uniteOfWork.RollbackTransactionAsync(
                    CancellationToken.None);

                return Result<CreateEmployeeDtos>.Failure(
                    Error.Failure("Employee.Create", "An error occurred while creating the employee."));
            }
        }

        public async Task<Result<IReadOnlyList<EmployeeEntity>>> GetAllAsync(EmployeeSpecificationParameters parameters, CancellationToken cancellationToken = default)
        {
            var specification = new EmployeeSpecification(parameters);

            var employees = await _uniteOfWork.Repository<int, EmployeeEntity>().GetAllAsyncWithSpecification(
                    specification,
                    cancellationToken);

            return Result<IReadOnlyList<EmployeeEntity>>
                .Success(employees);
        }
    }
}
