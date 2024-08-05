using MediatR;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface ICommandHandler : IRequestHandler<ICommand, Result> { }

public interface ICommandHandler<T> : IRequestHandler<ICommand<T>, Result<T>> { }
