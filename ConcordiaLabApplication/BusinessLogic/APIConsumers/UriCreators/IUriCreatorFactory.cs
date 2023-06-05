namespace BusinessLogic.APIConsumers.UriCreators
{
    public interface IUriCreatorFactory
    {
        string GetAllCardsOnToDoList();
        string GetAllCommentsOnABoard();
    }
}