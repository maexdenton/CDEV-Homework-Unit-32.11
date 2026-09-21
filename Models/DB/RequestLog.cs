namespace BlogPlatform.Models.DB
{
    public class RequestLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Environment { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public long ElapsedMs { get; set; }
        public string ClientIp { get; set; } = string.Empty;
    }
}
