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

        public ICollection<RolePermission>? RolePermisions { get; set; }
        public ICollection<RoleCategory>? RoleCategories { get; set; }




    }
}
