using MediatR;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, RequestHandlerResult<Result<TResponse>>>
    where TQuery : IQuery<TResponse>
{ }
