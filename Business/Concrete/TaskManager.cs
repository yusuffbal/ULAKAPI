using Business.Abstract;
using Dataaccess.Abstract;
using Entities.Dtos;
using Entities.Filter;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TaskManager : ITaskService
    {

        private readonly IUsersDal _userDal;
        private readonly IUserTeamsDal _userTeamDal;
        private readonly ITeamsDal _teamsDal;
        private readonly ISehirDal _sehirDal;
        private readonly ITasksDal _taskDal;
        private readonly ITaskLocationDal _taskLocationDal;

        public TaskManager( IUsersDal usersDal, IUserTeamsDal userTeamsDal, ITeamsDal teamsDal, ISehirDal sehirDal, ITasksDal taskDal, ITaskLocationDal taskLocationDal)
        {
            _userDal = usersDal;
            _userTeamDal = userTeamsDal;
            _teamsDal = teamsDal;
            _sehirDal = sehirDal;
            _taskDal = taskDal;
            _taskLocationDal = taskLocationDal;
        }

        public void AddTask(AddTaskDto task)
        {

            var cityId = _sehirDal.GetFirstOrDefault(x => x.SehirName.ToLower() == task.SelectedCity.ToLower()).Id;

            var projectTask = new ProjectTask
            {
                Title = task.Title,
                Description = task.Description,
                AssignedByUserId = task.AssignedByUserId,
                AssignedToTeamId = task.AssignedToTeamId,
                AssignedToUserId = task.AssignedToUserId,
                IsTeamTask = task.AssignedToUserId == null ? 1 : 0,
                TaskType = task.AssignedToUserId == null ? 1 : 0,
                TaskStatus = 0,
                TaskPriority = task.TaskPriority,
                DateOfStart = DateOnly.TryParse(task.DateOfStart, out var dateOfStart) ? dateOfStart : default,
                DateOfEnd = DateOnly.TryParse(task.DateOfEnd, out var dateOfEnd) ? dateOfEnd : default,
                TimeOfStart = TimeOnly.TryParse(task.TimeOfStart, out var timeOfStart) ? timeOfStart : default,
                Hour = task.Hour,
            };

            var addedTask = _taskDal.Add(projectTask);

            var taskLocation = new TaskLocation
            {
                TaskId = addedTask.TaskId,
                Latitude = task.Latitude,
                Longitude = task.Longitude,
                Name = task.Title,
                Address = task.Address,
                cityId = cityId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _taskLocationDal.Add(taskLocation);
        }

        public void AddUserTeam(UserTeam teamDto)
        {
            _userTeamDal.Add(teamDto);
        }

        public void CreateTeam(CreateTeamDto teamDto)
        {
            var Team = new Team
            {
                CreatedAt = DateTime.Now,
                TeamName = teamDto.TeamName,
                CreatedByUserId = teamDto.CreatedByUserId
            };
           
            _teamsDal.Add(Team);
        }

        public void DeleteTask(int taskId)
        {
            var task = _taskDal.GetFirstOrDefault(x => x.TaskId == taskId);
            var taskLoc = _taskLocationDal.GetFirstOrDefault(x => x.TaskId == taskId);
            if(task != null)
            {
                _taskLocationDal.Delete(taskLoc);
                _taskDal.Delete(task);
            }
        }

        public Task<List<sehir>> GetCityList()
        {
            var cityList = _sehirDal.GetList();
            var cityListed = new List<sehir>();
            foreach (var city in cityList)
            {
                cityListed.Add(city);
            }
            return Task.FromResult(cityListed);
        }

        public async Task<List<DashboardTaskListDto>> GetDashboardTask(DashboardMapFilter dashboardMapFilter)
        {
            var tasks = await Task.Run(() =>
                (from task in _taskDal.GetList()
                 join location in _taskLocationDal.GetList()
                 on task.TaskId equals location.TaskId
                 where
                     (dashboardMapFilter.CityId.Contains(-1) || dashboardMapFilter.CityId.Contains(location.cityId.GetValueOrDefault())) &&
                     (dashboardMapFilter.TeamId == -1 || task.AssignedToTeamId == dashboardMapFilter.TeamId) &&
                     (dashboardMapFilter.TaskStatus == -1 || task.TaskStatus == dashboardMapFilter.TaskStatus) &&
                     (dashboardMapFilter.TaskType == -1 || task.TaskType == dashboardMapFilter.TaskType)
                 select new DashboardTaskListDto
                 {
                     Id = task.TaskId,
                     Title = task.Title,
                     Description = task.Description,
                     Latitude = location.Latitude,
                     Longitude = location.Longitude,
                     Status= task.TaskStatus
                 }).ToList()
            );

            return tasks;
        }

        public async Task<List<DashboardTaskListDto>> GetMyTask(int userId)
        {
            var tasks = await Task.Run(() =>
               (from task in _taskDal.GetList()
                join location in _taskLocationDal.GetList()
                on task.TaskId equals location.TaskId
                where
                     (task.AssignedToUserId == userId) 
                select new DashboardTaskListDto
                {
                    Id = task.TaskId,
                    Title = task.Title,
                    Description = task.Description,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Status = task.TaskStatus
                }).ToList()
           );
            return tasks;
        }

        public Task<List<TaskTeamDto>> GetTeamList()
        {
            var teams = _teamsDal.GetList();
            var teamDto = new List<TaskTeamDto>();

            foreach (var team in teams)
            {
                var taskTeamDto = new TaskTeamDto
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName
                };
                teamDto.Add(taskTeamDto);
            }

            return Task.FromResult(teamDto); 
        }

        public Task<List<TeamPersonDto>> GetTeamPersonTable()
        {
            var userTeams = _userTeamDal.GetList();
            var teamPersonList = new List<TeamPersonDto>();

            foreach (var userTeam in userTeams)
            {
                var user = _userDal.GetFirstOrDefault(x => x.UserId == userTeam.UserId);
                var teams = _teamsDal.GetFirstOrDefault(x => x.TeamId == userTeam.TeamId);
                if (user != null)
                {
                    var teamDto = new TeamPersonDto
                    {
                        TeamName = teams.TeamName,
                        PersonName = user.FirstName,
                        PersonLastName = user.LastName
                    };

                    teamPersonList.Add(teamDto);
                }
            }

            return Task.FromResult(teamPersonList);
        }

        public Task<List<TeamListDto>> GetTeamTable()
        {
            var teamList = _teamsDal.GetList();
            var teamListDto = new List<TeamListDto>();

            foreach (var team in teamList)
            {
                var user = _userDal.GetFirstOrDefault(x => x.UserId == team.CreatedByUserId);

                if (user != null)
                {
                    var teamDto = new TeamListDto
                    {
                        Id = team.TeamId,
                        TeamName = team.TeamName,
                        TeamCreatorLastName = user.LastName,
                        TeamCreatorName = user.FirstName
                    };

                    teamListDto.Add(teamDto);
                }
            }

            return Task.FromResult(teamListDto);
        }

        public Task<List<TeamTaskListDto>> GetTeamTaskList()
        {
            var tasks = _taskDal.GetList();
            var teamTaskListDtos = new List<TeamTaskListDto>();

            foreach (var task in tasks)
            {
                var user = _userDal.GetFirstOrDefault(x => x.UserId == task.AssignedToUserId);
                var teamTaskList = new TeamTaskListDto 
                {
                    TaskId = task.TaskId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Title = task.Title,
                    Description = task.Description,
                    status = task.TaskStatus == 0 ? "Beklemede" :
                             task.TaskStatus == 1 ? "Aktif" :
                             task.TaskStatus == 2 ? "Tamamlandı" :
                             "Bilinmiyor",
                    TaskType = task.TaskType == 0 ? "Test" : "Bilinmiyor"
                };

                teamTaskListDtos.Add(teamTaskList);
            }
            return Task.FromResult(teamTaskListDtos);
        }

        public async Task<List<UserTaskDto>> GetUserList()
        {
            var users =  _userDal.GetList(); 
            var userTaskDtos = new List<UserTaskDto>(); 

            foreach (var user in users)
            {
                var userTaskDto = new UserTaskDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                };

                userTaskDtos.Add(userTaskDto);
            }

            return userTaskDtos;
        }

        public Task<List<UserSearchDto>> GetUserSearchList(string searchText)
        {
            var userList = _userDal.GetList(x =>
                (x.FirstName != null && x.FirstName.ToLower().Contains(searchText.ToLower())) ||
                (x.LastName != null && x.LastName.ToLower().Contains(searchText.ToLower()))
            );
            var userSearchDto = new List<UserSearchDto>();
            foreach(var user in userList)
            {
                userSearchDto.Add(new UserSearchDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }

            return Task.FromResult(userSearchDto);
        }
    }
}
