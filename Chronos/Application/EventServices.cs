using Chronos.Common;
using Chronos.Domain;
using Chronos.DTO.Event;
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

        public ApiResponse<string> CreateEvent(CreateEventDto dto)
        {
            try
            {
                //Check for inputs
                string validateInputResult = ValidateInput(dto);
                if (validateInputResult != "") return new ApiResponse<string> { Success = false, Message = validateInputResult };

                //Mapping
                Event e = Mapping(dto);

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

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                try
                {
                    DateTime start = Convert.ToDateTime(startDate);
                    DateTime end = Convert.ToDateTime(endDate);

                    IEnumerable<Event> events = _repo.GetEvent();
                    IEnumerable<EventDto> eventsDto = events.Where(x => x.DeletedAt == null && x.EventStartAt >= start && x.EventEndAt <= end).Select(x => new EventDto
                    {
                        EventName = x.EventName,
                        EventDescription = x.EventDescription,
                        EventEndAt = x.EventEndAt,
                        EventStartAt = x.EventStartAt,
                        EventNotes = x.EventNotes,
                        Location = x.Location,
                    });

                    return new ApiResponse<IEnumerable<EventDto>> { Success = true, Data = eventsDto };
                }
                catch
                {
                    return new ApiResponse<IEnumerable<EventDto>> { Success = false, Message = "Invalid date format. Please use (yyyy-MM-dd)" };
                }
            }
            else if (!string.IsNullOrEmpty(startDate))
            {
                try
                {
                    DateTime start = Convert.ToDateTime(startDate);

                    IEnumerable<Event> events = _repo.GetEvent();
                    IEnumerable<EventDto> eventsDto = events.Where(x => x.DeletedAt == null && x.EventStartAt >= start).Select(x => new EventDto
                    {
                        EventName = x.EventName,
                        EventDescription = x.EventDescription,
                        EventEndAt = x.EventEndAt,
                        EventStartAt = x.EventStartAt,
                        EventNotes = x.EventNotes,
                        Location = x.Location,
                    });

                    return new ApiResponse<IEnumerable<EventDto>> { Success = true, Data = eventsDto };
                }
                catch
                {
                    return new ApiResponse<IEnumerable<EventDto>> { Success = false, Message = "Invalid date format. Please use (yyyy-MM-dd)" };
                }
            }
            else if (!string.IsNullOrEmpty(endDate))
            {

                try
                {
                    DateTime end = Convert.ToDateTime(endDate);

                    IEnumerable<Event> events = _repo.GetEvent();
                    IEnumerable<EventDto> eventsDto = events.Where(x => x.DeletedAt == null && x.EventEndAt <= end).Select(x => new EventDto
                    {
                        EventName = x.EventName,
                        EventDescription = x.EventDescription,
                        EventEndAt = x.EventEndAt,
                        EventStartAt = x.EventStartAt,
                        EventNotes = x.EventNotes,
                        Location = x.Location,
                    });

                    return new ApiResponse<IEnumerable<EventDto>> { Success = true, Data = eventsDto };
                }
                catch
                {
                    return new ApiResponse<IEnumerable<EventDto>> { Success = false, Message = "Invalid date format. Please use (yyyy-MM-dd)" };
                }
            }
            else
            {
                IEnumerable<Event> events = _repo.GetEvent();
                IEnumerable<EventDto> eventsDto = events.Where(x => x.DeletedAt == null).Select(x => new EventDto
                {
                    EventName = x.EventName,
                    EventDescription = x.EventDescription,
                    EventEndAt = x.EventEndAt,
                    EventStartAt = x.EventStartAt,
                    EventNotes = x.EventNotes,
                    Location = x.Location,
                });

                return new ApiResponse<IEnumerable<EventDto>> { Success = true, Data = eventsDto };
            }
        }

        public ApiResponse<bool> DeleteEvent(Guid id)
        {
            bool deleteResult = _repo.Delete(id);
            return new ApiResponse<bool> { Success = deleteResult, Message = deleteResult ? "Delete event success" : "Event doesnt exist" };
        }

        public ApiResponse<string> UpdateEvent(Guid id, UpdateEventDto dto)
        {
            string validateInputResult = ValidateInput(dto);
            if (validateInputResult != "") return new ApiResponse<string> { Success = false, Message = validateInputResult };

            Event? e = _repo.Get(id);
            if (e == null) return new ApiResponse<string> { Success = false, Message = "Event doesnt exist" };
            if (e.DeletedAt != null) return new ApiResponse<string> { Success = false, Message = "Event has already deleted" };


            e.EventName = dto.EventName;
            e.EventDescription = dto.EventDescription;
            e.EventNotes = dto.EventNotes;
            e.EventStartAt = dto.EventStartAt;
            e.EventEndAt = dto.EventEndAt;
            e.Location = dto.Location;
            e.HasReminder = dto.HasReminder;

            if (!e.ValidateDate()) return new ApiResponse<string> { Success = false, Message = "Invalid date, date cannot be yesterday." };

            bool updateEventResult = _repo.Update(e);

            return new ApiResponse<string> { Success = updateEventResult, Message = updateEventResult ? "Update event success" : "Update failed." };
        }

        public ApiResponse<bool> CheckOverlapEvent(Guid id, UpdateEventDto dto)
        {
            IEnumerable<Event> events = _repo.GetEvent();
            if (events.Where(x => x.Id != id
             && x.EventStartAt < dto.EventStartAt && x.EventEndAt > dto.EventStartAt).Any())
                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Overlapping event exist. Continue?",
                    Data = true
                };

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "",
                Data = false
            };
        }

        /**/
        public string ValidateInput(EventBaseDto dto)
        {
            if (string.IsNullOrEmpty(dto.EventName)) return "Event Name cannot be empty.";
            if (dto.EventStartAt >= dto.EventEndAt) return "Invalid date, date start greater than date end.";

            return "";
        }

        public Event Mapping(CreateEventDto dto)
        {
            Event e = new()
            {
                EventName = dto.EventName,
                EventStartAt = dto.EventStartAt,
                EventEndAt = dto.EventEndAt,
                EventDescription = dto.EventDescription,
                EventNotes = dto.EventNotes,
                Location = dto.Location,
                HasReminder = dto.HasReminder,
                CreatedAt = DateTime.Now,
            };
            return e;
        }
    }
}
