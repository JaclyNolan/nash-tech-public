using System.ComponentModel.DataAnnotations;

namespace API_ASPNET_Assignment1.WebAPI.DTOs
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name must be between 1 and 50 characters", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name must be between 1 and 50 characters", MinimumLength = 1)]
        public string LastName { get; set; }

        //To-do: Gender should have use model binding to string or int in http request
        [Required(ErrorMessage = "Gender is required")]
        [Range(0, 3, ErrorMessage = "Value must be between 0 and 3.")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DoB { get; set; }

        [Required(ErrorMessage = "Birthplace is required")]
        [StringLength(100, ErrorMessage = "Birthplace must be between 1 and 100 characters", MinimumLength = 1)]
        public string Birthplace { get; set; }
    }
}
