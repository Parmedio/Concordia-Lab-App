using ConcordiaLab.Controllers.Mock_Data;
using ConcordiaLab.Controllers.Mock_Data.Mock_Gateway;
using ConcordiaLab.Controllers.Mock_Data.MockModels;
using ConcordiaLab.Controllers.Mock_Data.ViewModel;
using ConcordiaLab.Models;

using Microsoft.AspNetCore.Mvc;
using PersistentLayer.Models;
using System.Collections;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;

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
            _logger.LogInformation("Index was called");

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _mockGatewayScientist.GetAll();

            ViewData["ViewMode"] = new List<string>() { "status", "priority" };
            ViewData["SelectedViewMode"] = "status";

            var dashboard = BuildIndex(scientistId);
            return View(dashboard);
        }

        public IActionResult Priority(int scientistId = 0)
        {
            _logger.LogInformation("Priority was called");

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _mockGatewayScientist.GetAll();

            ViewData["ViewMode"] = new List<string>() { "status", "priority"};
            ViewData["SelectedViewMode"] = "priority";

            var priorityList = BuildPriority(scientistId);
            return View(priorityList);
        }

        public IActionResult Detail(int experimentId, int scientistId, string selectedViewMode)
        {
            _logger.LogInformation("Detail was called");
            
            var detail = BuildDetailedExperiment(experimentId);

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _mockGatewayScientist.GetAll();

            ViewData["ViewMode"] = new List<string>() { "status", "priority" };
            ViewData["SelectedViewMode"] = selectedViewMode;

            ViewData["ExperimentId"] = experimentId;
            ViewData["ExperimentScientistIds"] = detail.Experiment.IntScientists;
            
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

        private Dashboard BuildIndex(int scientistId)
        {
            var experimentWithAssegnee = AssignScientistToExperiment(scientistId);

            var lists = _mockGatewayList.GetAll()
                .Select(list =>
                {
                    var listExperiments = list.IntExperiments?
                        .SelectMany(id => experimentWithAssegnee.Where(experiment => id == experiment.Id));

                    return new MockList(list.Id, list.Name, list.IntExperiments)
                    {
                        Experiments = listExperiments
                    };
                });

            return new Dashboard(lists);
        }

        private Priority BuildPriority(int scientistId)
        {
            var experimentWithAssegnee = AssignScientistToExperiment(scientistId);
            
            var sortedExperiments = OrderAccordingToPriority(experimentWithAssegnee);

            return new Priority(sortedExperiments);
        }

        private ExperimentDetails BuildDetailedExperiment(int ExpId)
        {
            var scientists = _mockGatewayScientist.GetAll();

            var experiment = _mockGatewayExperiment.GetById(ExpId)!;

            var expScientists = experiment.IntScientists?
                .SelectMany(id => scientists.Where(scientist => id == scientist.Id));

            var detailedExperiment = new MockExperiment(experiment.Id, experiment.Title, experiment.Description, experiment.DueDate, experiment.Priority, experiment.LastComment, experiment.IntScientists)
            {
                Scientists = expScientists
            };

            return new ExperimentDetails(detailedExperiment);
        }

        private List<MockExperiment> OrderAccordingToPriority(IEnumerable<MockExperiment> listToOrder)
        {
            var today = DateTime.Today;
            var dueDateThreshold = today.AddDays(5);

            var highPriorityExperiments = listToOrder
                .Where(e => e.Priority.ToLower() == "high");

            var mediumPriorityExperimentsWithNearDueDate = listToOrder
                .Where(e => e.Priority.ToLower() == "medium" && e.DueDate <= dueDateThreshold);

            var lowPriorityExperimentsWithNearDueDate = listToOrder
                .Where(e => e.Priority.ToLower() == "low" && e.DueDate <= dueDateThreshold);

            var mediumPriorityExperiments = listToOrder
                .Where(e => e.Priority.ToLower() == "medium" && (e.DueDate > dueDateThreshold || e.DueDate is null));

            var lowPriorityExperiments = listToOrder
                .Where(e => e.Priority.ToLower() == "low" && (e.DueDate > dueDateThreshold || e.DueDate is null));

            var sortedExperiments = highPriorityExperiments
                .Concat(mediumPriorityExperimentsWithNearDueDate)
                .Concat(lowPriorityExperimentsWithNearDueDate)
                .Concat(mediumPriorityExperiments)
                .Concat(lowPriorityExperiments)
                .ToList();

            return sortedExperiments;
        }
       
        private IEnumerable<MockExperiment> AssignScientistToExperiment(int scientistId)
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

            return experiments;
        }
    }
}