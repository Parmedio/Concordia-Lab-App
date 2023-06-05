using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Models
{
    public record Label (int Id = default, string TrelloId = null!, string Title = null!);
}