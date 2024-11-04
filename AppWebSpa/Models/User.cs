using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWebSpa.Models
{
    public class User
    {
        //Properties with DataAnnotations
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar un número de teléfono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha de nacimiento")]
        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        public string Password { get; set; }

        ////Relations

        ////¿Prop TipoUsuario?

        //[ForeignKey("IdRole")]
        //public Role IdRole { get; set; } //RoleManager??
    }
}
