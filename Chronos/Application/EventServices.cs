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
                if(!e.ValidateDate()) return new ApiResponse<string> { Success = false, Message = "Invalid date, date cannot be yesterday." };

                //Save to DB
                Event? result = _repo.Create(e);
                if(result == null) return new ApiResponse<string> { Success = false, Message = "Save failed. Please try again." };

                return new ApiResponse<string> { Success = true, Message = "Create Event success." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Unexpected error occured on the server, please try again and make sure all data are correct." };
            }
        }

        public string ValidateInput(CreateEventDto dto)
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
