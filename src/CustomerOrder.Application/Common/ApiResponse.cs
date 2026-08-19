using System.Collections.Generic;

namespace CustomerOrder.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful")
        {
            return new ApiResponse<T> { Success = true, Data = data, Message = message };
        }
        public static ApiResponse<T> ErrorResponse(string message, List<string> errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }

    public static class ApiResponse
    {
        public static ApiResponse<object> SuccessResponse(string message)
        {
            return new ApiResponse<object> { Success = true, Message = message };
        }

        public static ApiResponse<object> ErrorResponse(string message, List<string> errors = null)
        {
            return ApiResponse<object>.ErrorResponse(message, errors);
        }
    }
}
