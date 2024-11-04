using System.ComponentModel.DataAnnotations;

namespace AppWebSpa.Models
{
    public class SpaService
    {
        //Properties with DataAnnotations
        [Key]
        public int IdSpaService { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre para el servicio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar una descripción del servicio")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Debe ingresar el precio del servicio")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage ="El precio debe ser mayor o igual que cero")]
        public decimal Price { get; set; }

        //RegistrationDateTime
        public DateTime RegistrationDateTime { get; set; }

        public SpaService() { 
            RegistrationDateTime = DateTime.Now;
        }

        ////Image
        ////¿DataAnnotations de Validación?
        //public string ImageUrl { get; set; }
        //public byte[] ImageData { get; set; }
    }
}
