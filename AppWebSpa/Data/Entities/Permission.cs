using System.ComponentModel.DataAnnotations;

namespace AppWebSpa.Data.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener maximo {1} caracter")]
        [Required(ErrorMessage = "Debe ingresar un nombre de rol")]
        public string Name { get; set; } = null!;

        [Display(Name = "Descripcion")]
        [MaxLength(512, ErrorMessage = "El campo {0} debe tener maximo {1} caracter")]
        [Required(ErrorMessage = "Debe ingresar un nombre de rol")]
        public string Description { get; set; } = null!;

        [Display(Name = "Modulo")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener maximo {1} caracter")]
        [Required(ErrorMessage = "Debe ingresar un nombre de rol")]
        public string Module { get; set; } = null!;

        public ICollection<RolePermission> RolePermisions { get; set; }
    }
}
