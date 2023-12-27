namespace PallyWad.Auth.Models
{
    public class Response
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
    public enum Status
    {
        Success = 1,
        Error = 2
    }
}
