# Lab Management App

## Persistent Layer

- DbContext
- Models
  - List
    - Title
    - Id
    - Virtual IEnumerable<Experiment>? Experiments
  - Scientist
    - Name
    - Id
  - Experiment
    - Title
    - Description
    - Deadline?
    - Priority {High, Medium, Low}
    - LastComment
    - IEnumerable<Int>? ScientistIds
    - ListId
    - 
    - Virtual IEnumerable<Scientist>? Assignee
    - Virtual <List> List
- Configurations
- Repositories
  - ListRepository (?)
    - <List>GetById(Id)
  - ExperimentRepository
    - <Experiment>Add(Experiment)
    - <Experiment>Remove(Id)
    - <Experiment>Update(Experiment) // Cambiare Listo o Aggiungere Last Comment
    - <Experiment>GetById(Id)
    - <IEnumerable<Experiment>>GetAll()
  
## Business Logic

- DTO
  - TrelloDTO
    - TrelloListDTO
    - TrelloExperimentDTO
  - BusinessObject
    - BusinessListDTO
    - BusinessScientistDTO
    - BusinessExperimentDTO
- Services
  - ListService
    - <TrelloListDTO>Add(TrelloListDTO)
  - ExperimentService
    - <TrelloExperimentDTO>Add(TrelloExperimentDTO)
- AutoMapperProfiles
- 
## MVC (Depend on BusinessLogic)

- Program.cs (IOC)
- ConnectionChecker // Handles Communication with Trello's API when available
  - Async method that checks connection to Trello's API every Delta T seconds or in the AppSettings.Json Timeframes
  - Calls EndpointRetrieveAllExperiments when connection is available every Delta T seconds
- Views
  - Dashboard Home
- Controller
  - TrelloController
    - Endpoint1UpdateExperiment (TrelloExperimentDTO) 
    - List<TrelloExperimentDTO> EndpointRetrieveAllExperiments ()
  - DashboardController
    - IActionResult<BusinessScientistDTO> Login (int ID)
    - IActionResult<List<BusinessExperimentDTO>> ShowMyExperiments (BusinessScientistDTO)
    - IActionResult<BusinessExperimentDTO> UpdateExperiment(BusinessExperimentDTO)
    - IActionResult<BusinessExperimentDTO> AddComment(string comment)
    - IActionResult<BusinessListDTO> UpdateList(TrelloListDTO) (Do we need this?)
- AppSettings.json (ConnectionString, LoggerOption, ConnectionTimePattern)


[Fetch]
API => TrelloDTO => MVC => DashboardController (Conversion to BusinessDTO) => Services (BusinessDTO) (Conversion to Model ) => Repository (Models)
                        => LoadHtml(BusinessDto)

[Push]
User => Update (View) => DashboardController (BusinessDto) => Services (BusinessDTO) (Conversion to Model ) => Repository (Models)
                                        (if connection up) => (Conversion to TrelloDTO) TrelloController (TrelloDTO) => EndpointAPIPut (TrelloDTO)

ConnectionChecker => TrelloController () => EndpointAPIGetAll() => TrelloDto (conversione to businessDTO) => Services (BusinessDTO) => Repository(Model)