using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.ViewModels
{
    public class CourseTraineesViewModel
    {
        public int CourseId { get; set; }
        public string TraineeId { get; set; }
        public IEnumerable<Trainee> Trainees { get; set; }
    }
}