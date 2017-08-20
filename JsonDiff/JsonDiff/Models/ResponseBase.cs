namespace JsonDiff.Models
{
    /// <summary>
    /// ResponseBase is a default response model.
    /// </summary>
    public class ResponseBase
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public ResponseBase(bool success = true) : this(success, string.Empty)
        {
        }

        public ResponseBase(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}