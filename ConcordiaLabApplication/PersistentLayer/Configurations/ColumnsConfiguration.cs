using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersistentLayer.Models;

namespace PersistentLayer.Configurations;

public class ColumnsConfiguration : IEntityTypeConfiguration<Column>
{
    public void Configure(EntityTypeBuilder<Column> entity)
    {
#if DEBUG
        entity.HasData(
                   new Column { Id = 1, TrelloId = "64760975fbea80d6ef329080", Title = "to do" },
                   new Column { Id = 2, TrelloId = "64760975fbea80d6ef329081", Title = "in progress" },
                   new Column { Id = 3, TrelloId = "64760975fbea80d6ef329082", Title = "completed" }
                   );
#else
        entity.HasData(
                   new Column { Id = 1, TrelloId = "64760804e47275c707e05d38", Title = "to do" },
                   new Column { Id = 2, TrelloId = "64760804e47275c707e05d39", Title = "in progress" },
                   new Column { Id = 3, TrelloId = "64760804e47275c707e05d3a", Title = "completed" }
                   );
#endif
    }
}
