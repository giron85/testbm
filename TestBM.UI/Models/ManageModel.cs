using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBM.UI.Models
{
    public class ManageModel
    {
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Budget mensile")]
        public double MontlyBudget { get; set; }
    }
}
