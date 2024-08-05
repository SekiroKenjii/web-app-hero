using MediatR;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface IQueryHandler : IRequestHandler<IQuery, Result> { }

public interface IQueryHandler<T> : IRequestHandler<IQuery<T>, Result<T>> { }
