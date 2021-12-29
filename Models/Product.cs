using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int StockCount { get; set; }
        public Category CategoryName { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public bool IsDeleted { get; set; }
    }
}
