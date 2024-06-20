using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Entities.Models;
using Entities.DTOs;

namespace EventPlanner.DAL
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int eventId);
        //Task AddEventAsync(Event ev);
        Task AddEventAsync(EventAddDTO ev);

        Task UpdateEventAsync(int eventId, EventUpdateDTO ev);
        Task DeleteEventAsync(int eventId);
        Task<IEnumerable<User>> GetEventAttendeesAsync(int eventId);
        Task AddEventAttendeeAsync(int eventId, int userId);
        Task RemoveEventAttendeeAsync(int eventId, int userId);

        Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId); 
    }

    public class EventRepository : IEventRepository
    {
        private readonly EventPlannerContext _context;

        public EventRepository(EventPlannerContext context)
        {
            _context = context;
        }



        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.AttendEvents)
                .Include(e => e.User)
                .ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int eventId)
        {
            return await _context.Events
                .Include(e => e.AttendEvents)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EventId == eventId);
        }

        public async Task AddEventAsync(EventAddDTO ev)
        {
            var newEvent = new Event
            {
                Name = ev.Name,
                Location = ev.Location,
                DateTime = ev.DateTime,
                Category = ev.Category,
                Description = ev.Description,
                Picture = ev.Picture,
                UserId = ev.UserId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(int eventId, EventUpdateDTO ev)
        {
            var eventToUpdate = await _context.Events.FindAsync(eventId);
            if (eventToUpdate != null)
            {
                eventToUpdate.Name = ev.Name;
                eventToUpdate.Location = ev.Location;
                eventToUpdate.DateTime = ev.DateTime;
                eventToUpdate.Category = ev.Category;
                eventToUpdate.Description = ev.Description;
                eventToUpdate.Picture = ev.Picture;
                eventToUpdate.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteEventAsync(int eventId)
        {
            var ev = await _context.Events.FindAsync(eventId);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetEventAttendeesAsync(int eventId)
        {
            return await _context.AttendEvents
                .Where(ae => ae.EventId == eventId)
                .Select(ae => ae.User)
                .ToListAsync();
        }

        public async Task AddEventAttendeeAsync(int eventId, int userId)
        {
            _context.AttendEvents.Add(new AttendEvent { EventId = eventId, UserId = userId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveEventAttendeeAsync(int eventId, int userId)
        {
            var attendEvent = await _context.AttendEvents
                .FirstOrDefaultAsync(ae => ae.EventId == eventId && ae.UserId == userId);

            if (attendEvent != null)
            {
                _context.AttendEvents.Remove(attendEvent);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId)
        {
            var eventsOrganized = await _context.Events
                                                 .Where(e => e.UserId == userId)
                                                 .ToListAsync();

            var eventsAttended = await _context.AttendEvents
                                               .Where(ae => ae.UserId == userId)
                                               .Select(ae => ae.Event)
                                               .ToListAsync();

            var allEvents = eventsOrganized.Union(eventsAttended).ToList();

            return allEvents;
        }
    }
}
