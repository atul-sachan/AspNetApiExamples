﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Dtos
{
    public class TourForCreation : TourAbstractBase
    {
        public Guid BandId { get; set; }
    }
}
