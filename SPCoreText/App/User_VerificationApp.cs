using Microsoft.Extensions.Configuration;
using SPCoreText.Common;
using SPCoreText.Interface;
using SPCoreText.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.App
{
    public class User_VerificationApp
    {
        private IAuth _auth;
        private IUnitWork _unitWork;
        public IRepository<User> _userapp;
        public IRepository<User_Verification> _userVerificationapp;
        public IConfiguration Configuration { get; }

        public User_VerificationApp(IAuth auth, IUnitWork unitWork, IRepository<User> userApp, IRepository<User_Verification> userVerificationapp, IConfiguration configuration)
        {
            _auth = auth;
            _unitWork = unitWork;
            _userapp = userApp;
            _userVerificationapp = userVerificationapp;
            Configuration = configuration;
        }

        //邮件发送验证码
        public Response SentVerificationCode(string useremail, string user)
        {
            var resp = new Response();
            //生成验证码
            string code = GenerateVerifyCode(user, "Email");
            //发生邮件给用户
            string emailtitle = "Verification Code:" + code.ToString();
            string emailcontent = "<span><h3>Verify your login:</h3><br/>Below is your one time passcode:<br/><h4>" + code.ToString() + "</h4><br/> it expires in 30 minutes <br/> <h4>-HKO IT Team</h4></span>";

            string emailAccount = Configuration["emailAccount"];
            string emailPassword = Configuration["emailPassword"];
            string smtpHost = Configuration["smtpHost"];
            int? smtpPort = Convert.ToInt32(Configuration["smtpPort"]);
            bool enableSsl = Configuration["enableSsl"] == "true" ? true : false;
            EmailHelper emailHelper = new EmailHelper(emailAccount, emailPassword, smtpHost, smtpPort, enableSsl);

            bool sentresult = emailHelper.SendMail(useremail, null, emailtitle, emailcontent, null);
            if (sentresult)
            {
                resp.Code = 200;
                resp.Message = "The verification code has been sent";
            }
            else
            {
                resp.Code = 1;
                resp.Message = "The email sending failed!";
            }
            return resp;
        }
        //验证 验证码 是否正确
        public Response CheckVerificationCode(string username, string verificationCode, string useremail)
        {
            var resp = new Response();
            var userlist = _userapp.FindSingle(c => c.Account == username);
            if (userlist != null)
            {
                string userid = userlist.Id;
                var validatelist = _userVerificationapp.FindSingleLastOrDefault(c => c.UserId == userid);
                if (validatelist == null)
                {
                    resp.Code = 2;
                    resp.Message = "The verification code does not exist!";
                    return resp;
                }
                else
                {
                    if (validatelist.VerificationCode != verificationCode)
                    {
                        resp.Code = 2;
                        resp.Message = "The verification code has expired!";
                        return resp;
                    }
                }
                //判斷是否是最後一個驗證碼
                DateTime time_validate = validatelist.VerificationTime;
                TimeSpan tlast = new TimeSpan(time_validate.Ticks);
                TimeSpan tnow = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = tlast.Subtract(tnow).Duration();
                var Minutes = ts.TotalMinutes;//两时间相差的分钟数
                                              //if (validatelist.Reserve1 == "SMS")
                                              //{
                                              //    if (Minutes > 5)
                                              //    {
                                              //        resp.Code = 3;
                                              //        resp.Message = "The verification code has expired!";
                                              //        return resp;
                                              //    }
                                              //}
                                              //else
                                              //{
                if (Minutes > 30)
                {
                    resp.Code = 3;
                    resp.Message = "The verification code has expired!";
                    return resp;
                }
                //}
            }
            else
            {
                resp.Code = 1;
                resp.Message = "User does not exist!";
                return resp;
            }
            return resp;
        }

        //生成验证码并保存至数据
        public string GenerateVerifyCode(string user, string senttype)
        {
            Random random = new Random();
            int code = random.Next(100000, 999999);
            if (senttype.Contains("SMS"))
            {
                //如果是SMS就用郵箱的那個驗證碼
                var userVerify = _userVerificationapp.FindSingleLastOrDefault(u => u.UserCode == user && u.Reserve1 == "Email");
                code = Convert.ToInt32(userVerify.VerificationCode);
            }
            var userInfo = _userapp.FindSingle(u => u.Account == user);
            if (userInfo != null)
            {
                //更新到数据库字段
                User_Verification verificatemodel = new User_Verification();
                verificatemodel.VerificationCode = code.ToString();
                verificatemodel.UserCode = userInfo.Account;
                verificatemodel.UserId = userInfo.Id;
                verificatemodel.VerificationTime = DateTime.Now;
                verificatemodel.Reserve1 = senttype;
                _unitWork.Add<User_Verification>(verificatemodel);
                _unitWork.Save();
            }
            return code.ToString();
        }

    }
}
