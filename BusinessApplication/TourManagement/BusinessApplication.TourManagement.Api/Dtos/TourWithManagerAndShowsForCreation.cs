using System.Collections.Generic;

namespace BusinessApplication.TourManagement.Api.Dtos
{
    public class TourWithManagerAndShowsForCreation : TourWithManagerForCreation
    {
        public ICollection<ShowForCreation> Shows { get; set; }
            = new List<ShowForCreation>();
    }
}
