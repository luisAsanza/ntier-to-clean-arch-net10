namespace CleanCRUDSolution.Application.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public IReadOnlyList<Error> Errors { get; set; }

        protected Result(bool isSuccess, IReadOnlyList<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => new Result(true, Array.Empty<Error>());
        public static Result Failure(params Error[] errors) => new (false, errors);
        public static Result Failure(IEnumerable<Error> errors) => new (false, errors.ToArray());
    }
}
