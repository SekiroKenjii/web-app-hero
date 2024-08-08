using MediatR;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface ICommand : IRequest<RequestHandlerResult<Result>> { }

public interface ICommand<T> : IRequest<RequestHandlerResult<Result<T>>> { }
