namespace Dockyard.Web.Models;

public class UserRecord
{
    public long Id { get; set; }
    public string OidcSub { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
    public string? LastLoginAt { get; set; }
}
 
public class AppRecord
{
    public long Id { get; set; }
    public string ContainerId { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PortsJson { get; set; } = "[]";
    public string MetadataJson { get; set; } = "{}";
    public string UpdatedAt { get; set; } = string.Empty;
}
 
public class ContainerPermission
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string ContainerName { get; set; } = string.Empty;
    public bool CanView { get; set; }
    public bool CanStart { get; set; }
    public bool CanStop { get; set; }
}
 
public class AuditLogEntry
{
    public long Id { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = "info";
    public string Username { get; set; } = string.Empty;
    public string OidcSub { get; set; } = string.Empty;
    public string? ContainerName { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string MetadataJson { get; set; } = "{}";
}