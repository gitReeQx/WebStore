using System.Collections;
using System.Collections.Generic;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSection();

        IEnumerable<Brand> GetBrands();
    }
}
