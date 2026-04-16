using System.Net;

namespace AnroveBPO.Web.EndpointResults;

public class SuccessResult : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var envelope = Envelope.Ok();

        httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

        return httpContext.Response.WriteAsJsonAsync(envelope);
    }
}

public class SuccessResult<TValue>(TValue value) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var envelope = Envelope.Ok(value);

        httpContext.Response.StatusCode = StatusCodes.Status200OK;

        return httpContext.Response.WriteAsJsonAsync(envelope);
    }
}