using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventPlanner.DAL;
using Entities.Models;
using Entities.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;

        public EventController(IEventRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _repository.GetAllEventsAsync();
            var fixedEvents = _mapper.Map<ICollection<EventDTO>>(events);
            return Ok(fixedEvents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var ev = await _repository.GetEventByIdAsync(id);
            if (ev == null)
            {
                return NotFound();
            }
            var fixedEvent = _mapper.Map<EventDTO>(ev);
            return Ok(fixedEvent);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByUser(int userId)
        {
            var events = await _repository.GetEventsByUserIdAsync(userId);
            var fixedEvents = _mapper.Map<ICollection<EventDTO>>(events);
            return Ok(fixedEvents);
        }

        [HttpPost]
        [Authorize(Roles = "host")]
        public async Task<ActionResult<Event>> PostEvent(EventAddDTO ev)
        {
            await _repository.AddEventAsync(ev);
            return CreatedAtAction(nameof(GetEvent), new { id = ev.UserId }, ev);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "host")]
        public async Task<IActionResult> PutEvent(int id, EventUpdateDTO ev)
        {
            await _repository.UpdateEventAsync(id, ev);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "host")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _repository.GetEventByIdAsync(id);
            if (ev == null)
            {
                return NotFound();
            }
            await _repository.DeleteEventAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/attendees")]
        public async Task<ActionResult<IEnumerable<User>>> GetEventAttendees(int id)
        {
            var attendees = await _repository.GetEventAttendeesAsync(id);
            var fixedAttendees = _mapper.Map<IEnumerable<UserDTO>>(attendees);
            return Ok(fixedAttendees);
        }

        [HttpPost("{id}/attendees")]
        [Authorize(Roles = "host")]
        public async Task<IActionResult> AddEventAttendee(int id, int userId)
        {
            await _repository.AddEventAttendeeAsync(id, userId);
            return NoContent();
        }

        [HttpDelete("{id}/attendees/{userId}")]
        [Authorize(Roles = "host")]
        public async Task<IActionResult> RemoveEventAttendee(int id, int userId)
        {
            await _repository.RemoveEventAttendeeAsync(id, userId);
            return NoContent();
        }
    }
}
