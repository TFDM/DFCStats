namespace DFCStats.Domain.Exceptions
{
    public class DFCStatsException : Exception
    {
        public DFCStatsException() : base() {}

        public DFCStatsException(string message) : base(message) { }

        public DFCStatsException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}