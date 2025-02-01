using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace Infrastructure.Data;

public class ApplicationUser : IdentityUser
{
    public IList<TaskItem> CreatedTaskItems { get; set; } = null!;
    public IList<TaskItem> updatedTaskItems { get; set; } = null!;

}
