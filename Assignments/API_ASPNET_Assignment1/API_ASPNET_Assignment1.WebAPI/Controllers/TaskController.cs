using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_ASPNET_Assignment1.Models.Entities;
using API_ASPNET_Assignment1.Models.DTOs;
using AutoMapper;

namespace API_ASPNET_Assignment1.WebAPI.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskContext _context;
        private readonly IMapper _mapper;

        public TaskController(TaskContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("generate")]
        public async Task<ActionResult> GenerateTasks(int? count)
        {
            try
            {
                List<TaskModel> tasks = TaskSampleDbSet.GenerateTaskModels(count ?? 10);

                // Add tasks to the database
                await _context.Tasks.AddRangeAsync(tasks);
                await _context.SaveChangesAsync();

                return Ok("Tasks generated and saved to the database.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // GET: api/Task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetTasks()
        {
            var taskModels = await _context.Tasks.ToListAsync();
            return _mapper.Map<List<TaskViewModel>>(taskModels);
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskViewModel>> GetTaskModel(Guid id)
        {
            var taskModel = await _context.Tasks.FindAsync(id);

            if (taskModel == null)
            {
                return NotFound();
            }

            return _mapper.Map<TaskViewModel>(taskModel);
        }

        // PUT: api/Task/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskModel(Guid id, TaskModel taskModel)
        {
            if (id != taskModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Task
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskViewModel>> PostTaskModel(TaskRequestCreate taskRequestCreate)
        {
            var taskModel = _mapper.Map<TaskModel>(taskRequestCreate);
            _context.Tasks.Add(taskModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskModel), new { id = taskModel.Id }, taskModel);
        }

        // POST: api/tasks/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult> PostTaskModelsAsync([FromBody] List<TaskRequestCreate> taskRequestCreates)
        {
            if (taskRequestCreates == null || taskRequestCreates.Count == 0)
            {
                return BadRequest("No tasks provided.");
            }
            List<TaskModel> taskModels = _mapper.Map<List<TaskModel>>(taskRequestCreates);
            _context.Tasks.AddRange(taskModels);
            await _context.SaveChangesAsync();
            return Created();
        }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskModelAsync(Guid id)
        {
            var taskModel = await _context.Tasks.FindAsync(id); 
            if (taskModel == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(taskModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/tasks/bulk
        [HttpDelete("bulk")]
        public async Task<ActionResult> DeleteTaskModelsAsync([FromBody] List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest("No tasks provided.");
            }
            var taskModels = await _context.Tasks.Where(t => ids.Contains(t.Id)).ToListAsync();
            var idsDeleted = taskModels.Select(t => t.Id).ToList();
            var idsFailed = ids.Except(idsDeleted);
            _context.Tasks.RemoveRange(taskModels);
            await _context.SaveChangesAsync();

            return Ok( new
                {
                    Deleted = idsDeleted,
                    Failed = idsFailed
                }
            );
        }

        private bool TaskModelExists(Guid id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
