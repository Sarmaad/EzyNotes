using EzyNotes.Models;

namespace EzyNotes.Features.Notes.Shared
{
    public class NoteResponse(Note note)
    {
        public string Id { get; } = note.Id;
        public string Content { get; } = note.Content;
        public DateTimeOffset CreatedOn { get; } = note.CreatedOn;
        public DateTimeOffset? UpdatedOn { get; } = note.UpdatedOn;
    }
}
