using System;

namespace TodoClient.Models
{
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool Editing { get; set; } = false;
        public bool Deleted { get; set; } = false;
        public DateTime? DateCreate { set; get; } = DateTime.UtcNow.AddHours(7);
        public DateTime? DateDue { set; get; }

    }
}