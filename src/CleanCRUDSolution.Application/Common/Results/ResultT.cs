namespace CleanCRUDSolution.Application.Common.Results
{
    public sealed class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, T? value, IReadOnlyList<Error> errors) : base(isSuccess, errors)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new (true, value, Array.Empty<Error>());
        public static new Result<T> Failure(params Error[] errors) => new (false, default, errors);
        public static new Result<T> Failure(IEnumerable<Error> errors) => new(false, default, errors.ToArray());
    }
}
