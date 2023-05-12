namespace BookStore.API.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public object Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(bool success, object message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
