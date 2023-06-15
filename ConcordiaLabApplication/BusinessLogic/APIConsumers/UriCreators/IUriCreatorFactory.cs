namespace BusinessLogic.APIConsumers.UriCreators
{
    public interface IUriCreatorFactory
    {
        string GetAllCardsOnToDoColumn();
        string GetAllCommentsOnABoard();
        string AddACommentOnACard(string cardId, string text, string authToken);
        string UpdateAnExperiment(string cardId, string ColumnId);
    }
}