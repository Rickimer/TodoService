using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoService.DAL.Data.Models
{
    public interface IEntity
    {
        public ulong Id { get; set; }
        public DateTime Created { get; set; }
    }
}
