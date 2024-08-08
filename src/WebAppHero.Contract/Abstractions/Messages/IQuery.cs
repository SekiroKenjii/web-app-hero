using MediatR;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface IQuery<TResponse> : IRequest<RequestHandlerResult<Result<TResponse>>>
{ }
