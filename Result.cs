public class Result
{
    public Result(bool isSuccess, string? error)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("Success result cannot have an error.");

        if (!isSuccess && string.IsNullOrEmpty(error))
            throw new InvalidOperationException("Failure result must have an error message.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public string? Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
    public static implicit operator Result(string error) => Failure(error);
}

public class Result<TValue> : Result
{
    private readonly TValue _value;

    public Result(TValue value, bool isSuccess, string? error) 
        : base(isSuccess, error)
    {
        if (isSuccess && value is null)
            throw new ArgumentNullException(nameof(value));

        _value = value;
    }

    public TValue Value => IsSuccess 
        ? _value 
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static Result<TValue> Success(TValue value) => new(value, true, null);
    public new static Result<TValue> Failure(string error) => new(default!, false, error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(string error) => Failure(error);
}