using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EzyNotes.Models
{
    public class Note : ITenantEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
