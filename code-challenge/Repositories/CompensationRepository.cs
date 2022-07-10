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
    public class CompensationRepository : ICompensationRepository //DataAccess handler for Task#2
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation) //creates new compensation record
        {//RMA 20220710
            compensation.CompensationId = Guid.NewGuid().ToString(); //create key
            _compensationContext.Compensation.Add(compensation); //add to db
            return compensation; 
        }

        public Compensation GetById(string id) //retrieve the compensation record
        {//RMA 20220710
            var output = _compensationContext.Compensation.ToList().Single(a => a.Employee == id); //retrieves from db based on employee id
            return output;
        }
        public Task SaveAsync() //save the database
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
