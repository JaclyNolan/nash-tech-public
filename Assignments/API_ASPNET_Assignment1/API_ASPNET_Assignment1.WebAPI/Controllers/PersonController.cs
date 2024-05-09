using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_ASPNET_Assignment1.Models.Entities;
using API_ASPNET_Assignment1.BusinessLogic.Services;
using API_ASPNET_Assignment1.Models.DTOs;
using API_ASPNET_Assignment1.WebAPI.DTOs;
using AutoMapper;
using API_ASPNET_Assignment1.BusinessLogic.Exceptions;

namespace API_ASPNET_Assignment1.WebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonBusinessLogic _personBusinessLogic;
        private readonly IMapper _mapper;

        public PersonController(IPersonBusinessLogic personBusinessLogic, IMapper mapper)
        {
            _personBusinessLogic = personBusinessLogic;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint to generate tasks.
        /// </summary>
        /// <param name="count">Optional. The number of tasks to generate.</param>
        /// <returns>A message indicating that tasks were generated and saved.</returns>
        [HttpPost("people/generate")]
        public async Task<ActionResult> GenerateTasks(int? count)
        {
            try
            {
                List<Person> people = _personBusinessLogic.GeneratePeopleAsync(count ?? 10);
                await _personBusinessLogic.AddPeopleAsync(people);

                return Ok("Tasks generated and saved to the database.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint to retrieve a list of people based on the provided query parameters.
        /// </summary>
        /// <param name="personGetRequest">The query parameters to filter people.</param>
        /// <returns>A list of people.</returns>
        [HttpGet("people")]
        public async Task<ActionResult<IEnumerable<PersonViewModel>>> GetPeopleAsync([FromQuery] PersonGetRequest personGetRequest)
        {
            return _mapper.Map<List<PersonViewModel>>(await _personBusinessLogic.GetPeopleAsync(personGetRequest));
        }

        /// <summary>
        /// Endpoint to retrieve a specific person by ID.
        /// </summary>
        /// <param name="id">The ID of the person to retrieve.</param>
        /// <returns>The person information.</returns>
        [HttpGet("person/{id}")]
        public async Task<ActionResult<PersonViewModel>> GetPerson(Guid id)
        {
            var person = await _personBusinessLogic.GetPersonAsync(id);

            if (person == null) return NotFound();

            return _mapper.Map<PersonViewModel>(person);
        }

        /// <summary>
        /// Endpoint to update a person's information.
        /// </summary>
        /// <param name="id">The ID of the person to update.</param>
        /// <param name="personUpdateRequest">The updated information of the person.</param>
        /// <returns>No content if the update was successful, otherwise not found.</returns>
        [HttpPut("person/{id}")]
        public async Task<IActionResult> PutPerson(Guid id, [FromBody] PersonUpdateRequest personUpdateRequest)
        {
            try
            {
                await _personBusinessLogic.UpdatePersonAsync(id, personUpdateRequest);
            }
            catch (PersonNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Endpoint to create a new person.
        /// </summary>
        /// <param name="personCreateRequest">The information of the person to create.</param>
        /// <returns>The created person information.</returns>
        [HttpPost("person")]
        public async Task<ActionResult<PersonViewModel>> PostPerson([FromBody] PersonCreateRequest personCreateRequest)
        {
            Guid id = await _personBusinessLogic.CreatePersonAsync(personCreateRequest);
            return CreatedAtAction("GetPerson", new { id = id }, GetPerson(id));
        }

        /// <summary>
        /// Endpoint to delete a person by ID.
        /// </summary>
        /// <param name="id">The ID of the person to delete.</param>
        /// <returns>No content if the deletion was successful, otherwise not found.</returns>
        [HttpDelete("person/{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            try
            {
                await _personBusinessLogic.DeletePersonAsync(id);
            }
            catch (PersonNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
