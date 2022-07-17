using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup] 
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

     
        [TestMethod]
        public void GetReportStructure_Returns_OK()
        { //RMA 202207
            string employeeId = "62c1084e-6e34-4630-93fd-9153afb65309"; //employee with no directReports
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;
            var reportingStructure = response.DeserializeContent<ReportingStructure>();

            Assert.IsNotNull(reportingStructure.NumberOfReports);
            Assert.IsNotNull(reportingStructure.EmployeeId);

        }
        [TestMethod]
        public void GetReportStructure_Returns_NotFound()
        { //RMA 202207
            string employeeId = "Invalid_ID"; //employee with no directReports
            var getRequestTask = _httpClient.GetAsync($"/api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }
        [TestMethod]
        public void GetReportStructure_Test_SelfReport_References()
        {//RMA 20220716
            //This test retrieves the reporting structure of an individual who is referencing himself as a direct report,
            //an employee who directly references himself SHOULD have a zero in their numberOfReports
            string employeeIdSelfDirectReport = "akdjfdd-16bd-axvvc-bddbd-allld883"; //employee with self as direct report
            var getRequestTask = _httpClient.GetAsync($"/api/reportingstructure/{employeeIdSelfDirectReport}");
            var response = getRequestTask.Result;
            var reportingStructure = response.DeserializeContent<ReportingStructure>();

            Assert.AreEqual(reportingStructure.NumberOfReports, 0);

        }
        [TestMethod]
        public void GetReportStructure_Test_SecondarySelfReport_References()
        {//RMA 20220716
            //This test retrieves the reporting structure of an individual who is referencing a direct report that is then referencing themself (themself == direct report, not original member) as a direct report, an employee who is referencing himself should have zero, but the parent (whom we are testing) will have one.
            string employeeIdSelfDirectReport = "afjdj0-7ohy8-oopk-a1a12-del6tyydj"; //employee with direct report who then self direct reports
            var getRequestTask = _httpClient.GetAsync($"/api/reportingstructure/{employeeIdSelfDirectReport}");
            var response = getRequestTask.Result;
            var reportingStructure = response.DeserializeContent<ReportingStructure>();

            Assert.AreEqual(reportingStructure.NumberOfReports, 1);

        }
        [TestMethod]
        public void GetReportStructure_Test_SecondaryRecursive_reference()
        {//RMA 20220717
            //This test retrieves the reporting structure of an individual who is referencing a direct report that is then referencing the original employee.  
            string employeeIdSelfDirectReport = "ww231-asdghhf-oopk-a1a12-hpr8273d"; //employee with direct report who then has direct report of original member
            var getRequestTask = _httpClient.GetAsync($"/api/reportingstructure/{employeeIdSelfDirectReport}");
            var response = getRequestTask.Result;
            var reportingStructure = response.DeserializeContent<ReportingStructure>();

            Assert.AreEqual(reportingStructure.NumberOfReports, 2);

        }
    }
}
