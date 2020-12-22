﻿using WebStore.Domain.Entities.Base.Interface;

namespace WebStore.ViewModels
{
    public class BrandViewModel : INamedEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
