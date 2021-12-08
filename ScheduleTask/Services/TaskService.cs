using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScheduleTask.Data;
using ScheduleTask.Interface;
using ScheduleTask.Models.Home;
using TaskStatus = ScheduleTask.Data.TaskStatus;

namespace ScheduleTask.Services
{
    public class TaskService:ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsExistTaskInNow(string title, string userId)
        {
            var task = await _context
                .Tasks
                .FirstOrDefaultAsync(x => EF.Functions.DateDiffDay(x.CreateDate,DateTime.Now)==0
                                          && x.Title == title && x.UserId==userId);
            return task != null;
        }

        public async Task<NewTask> GetTaskByTitle(string userId, string title)
        {
            return await _context
                .Tasks
                .FirstOrDefaultAsync(x => x.Title == title &&x.UserId==userId);
        }

        public async Task<NewTask> GetTask(int taskId)
        {
            return await _context.Tasks.FindAsync(taskId);
        }
        
        public async Task AddTask(NewTask newTask)
        {
            await _context
                .Tasks
                .AddAsync(newTask);
        }

        public void DeleteTask(NewTask task)
        { 
            _context
                    .Tasks
                    .Remove(task);
        }

        public void DoTask(NewTask task)
        {
            task.Status = TaskStatus.Done;
        }

        public void UnDoTask(NewTask task)
        {
            task.Status = TaskStatus.Pending;
        }
        
        public async Task<List<NewTask>> GetAllTasks(string userId="", DateTime? date=null,  DateTime? fromDate=null,
            DateTime? toDate=null, TaskStatus? status=null)
        {
            var query = _context
                .Tasks
                .AsNoTracking();
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }
            if (date!=null)
            {
                query = query.Where(x => EF.Functions.DateDiffDay(x.CreateDate,date)==0);
            }
            else if (fromDate!=null && toDate!=null)
                query = query.Where(x => EF.Functions.DateDiffDay(fromDate, x.CreateDate) >= 0
                                         && EF.Functions.DateDiffDay(x.CreateDate, toDate) >= 0);
            
            if (status!=null)
            {
                query = query.Where(x => x.Status == status);
            }
            return await query.ToListAsync();
        }

        public async Task<DashboardViewModel> ReportTasks(string userId="")
        {
            var query = _context.Tasks
                .AsNoTracking();
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }

            var totalCount = await query.CountAsync();
            var doneCount = await query.Where(x => x.Status == TaskStatus.Done)
                .CountAsync();
            var pendingCount = await query.Where(x => x.Status == TaskStatus.Pending)
                .CountAsync();
            
            var model=new DashboardViewModel()
            {
                Total = totalCount,
                Done = doneCount,
                Pending = pendingCount
            };
            if (model.Total!=0)
            {
                model.DoneD = (int) ((model.Done / (double)model.Total) * 100);
                model.PendingD = (int) (model.Pending / (double)model.Total * 100);
            }
            else
            {
                model.DoneD = 0;
                model.PendingD = 0;
            }

            return model;
        }
    }
}