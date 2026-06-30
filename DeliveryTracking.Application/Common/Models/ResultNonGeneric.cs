using System;
using System.Collections.Generic;

namespace DeliveryTracking.Application.Common.Models
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();

        public static Result SuccessResult(string message = "")
        {
            return new Result
            {
                Success = true,
                Message = message,
                Errors = new()
            };
        }

        public static Result FailureResult(string message, List<string>? errors = null)
        {
            return new Result
            {
                Success = false,
                Message = message,
                Errors = errors ?? new()
            };
        }
    }
}
