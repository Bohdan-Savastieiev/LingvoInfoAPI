namespace LanguageStudyAPI.Authentication
{
    public interface IApiAuthenticationService
    {
        /// <summary>
        /// returns a bearer token after authentication
        /// </summary>
        /// <returns></returns>

        Task<string> AuthenticateAsync();
    }
}
