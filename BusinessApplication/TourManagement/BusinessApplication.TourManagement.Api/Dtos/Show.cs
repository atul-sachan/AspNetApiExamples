using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Dtos
{
    public class Show
    {
        public Guid ShowId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Venue { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
