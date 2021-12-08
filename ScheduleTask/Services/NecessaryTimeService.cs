using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScheduleTask.Data;
using ScheduleTask.Interface;

namespace ScheduleTask.Services
{
    public class NecessaryTimeService:INecessaryTime
    {
        private readonly AppDbContext _context;

        public NecessaryTimeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsExistTime(string name)
        {
            var time = await _context
                .Times
                .FirstOrDefaultAsync(x => x.Name == name);
            return time != null;
        }

        public async Task<NecessaryTime> GetTime(string name)
        {
            var time = await _context
                .Times
                .FirstOrDefaultAsync(x => x.Name == name);
            return time;
        }

        public async Task<bool> AddTime(string name, byte hour, byte minute)
        {
            if (hour>24 || minute>60)
            {
                return false;
            }

            await _context.Times.AddAsync(new NecessaryTime()
                {Name = name, Hour = hour, Minute = minute});
            return true;
        }
    }
}