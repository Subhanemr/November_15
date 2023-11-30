using _15_11_23.DAL;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.Servicers
{
    public class LayoutServices
    {
        private readonly AppDbContext _context;

        public LayoutServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> GetSttings()
        {
            Dictionary<string, string> keyValuePairs = await _context.Settings.ToDictionaryAsync(p=>p.Key,p=>p.Value);
            return keyValuePairs;
        }
    }
}
