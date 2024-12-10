using ViaAPI.Helpers;
using ViaAPI.ViewModel;

namespace ViaAPI.Services.UserService
{
    public interface IUserService
    {
        Task<Result> RegisterAsync(RegisterUserViewModel registerUser);
        Task<Result> LoginAsync(LoginUserViewModel loginUser);
        Task<Result> ForgotPasswordAsync(string email);
        Task<Result> ResetPasswordAsync(string email, string token, string newPassword);
        Task<UserViewModel> GetCurrentUserAsync();
        Task<Result> UpdateUserDataAsync(UpdateUserViewModel updateUserViewModel);
         
    }
}
