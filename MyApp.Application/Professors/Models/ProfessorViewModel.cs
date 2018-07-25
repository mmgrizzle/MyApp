using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Application.Professors.Models
{
    public class ProfessorViewModel
    {
        public ProfessorDto Professor { get; set; }
        public bool EditEnabled { get; set; }
        public bool DeleteEnabled { get; set; }
    }
}
