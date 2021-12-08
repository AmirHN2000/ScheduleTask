using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ScheduleTask.Data
{
    public class User:IdentityUser
    {
        [Required]
        [StringLength(80)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(80)]
        public string Family { get; set; }
    }
}