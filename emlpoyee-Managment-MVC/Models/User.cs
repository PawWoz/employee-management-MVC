using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Imię może zawierać od 2 do 30 znaków.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage ="Imię może zawierać tylko litery.")]
        public required string FirstName { get; set;}

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Nazwisko może zawierać od 2 do 50 znaków.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "Nazwisko może zawierać tylko litery.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Data zatrudnienia jest wymagana.")]
        [DataType(DataType.Date, ErrorMessage = "Nieprawidłowy format daty.")]
        public required DateTime EmploymentDate { get; set; }

        [Required(ErrorMessage = "Kwota wypłaty jest wymagana.")]
        [Range(0, 9999999.99, ErrorMessage = "Wynagrodzenie musi być liczbą dodatnią z maksymalnie dwiema cyframi po przecinku.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public required decimal Salary { get; set; }
        public required List<Department> Departments { get; set; } = new List<Department>();

        [NotMapped]
        [Required(ErrorMessage = "Pracownik musi być przypisany przynajmniej do jednego działu.")]
        public List<int> SelectedDepartmentIds { get; set; } = new List<int>();

    }
}
