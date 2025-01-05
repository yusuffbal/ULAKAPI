using Business.Abstract;
using Entities.Dtos;
using Entities.Filter;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ULAKAPI.Hubs;

namespace ULAKAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpGet("GetUserList")]
        public async Task<ActionResult<List<UserTaskDto>>> GetTaskForUserList()
        {
            try
            {
                var userList = await _taskService.GetUserList();

                if (userList == null || userList.Count == 0)
                {
                    return NotFound("No messages found.");
                }
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetTeamList")]
        public async Task<ActionResult<List<TaskTeamDto>>> GetTaskForTeamList()
        {
            try
            {
                var teamList = await _taskService.GetTeamList();

                if (teamList == null || teamList.Count == 0)
                {
                    return NotFound("No messages found.");
                }
                return Ok(teamList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] AddTaskDto task)
        {
            if (task == null)
            {
                return BadRequest("Task data is required.");
            }

            try
            {
                _taskService.AddTask(task);
                return Ok("Task successfully added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("GetCity")]
        public async Task<ActionResult<List<sehir>>> GetCity()
        {
            var cityList = await _taskService.GetCityList();

            return Ok(cityList);

        }
        [HttpPost("GetDashboardTask")]
        public async Task<ActionResult<List<DashboardTaskListDto>>> GetDashboardTask([FromBody] DashboardMapFilter dashboardMapFilter)
        {
            var taskList = await _taskService.GetDashboardTask(dashboardMapFilter);

            return Ok(taskList);

        }

        [HttpGet("GetDashboardMyTask/{userId}")]
        public async Task<ActionResult<List<DashboardTaskListDto>>> GetDashboardMyTask(int userId)
        {
            var taskList = await _taskService.GetMyTask(userId);

            return Ok(taskList);

        }
        [HttpGet("GetTeamTable")]

        public async Task<ActionResult<List<TeamListDto>>> GetTeamTable()
        {
            var teamList = await _taskService.GetTeamTable();

            return Ok(teamList);

        }

        [HttpPost("GetUserBySearch/{SearchText}")]
        public async Task<ActionResult<List<UserSearchDto>>> GetUserSearchList(string SearchText)

        {
            var userSearch = _taskService.GetUserSearchList(SearchText);

            return Ok(userSearch);
        }

        [HttpPost("CreateTeam")]
        public IActionResult CreateTeam([FromBody] CreateTeamDto teamDto)
        {
            if (teamDto == null)
            {
                return BadRequest("Task data is required.");
            }

            try
            {
                _taskService.CreateTeam(teamDto);
                return Ok("Task successfully added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetTeamPersonTable")]

        public async Task<ActionResult<List<TeamPersonDto>>> GetTeamPersonTable()
        {
            var teamList = await _taskService.GetTeamPersonTable();

            return Ok(teamList);

        }
        [HttpPost("AddUserTeam")]

        public async Task<ActionResult<List<TeamPersonDto>>> AddUserTeam([FromBody]UserTeam userTeam)
        {
             _taskService.AddUserTeam(userTeam);

            return Ok("Task successfully added.");

        }

        [HttpGet("GetTeamTaskList")]
        public async Task<ActionResult<List<TeamTaskListDto>>> GetTeamTaskList()
        {
            var teamTaskList = await _taskService.GetTeamTaskList();
            return Ok(teamTaskList);
        }

        [HttpDelete("DeleteTask/{taskId}")]
        public IActionResult AddTask( int taskId)
        {
            _taskService.DeleteTask(taskId);

            return Ok("Görev Başaraıyla Silindi");

        }
    }
}
