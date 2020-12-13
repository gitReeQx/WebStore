using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class EmployeesViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>Имя</summary>
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Это обязательное поле")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Длина должна быть от 3-х до 15-ти символов")]
        [RegularExpression(@"([A-Z][a-z]+)|([А-ЯЁ][а-яё]+)", ErrorMessage = "Ошибка формата имени")]
        public string FirstName { get; set; }

        /// <summary>Фамилия</summary>
        [Display(Name = "Фамилия")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2-х до 15-ти символов")]
        public string LastName { get; set; }

        /// <summary>Отчество</summary>
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        /// <summary>Возраст</summary>
        [Display(Name = "Возраст")]
        [Range(18, 55, ErrorMessage = "От 18-ти до 55-ти")]
        public int Age { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            yield return ValidationResult.Success;
        }
    }
}
