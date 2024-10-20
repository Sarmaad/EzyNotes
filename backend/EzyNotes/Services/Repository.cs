using EzyNotes.Infrastructure.Extensions;
using EzyNotes.Infrastructure.Mongodb;
using EzyNotes.Models;
using MongoDB.Driver;

namespace EzyNotes.Services;

public interface IRepository
{
    ModelSet<Note> Notes { get; }
    ModelSet<User> Users { get; }


    TModel? Get<TModel>(string id, bool forTenant = false) where TModel : class, new();
    string? GetLoggedInUserId();
    UserDto? GetLoggedInUserDto();
}

public class Repository : IRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Repository(IHttpContextAccessor httpContextAccessor, IMongoClient client, DatabaseOptions dbOptions)
    {
        _httpContextAccessor = httpContextAccessor;

        var database = client.GetDatabase(dbOptions.DatabaseName);

        Notes = new(database, "notes");
        Users = new(database, "users");
    }


    public ModelSet<Note> Notes { get; }
    public ModelSet<User> Users { get; }


    public TModel? Get<TModel>(string id, bool forTenant = false) where TModel : class, new()
    {
        if (forTenant)
        {
            var userId = GetLoggedInUserId();
            if (string.IsNullOrWhiteSpace(userId)) throw new InvalidOperationException($"User is not logged in!");

            return new TModel() switch
            {
                Note => Notes.Collection.Find(Notes.Filter.Eq(x => x.Id, id) & Notes.Filter.Eq(x => x.UserId, userId)).SingleOrDefault() as TModel,

                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return new TModel() switch
        {
            User => Users.Collection.Find(Users.Filter.Eq(x => x.Id, id)).SingleOrDefault() as TModel,

            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string? GetLoggedInUserId()
    {
        return _httpContextAccessor.GetUserId();
    }

    public UserDto? GetLoggedInUserDto()
    {
        var userId = _httpContextAccessor.GetUserId();
        if (string.IsNullOrWhiteSpace(userId)) return null;

        var user = Get<User>(userId);
        if (user is null) return null;

        return new(user.Id, user.Name);
    }
}
