using EnsureThat;
using MediatR;

namespace OneBeyond.Studio.Obelisk.Authentication.Domain.TfaAuthentication.Commands;

public sealed class SignInTfaWithRecoveryCode : IRequest<SignInWithRecoveryCodeResult>
{
    public SignInTfaWithRecoveryCode(string recoveryCode)
    {
        EnsureArg.IsNotNullOrWhiteSpace(recoveryCode, nameof(recoveryCode));

        RecoveryCode = recoveryCode;
    }

    public string RecoveryCode { get; }
}
