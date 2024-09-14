
using ContactManager.Services.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Contact
    {
        [Key]
        [ValidPhoneNumber]
        public string Phone { get; set; }
        [ValidName]
        public string Name { get; set; }
        [ValidDateOfBirth]
        public DateTime DateOfBirth { get; set; }
        public bool Married { get; set; }
        [ValidSalary]
        public decimal Salary { get; set; }
    }
}
