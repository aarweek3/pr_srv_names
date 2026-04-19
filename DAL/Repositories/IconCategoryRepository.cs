using DAL.Models.GeneralModels;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class IconCategoryRepository : Repository<IconCategory>, IIconCategoryRepository
    {
        public IconCategoryRepository(AppDbContext context) : base(context)
        {
        }

    }
}
