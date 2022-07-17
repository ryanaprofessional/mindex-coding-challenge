using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using challenge.Repositories;
using challenge.Services;
using Microsoft.Extensions.Logging;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService //Handles business logic for task#1
    { //this service retrieves the reporting structure
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public ReportingStructureService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository) //constructor
        {
            _employeeRepository = employeeRepository; //dependency injection
            _logger = logger;
        }
        public ReportingStructure GetById(string id) //receives the employee ID and returns ReportingStructure class which contains the employee ID in json format as well as # of reports.  Seed Employee is the id that you are passing to the function, if you pass a bad id you will receive null result
        {
            ReportingStructure output = new ReportingStructure(); //The output of this function, will be set to null if any errors.
            try
            {
                if (String.IsNullOrEmpty(id)) { output = null; } else { //if blank id then return null.
                    Employee seedEmployee = _employeeRepository.GetFullEmployee(id); //Get the employee ID
                    if (seedEmployee == null) { return null; } else { //If we never found an employee, do not enter this logic block.

                        Dictionary<string, int> dict = new Dictionary<string, int>(); //this is where the return of the GetReportStructure method ends up.  It is a dictionary which will contain all of the employeeIDs whom were investigated as well as a count of their direct reoprts. In the end, we calculate sum to determine the total number of direct reports
                        dict.Add(seedEmployee.EmployeeId, seedEmployee.DirectReports.Count); //we will first add our seed employee to the list. This way, if the functions returns nothing new, we aren't returning blank. 

                        output.NumberOfReports = GetReportStructure(seedEmployee, dict).Values.Sum(); // Recursive function which will get our full list of employees underneath the seed and their direct reports. Calculate the sum for the total.
                        output.EmployeeId = id; //Id of our SeedEmployee
                    }

                }
            }
            catch(Exception ex)
            {
                output = null;
            }
            return output;
        }
        private Dictionary<String, int> GetReportStructure(Employee inputEmployee, Dictionary<string, int> inputEmployeesAlreadyTested)
        { //RMA202217
          //Recursive function which returns list of all employees under an original seed employee, and the count of their direct reports
                if (inputEmployee.DirectReports != null && inputEmployee.DirectReports.Count > 0 && inputEmployee != null) { //if there are even direct reports && id is valid, otherwise, no use. 
                    foreach (var directReport in inputEmployee.DirectReports) //foreach directReport in employee
                    {
                        if((inputEmployeesAlreadyTested.ContainsKey(directReport.EmployeeId)) || (directReport.EmployeeId == inputEmployee.EmployeeId)) { return inputEmployeesAlreadyTested; } // 1) Did we already test this employee?  Yes == skip, No == continue 2) Are we dealing with a possible recursive reference, i.e. an employee has himself as a direct report. if Yes == Skip, No == continue....You can not test for directReports yet because you don't have them.
                        //ELSE
                        Employee nextEmployee = _employeeRepository.GetFullEmployee(directReport.EmployeeId); //Retrieve the Employee from the database.  Why not just use the directReport?  Because we need a full object.  
                        if (nextEmployee.DirectReports != null && nextEmployee.DirectReports.Count > 0 && !inputEmployeesAlreadyTested.ContainsKey(nextEmployee.EmployeeId)) //if even direct reports, continue
                        {
                            inputEmployeesAlreadyTested.Add(nextEmployee.EmployeeId, nextEmployee.DirectReports.Count); //Add Employee to our list
                            inputEmployeesAlreadyTested = GetReportStructure(nextEmployee, inputEmployeesAlreadyTested); //Send that employee in to interrogate direct reports
                        }
                    }
                }
            return inputEmployeesAlreadyTested;
        }

    }
}
