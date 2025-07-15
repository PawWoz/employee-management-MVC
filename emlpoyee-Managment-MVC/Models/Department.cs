using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Department
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Dział jest wymagany.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "Dział może zawierać tylko litery.")]
        public required string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
