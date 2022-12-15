using EnsureThat;
using MediatR;

namespace OneBeyond.Studio.Obelisk.Authentication.Domain.JwtAuthentication.Commands;

public sealed class SignInJwtToken : IRequest<JwtToken>
{
    public SignInJwtToken(
        string username,
        string password
        )
    {
        EnsureArg.IsNotNull(username, nameof(username));
        EnsureArg.IsNotNull(password, nameof(password));

        Username = username;
        Password = password;
    }

    public string Username { get; private set; }
    public string Password { get; private set; }
}
