using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using ViaAPI.Helpers;
using ViaAPI.Models;
using ViaAPI.Services.EmailService;
using ViaAPI.Services.JwtService;
using ViaAPI.ViewModel;

namespace ViaAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IEmailSenderService emailSenderService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _emailSenderService = emailSenderService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Result> LoginAsync(LoginUserViewModel loginUser)

        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);
            if (user == null)
                return Result.Failure("Tentativa de login inválida.");

            var result = await _signInManager.PasswordSignInAsync(user, loginUser.Senha, false, false);

            if (!result.Succeeded)
                return Result.Failure("Tentativa de login inválida.");

            var token = _jwtService.GerarJwt(user);
            return Result.Success(token);
        }

        public async Task<Result> RegisterAsync(RegisterUserViewModel registerUser)
        {
            if (!IsValidCpf(registerUser.CPF))
            {
                return Result.Failure("CPF inválido.");
            }
            if (!IsValidEmail(registerUser.Email))
            {
                return Result.Failure("E-mail inválido.");
            }
            var existingUser = await _userManager.FindByEmailAsync(registerUser.Email);
            if (existingUser != null)
            {
                return Result.Failure("E-mail já está em uso.");
            }
            var user = new ApplicationUser
            {
                Nome = registerUser.Nome,
                DataNascimento = registerUser.DataNascimento,
                CPF = registerUser.CPF,
                PhoneNumber = registerUser.PhoneNumber,
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Senha);

            if (result.Succeeded)
            {
                return Result.Success("Usuário registrado com sucesso");
            }

            return Result.Failure("Usuário não registrado!");
        }
        private bool IsValidCpf (string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || !long.TryParse(cpf, out _))
                return false;

            int[] multiplicador1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito1 = resto.ToString();
            tempCpf += digito1;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito2 = resto.ToString();

            return cpf.EndsWith(digito1 + digito2);
        }
        public bool IsValidEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
        return false;
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
        public async Task<Result> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure("Usuário não encontrado.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = HttpUtility.UrlEncode(token);

            var resetLink = $"https://seu-app.com/reset-password?email={HttpUtility.UrlEncode(email)}&token={encodedToken}";



            await _emailSenderService.SendEmailAsync(user.Email, "Redefinir Senha", $"Clique <a href='{resetLink}'>aqui</a> para redefinir sua senha.");

            return Result.Success("Link de redefinição de senha enviado com sucesso.");
        }
        public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result.Failure("Usuário não encontrado.");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
                return Result.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

            return Result.Success("Senha redefinida com sucesso.");
        }

        public async Task<Result> UpdateUserDataAsync(UpdateUserViewModel updateUserViewModel)
        {
            var userEmail = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))!.Value;


            if (string.IsNullOrEmpty(userEmail)) return Result.Failure("Usuário não encontrado.");

            var userLogado = await _userManager.FindByEmailAsync(userEmail);

            if (userLogado == null)
                return Result.Failure("Usuário não encontrado.");

            userLogado.Nome = updateUserViewModel.Nome;
            userLogado.Email = updateUserViewModel.Email;
            userLogado.PhoneNumber = updateUserViewModel.PhoneNumber;
            userLogado.DataNascimento = updateUserViewModel.DataNascimento;

            var result = await _userManager.UpdateAsync(userLogado);

            if (result.Succeeded)
            {
                var token = _jwtService.GerarJwt(userLogado);
                return Result.Success("Dados atualizados com sucesso.", token);
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(errors);
        }

        public async Task<UserViewModel> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Claims
        .FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var userLogado = await _userManager.FindByIdAsync(userId);

            if (userLogado == null)
            {
                return null;
            }
            var userViewModel = new UserViewModel
            {
                Nome = userLogado.Nome,
                Email = userLogado.Email,
                PhoneNumber = userLogado.PhoneNumber,
                DataNascimento = userLogado.DataNascimento,
                CPF = userLogado.CPF
            };

            return userViewModel;
        }
    }
}


