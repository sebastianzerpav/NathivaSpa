using System.ComponentModel.DataAnnotations;

namespace AppWebSpa.Data.Entities
{
    public class NathivaRole
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener maximo {1} caracter")]
        [Required(ErrorMessage = "Debe ingresar un nombre de rol")]
        public string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermisions { get; set; } 

        [Required(ErrorMessage = "Debe seleccionar el estado")]
        public int State { get; set; } = 1; //1 activo, 0 inactivo. Por defecto se crea activo 

    }
}
