using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base.Interface;

namespace WebStore.ViewModels
{
    public class SectionViewModel : INamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public List<SectionViewModel> ChildSection { get; set; } = new();

        public SectionViewModel ParentSection { get; set; }

        public int ProductsCount { get; set; }
    }
}
