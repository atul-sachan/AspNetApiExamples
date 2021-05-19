﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Dtos
{
    public class Tour : TourAbstractBase
    {
        public Guid TourId { get; set; }
        public string Band { get; set; }
        
    }
}
