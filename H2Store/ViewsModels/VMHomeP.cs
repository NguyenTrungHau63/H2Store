using H2Store.Models;
using X.PagedList;

namespace H2Store.ViewsModels
{
    public class VMHomeP
    {
        public IPagedList<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public User User { get; set; }
    }
}
