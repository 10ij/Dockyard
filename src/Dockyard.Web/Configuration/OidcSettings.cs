using System.ComponentModel.DataAnnotations;
 
namespace Dockyard.Web.Configuration;
 
public sealed class OidcSettings
{
    public const string SectionName = "Oidc";
 
    [Required(AllowEmptyStrings = false)]
    [Url]
    public string ProviderUrl { get; set; } = "";
 
    [Required(AllowEmptyStrings = false)]
    public string ClientId { get; set; } = "";
 
    [Required(AllowEmptyStrings = false)]
    public string ClientSecret { get; set; } = "";
 
    // Empty = first user becomes admin. non-empty = match OIDC "groups" claim
    public string AdminGroup { get; set; } = "";
 
    public bool RequireHttpsMetadata { get; set; } = true;
}