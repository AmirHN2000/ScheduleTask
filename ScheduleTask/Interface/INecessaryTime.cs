using System.Threading.Tasks;
using ScheduleTask.Data;

namespace ScheduleTask.Interface
{
    public interface INecessaryTime
    {
        public Task<bool> IsExistTime(string name);

        public Task<NecessaryTime> GetTime(string name);

        public Task<bool> AddTime(string name, byte hour, byte minute);
    }
}