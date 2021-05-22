using System.Collections.Generic;

namespace BusinessApplication.TourManagement.Api.Dtos
{
    public class TourWithEstimatedProfitsAndShows : TourWithEstimatedProfits
    {
        public ICollection<Show> Shows { get; set; }
              = new List<Show>();
    }
}
