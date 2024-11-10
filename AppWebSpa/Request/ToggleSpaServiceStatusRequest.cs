namespace AppWebSpa.Request
{
    public class ToggleSpaServiceStatusRequest
    {
        public int SpaServiceId { get; set; }

        public bool Hide { get; set; }
    }
}
