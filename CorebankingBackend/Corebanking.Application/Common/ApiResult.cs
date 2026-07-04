using System;
using System.Collections.Generic;
using System.Text;

namespace Corebanking.Application.Common
{
    public sealed class ApiResult<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string[] Errors { get; }

        private ApiResult(bool isSuccess, T? value, string[] errors)
        {
            IsSuccess = isSuccess;
            Value = value;
            Errors = errors;
        }

        public static ApiResult<T> Success(T value) => new(true, value, []);
        public static ApiResult<T> Failure(params string[] errors) => new(false, default, errors);
    }
}
