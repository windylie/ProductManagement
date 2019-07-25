namespace ProductManagementApi.OutputDto
{
    public class OperationResponse
    {
        private OperationResponse(bool isSuccessful, string[] messages, object data)
        {
            IsSuccessful = isSuccessful;
            Messages = messages;
            Data = data;
        }

        public bool IsSuccessful { get; private set; }
        public string[] Messages { get; private set; }
        public object Data { get; private set; }

        public static OperationResponse Succeed(object data = null)
        {
            return new OperationResponse(true, null, data);
        }

        public static OperationResponse Fail(params string[] messages)
        {
            return new OperationResponse(false, messages, null);
        }
    }
}
