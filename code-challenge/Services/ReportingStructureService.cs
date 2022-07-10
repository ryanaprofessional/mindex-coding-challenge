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
        public ReportingStructure GetById(string id) //receives the employee ID and returns ReportingStructure class which contains the employee ID in json format as well as # of reports
        {
            ReportingStructure output = new ReportingStructure();
            output = null;
            try
            {
                if (!String.IsNullOrEmpty(id))
                {
                    output = GetReportStructure(id); // _employeeRepository.GetById(id);
                }
            }
            catch(Exception ex)
            {
                output = null;
            }

            return output;
        }
        private ReportingStructure GetReportStructure(string id)  //receives an employee ID and returns the reporting structure for front end.  ReportingStructure contains the employee ID and the count
        {//RMA 20220708
            ReportingStructure output = new ReportingStructure(); //output of the function, will contain the id of the employee and the 
            List<Employee> employees = new List<Employee>(); //This is the list of employees that we will be iterating through and adding to as we go
            try
            {
                Employee seedEmployee = _employeeRepository.GetFullEmployee(id);  //input employee
                output.EmployeeId = seedEmployee.EmployeeId; //set id for output
                employees.Add(seedEmployee); //Add him to the list

                for (var iter = 0; iter < employees.Count && iter >= 0 && iter < 2147483647; iter++)  //iterate throug the growing list of employees, this will expand as more direct reports are found.  Prevent from going negative || positive overflow.  
                {
                    Employee thisEmployee = employees[iter]; //Get this iterations employee.  Again, Don't use foreach loop here as you can't expand length from inside loop

                    if (thisEmployee.DirectReports != null) //test for null ( do DirectReports exist?, if not skip, if yes, more logic)
                    {
                        foreach (var directReport in thisEmployee.DirectReports)  //if direct reports exist cycle through them.
                        {
                            Employee directReportEmployee = _employeeRepository.GetFullEmployee(directReport.EmployeeId); //Retrieve direct report employee, this returns the full employee record (including more direct reports) for the employee who is under the outer loop employee. 
                            bool employeeAlreadyTested = employees.Any(x => x.EmployeeId == directReport.EmployeeId); //test to see whether or not we already tested this direct report
                            if (!employeeAlreadyTested) //if not already tested
                            {
                                output.NumberOfReports++; //add to number of reports
                                employees.Add(directReportEmployee); //add this directReportEmployee to the list.  We will then test him in the for loop for more direct reports.
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output = null;
            }


            return output;
        }


    }
}
