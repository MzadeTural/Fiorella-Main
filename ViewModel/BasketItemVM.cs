
namespace Fiorella_second.ViewModel
{
    public class BasketItemVM
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int StockCount { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
