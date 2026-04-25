using DAL.Models.GeneralModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SeoDataRepository : Repository<SeoData>, ISeoDataRepository
    {
        private AppDbContext _context => (AppDbContext)Context;
        public SeoDataRepository(AppDbContext context) : base(context) { }
    }
}
