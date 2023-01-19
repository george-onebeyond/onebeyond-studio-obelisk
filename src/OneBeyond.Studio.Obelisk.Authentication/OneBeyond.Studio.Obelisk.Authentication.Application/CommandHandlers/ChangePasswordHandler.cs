using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OneBeyond.Studio.Obelisk.Authentication.Application.Entities;
using OneBeyond.Studio.Obelisk.Authentication.Domain;
using OneBeyond.Studio.Obelisk.Authentication.Domain.Commands;

namespace OneBeyond.Studio.Obelisk.Authentication.Application.CommandHandlers;

internal sealed class ChangePasswordHandler : IRequestHandler<ChangePassword, ChangePasswordResult>
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly SignInManager<AuthUser> _signInManager;

    public ChangePasswordHandler(
        UserManager<AuthUser> userManager,
        SignInManager<AuthUser> signInManager
        )
    {
        EnsureArg.IsNotNull(userManager, nameof(userManager));
        EnsureArg.IsNotNull(signInManager, nameof(signInManager));

        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ChangePasswordResult> Handle(ChangePassword command, CancellationToken cancellationToken)
    {
        EnsureArg.IsNotNull(command, nameof(command));

        var identityUser = await _userManager.FindByIdAsync(command.LoginId!).ConfigureAwait(false);
        if (identityUser == null)
        {
            return new ChangePasswordResult(ChangePasswordStatus.UnknownUser, $"Login with id {command.LoginId} not found");
        }

        var hasPassword = await _userManager.HasPasswordAsync(identityUser).ConfigureAwait(false);
        if (!hasPassword)
        {
            return new ChangePasswordResult(ChangePasswordStatus.UserDoesNotHaveAPasswordYet, "Login has no a password yet, use SetPassword to set it");
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(identityUser, command.OldPassword, command.NewPassword).ConfigureAwait(false);

        if (!changePasswordResult.Succeeded)
        {
            return new ChangePasswordResult(ChangePasswordStatus.OperationFailure, string.Join(",", changePasswordResult.Errors.Select(x => x.Description)));
        }

        await _signInManager.SignInAsync(identityUser, isPersistent: false).ConfigureAwait(false);

        return new ChangePasswordResult(ChangePasswordStatus.Success);
    }
}
