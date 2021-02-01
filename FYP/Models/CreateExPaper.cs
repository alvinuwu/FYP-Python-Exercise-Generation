using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public class CreateExPaper
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Exercise Paper Name required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "One or more topics required")]
        public List<int> Topics { get; set; }

        [Required(ErrorMessage = "At least one question required")]
        [Range(1, 30, ErrorMessage ="Only 30 questions maximum can be selected")]
        public int OETotalQns { get; set; }

        [Range(0, 500, ErrorMessage = "The time set cannot be a negative value!")]
        public int? Time { get; set; }
    }
}
