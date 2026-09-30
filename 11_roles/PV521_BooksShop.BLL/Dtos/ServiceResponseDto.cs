namespace PV521_BooksShop.BLL.Dtos
{
    public class ServiceResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;
        public object? Payload { get; set; }

        public static ServiceResponseDto Success(string message, object? payload = null)
        {
            return new ServiceResponseDto
            {
                Message = message,
                IsSuccess = true,
                Payload = payload
            };
        }

        public static ServiceResponseDto Error(string message, object? payload = null)
        {
            return new ServiceResponseDto
            {
                Message = message,
                Payload = payload
            };
        }
    }
}
