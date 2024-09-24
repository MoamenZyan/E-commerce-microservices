namespace UserService.Application.Common.Responses
{
    public class Result<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string[]? Errors { get; set; }
    }
}
