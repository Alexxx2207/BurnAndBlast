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

        [DateTimeValidation(ErrorMessage = "Date & Time must be today or in the future.")]
        public DateTime? StartingDateTime { get; set; }

        [Required(ErrorMessage = "The Duration field is required.")]
        [Range(5, GlobalConstants.GlobalConstants.ClassMaxSeats)]
        public int? DurationInMinutes { get; set; }

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(1, GlobalConstants.GlobalConstants.ClassMaxPrice)]
        public decimal? Price { get; set; }


        [Required(ErrorMessage = "The All Seats field is required.")]
        [Range(GlobalConstants.GlobalConstants.ClassMinDuration, GlobalConstants.GlobalConstants.ClassMaxDuration)]
        public int? AllSeats { get; set; }


    }
}
