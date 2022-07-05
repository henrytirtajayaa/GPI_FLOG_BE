using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using FLOG_BE.Model.Central.Entities;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace FLOG_BE.Features.Authentication.ResetPassword
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {


            var usr = _context.Persons.FirstOrDefault(x => x.EmailAddress == request.Body.Email);
            

            if (usr != null)
            {
                //update
                string newPassword = CreatePassword(8);
                usr.PersonPassword = _login.Encrypt(newPassword);
                usr.ModifiedBy = request.Initiator.UserId;
                usr.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                string FilePath = System.IO.Directory.GetCurrentDirectory()+ "/Infrastructure/TemplateEmail/ResetPassword.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[newusername]", usr.PersonFullName);
                MailText = MailText.Replace("[NewPassword]", newPassword);
                string subject = "Reset Password";
                MailMessage _mailmsg = new MailMessage();
                _mailmsg.IsBodyHtml = true;
                //_mailmsg.From = new MailAddress("logistisample38@gmail.com");
                _mailmsg.From = new MailAddress("henrytirtajaya9311.office@gmail.com");
                //_mailmsg.To.Add("dadan.suwandi83@gmail.com");
                _mailmsg.To.Add("henrytirtajaya9311.office@gmail.com");
                _mailmsg.Subject = subject;
                _mailmsg.Body = MailText;
                SmtpClient _smtp = new SmtpClient();
                _smtp.Host = "smtp.gmail.com";
                _smtp.Port = 587;
                _smtp.EnableSsl = true;
                NetworkCredential _network = new NetworkCredential("logistisample38@gmail.com", "D@dan12345");
 
                _smtp.Credentials = _network;
               
                _smtp.Send(_mailmsg);

            }
            else {
                return ApiResult<Response>.ValidationError("INCORRECT EMAIL ADDRESS FORMAT!");
            }

            return ApiResult<Response>.Ok(new Response()
            {
                Email = request.Body.Email
            });


        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

    }
}
