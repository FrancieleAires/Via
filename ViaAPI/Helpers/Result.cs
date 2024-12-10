namespace ViaAPI.Helpers
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string[] Errors { get; private set; }
        public string Value { get; private set; }
        public string Token { get; private set; }
        public static Result Success(string value, string token = null)
        {
            return new Result { IsSuccess = true, Value = value, Token = token };
        }

        public static Result Failure(string error)
        {
            return new Result { IsSuccess = false, Errors = new[] { error } };
        }
    }
}
