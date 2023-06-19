using Newtonsoft.Json;

namespace Turnit.GenericStore.Data
{
    public class ExceptionDetails
    {
        public string? ErrorCode { get; set; }

        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public string? StackTrace { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
