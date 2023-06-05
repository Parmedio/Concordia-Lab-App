using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Abstract;

// Gli scienziati possono aggiungere un commento
// Gli scienziati possono spostare un esperimento da una scheda all'altra

// Penso alla struttura del controller e di cio' che richiede
// Il controller si occupera' del login, quindi fatto il logic



public interface IDataService
{
    // Dovrebbe restituirmi tutte le liste inerenti allo scienziato in teoria, quindi tutte le liste con i soli esperimenti assegnati allo scienziato oppure
    // non assegnati a nessuno

    public Task<bool> UpdateConnectionStateAsync(bool connectionState);

    public List<BusinessListDto>? GetAllLists(int scientistId);

    // Per L'API ho bisogno di: Conversione IdList locale a IdTrelloList
    // Ho bisogno del Token dello scienziato
    // Ho bisogno dell'id Trello dello scienziato
    // Ho bisogno del testo del commento
    // Ho bisogno dell'idTrello dell'esperimento
    // Ho bisogno dell'id locale dell'esperimento
    // Ho bisogno dell'id dello scienziato

    // Che tipo devo restituire al Controller? Bool o basta void? O forse potrei restituire l'id dell'esperimento
    // che sono andato ad aggiornare?

    // E' meglio prendersi qui l'oggetto scientistDTO chiedendolo al DB tramite repository o e' piu'
    // facile lavorare passando gia' alla web app sottostante un oggetto di tipo ScientistDTO?
    public void AddComment(BusinessExperimentDto businessExperimentDto, int scientistId);
    public void MoveExperiment(BusinessExperimentDto businessExperimentDto, int scientistId);


}
