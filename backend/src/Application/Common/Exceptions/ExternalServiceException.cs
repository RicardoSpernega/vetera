namespace Application.Common.Exceptions;

public sealed class ExternalServiceException(string message) : AppException(message, 502)
{
}
