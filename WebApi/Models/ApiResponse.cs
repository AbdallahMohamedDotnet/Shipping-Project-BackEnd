namespace WebApi.Models
{
    public class ApiResponse<T>
    {
        // Default constructor
        public ApiResponse()
        {
            
        }
        // Constructor with parameters
        public ApiResponse(bool success, string message, T data)
        {
            this.Success = success;
            this.Message = message ?? string.Empty;
            this.Data = data;
        }
        // head flag for the statue response
        public bool Success { get; set; }
        // message for the response API head 
        public string Message { get; set; } = "";
        // data for the response API body
        public T? Data { get; set; }
        // Error List for the response API body
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message ?? string.Empty,
                Data = data
            };
        }

        public static ApiResponse<T> FailResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message ?? string.Empty,
                Errors = errors
            };
        }

    }
}
