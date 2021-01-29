using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Components
{
    // [ViewComponent(Name = "Sections")] - если не наследовать класс ViewComponent
    public class SectionsViewComponent: ViewComponent
    {
        private readonly IProductData productData;
        public SectionsViewComponent(IProductData _productData) => productData = _productData;

        // по умолчанию берется представление с именем Default
        public IViewComponentResult Invoke()
        {
            var sections = productData.GetSections().ToArray();

            var parent_sections = sections.Where(s => s.ParentId is null);

            var parent_sections_views = parent_sections
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,
                    ProductsCount = s.FromDTO().Products.Count
                })
                .ToList();

            foreach (var parent_section in parent_sections_views)
            {
                var childs = sections.Where(s => s.ParentId == parent_section.Id);

                foreach (var childs_section in childs)
                {
                    parent_section.ChildSection.Add(new SectionViewModel
                    {
                        Id = childs_section.Id,
                        Name = childs_section.Name,
                        Order = childs_section.Order,
                        ParentSection = parent_section,
                        ProductsCount = childs_section.FromDTO().Products.Count
                    });
                }
                parent_section.ChildSection.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }

            parent_sections_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return View(parent_sections_views);
        }

        //public async Task<IViewComponentResult> InvokeAsync() => View(); - для асинхронного вызова
    }
}
