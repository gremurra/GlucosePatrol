﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlucosePatrol.Models
{
    public class EntryListItem
    {
        [Required]
        public int PatientId { get; set; }
        public int EntryId { get; set; }
        [Range(30, 700)]
        public int BloodSugarReading { get; set; }
        [Display(Name ="Created")]
        public DateTimeOffset CreatedUtc { get; set; }
         
    }
}
