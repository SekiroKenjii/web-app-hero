using MediatR;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, RequestHandlerResult<Result>>
  where TCommand : ICommand
{ }

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, RequestHandlerResult<Result<TResponse>>>
    where TCommand : ICommand<TResponse>
{ }
