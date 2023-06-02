using ConcordiaLab.Controllers.Mock_Data.Mock_Gateway;
using ConcordiaLab.Controllers.Mock_Data.MockModels;
using ConcordiaLab.Controllers.Mock_Data.ViewModel;
using ConcordiaLab.Models;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using System.Linq;

namespace ConcordiaLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MockGatewayList _mockGatewayList;
        private readonly MockGatewayExperiment _mockGatewayExperiment;
        private readonly MockGatewayScientist _mockGatewayScientist;

        public HomeController(ILogger<HomeController> logger, MockGatewayList mockGatewayList, MockGatewayExperiment mockGatewayExperiment, MockGatewayScientist mockGatewayScientist)
        {
            _logger = logger;
            _mockGatewayList = mockGatewayList;
            _mockGatewayExperiment = mockGatewayExperiment;
            _mockGatewayScientist = mockGatewayScientist;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Message.Index was called");
            return View(BuildViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private MockDashboard BuildViewModel()
        {
            var scientists = _mockGatewayScientist.GetAll();

            var experiments = _mockGatewayExperiment.GetAll()
                .Select(experiment =>
                {
                    var expScientists = experiment.IntScientists?
                        .SelectMany(id => scientists.Where(scientist => id == scientist.Id));

                    return new MockExperiment(experiment.Id, experiment.Title, experiment.Description, experiment.DueDate, experiment.Priority, experiment.LastComment, experiment.IntScientists)
                    {
                        Scientists = expScientists
                    };
                });
            

            var lists = _mockGatewayList.GetAll()
                .Select(list =>
                {
                    var listExperiments = list.IntExperiments?
                        .SelectMany(id => experiments.Where(experiment => id == experiment.Id));

                    return new MockList(list.Id, list.Name, list.IntExperiments)
                    {
                        Experiments = listExperiments
                    };
                });

            return new(lists);
        }
    }
}