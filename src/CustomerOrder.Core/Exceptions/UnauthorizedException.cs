namespace CustomerOrder.Core.Exceptions
{
    public sealed class UnauthorizedException : DomainException
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
