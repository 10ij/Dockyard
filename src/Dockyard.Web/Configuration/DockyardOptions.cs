using System.ComponentModel.DataAnnotations;

namespace Dockyard.Web.Configuration;

public sealed class DockyardOptions
{
    public const string SectionName = "Dockyard";
 
    [Required(AllowEmptyStrings = false)]
    public string DockerSocketPath { get; set; } = "/var/run/docker.sock";
    
    // 5 Minutes, in the future I want to be able to stagger for each container
    // Afterall this is for my light home server so sharp loads is a no no
    [Range(5, 3600)]
    public int SyncIntervalSeconds { get; set; } = 300;
 
    [Range(1, 3650)]
    public int AuditRetentionDays { get; set; } = 90;
 
    [Range(1, 100)]
    public int MaxBackupFiles { get; set; } = 7;
 
    [Required(AllowEmptyStrings = false)]
    public string BackupDirectory { get; set; } = "/app/data/backups";
}