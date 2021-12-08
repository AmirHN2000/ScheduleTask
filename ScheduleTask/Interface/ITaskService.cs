using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScheduleTask.Data;
using ScheduleTask.Models.Home;
using TaskStatus = ScheduleTask.Data.TaskStatus;

namespace ScheduleTask.Interface
{
    public interface ITaskService
    {
        public Task<bool> IsExistTaskInNow(string title, string userId);
        
        public Task<NewTask> GetTaskByTitle(string userId, string title);
        
        public Task<NewTask> GetTask(int taskId);
        
        public Task AddTask(NewTask newTask);

        public void DeleteTask(NewTask task);

        public void DoTask(NewTask task);
        
        public void UnDoTask(NewTask task);
        
        public Task<List<NewTask>> GetAllTasks(string userId="", DateTime? date=null, DateTime? fromDate=null,
            DateTime? toDate=null, TaskStatus? status=null);

        public Task<DashboardViewModel> ReportTasks(string userId = "");
    }
}