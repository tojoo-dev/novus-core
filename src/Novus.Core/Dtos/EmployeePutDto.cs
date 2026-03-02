using System.ComponentModel.DataAnnotations;

namespace Novus.Core.Dtos
{
    public class EmployeePutDto
    {
        [Required]
        public string LastName { get; set; }
    }
}
