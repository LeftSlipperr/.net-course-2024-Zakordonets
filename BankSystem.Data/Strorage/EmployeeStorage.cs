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

        public List<Employee> Get(Guid id)
        {
            var employee = _bankSystemDbContext.Employees
                .Where(e => e.Id == id)
                .ToList();

            return employee;
        }

        public void Add(Employee employee)
        {
            _bankSystemDbContext.Employees.Add(employee);
            _bankSystemDbContext.SaveChanges();
        }

        public void Update(Employee employee)
        {
            var existingEmployee = _bankSystemDbContext.Employees
                .FirstOrDefault(e => e.Id == employee.Id);

            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Age = employee.Age;
                existingEmployee.PasNumber = employee.PasNumber;
                existingEmployee.PhoneNumber = employee.PhoneNumber;
                existingEmployee.Contract = employee.Contract;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.IsOwner = employee.IsOwner;

                _bankSystemDbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var employee = _bankSystemDbContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                _bankSystemDbContext.Employees.Remove(employee);
                _bankSystemDbContext.SaveChanges();
            }
        }

        public List<Employee> GetAllEmployees()
        {
            return _bankSystemDbContext.Employees.ToList();
        }
    }
}
