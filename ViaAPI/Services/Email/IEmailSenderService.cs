﻿namespace ViaAPI.Services.EmailService
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
