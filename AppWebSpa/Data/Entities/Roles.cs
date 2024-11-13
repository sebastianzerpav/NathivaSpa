using System.ComponentModel.DataAnnotations;

namespace AppWebSpa.Data.Entities
{
    public class RolesForUser
    {
        [Key]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre de rol")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el estado")]

        public int State { get; set; } = 1; //1 activo, 0 inactivo. Por defecto se crea activo 
    }
}
