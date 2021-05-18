using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Entities
{
    [Table("Bands")]
    public class Band : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BandId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
