using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Dtos
{
    public class TourWithEstimatedProfit: Tour
    {
        public decimal EstimatedProfits { get; set; }
    }
}
