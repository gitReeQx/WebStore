using System;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base.Interface;

namespace WebStore.ViewModels
{
    public class ProductViewModel : INamedEntity
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public string ImageUrl;

        public decimal Price;

        public string Brand { get; set; }
    }
}
