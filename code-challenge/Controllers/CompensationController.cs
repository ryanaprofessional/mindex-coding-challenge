using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller //Controller for handling compensation tasks, part of task #2
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService) //constructor & dependency injection
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation) //Creates a Compensation Record
        {

            _compensationService.Create(compensation); //create the record in the compensation DB
            if(compensation != null)
            {
                return CreatedAtRoute("getCompensationById", new { id = compensation.CompensationId }, compensation);
            } else
            {
                return NotFound();
            }

        }

        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id) //Returns a compensation record
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var compensation = _compensationService.GetById(id); //retrieve record

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
