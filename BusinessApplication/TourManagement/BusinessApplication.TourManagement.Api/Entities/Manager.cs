using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Entities
{
    public class Manager : AuditableEntity
    {
        public Guid ManagerId { get; set; }
        public string Name { get; set; }
    }
}
