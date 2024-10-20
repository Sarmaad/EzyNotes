using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EzyNotes.Models;

public class User
{
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public UserVerifications Verifications { get; set; } = new();
}

public class UserVerifications
{
    public bool EmailVerified { get; set; }
    public DateTimeOffset? EmailVerifiedOn { get; set; }
}