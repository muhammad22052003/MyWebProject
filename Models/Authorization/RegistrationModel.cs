using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebProject.Models.Requests;

namespace WebProject.Models.auth
{
    public struct LoginModel
    {
        public LoginModel
        (
            UserLoginRequest request,
            ModelStateDictionary errorsDictionary
        )
        {
            Request = request;
            
            Errors = new Dictionary<string, string> ();

            foreach ( var item in errorsDictionary )
            {
                if (item.Value.Errors.Count != 0)
                {
                    Errors.Add(item.Key, item.Value.Errors[0].ErrorMessage);
                }
            }
        }

        public UserLoginRequest Request;

        public Dictionary<string, string> Errors { get; set; }
    }

    public struct RegistrationModel
    {
        public RegistrationModel
        (
            UserRegistrationRequest request,
            ModelStateDictionary errorsDictionary
        )
        {
            Request = request;

            Errors = new Dictionary<string, string>();

            foreach (var item in errorsDictionary)
            {
                if (item.Value.Errors.Count != 0)
                {
                    Errors.Add(item.Key, item.Value.Errors[0].ErrorMessage);
                }
            }
        }

        public UserRegistrationRequest Request;

        public Dictionary<string, string> Errors { get; set; }
    }
}
