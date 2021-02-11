using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain
{
    public class ProductFilter
    {
        public int? SectionId { get; init; }

        public int? BrandId { get; init; }

        public int[] Ids { get; init; }

        public int Page { get; init; }

        public int? PageSize { get; init; }
    }
}
