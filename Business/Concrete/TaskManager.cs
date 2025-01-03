using Business.Abstract;
using Dataaccess.Abstract;
using Entities.Dtos;
using Entities.Filter;
using Entities.Models;
using System;
using System.Collections.Generic;
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


        public async Task<List<UserTaskDto>> GetUserList()
        {
            // Kullanıcıları listele
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

    }
}
