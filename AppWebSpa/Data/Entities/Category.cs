using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace AppWebSpa.Data.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Nombre de la categoria")]
        [MaxLength(64, ErrorMessage = "El Campo {0} debe tener un maximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; }

        [Display(Name = "Descripcion")]
        [MaxLength(200, ErrorMessage = "El Campo {0} debe tener un maximo {1} caracteres.")]
        public string? Description { get; set; }

        [Display(Name = "Esta oculta")]
        public bool IsHidden { get; set; }

        public ICollection<RoleCategory>? RoleCategories { get; set; }

    }
}
