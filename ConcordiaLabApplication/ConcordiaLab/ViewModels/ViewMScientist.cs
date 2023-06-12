namespace ConcordiaLab.ViewModels
{
    public record ViewMScientist
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;

        public ViewMScientist(int id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}
