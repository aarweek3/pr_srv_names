using DAL.Models.GeneralModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class IconRepository : Repository<Icon>, IIconRepository
    {
        private AppDbContext _appContext => (AppDbContext)Context;

        public IconRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<string?> GetContentByNameAsync(string name)
        {
            return await _appContext.Icons
                .Where(i => i.Name == name)
                .Select(i => i.SvgContent)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _appContext.Icons.AnyAsync(i => i.Name == name);
        }
    }
}
