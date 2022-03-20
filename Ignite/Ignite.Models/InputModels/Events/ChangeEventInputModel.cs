using Ignite.Infrastructure.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Models.InputModels.Events
{
    public class ChangeEventInputModel
    {
        public string Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string? Description { get; set; }

        [DateTimeValidation(ErrorMessage = "Date & Time must be today or in the future.")]
        public DateTime? StartingDateTime { get; set; }
    }
}
