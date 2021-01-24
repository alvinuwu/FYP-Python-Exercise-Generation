using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public class AutoGenerateQn
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "One or more topics required")]
        public List<int> Topics { get; set; }

        //public int UseCount { get; set; }
    }
}
