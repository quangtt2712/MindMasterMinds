using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MindMasterMinds_Data;
using MindMasterMinds_Data.Entities;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Service.Interfaces;
using MindMasterMinds_Utility.Exceptions;
using MindMasterMinds_Utility.Helpers;
using MindMasterMinds_Utility.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindMasterMinds_Service.Implementations
{
    public class UserService : BaseService, IUserService
    {
        private readonly MailSettings mailSettings;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<MailSettings> _mailSettings) : base(unitOfWork, mapper)
        {
            mailSettings = _mailSettings.Value;
        }

        public void SendEmail(string toEmail, string subject, string otp)
        {
            try
            {
                string pathToHtml = Path.Combine("Htmls", "EmailBody.html");
                string htmlBody = File.ReadAllText(pathToHtml);


                // Thay thế placeholder bằng giá trị thực
                htmlBody = htmlBody.Replace("{OTP}", otp);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("MindMasterMinds", mailSettings.Mail.ToString()));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(mailSettings.Host, mailSettings.Port, false);
                    client.Authenticate(mailSettings.Mail, mailSettings.Password);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch
            {
                // Log or handle the error as needed
                throw new BadRequestException($"Gửi OTP thất bại.");
            }
        }

        public async Task<ErrorResponse> SendOTPEmail(string email)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {   
                    if(!IsValid(email))
                    {
                        throw new BadRequestException($"Email không hợp lệ");
                    }

                    var userWithEmail = await _unitOfWork.User.FirstOrDefaultAsync(u => u.Email == email);

                    if (userWithEmail != null && userWithEmail.EmailConfirmed)
                    {
                        throw new ConflictException($"Email {email} đã được đăng kí tài khoản");
                    }

                    var otpEmail = GenerateOTP(6);

                    if (userWithEmail != null)
                    {
                        UpdateExistingUserOTP(userWithEmail, otpEmail);
                    }
                    else
                    {
                        CreateNewUser(email, otpEmail);
                    }

                    SendEmail(email, $"{otpEmail} là mã xác nhận tài khoản MindMasterMinds của bạn", otpEmail);

                    transaction.Commit();

                    return new ErrorResponse
                    {
                        Message = "Gửi mã OTP thành công"
                    };
                }
                catch 
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void UpdateExistingUserOTP(User existingUser, string otp)
        {
            existingUser.OTPEmailCode = otp;
            existingUser.OTPEmailExpirationDate = DateTime.UtcNow.AddMinutes(30);
            _unitOfWork.User.Update(existingUser);
            _unitOfWork.SaveChangesAsync(); 
        }

        private void CreateNewUser(string email, string otp)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                OTPEmailCode = otp,
                OTPEmailExpirationDate = DateTime.UtcNow.AddMinutes(30),
            };

            _unitOfWork.User.Add(newUser);
            _unitOfWork.SaveChangesAsync(); 
        }
        public static bool IsValid(string emailaddress)
        {
            try
            {
                string pattern = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
                return Regex.IsMatch(emailaddress, pattern);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string GenerateOTP(int length)
        {
            const string characters = "0123456789";
            Random random = new Random();
            char[] otp = new char[length];

            for (int i = 0; i < length; i++)
            {
                otp[i] = characters[random.Next(characters.Length)];
            }

            return new string(otp);
        }

        public async Task<ErrorResponse> AccountRegister(RegisterStudentRequest registerStudentRequest)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (!IsValid(registerStudentRequest.Email))
                    {
                        throw new BadRequestException($"Email không hợp lệ");
                    }

                    var userWithEmail = await _unitOfWork.User.FirstOrDefaultAsync(u => u.Email == registerStudentRequest.Email);

                    if (userWithEmail != null && userWithEmail.EmailConfirmed)
                    {
                        throw new ConflictException($"Email {registerStudentRequest.Email} đã được đăng kí tài khoản");
                    }

                    if (userWithEmail != null && !userWithEmail.EmailConfirmed)
                    {
                        if (userWithEmail.OTPEmailCode != registerStudentRequest.OTPEmailCode)
                        {
                            throw new BadRequestException($"Mã OTP không chính xác");
                        }

                        if (userWithEmail.OTPEmailExpirationDate < DateTime.UtcNow)
                        {
                            throw new BadRequestException($"Mã OTP đã hết hạn");
                        }

                        var roleStudent = await _unitOfWork.UserRole.FirstOrDefaultAsync(x => x.RoleName == MindMasterMinds_Utility.Constants.UserRole.Student);

                        if (roleStudent == null)
                        {
                            var student = new MindMasterMinds_Data.Entities.UserRole
                            {
                                Id = Guid.NewGuid(),
                                RoleName = MindMasterMinds_Utility.Constants.UserRole.Student
                            };

                            _unitOfWork.UserRole.Add(student);

                            await _unitOfWork.SaveChangesAsync();
                        }

                        var roleStudentId = await _unitOfWork.UserRole.FirstOrDefaultAsync(x => x.RoleName == MindMasterMinds_Utility.Constants.UserRole.Student);

                        var passwordSalt = GenerateOTP(6);

                        userWithEmail.EmailConfirmed = true;
                        userWithEmail.FirstName = registerStudentRequest.FirstName;
                        userWithEmail.LastName = registerStudentRequest.LastName;
                        userWithEmail.PasswordHash = PasswordHasher.HashPassword(registerStudentRequest.Password + passwordSalt);
                        userWithEmail.PasswordSalt = passwordSalt;
                        userWithEmail.CreationDate = DateTime.UtcNow;
                        userWithEmail.OTPEmailExpirationDate = null;
                        userWithEmail.OTPEmailCode = null;
                        userWithEmail.UserRoleId = roleStudentId.Id;
                        _unitOfWork.User.Update(userWithEmail);
                        await _unitOfWork.SaveChangesAsync();

                        var wallet = new Wallet
                        {
                            Id = Guid.NewGuid(),
                            UserId = userWithEmail.Id,
                            Balance = 0,
                            CreationDate = DateTime.UtcNow
                        };

                        _unitOfWork.Wallet.Add(wallet);
                        await _unitOfWork.SaveChangesAsync();

                        transaction.Commit();

                        return new ErrorResponse
                        {
                            Message = "Đăng kí tài khoản thành công"
                        };
                    }
                    throw new BadRequestException($"Email {registerStudentRequest.Email} chưa được xác nhận");
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


    }
}
