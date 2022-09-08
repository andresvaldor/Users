using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Users.Infrastructure.Data.Models;

public class UserDataModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Firstname { get; set; } = string.Empty;

    [Required]
    public string Lastname { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;
}