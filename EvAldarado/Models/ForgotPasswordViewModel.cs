using System.ComponentModel.DataAnnotations;

namespace EvAldarado.ViewModels
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}
