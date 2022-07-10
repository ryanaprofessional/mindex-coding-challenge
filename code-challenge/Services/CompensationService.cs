using System;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class CompensationService : ICompensationService //handles the business logic for the compensationtype
    {
        private readonly ICompensationRepository _compensationRepository; //Data Access for compensation db
        private readonly IEmployeeRepository _employeeRepository; //employee db access for retrieving the full employee
        private readonly ILogger<CompensationService> _logger;
        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository, IEmployeeRepository employeeRepository) //constructor 
        {
            _compensationRepository = compensationRepository; //dependency injection
            _employeeRepository = employeeRepository; // ^^
            _logger = logger;
        }

        public Compensation Create(Compensation compensation) //adds a new compensation object to the db
        {//RMA 20220709
            try
            {
                if (compensation != null)
                {
                    compensation.CompensationId = Guid.NewGuid().ToString(); //generate the id for the object
                    _compensationRepository.Add(compensation); //Add the object to the database
                    _compensationRepository.SaveAsync().Wait(); //Save the database
                }
            }
            catch(Exception ex)
            {
                compensation = null;
            }
            return compensation; //return result;
        }
        public Compensation GetById(string id) //return the compensation object based on EmployeeID
        {//RMA 20220709
            Compensation output = new Compensation();
            try
            {
                output = _compensationRepository.GetById(id);
            }
            catch (Exception ex)
            {
                output = null;
                
            }
            return output;
        }
    }
}
