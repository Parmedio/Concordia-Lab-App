using AutoMapper;
using BusinessLogic.DataTransferLogic.Abstract;
using ConcordiaLab.ViewModels;
using ConcordiaLab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusinessLogic.DTOs.BusinessDTO;
using System.Data.SqlClient;
using PersistentLayer.Models;

namespace ConcordiaLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IClientService _clientService;
        private static readonly List<string> _viewMode = new List<string>() { "status", "priority" };
        private static List<ViewMColumn> _progressStatuses = new List<ViewMColumn>();
        private static List<ViewMScientist> _allScientist = new List<ViewMScientist>();

        public HomeController(ILogger<HomeController> logger, IMapper mapper, IClientService clientService)
        {
            _logger = logger;
            _mapper = mapper;
            _clientService = clientService;

            if (!_progressStatuses.Any())
                _progressStatuses = _mapper.Map<List<ViewMColumn>>(_clientService.GetAllSimple());

            if (!_allScientist.Any())
                _allScientist = _mapper.Map<List<ViewMScientist>>(_clientService.GetAllScientist());
        }

        [HttpGet]
        public IActionResult Index(int scientistId = 0)
        {
            _logger.LogInformation("Index was called");

            IEnumerable<ViewMColumn> dashboard = new List<ViewMColumn>();

            try
            {
                dashboard = BuildIndex(scientistId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while retrieving columns in database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing your request.");
            }

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _allScientist;

            ViewData["ViewMode"] = _viewMode;
            ViewData["SelectedViewMode"] = "status";

            return View(dashboard);
        }

        [HttpGet]
        public IActionResult Priority(int scientistId = 0)
        {
            _logger.LogInformation("Priority was called");

            IEnumerable<ViewMExperiment> priorityList = new List<ViewMExperiment>();

            try
            {
                priorityList = BuildPriority(scientistId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while retrieving experiments in database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing your request.");
            }

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _allScientist;

            ViewData["ViewMode"] = _viewMode;
            ViewData["SelectedViewMode"] = "priority";

            return View(priorityList);
        }

        [HttpGet]
        public IActionResult Detail(int experimentId, int scientistId, string selectedViewMode)
        {
            _logger.LogInformation("Detail was called");

            var detail = new ViewMExperiment();

            try
            {
                detail = BuildDetailedExperiment(experimentId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while retrieving experiment in database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing your request.");
            }

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _allScientist;

            ViewData["ProgressStatuses"] = _progressStatuses;

            ViewData["ViewMode"] = _viewMode;
            ViewData["SelectedViewMode"] = selectedViewMode;

            ViewData["ExperimentId"] = experimentId;
            ViewData["ExperimentScientistIds"] = detail?.Scientists?.Any() ?? false ? detail.Scientists?.Select(x => x.Id).ToList() : new List<int> { 0 };

            return View(detail);
        }

        [HttpPost]
        public IActionResult UpdateExperimentStatus(int experimentId, int scientistId, string selectedViewMode, int statusId)
        {
            _logger.LogInformation("Update experiment status was called");

            var updatedExperiment = _clientService.GetExperimentById(experimentId);
            updatedExperiment.ColumnName = _progressStatuses.FirstOrDefault(x => x.Id == statusId)!.Name;
            updatedExperiment.ColumnId = statusId;

            try
            {
                _clientService.MoveExperiment(updatedExperiment);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while updating experiment status in database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing your request.");
            }

            return RedirectToAction("Detail", new { experimentId, scientistId, selectedViewMode });
        }

        [HttpPost]
        public IActionResult PostNewComment(int experimentId, int scientistId, string commentAuthorName, string selectedViewMode, string commentText)
        {
            _logger.LogInformation("Post new comment was called");

            var newComment = new BusinessCommentDto
            {
                CardID = experimentId,
                CommentText = commentText,
                CreatorName = commentAuthorName
            };

            try
            {
                _clientService.AddComment(newComment, scientistId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while adding comment to database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing your request.");
            }

            return RedirectToAction("Detail", new { experimentId, scientistId, selectedViewMode });
        }

        [HttpGet]
        public IActionResult About(int scientistId, string selectedViewMode)
        {
            _logger.LogInformation("About was called");

            ViewData["SelectedScientistId"] = scientistId;
            ViewData["Scientists"] = _allScientist;

            ViewData["ViewMode"] = _viewMode;
            ViewData["SelectedViewMode"] = selectedViewMode;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IEnumerable<ViewMColumn> BuildIndex(int scientistId)
        {
            var Columns = scientistId == 0 ?
                _mapper.Map<IEnumerable<ViewMColumn>>(_clientService.GetAllColumns()) :
                _mapper.Map<IEnumerable<ViewMColumn>>(_clientService.GetAllColumns(scientistId));

            return (Columns);
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
            var detailedExperiment = _mapper.Map<ViewMExperiment>(_clientService.GetExperimentById(ExpId));

            return detailedExperiment;
        }

        private List<ViewMExperiment> OrderAccordingToPriority(IEnumerable<ViewMExperiment> listToOrder)
        {
            var today = DateTime.Today;
            var dueDateThreshold = today.AddDays(5);

            var nullPriorityExperimentsWithNearDueDate = listToOrder
                .Where(e => e.Priority == null && e.DueDate <= dueDateThreshold).OrderBy(e => e.DueDate);

            var nullPriorityExperimentsWithFarOrNullDueDate = listToOrder
                .Where(e => e.Priority == null && (e.DueDate > dueDateThreshold || e.DueDate is null)).OrderBy(e => e.DueDate);

            var highPriorityExperiments = listToOrder
                .Where(e => e.Priority?.ToLower() == "high").OrderBy(e => e.DueDate);

            var mediumPriorityExperimentsWithNearDueDate = listToOrder
                .Where(e => e.Priority?.ToLower() == "medium" && e.DueDate <= dueDateThreshold).OrderBy(e => e.DueDate);

            var lowPriorityExperimentsWithNearDueDate = listToOrder
                .Where(e => e.Priority?.ToLower() == "low" && e.DueDate <= dueDateThreshold).OrderBy(e => e.DueDate);

            var mediumPriorityExperimentsWithFarOrNullDueDate = listToOrder
                .Where(e => e.Priority?.ToLower() == "medium" && (e.DueDate > dueDateThreshold || e.DueDate is null)).OrderBy(e => e.DueDate);

            var lowPriorityExperimentsWithFarOrNullDueDate = listToOrder
                .Where(e => e.Priority?.ToLower() == "low" && (e.DueDate > dueDateThreshold || e.DueDate is null)).OrderBy(e => e.DueDate);

            var sortedExperiments = highPriorityExperiments
                .Concat(mediumPriorityExperimentsWithNearDueDate)
                .Concat(lowPriorityExperimentsWithNearDueDate)
                .Concat(nullPriorityExperimentsWithNearDueDate)
                .Concat(mediumPriorityExperimentsWithFarOrNullDueDate)
                .Concat(lowPriorityExperimentsWithFarOrNullDueDate)
                .Concat(nullPriorityExperimentsWithFarOrNullDueDate)
                .Where(e => e.ColumnId != 3)
                .ToList();

            return sortedExperiments;
        }
    }
}