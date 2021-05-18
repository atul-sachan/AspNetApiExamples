using BusinessApplication.TourManagement.Api.Entities;
using BusinessApplication.TourManagement.Api.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Contexts
{
    public class TourManagementContext: DbContext
    {
        private readonly IUserInfoService userInfoService;

        public DbSet<Tour> Tours { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Manager> Managers { get; set; }

        public TourManagementContext(DbContextOptions<TourManagementContext> options, IUserInfoService userInfoService): base(options)
        {
            this.userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var addedOrUpdatedEntries = ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in addedOrUpdatedEntries)
            {
                var entity = entry.Entity as AuditableEntity;

                if(entry.State == EntityState.Added)
                {
                    entity.CreatedBy = userInfoService.UserId;
                    entity.CreatedOn = DateTime.UtcNow;
                }

                entity.UpdatedBy = userInfoService.UserId;
                entity.UpdatedOn = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
