using Core.Utilities.Results.Abstract;

namespace Core.Helpers.Abstract
{
    public interface IEmailService
    {
        bool IsValidEmail(string email);
        Task<IResult> SendEmailAsync(string to, string subject, string body);
    }
}