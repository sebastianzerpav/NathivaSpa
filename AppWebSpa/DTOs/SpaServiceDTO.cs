using AppWebSpa.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWebSpa.DTOs
{
    public class SpaServiceDTO
    {
        //Properties with DataAnnotations
        [Key]
        public int IdSpaService { get; set; }

        [Display(Name = "Nombre del servicio")]
        [Required(ErrorMessage = "Debe ingresar un nombre para el servicio")]
        public string Name { get; set; }

        [Display(Name = "Descripción del servicio")]
        [Required(ErrorMessage = "Debe ingresar una descripción del servicio")]
        public string Description { get; set; }

        [Display(Name = "Precio del servicio")]
        [Required(ErrorMessage = "Debe ingresar el precio del servicio")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "El precio debe ser mayor o igual que cero")]
        public decimal Price { get; set; }

        //RegistrationDateTime
        public DateTime RegistrationDateTime { get; set; }

        [Display(Name = "¿Esta OCulta?")]
        public bool IsHidden { get; set; } = false;

        public CategoryService? CategoryService { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo '{0}' es requerido")]
        [Range (1, int.MaxValue,ErrorMessage ="Debe seleccionar una categoria")]
        public int CategoryId { get; set; }

        public SpaServiceDTO()
        {
            RegistrationDateTime = DateTime.Now;
        }

      
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
