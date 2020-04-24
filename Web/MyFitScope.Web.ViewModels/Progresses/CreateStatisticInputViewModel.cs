﻿namespace MyFitScope.Web.ViewModels.Progresses
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class CreateStatisticInputViewModel
    {
        [Required]
        [Range(20, double.MaxValue, ErrorMessage = "The Weight value must be greater than 20kg")]
        public double Weight { get; set; }

        public double? Biceps { get; set; }

        public double? Chest { get; set; }

        public double? Stomach { get; set; }

        public double? Hips { get; set; }

        public double? Thigh { get; set; }

        public double? Calf { get; set; }
    }
}
