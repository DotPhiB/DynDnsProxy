using System.ComponentModel.DataAnnotations;

namespace DynDnsProxy;

public class DynDnsConfiguration
{
    [Required] public required string UpdateUrl { get; init; }
    [Required] public required string UserName { get; init; }
    [Required] public required string Password { get; init; }
}
