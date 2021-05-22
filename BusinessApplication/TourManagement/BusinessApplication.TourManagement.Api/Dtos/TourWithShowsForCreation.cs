using System.Collections.Generic;

namespace BusinessApplication.TourManagement.Api.Dtos
{
    public class TourWithShowsForCreation : TourForCreation
    {
        public ICollection<ShowForCreation> Shows { get; set; }
          = new List<ShowForCreation>();
    }
}
