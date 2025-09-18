namespace FastFood.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string receivers, string subject, string body);
    }
}