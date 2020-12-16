using System;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base.Interface;

namespace WebStore.ViewModels
{
    public class ProductViewModel : INamedEntity, IOrderedEntity
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public int Order { get; set; }

        public string ImageUrl;

        public decimal Price;
    }
}
