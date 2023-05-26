using MediatR;

namespace OneBeyond.Studio.Obelisk.Domain.Features.Examples.Commands;

/// <summary>
/// Bulk Insert todos
/// </summary>
public sealed record BulkInsert(int Count): IRequest
{
}
