using Microsoft.EntityFrameworkCore;
using MvcStartApp.Models.Db;

namespace MvcStartApp.Models.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly BlogContext _context;

        public RequestRepository(BlogContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Request request)
        {
            request.Date = DateTime.Now;
            request.Id = Guid.NewGuid();

            var entry = _context.Entry(request);
            if (entry.State == EntityState.Detached)
            {
                await _context.Requests.AddAsync(request);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Request[]> GetRequests()
        {
           return await _context.Requests.ToArrayAsync();
        }
    }
}
