using MvcStartApp.Models.Db;

namespace MvcStartApp.Models.Repositories
{
    public interface IRequestRepository
    {
        Task AddAsync(Request request);
        Task<Request[]> GetRequests();
    }
}
