using System;

namespace BusinessApplication.TourManagement.Api.Dtos
{
    public class Tour : TourAbstractBase
    {
        public Guid TourId { get; set; }
        public string Band { get; set; }   
    }
}
