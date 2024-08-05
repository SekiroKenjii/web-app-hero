namespace WebAppHero.Contract.Abstractions.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<T> Success<T>(T data) => new(data, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Failure<T>(Error error) => new(default, false, error);

    public static Result<T> Create<T>(T data)
    {
        return data is not null
            ? Success(data)
            : Failure<T>(Error.NullValue);
    }
}

public class Result<T> : Result
{
    private readonly T? _data;

    protected internal Result(T? data, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _data = data;
    }

    public T? Data => IsSuccess
        ? _data
        : throw new InvalidOperationException("The data of a failure result can not be accessed.");

    public static implicit operator Result<T>(T value) => Create(value);
}
