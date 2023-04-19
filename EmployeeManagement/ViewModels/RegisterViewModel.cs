using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		[EmailAddress]
		[Remote(action:"IsEmailInUse", controller:"action")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[Display(Name = "Confirm Password")]
		[Compare("Password", ErrorMessage = "Password and confirmation password do not much")]
		public string ConfirmPassword { get; set; }

	}
}
