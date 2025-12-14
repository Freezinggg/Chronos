using Chronos.Common;
using Chronos.Domain;
using Chronos.DTO.Event;
using Chronos.Mapping;
using Chronos.Validator;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Chronos.Application
{
    public class EventServices
    {
        public IEventRepository _repo { get; set; }
        public EventServices(IEventRepository repo)
        {
            _repo = repo;
        }

        public ApiResponse<EventDto?> Get(Guid id)
        {
            try
            {
                Event? e = _repo.Get(id);
                if (e == null) new ApiResponse<EventDto?> { Success = false, Message = "Event doesnt exist." };

                EventDto dto = new()
                {
                    EventDescription = e.EventDescription,
                    EventEndAt = e.EventEndAt,
                    EventStartAt = e.EventStartAt,
                    EventName = e.EventName,
                    Id = e.Id,
                    EventNotes = e.EventNotes,
                    HasReminder = e.HasReminder,
                    Location = e.Location,
                };

                return new ApiResponse<EventDto?> { Success = true, Data = dto };
            }
            catch
            {
                return new ApiResponse<EventDto?> { Success = false, Message = "Error occured when getting Event." };
            }
        }
        public ApiResponse<string> CreateEvent(CreateEventDto dto)
        {
            try
            {
                //Check for inputs
                string validateInputResult = EventValidator.ValidateInput(dto);
                if (validateInputResult != "") return new ApiResponse<string> { Success = false, Message = validateInputResult };

                //Mapping
                Event e = EventMapping.MappingToEntity(dto);
                e.CreatedAt = DateTime.Now;

                //Domain validation
                if (!e.ValidateDate()) return new ApiResponse<string> { Success = false, Message = "Invalid date, date cannot be yesterday." };

                //Save to DB
                Event? result = _repo.Create(e);
                if (result == null) return new ApiResponse<string> { Success = false, Message = "Save failed. Please try again." };

                return new ApiResponse<string> { Success = true, Message = "Create Event success.", Data = result.Id.ToString() };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Unexpected error occured on the server, please try again and make sure all data are correct." };
            }
        }

        public ApiResponse<IEnumerable<EventDto>> GetEvents(string? startDate, string? endDate)
        {
            DateTime? start = null;
            DateTime? end = null;

            DateTime parsedStart;
            DateTime parsedEnd;


            //Check date time formatting 
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                if (!DateTime.TryParse(startDate, out parsedStart))
                {
                    return new ApiResponse<IEnumerable<EventDto>>
                    {
                        Success = false,
                        Message = "Invalid startDate format. Use dd-MM-yyyy"
                    };
                }

                start = parsedStart;
            }

            if (!string.IsNullOrWhiteSpace(endDate))
            {
                if (!DateTime.TryParse(endDate, out parsedEnd))
                {
                    return new ApiResponse<IEnumerable<EventDto>>
                    {
                        Success = false,
                        Message = "Invalid endDate format. Use dd-MM-yyyy"
                    };
                }

                end = parsedEnd;
            }
            
            IEnumerable<Event> events = _repo.GetEvent().Where(x => x.DeletedAt == null);
            IEnumerable<EventDto> eventsDto = events.Select(x => new EventDto(x));

            if (start.HasValue && end.HasValue)
            {
                eventsDto = events.Where(x => x.EventStartAt >= start && x.EventEndAt <= end).Select(x => new EventDto(x));
            }
            else if (start.HasValue)
            {
                eventsDto = events.Where(x => x.EventStartAt >= start).Select(x => new EventDto(x));
            }
            else if (end.HasValue)
            {
                eventsDto = events.Where(x => x.EventEndAt <= end).Select(x => new EventDto(x));
            }

            return new ApiResponse<IEnumerable<EventDto>> { Success = true, Data = eventsDto };
        }

        public ApiResponse<bool> DeleteEvent(Guid id)
        {
            bool deleteResult = _repo.Delete(id);
            return new ApiResponse<bool> { Success = deleteResult, Message = deleteResult ? "Delete event success" : "Event doesnt exist" };
        }

        public ApiResponse<string> UpdateEvent(UpdateEventDto dto)
        {
            string validateInputResult = EventValidator.ValidateInput(dto);
            if (validateInputResult != "") return new ApiResponse<string> { Success = false, Message = validateInputResult };

            Event? e = _repo.Get((Guid)dto.Id);
            if (e == null) return new ApiResponse<string> { Success = false, Message = "Event doesnt exist" };
            if (e.DeletedAt != null) return new ApiResponse<string> { Success = false, Message = "Event has already deleted" };

            e = EventMapping.MappingToEntity(dto);
            e.Id = (Guid)dto.Id;

            if (!e.ValidateDate()) return new ApiResponse<string> { Success = false, Message = "Invalid date, date cannot be yesterday." };

            bool updateEventResult = _repo.Update(e);

            return new ApiResponse<string> { Success = updateEventResult, Message = updateEventResult ? "Update event success" : "Update failed." };
        }

        public ApiResponse<bool> CheckOverlapEvent(Guid id, UpdateEventDto dto)
        {
            var events = _repo.GetEvent()
                .Where(x => x.Id != id && x.DeletedAt == null);

            bool isOverlapping = events.Any(x =>
                dto.EventStartAt < x.EventEndAt &&
                dto.EventEndAt > x.EventStartAt
            );

            return new ApiResponse<bool>
            {
                Success = true,
                Message = isOverlapping ? "Overlapping event exists. Continue?" : "",
                Data = !isOverlapping
            };
        }
    }
}
