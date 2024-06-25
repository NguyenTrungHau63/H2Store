using H2Store.Models;

namespace H2Store.ViewsModels
{
    public class VMHomeC
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
