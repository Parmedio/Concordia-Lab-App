using AutoMapper;
using BusinessLogic.DataTransferLogic.Abstract;
using ConcordiaLab.ViewModels;
using ConcordiaLab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusinessLogic.DTOs.BusinessDTO;

namespace ConcordiaLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private static readonly List<string> _viewMode= new List<string>() { "status", "priority" };
        private static List<ViewMColumn> _progressStatuses = new List<ViewMColumn>();
        private static List<ViewMScientist> _allScientist = new List<ViewMScientist>();

        public HomeController(ILogger<HomeController> logger, IMapper mapper, IClientService clientService)
        {
            _logger = logger;
            _mapper = mapper;
            _clientService = clientService;
            if (!_progressStatuses.Any())
            {
                _progressStatuses = _mapper.Map<List<ViewMColumn>>(_clientService.GetAllSimple());
            }
            if (!_allScientist.Any())
            {
                _allScientist = _mapper.Map<List<ViewMScientist>>(_clientService.GetAllScientist());
            }
        }

        public IActionResult Index(int scientistId = 0)
        {
            _logger.LogInformation("Index was called");

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _allScientist;

            ViewData["ViewMode"] = _viewMode;
            ViewData["SelectedViewMode"] = "status";

            var dashboard = BuildIndex(scientistId);
            return View(dashboard);
        }

        public IActionResult Priority(int scientistId = 0)
        {
            _logger.LogInformation("Priority was called");

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _allScientist;

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
            ViewData["Scientists"] = _allScientist;

            ViewData["ProgressStatuses"] = _progressStatuses;

            ViewData["ViewMode"] = _viewMode;
            ViewData["SelectedViewMode"] = selectedViewMode;

            ViewData["ExperimentId"] = experimentId;
            ViewData["ExperimentScientistIds"] = detail.Scientists?.Any() ?? false ? detail.Scientists?.Select(x => x.Id).ToList() : new List<int>();

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
                _mapper.Map<IEnumerable<ViewMColumn>>(_clientService.GetAllColumns()) :
                _mapper.Map<IEnumerable<ViewMColumn>>(_clientService.GetAllColumns(scientistId));

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

            var nullPriorityExperimentsWithNearDueDate = listToOrder
                .Where(e => e.Priority == null && e.DueDate <= dueDateThreshold);

            var nullPriorityExperimentsWithFarOrNullDueDate = listToOrder
                .Where(e => e.Priority == null && (e.DueDate > dueDateThreshold || e.DueDate is null));

            var highPriorityExperiments = listToOrder
                .Where(e => e.Priority?.ToLower() == "high");

            var mediumPriorityExperimentsWithNearDueDate = listToOrder
                .Where(e => e.Priority?.ToLower() == "medium" && e.DueDate <= dueDateThreshold);

            var lowPriorityExperimentsWithNearDueDate = listToOrder
                .Where(e => e.Priority?.ToLower() == "low" && e.DueDate <= dueDateThreshold);

            var mediumPriorityExperimentsWithFarOrNullDueDate = listToOrder
                .Where(e => e.Priority?.ToLower() == "medium" && (e.DueDate > dueDateThreshold || e.DueDate is null));

            var lowPriorityExperimentsWithFarOrNullDueDate = listToOrder
                .Where(e => e.Priority?.ToLower() == "low" && (e.DueDate > dueDateThreshold || e.DueDate is null));

            var sortedExperiments = highPriorityExperiments
                .Concat(mediumPriorityExperimentsWithNearDueDate)
                .Concat(lowPriorityExperimentsWithNearDueDate)
                .Concat(nullPriorityExperimentsWithNearDueDate)
                .Concat(mediumPriorityExperimentsWithFarOrNullDueDate)
                .Concat(lowPriorityExperimentsWithFarOrNullDueDate)
                .Concat(nullPriorityExperimentsWithFarOrNullDueDate)
                .ToList();

            return sortedExperiments;
        }
    }
}