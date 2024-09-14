﻿namespace ContactManager.Models;

public class Result<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string Error { get; set; }

    public static Result<T> Ok(T data)
    {
        return new Result<T> { Success = true, Data = data };
    }

    public static Result<T> Fail(string error)
    {
        return new Result<T> { Success = false, Error = error };
    }
}

public class Result
{
    public bool Success { get; set; }
    public string Error { get; set; }

    public static Result Ok()
    {
        return new Result { Success = true };
    }

    public static Result Fail(string error)
    {
        return new Result { Success = false, Error = error };
    }
}