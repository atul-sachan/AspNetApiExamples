using BusinessApplication.TourManagement.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Services
{
    public interface ITourManagementRepository
    {
        Task AddTour(Tour tour);
        Task DeleteTour(Tour tour);
        Task<Tour> GetTour(Guid tourId, bool includeShows = false);
        Task<IEnumerable<Tour>> GetTours(bool includeShows = false);
        Task<IEnumerable<Tour>> GetToursForManager(Guid managerId, bool includeShows = false);
        Task<bool> IsTourManager(Guid tourId, Guid managerId);
        Task<bool> SaveAsync();
        Task<bool> TourExists(Guid tourId);
        Task UpdateTour(Tour tour);
        Task<IEnumerable<Show>> GetShows(Guid tourId);
        Task<IEnumerable<Show>> GetShows(Guid tourId, IEnumerable<Guid> showIds);
        Task AddShow(Guid tourId, Show show);
        Task<IEnumerable<Band>> GetBands();
        Task<IEnumerable<Manager>> GetManagers();
    }
}
