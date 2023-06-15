using AutoMapper;
using BusinessLogic.DataTransferLogic.Abstract;
using ConcordiaLab.ViewModels;
using ConcordiaLab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ConcordiaLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private static readonly List<string> _viewMode= new List<string>() { "status", "priority" };

        public HomeController(ILogger<HomeController> logger, IMapper mapper, IClientService clientService)
        {
            _logger = logger;
            _mapper = mapper;
            _clientService = clientService;
        }

        public IActionResult Index(int scientistId = 0)
        {
            _logger.LogInformation("Index was called");

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _clientService.GetAllScientist();

            ViewData["ViewMode"] = _viewMode;
            ViewData["SelectedViewMode"] = "status";

            var dashboard = BuildIndex(scientistId);
            return View(dashboard);
        }

        public IActionResult Priority(int scientistId = 0)
        {
            _logger.LogInformation("Priority was called");

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _clientService.GetAllScientist();

            ViewData["ViewMode"] = _viewMode;
            ViewData["SelectedViewMode"] = "priority";

            var priorityList = BuildPriority(scientistId);

            return View(priorityList);
        }

        public IActionResult Detail(int experimentId, int scientistId, string selectedViewMode)
        {
            _logger.LogInformation("Detail was called");
            
            var detail = BuildDetailedExperiment(experimentId);

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _clientService.GetAllScientist();

            ViewData["ViewMode"] = _viewMode;
            ViewData["SelectedViewMode"] = selectedViewMode;

            ViewData["ExperimentId"] = experimentId;
            ViewData["ExperimentScientistIds"] = detail.Scientists.Select(x => x.Id);
            
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

        private IEnumerable<ViewMColumn> BuildIndex(int scientistId)
        {
            var unorderedExperiments = scientistId == 0 ?
                _mapper.Map<IEnumerable<ViewMColumn>>(_clientService.GetallColumns()) :
                _mapper.Map<IEnumerable<ViewMColumn>>(_clientService.GetallColumns(scientistId));

            return (unorderedExperiments);
        }

        private IEnumerable<ViewMExperiment> BuildPriority(int scientistId)
        {
            var unorderedExperiments = scientistId == 0 ?
                _mapper.Map<IEnumerable<ViewMExperiment>>(_clientService.GetAllExperiments()) :
                _mapper.Map<IEnumerable<ViewMExperiment>>(_clientService.GetAllExperiments(scientistId));

            return OrderAccordingToPriority(unorderedExperiments);
        }

        private ViewMExperiment BuildDetailedExperiment(int ExpId)
        {
            var detailedExperiment = _mapper.Map<ViewMExperiment>(_clientService.GetExperimentById(ExpId)); //fare mappatura

            return detailedExperiment;
        }

        private List<ViewMExperiment> OrderAccordingToPriority(IEnumerable<ViewMExperiment> listToOrder)
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
    }
}