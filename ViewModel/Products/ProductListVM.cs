using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.ViewModel.Products
{
    public class ProductListVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
        public int CureentImage { get; set; }
        public int PageCount { get; set; }

    }
}
