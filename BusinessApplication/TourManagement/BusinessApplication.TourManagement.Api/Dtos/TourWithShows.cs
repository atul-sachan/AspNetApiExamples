using System.Collections.Generic;

namespace BusinessApplication.TourManagement.Api.Dtos
{
    public class TourWithShows : Tour
    {
        public ICollection<Show> Shows { get; set; }
            = new List<Show>();
    }
}
