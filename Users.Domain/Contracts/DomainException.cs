using System.Runtime.Serialization;

namespace Users.Domain.Contracts;

[Serializable]
public abstract class DomainException<TErrorCodeEnum> : Exception where TErrorCodeEnum : Enum
{
    public abstract TErrorCodeEnum ErrorCode { get; }
    public abstract string Template { get; }
    public override string Message { get; } = string.Empty;

    protected DomainException(params object[] args)
    {
        Message = string.Format(Template, args);
    }

    protected DomainException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    {
    }

    private DomainException()
    { }
}