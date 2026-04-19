using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Base;
using DAL.Models.AuthorizationModels;
using DAL.Models.GeneralModels;
using DAL.Models.SampleModels;
using DAL.Models.LocalizationModels;

namespace DAL
{
    public partial class AppDbContext
    {
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var now = DateTime.UtcNow;

            // Обновление BaseEntity (наследуемые сущности)
            var baseEntityEntries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State is EntityState.Added or EntityState.Modified);
            foreach (var entry in baseEntityEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                }

                entry.Entity.UpdatedAt = now;
            }

            // Обновление ApplicationUser (специальная логика)
            var userEntries = ChangeTracker.Entries<ApplicationUser>()
                .Where(e => e.State is EntityState.Modified);
            foreach (var entry in userEntries)
            {
                entry.Entity.UpdatedAt = now;
            }

            // Обновление UserSession (специальная логика для RevokedAt)
            var sessionEntries = ChangeTracker.Entries<UserSession>()
                .Where(e => e.State is EntityState.Modified);
            foreach (var entry in sessionEntries)
            {
                if (entry.Entity.IsRevoked && entry.Entity.RevokedAt == null)
                {
                    entry.Entity.RevokedAt = now;
                }
            }

            // Обновление сущностей без наследования BaseEntity
            UpdateTimestamps<LanguageApp>(now);
            UpdateTimestamps<SampleMain>(now);
            UpdateTimestamps<SampleMainDescription>(now);
            UpdateTimestamps<SampleMainSeo>(now);
            UpdateTimestamps<SampleMainDescriptionSeo>(now);
            UpdateTimestamps<DAL.Models.Business.Platform>(now);
            UpdateTimestamps<DAL.Models.Business.PlatformTranslation>(now);
        }

        /// <summary>
        /// Обновление временных меток для сущностей, не наследующих BaseEntity
        /// </summary>
        private void UpdateTimestamps<T>(DateTime now) where T : class
        {
            var entries = ChangeTracker.Entries<T>()
                .Where(e => e.State is EntityState.Added or EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var updateProperty = entity.GetType().GetProperty("UpdatedAt");
                var createdProperty = entity.GetType().GetProperty("CreatedAt");

                if (entry.State == EntityState.Added && createdProperty != null)
                {
                    createdProperty.SetValue(entity, now);
                }

                if (updateProperty != null)
                {
                    updateProperty.SetValue(entity, now);
                }
            }
        }
    }
}
