using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Infrastructure.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeValidation : ValidationAttribute
    {
        private readonly DateTime startDateTime;

        public DateTimeValidation()
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
               
                return false;
            }
            return true;

        }
    }
}
