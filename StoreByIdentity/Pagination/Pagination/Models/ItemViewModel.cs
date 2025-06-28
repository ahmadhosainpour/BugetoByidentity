namespace Pagination.Models
{
    public class ItemViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
