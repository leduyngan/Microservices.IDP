using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microservices.IDP.Infrastructure.Domains;
using Microsoft.AspNetCore.Identity;

namespace Microservices.IDP.Infrastructure.Entities;

public class Permission : EntityBase<long>
{
    public Permission(string function, string command, string roleId)
    {
        Function = function;
        Command = command;
        RoleId = roleId;
    }

    [Key]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Function { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string RoleId { get; set; }

    [ForeignKey("RoleId")] 
    
    public virtual IdentityRole Role { get; set; }

    [Key]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Command { get; set; }
}