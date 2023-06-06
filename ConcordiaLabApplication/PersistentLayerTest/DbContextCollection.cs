using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayerTest
{
    [CollectionDefinition("DbContextCollection")]
    public class DbContextCollection : ICollectionFixture<DbContextFixture>
    {
    }

}
