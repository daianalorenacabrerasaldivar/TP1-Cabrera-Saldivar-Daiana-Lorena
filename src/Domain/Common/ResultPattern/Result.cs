namespace Domain.Common.ResultPattern
{
    public abstract class Result<TResult>
    {
        public abstract TResult Value { get; }

        protected abstract DataStatus DataStatus { get; }

        public bool HasValue => DataStatus == DataStatus.Ok && Value != null;

        public bool IsFailed => DataStatus == DataStatus.Failed;

        public abstract string Info { get; }

        public static implicit operator TResult(Result<TResult> @this)
        {
            return @this.Value;
        }

    }
    public enum DataStatus
    {
        Failed,
        Ok
    }
}
