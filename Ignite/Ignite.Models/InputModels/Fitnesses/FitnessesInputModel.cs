﻿using System.ComponentModel.DataAnnotations;

namespace Ignite.Models.InputModels.Fitnesses
{
    public class FitnessesInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
