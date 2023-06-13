using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionService.Abstract
{
    public interface IRetriveConnectionInterval
    {
        public interface IRetrieveConnectionTimeInterval
        {
            (bool, TimeSpan) IsTimeInInterval(DateTime currentDate);
        }
    }
}
