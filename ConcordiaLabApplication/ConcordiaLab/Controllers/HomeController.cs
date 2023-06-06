using ConcordiaLab.Controllers.Mock_Data;
using ConcordiaLab.Controllers.Mock_Data.Mock_Gateway;
using ConcordiaLab.Controllers.Mock_Data.MockModels;
using ConcordiaLab.Controllers.Mock_Data.ViewModel;
using ConcordiaLab.Models;

using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Diagnostics;

namespace ConcordiaLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MockGatewayList _mockGatewayList;
        private readonly MockGatewayExperiment _mockGatewayExperiment;
        private readonly MockGatewayScientist _mockGatewayScientist;
        private readonly UserSetting _userSetting;

        public HomeController(ILogger<HomeController> logger, MockGatewayList mockGatewayList, MockGatewayExperiment mockGatewayExperiment, MockGatewayScientist mockGatewayScientist, UserSetting userSetting)
        {
            _logger = logger;
            _mockGatewayList = mockGatewayList;
            _mockGatewayExperiment = mockGatewayExperiment;
            _mockGatewayScientist = mockGatewayScientist;
            _userSetting = userSetting;
        }

        public IActionResult Index(int scientistId = 0)
        {
            _logger.LogInformation("Message.Index was called");

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _mockGatewayScientist.GetAll();

            var dashboard = BuildIndexPerStatus(scientistId);
            return View(dashboard);
        }

        public IActionResult Detail(int experimentId, int scientistId)
        {
            _logger.LogInformation("Message.Detail was called");

            var detail = BuildDetailedExperiment(experimentId);

            ViewData["ExperimentId"] = experimentId;
            ViewData["ExperimentScientistIds"] = detail.Experiment.IntScientists?.ToList() ?? new List<int>();
            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _mockGatewayScientist.GetAll();

            return View(detail);
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

        private MockDashboard BuildIndexPerStatus(int scientistId)
        {
            var scientists = _mockGatewayScientist.GetAll();

            var experiments = (scientistId == 0 ? _mockGatewayExperiment.GetAll() : _mockGatewayExperiment.GetAllFromScientistId(scientistId))
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

            return new MockDashboard(lists);
        }

        private MockExperimentDetails BuildDetailedExperiment(int ExpId)
        {
            var scientists = _mockGatewayScientist.GetAll();

            var experiment = _mockGatewayExperiment.GetById(ExpId)!;

            var expScientists = experiment.IntScientists?
                .SelectMany(id => scientists.Where(scientist => id == scientist.Id));

            var detailedExperiment = new MockExperiment(experiment.Id, experiment.Title, experiment.Description, experiment.DueDate, experiment.Priority, experiment.LastComment, experiment.IntScientists)
            {
                Scientists = expScientists
            };

            return new MockExperimentDetails(detailedExperiment);
        }
    }
}