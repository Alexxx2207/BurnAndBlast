using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Infrastructure.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EventDateTimeValidation : ValidationAttribute
    {
        private readonly DateTime startDateTime;

        public EventDateTimeValidation()
        {
            startDateTime = DateTime.Now;
        }

        public override bool IsValid(object? value)
        {
            if (value == null) 
                return false;
            else if(!DateTime.TryParse(value.ToString(), out DateTime result))
                return false;

            if ((DateTime)value < startDateTime)
            {
                ErrorMessage = "Date & Time must be today or in the future.";
                return false;
            }
            return true;

        }
    }
}
