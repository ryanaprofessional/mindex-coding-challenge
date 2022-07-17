using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
        public Employee GetFullEmployee(string id)  //Generates full employee with first level of direct reports. 
        { //RMA 202207  

            //Edit: 20220717 - original call was incorrect and was putting entire database to memory, new update just pages the database.  
            //               - Also added a little more security with a try catch statement and null checking.

            Employee output = new Employee();
            try
            {
                if (String.IsNullOrEmpty(id)) { output = null; } //if blank, result == null
                {
                    output = (from s in _employeeContext.Employees.Include(nameof(Employee.DirectReports))
                              where s.EmployeeId == id
                              select s).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                output = null;
            }

            return output;
 
        }
    }
}
