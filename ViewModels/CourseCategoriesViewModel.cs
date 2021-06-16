using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingManagementSystem.Models;
using System.Data.Entity;

namespace TrainingManagementSystem.ViewModels
{
    public class CourseCategoriesViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}