using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Views
{
    public class ListViewModel<T>
    {
        public PaginationViewModel Pagination { get; set; } = null!;
        public ICollection<T> Data { get; set; } = null!;
    }
}
