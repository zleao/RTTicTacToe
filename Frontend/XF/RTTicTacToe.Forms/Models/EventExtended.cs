using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RTTicTacToe.Forms.Models
{
    public class EventExtended
    {
        public Event Event { get; }

        public string EventTypeDisplay { get; }
        public string EventJsonFormatted { get; }

        public EventExtended(Event @event)
        {
            Event = @event;
            EventTypeDisplay = Event.EventType.Substring(Event.EventType.LastIndexOf('.') + 1);
            EventJsonFormatted = JToken.Parse(Event.SerializedEvent).ToString(Formatting.Indented);
        }
    }
}
