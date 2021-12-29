using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.ViewModel
{
    public class Paginate<T>
    {

        public Paginate(List<T> item,int currentPAge,int pageCount)
        {
            Item = item;
            CurrentPage = currentPAge;
            PageCount = pageCount;
        }
        public List<T> Item { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
