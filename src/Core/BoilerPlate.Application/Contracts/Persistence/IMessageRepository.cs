using BoilerPlate.Domain.Entities;
using System.Threading.Tasks;

namespace BoilerPlate.Application.Contracts.Persistence
{
    public interface IMessageRepository : IAsyncRepository<Message>
    {
        public Task<Message> GetMessage(string Code, string Lang);
    }
}
