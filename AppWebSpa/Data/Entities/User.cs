using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWebSpa.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Documento")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} carateres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Document { get; set; } = null!;

        [Display(Name= "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = null!;

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateOnly BirthDate { get; set; }

        public NathivaRole NathivaRole { get; set; }

        public int NathivaRoleId { get; set; } = 0; // rol que por determinado es 0 que es usuario.  Al administrador, a nivel de base de datos nosotros lo modificamos y le asignamos el 1 que es ADMIN
        // y ya este podra asignar los demas roles que cree.
    }
}
