using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Repository.TienDM.ModelExtensions
{
    public class PaginationResult<T> where T : class
    {
        public int TotalItems;
        public int TotalPages;
        public int CurrentPages;
        public int PageSize;
        public T Items { get; set; }
    }
}
