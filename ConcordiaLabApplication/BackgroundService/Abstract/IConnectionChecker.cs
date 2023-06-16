using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundServices.Abstract
{
    public interface IConnectionChecker
    {
        public Task<bool> CheckConnection();
    }
}
