using WebStore.Domain.Entities.Base.Interface;

namespace WebStore.Domain.ViewModels
{
    public class BrandViewModel : INamedEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ProductsCount { get; set; }
    }
}
