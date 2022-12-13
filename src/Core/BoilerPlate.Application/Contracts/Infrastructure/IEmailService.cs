using BoilerPlate.Application.Models.Mail;
using System.Threading.Tasks;

namespace BoilerPlate.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
