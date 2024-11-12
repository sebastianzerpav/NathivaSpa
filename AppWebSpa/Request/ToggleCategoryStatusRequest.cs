namespace AppWebSpa.Request
{
    public class ToggleCategoryStatusRequest
    {
        public int CategoryId { get; set; }

        public bool Hide { get; set; }
    }
}
