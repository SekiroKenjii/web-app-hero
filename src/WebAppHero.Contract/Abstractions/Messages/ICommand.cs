using MediatR;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface ICommand : IRequest<Result> { }

public interface ICommand<T> : IRequest<Result<T>> { }
