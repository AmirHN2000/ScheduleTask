using System.ComponentModel.DataAnnotations;

namespace ScheduleTask.Data
{
    public class NecessaryTime
    {
        [Key] 
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required] 
        public byte Hour { get; set; }
        [Required] 
        public byte Minute { get; set; }

        public override string ToString()
        {
            var h = Hour.ToString();
            var m = Minute.ToString();
            if (h.Length==1)
            {
                h = "0" + h;
            }
            if (m.Length==1)
            {
                m = "0" + m;
            }
            return $"{h}:{m}";
        }
    }
}