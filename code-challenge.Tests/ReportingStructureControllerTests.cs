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
        {
            string employeeId = "62c1084e-6e34-4630-93fd-9153afb65309"; //employee with no directReports
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;
            var reportingStructure = response.DeserializeContent<ReportingStructure>();

            Assert.IsNotNull(reportingStructure.NumberOfReports);
            Assert.IsNotNull(reportingStructure.EmployeeId);

        }
        [TestMethod]
        public void GetReportStructure_Returns_NotFound()
        {
            string employeeId = "Invalid_ID"; //employee with no directReports
            var getRequestTask = _httpClient.GetAsync($"/api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }
    }
}
