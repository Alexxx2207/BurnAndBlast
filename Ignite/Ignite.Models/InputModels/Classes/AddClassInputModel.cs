using Ignite.GlobalConstants;
using Ignite.Infrastructure.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.InputModels.Classes
{
    public class AddClassInputModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [DateTimeValidation]
        public DateTime? StartingDateTime { get; set; }

        [Required]
        public int? DurationInMinutes { get; set; }

        [Range(1, GlobalConstants.GlobalConstants.ClassMaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, GlobalConstants.GlobalConstants.ClassMaxSeats)]
        public int? AllSeats { get; set; }


    }
}
