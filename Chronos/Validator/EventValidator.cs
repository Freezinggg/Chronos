using Chronos.Domain;
using Chronos.DTO.Event;

namespace Chronos.Validator
{
    public static class EventValidator
    {
        public static string ValidateInput(EventBaseDto dto)
        {
            if (string.IsNullOrEmpty(dto.EventName)) return "Event Name cannot be empty.";
            if (dto.EventStartAt >= dto.EventEndAt) return "Invalid date, date start greater than date end.";

            return "";
        }


       
    }
}
