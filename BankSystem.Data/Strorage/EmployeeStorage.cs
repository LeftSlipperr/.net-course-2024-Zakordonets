using BankSystem.App.Interfaces;
using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ClientStorage;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure
{
    public class EmployeeStorage : IStorage<Employee, List<Employee>>
    {
        private readonly BankSystemDbContext _bankSystemDbContext;

        public EmployeeStorage(BankSystemDbContext bankSystemDbContext)
        {
            _bankSystemDbContext = bankSystemDbContext;
        }

        public async Task<List<Employee>> GetAsync(Guid id)
        {
            var employee =  await _bankSystemDbContext.Employees
                .Where(e => e.Id == id)
                .ToListAsync();

            return employee;
        }

        public async Task AddAsync(Employee employee)
        {
            await _bankSystemDbContext.Employees.AddAsync(employee);
            await _bankSystemDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            var existingEmployee = await _bankSystemDbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Age = employee.Age;
                existingEmployee.PasNumber = employee.PasNumber;
                existingEmployee.PhoneNumber = employee.PhoneNumber;
                existingEmployee.Contract = employee.Contract;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.IsOwner = employee.IsOwner;

                await _bankSystemDbContext.SaveChangesAsync();
            }
        }

        public async Task  DeleteAsync(Guid id)
        {
            var employee = await _bankSystemDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
                _bankSystemDbContext.Employees.Remove(employee);
                await _bankSystemDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Employee>> GetEmployeesByParameters(
            string name = null, string secondName = null, string thirdName = null,
            string phoneNumber = null, string pasNumber = null,
            int? age = null, string contract = null, decimal? salary = null,
            bool? isOwner = null, int pageNumber = 1, int pageSize = 10, string sortBy = "Name")
        {
            var query = _bankSystemDbContext.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(name)) query = query.Where(e => e.Name.Contains(name));
            if (!string.IsNullOrEmpty(secondName)) query = query.Where(e => e.SecondName.Contains(secondName));
            if (!string.IsNullOrEmpty(thirdName)) query = query.Where(e => e.ThirdName.Contains(thirdName));
            if (!string.IsNullOrEmpty(phoneNumber)) query = query.Where(e => e.PhoneNumber.Contains(phoneNumber));
            if (!string.IsNullOrEmpty(pasNumber)) query = query.Where(e => e.PasNumber == pasNumber);
            if (age.HasValue) query = query.Where(e => e.Age == age.Value);
            if (!string.IsNullOrEmpty(contract)) query = query.Where(e => e.Contract.Contains(contract));
            if (salary.HasValue) query = query.Where(e => e.Salary == salary.Value);
            if (isOwner.HasValue) query = query.Where(e => e.IsOwner == isOwner.Value);

            if (sortBy == "Name")
            {
                query = query.OrderBy(e => e.Name);
            }
            else if (sortBy == "Age")
            {
                query = query.OrderBy(e => e.Age);
            }
            else if (sortBy == "Salary")
            {
                query = query.OrderBy(e => e.Salary);
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
