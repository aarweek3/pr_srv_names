using DAL.Models.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IPlatformRepository : IRepository<Platform>
    {
        Task<Platform?> GetWithTranslationsAndSeoAsync(Guid id);
        Task<bool> IsCodeUniqueAsync(string code, Guid? excludeId = null);
    }
}
