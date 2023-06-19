namespace Turnit.GenericStore.Data
{
    public class ExceptionBase : Exception
    {
        public string? ErrorCode { get; }

        public ExceptionBase() { }

        public ExceptionBase(string message) : base(message) { }

        public ExceptionBase(string message, Exception innerException) : base(message, innerException) { }

        public ExceptionBase(string message, Exception innerException, string errorCode) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
