using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleTask.Data
{
    public class NewTask
    {
        public NewTask()
        {
            CreateDate = DateTime.Now;
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Required] public TaskStatus Status { get; set; } = TaskStatus.Pending;

        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }

        [Required]
        [Range(0,24)]
        public byte Hour { get; set; }
        [Required]
        [Range(0,60)]
        public byte Minute { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}