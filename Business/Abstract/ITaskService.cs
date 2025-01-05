using Entities.Dtos;
using Entities.Filter;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITaskService
    {
        Task<List<UserTaskDto>> GetUserList();
        Task<List<TaskTeamDto>> GetTeamList();
        void AddTask(AddTaskDto task);
        Task<List<sehir>> GetCityList();
        Task<List<DashboardTaskListDto>> GetDashboardTask(DashboardMapFilter dashboardMapFilter);
        Task<List<DashboardTaskListDto>> GetMyTask(int userId);
        Task<List<TeamListDto>> GetTeamTable();
        Task<List<UserSearchDto>> GetUserSearchList(string searchText);
        void CreateTeam(CreateTeamDto teamDto);
        Task<List<TeamPersonDto>> GetTeamPersonTable();
        void AddUserTeam(UserTeam teamDto);
        Task<List<TeamTaskListDto>> GetTeamTaskList();
        void DeleteTask(int taskId);


    }
}
