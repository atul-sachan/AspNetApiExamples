using BusinessApplication.TourManagement.Api.Contexts;
using BusinessApplication.TourManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Services
{
    public class TourManagementRepository : ITourManagementRepository
    {
        private readonly TourManagementContext context;

        public TourManagementRepository(TourManagementContext context)
        {
            this.context = context;
        }

        public async Task<bool> TourExists(Guid tourId)
        {
            return await this.context.Tours.AnyAsync(x => x.TourId == tourId);
        }

        public async Task<IEnumerable<Tour>> GetTours(bool includeShows = false)
        {
            if (includeShows)
            {
                return await this.context.Tours.Include(x => x.Shows).ToListAsync();
            }
            else
            {
                return await this.context.Tours.ToListAsync();
            }
        }

        public async Task<IEnumerable<Tour>> GetToursForManager(Guid managerId, bool includeShows = false)
        {
            if (includeShows)
            {
                return await this.context.Tours
                    .Where(t => t.ManagerId == managerId)
                    .Include(t => t.Band)
                    .Include(t => t.Shows)
                    .ToListAsync();
            }
            else
            {
                return await this.context.Tours
                    .Where(t => t.ManagerId == managerId)
                    .Include(t => t.Band)
                    .ToListAsync();
            }
        }

        public async Task<Tour> GetTour(Guid tourId, bool includeShows = false)
        {
            if (includeShows)
            {
                return await this.context.Tours
                    .Include(t => t.Band)
                    .Include(t => t.Shows)
                    .Where(t => t.TourId == tourId)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await this.context.Tours
                    .Include(t => t.Band)
                    .Where(t => t.TourId == tourId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<bool> IsTourManager(Guid tourId, Guid managerId)
        {
            return await this.context.Tours
                .AnyAsync(t => t.TourId == tourId && t.ManagerId == managerId);
        }

        public async Task AddTour(Tour tour)
        {
            await this.context.Tours.AddAsync(tour);
        }

#pragma warning disable 1998
        // disable async warning - no code 
        public async Task UpdateTour(Tour tour)
        {
            // no code in this implementation
        }
#pragma warning restore 1998

#pragma warning disable 1998
        // disable async warning - no RemoveAsync available
        public async Task DeleteTour(Tour tour)
        {
            this.context.Tours.Remove(tour);
        }
#pragma warning restore 1998

        public async Task<IEnumerable<Show>> GetShows(Guid tourId)
        {
            return await this.context.Shows
                .Where(s => s.TourId == tourId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetShows(Guid tourId, IEnumerable<Guid> showIds)
        {
            return await this.context.Shows
                 .Where(s => s.TourId == tourId && showIds.Contains(s.ShowId))
                 .ToListAsync();
        }

        public async Task AddShow(Guid tourId, Show show)
        {
            var tour = await GetTour(tourId);
            if (tour == null)
            {
                // throw an exception - this is a race condition
                // that's mostly caught by checking if the tour exists
                // right before calling into this method - if that method is not
                // called the condition can happen, otherwise the tour
                // will already be loaded on the context
                throw new Exception($"Cannot add show to tour with id {tourId}: tour not found.");
            }
            tour.Shows.Add(show);
        }

        public async Task<IEnumerable<Band>> GetBands()
        {
            return await this.context.Bands.ToListAsync();
        }

        public async Task<IEnumerable<Manager>> GetManagers()
        {
            return await this.context.Managers.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await this.context.SaveChangesAsync() >= 0);
        }

    }
}
