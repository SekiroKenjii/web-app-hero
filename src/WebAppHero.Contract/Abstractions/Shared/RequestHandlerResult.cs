namespace WebAppHero.Contract.Abstractions.Shared;

public class RequestHandlerResult<TResult> where TResult : Result
{
    public required TResult Result { get; set; }

    public int HttpStatusCode { get; set; }

    public static RequestHandlerResult<TResult> Create(TResult result, int httpStatusCode)
    {
        return new RequestHandlerResult<TResult> {
            Result = result,
            HttpStatusCode = httpStatusCode
        };
    }
}
