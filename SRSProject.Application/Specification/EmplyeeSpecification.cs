using SRSProject.Application.Dtos;
using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Specification
{
    public sealed class EmployeeSpecification
        : BaseSpecifications<int, EmployeeEntity>
    {
        public EmployeeSpecification(
            EmployeeSpecificationParameters parameters)
            : base(employee =>
                (
                    string.IsNullOrWhiteSpace(parameters.Search) ||
                    employee.FullName.Contains(parameters.Search!) ||
                    employee.NationalId.Contains(parameters.Search!)
                )
                &&
                (
                    !parameters.IsActive.HasValue ||
                    employee.IsActive ==
                    parameters.IsActive.Value
                )
                &&
                (
                    !parameters.MinimumSalary.HasValue ||
                    employee.BasicSalary >=
                    parameters.MinimumSalary.Value
                )
                &&
                (
                    !parameters.MaximumSalary.HasValue ||
                    employee.BasicSalary <=
                    parameters.MaximumSalary.Value
                ))
        {
            ApplySorting(parameters.Sort);

            EnablePagination(
                pageSize: parameters.PageSize,
                pageIndex: parameters.PageNumber,
                IsPagination: true);
        }

        public EmployeeSpecification(string nationalId)
            : base(employee =>
                employee.NationalId == nationalId)
        {
        }

        private void ApplySorting(string? sort)
        {
            switch (sort?.Trim().ToLowerInvariant())
            {
                case "nameasc":
                    AddOrderByAsyndec(
                        employee => employee.FullName);
                    break;

                case "namedesc":
                    AddOrderByDecendc(
                        employee => employee.FullName);
                    break;

                case "salaryasc":
                    AddOrderByAsyndec(
                        employee => employee.BasicSalary);
                    break;

                case "salarydesc":
                    AddOrderByDecendc(
                        employee => employee.BasicSalary);
                    break;

                case "contractdateasc":
                    AddOrderByAsyndec(
                        employee => employee.ContractDate);
                    break;

                case "contractdatedesc":
                    AddOrderByDecendc(
                        employee => employee.ContractDate);
                    break;

                default:
                    AddOrderByAsyndec(
                        employee => employee.FullName);
                    break;
            }
        }
        
    }
}
