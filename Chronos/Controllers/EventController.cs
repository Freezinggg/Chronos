using Chronos.Application;
using Chronos.DTO.Event;
using Microsoft.AspNetCore.Mvc;

namespace Chronos.Controllers
{
    [Route("/Event")]
    public class EventController : Controller
    {
        EventServices _eventServices { get; set; }
        public EventController(EventServices eventServices)
        {
            _eventServices = eventServices;
        }

        public IActionResult Index()
        {
            //return View("~/Views/Event/_CreateEventForm.cshtml");
            return View();
        }

        [HttpGet("CreatePartial")]
        public IActionResult CreatePartial()
        {
            var dto = new EventDto();
            return PartialView("_EventForm", dto);
        }

        [HttpGet("EditPartial/{id}")]
        public IActionResult EditPartial(Guid id)
        {
            var dto = _eventServices.Get(id); // fetch data
            return PartialView("_EventForm", dto.Data);
        }


        [HttpPost("CreateEvent")]
        public IActionResult CreateEvent([FromBody] CreateEventDto dto)
        {
            var result = _eventServices.CreateEvent(dto);
            return Json(result);
        }

        [HttpPost("CheckOverlapEvent")]
        public IActionResult CheckOverlapEvent(Guid id, [FromBody] UpdateEventDto dto)
        {
            var result = _eventServices.CheckOverlapEvent(id, dto);
            return Json(result);
        }

        [HttpGet("GetEvents")]
        public IActionResult GetEvents(string? startDate, string? endDate)
        {
            var result = _eventServices.GetEvents(startDate, endDate);
            return Json(result);
        }

        [HttpPost("DeleteEvent/{id}")]
        public IActionResult DeleteEvent(Guid id)
        {
            var result = _eventServices.DeleteEvent(id);
            return Json(result);
        }

        [HttpPut("UpdateEvent")]
        public IActionResult UpdateEvent([FromBody] UpdateEventDto dto)
        {
            var result = _eventServices.UpdateEvent(dto);
            return Json(result);
        }
    }
}
