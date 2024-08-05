using MediatR;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface IQuery : IRequest<Result> { }

public interface IQuery<T> : IRequest<Result<T>> { }
