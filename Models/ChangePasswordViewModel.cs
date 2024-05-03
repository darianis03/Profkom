using System.ComponentModel.DataAnnotations;

namespace Profkom.Models;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Old Password is required")]
    [DataType(DataType.Password)]
    public string? OldPassword { get; set; }

    [Required(ErrorMessage = "New Password is required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
        ErrorMessage = "Password must meet complexity requirements")]
    public string? NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
}