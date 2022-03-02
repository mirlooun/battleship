namespace Contracts.Validator
{
    public class BaseValidatorResponse
    {
        public bool IsValid { get; set; }
        
        public string Message { get; set; } = default!;
    }
}
