namespace STEPIFY.DTOs.ResultDTO
{
    public class ResultDTO<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

    }
}
