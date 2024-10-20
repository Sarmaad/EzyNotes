using EzyNotes.Infrastructure.Auth0;
using EzyNotes.Models;

namespace EzyNotes.Services;

public interface IUserService
{
    Task<User> EnrollUser(string identityId);
}

public class UserService : IUserService
{
    readonly IRepository _repository;
    readonly ManagementClientFactory _managementClientFactory;


    public UserService(IRepository repository, ManagementClientFactory managementClientFactory)
    {
        _repository = repository;
        _managementClientFactory = managementClientFactory;

    }

    public async Task<User> EnrollUser(string identityId)
    {
        // ensure user does not exists
        var user = await _repository.Users.Find(x => x.Id, identityId, CancellationToken.None);
        if (user != null) return user;

        // get user from auth0 using management api
        var client = await _managementClientFactory.GetClient();
        var mUser = await client.Users.GetAsync(identityId);

        user = await _repository.Users.Create(new()
        {
            Id = identityId,

            Name = mUser.FullName,
            Email = mUser.Email,

            Verifications =
            {
                EmailVerified = mUser.EmailVerified ?? false,
                EmailVerifiedOn = mUser.EmailVerified.GetValueOrDefault(false) ? DateTimeOffset.UtcNow : null
            },

            CreatedOn = DateTimeOffset.UtcNow
        });

        return user;
    }
}
