using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using System;


namespace challenge.Controllers
{
    [Route("api/reportingstructure")]
    public class ReportingStructureController : Controller //handles all front end ReportingStructure calls
    {
        // GET: ReportingStructureController
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService) //constructor
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{id}", Name = "getReportStructureById")]
        public IActionResult GetReportStructureById(String id) //retrieves the reportingStructure based on employee ID
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var reportingStructure = _reportingStructureService.GetById(id);  //get the reporting structure and number of reports from db

            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }




    }
}
