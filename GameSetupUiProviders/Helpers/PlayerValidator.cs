using Contracts.Validator;

namespace GameSetupUiProviders.Helpers
{
    public static class PlayerValidator
    {
        public static PlayerValidatorResponse IsNameValid(string? name)
        {
            var response = new PlayerValidatorResponse();

            if (string.IsNullOrEmpty(name))
            {
                response.Message = "Name can't be empty";
            } else if (name.Length > 25)
            {
                response.Message = "Name can't be longer than 25 characters";
            }

            response.IsValid = string.IsNullOrEmpty(response.Message);
            
            return response;
        }
    }
    
    public class PlayerValidatorResponse : BaseValidatorResponse
    {
    }
}
