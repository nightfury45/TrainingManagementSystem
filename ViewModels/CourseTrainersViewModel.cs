using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.ViewModels
{
    public class CourseTrainersViewModel
    {
        public int CourseId { get; set; }
        public string TrainerId { get; set; }
        public IEnumerable<Trainer> Trainers { get; set; }
    }
}