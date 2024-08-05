namespace WebAppHero.Domain.Abstractions.Entities;

public interface IAuditable : IDateTracking, IUserTracking, ISoftDelete { }
