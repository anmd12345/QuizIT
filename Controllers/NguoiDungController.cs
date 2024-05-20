using QUIZ_IT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QUIZ_IT.Models.Payments;
using QUIZ_IT.Models.SendGmail;
using GoogleAuthentication.Services;
using Newtonsoft.Json;
using QUIZ_IT.Models.Google;
using System.Threading.Tasks;
using Octokit;
using System.Net.Http;
using QUIZ_IT.Models.Github;
using QUIZ_IT.Constant;
using System.Security.Cryptography;
using System.Text;

namespace QUIZ_IT.Controllers
{
    public class NguoiDungController : Controller
    {
        private QuizITOpenConnectionDataContext db = new QuizITOpenConnectionDataContext();
        private Gmail gmail = new Gmail();
        public ActionResult Index ()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy (string username, string password, string rePassword, string email, string name)
        {
            NguoiDung user = CreateUser(username, ComputeMD5Hash(password), email, name, false, 0, 2);
            user.countLoginFail = 0;
            db.NguoiDungs.InsertOnSubmit(user);
            db.SubmitChanges();
            Session[ApplicationConstant.SESSION.SESSION_LOGIN] = user;
            return View();
        }

        public ActionResult DangNhap ()
        {
            var ClientIdGithub = ApplicationConstant.GITHUB.CLIENT_ID;
            var urlGithub = ApplicationConstant.GITHUB.REDIRECT_URL;

            var urlLoginWithGitHub = "https://github.com/login/oauth/authorize?client_id=" + ClientIdGithub + "&redirect_uri=" + urlGithub + "&scope=user:email";
            ViewBag.UrlLoginWithGithub = urlLoginWithGitHub;

            var ClientIDGoogle = ApplicationConstant.GOOGLE.CLIENT_ID;
            var urlGoogle = ApplicationConstant.GOOGLE.REDIRECT_URL;
            var urlLoginWithGoogle = GoogleAuth.GetAuthUrl(ClientIDGoogle, urlGoogle);
            ViewBag.UrlLoginWithGoogle = urlLoginWithGoogle;
            return View();
        }

        public string ComputeMD5Hash (string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public bool VerifyMD5Hash (string input, string hashToVerify)
        {
            string computedHash = ComputeMD5Hash(input);

            return string.Equals(computedHash, hashToVerify, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<ActionResult> LoginWithGoogle (string code)
        {
            try
            {
                var ClientSecret = ApplicationConstant.GOOGLE.CLIENT_SECRET;
                var ClientID = ApplicationConstant.GOOGLE.CLIENT_ID;
                var url = ApplicationConstant.GOOGLE.REDIRECT_URL;
                var token = await GoogleAuth.GetAuthAccessToken(code, ClientID, ClientSecret, url);
                var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
                var googleUser = JsonConvert.DeserializeObject<GoogleProfile>(userProfile);

                var NguoiDung = db.NguoiDungs.FirstOrDefault(n => n.Email == googleUser.Email);

                if (NguoiDung != null)
                {
                    Session[ApplicationConstant.SESSION.SESSION_LOGIN] = NguoiDung;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    NguoiDung = CreateUser(googleUser.Email, ComputeMD5Hash("*"), googleUser.Email, googleUser.Name, false, 0, 2);
                    db.NguoiDungs.InsertOnSubmit(NguoiDung);
                    db.SubmitChanges();
                    Session[ApplicationConstant.SESSION.SESSION_LOGIN] = NguoiDung;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("DangNhap", "NguoiDung");
        }

        public async Task<ActionResult> LoginWithGithub (string code)
        {
            try
            {
                var client = new HttpClient();
                var parameters = new Dictionary<string, string> {
                    { "client_id", ApplicationConstant.GITHUB.CLIENT_ID },
                    {"client_secret", ApplicationConstant.GITHUB.CLIENT_SECRET },
                    { "code", code },
                    { "redirect_uri", ApplicationConstant.GITHUB.REDIRECT_URL}
                };
                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync(ApplicationConstant.GITHUB.TOKEN_URL, content);

                var responseContent = await response.Content.ReadAsStringAsync();
                var values = HttpUtility.ParseQueryString(responseContent);
                var access_token = values["access_token"];
                var client1 = new GitHubClient(new ProductHeaderValue(ApplicationConstant.GITHUB.PROJECT));
                var tokenAuth = new Credentials(access_token);
                client1.Credentials = tokenAuth;
                var user = await client1.User.Current();
                var emailAddresses = await client1.User.Email.GetAll();

                string email = "";
                foreach (var emailAddress in emailAddresses)
                {
                    if (emailAddress != null && !string.IsNullOrEmpty(emailAddress.Email))
                    {
                        email = emailAddress.Email;
                        break;
                    }
                }
                var GitProfile = new GithubProfile
                {
                    Login = user.Login,
                    AvatarUrl = user.AvatarUrl,
                    Email = email
                };

                var NguoiDung = db.NguoiDungs.FirstOrDefault(n => n.Email == GitProfile.Email);

                if (NguoiDung != null)
                {
                    Session[ApplicationConstant.SESSION.SESSION_LOGIN] = NguoiDung;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    NguoiDung = CreateUser(GitProfile.Email, ComputeMD5Hash("*"), GitProfile.Email, GitProfile.Email, false, 0, 2); ;
                    db.NguoiDungs.InsertOnSubmit(NguoiDung);
                    db.SubmitChanges();
                    Session[ApplicationConstant.SESSION.SESSION_LOGIN] = NguoiDung;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("DangNhap", "NguoiDung");
        }

        [HttpPost]
        public ActionResult DangNhap (string username, string password)
        {
            NguoiDung nguoiDung = db.NguoiDungs.FirstOrDefault(u => u.TaiKhoan == username && u.TrangThai == false);
            if (nguoiDung != null)
            {
                if (VerifyMD5Hash(password, nguoiDung.MatKhau))
                {
                    nguoiDung.countLoginFail = 0;
                    db.SubmitChanges();
                    Session[ApplicationConstant.SESSION.SESSION_LOGIN] = nguoiDung;
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    var json = JsonConvert.SerializeObject(nguoiDung, settings);
                    return Json("OK");
                }
                else
                {
                    nguoiDung.countLoginFail++;
                    if (nguoiDung.countLoginFail == 6)
                    {
                        nguoiDung.TrangThai = true;
                    }
                    db.SubmitChanges();
                    return Json(nguoiDung.countLoginFail);
                }

            }
            return Json("NO");
        }

        public ActionResult DangXuat ()
        {
            Session.Remove("NguoiDung");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Pro ()
        {
            if (UserIsEmpty((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult NangCapCoBan ()
        {
            if (UserIsEmpty((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult ThanhToan (int upgradeType)
        {
            if (UserIsEmpty((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]))
            {
                return RedirectToAction("Index", "Home");
            }

            var listGiaoDich = db.GiaoDiches.ToList();

            string MaGiaoDich = "#" + GenerateRandomNumber();

            if (listGiaoDich.Count > 0)
            {
                MaGiaoDich += listGiaoDich.Last().Id;
            }

            var urlPayments = "";

            GiaoDich giaoDich = CreateOrder(MaGiaoDich, false, DateTime.Now, ((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]).Id);

            string vnp_Returnurl = ApplicationConstant.VNPAY.VNP_RETURN_URL;
            string vnp_Url = ApplicationConstant.VNPAY.VNP_URL;
            string vnp_TmnCode = ApplicationConstant.VNPAY.VNP_TMN_CODE;
            string vnp_HashSecret = ApplicationConstant.VNPAY.VNP_HASH_SECRET;

            VnPayLibrary vnp_Params = new VnPayLibrary();

            vnp_Params.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnp_Params.AddRequestData("vnp_Command", "pay");
            vnp_Params.AddRequestData("vnp_TmnCode", vnp_TmnCode);

            if (upgradeType == 1)
            {
                giaoDich.GiaTien = 25000;
                vnp_Params.AddRequestData("vnp_Amount", (25000 * 100).ToString());
            }
            else if (upgradeType == 2)
            {
                giaoDich.GiaTien = 100000;
                vnp_Params.AddRequestData("vnp_Amount", (100000 * 100).ToString());
            }

            vnp_Params.AddRequestData("vnp_BankCode", "VNBANK");
            vnp_Params.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnp_Params.AddRequestData("vnp_CurrCode", "VND");
            vnp_Params.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnp_Params.AddRequestData("vnp_Locale", "vn");
            vnp_Params.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + MaGiaoDich);
            vnp_Params.AddRequestData("vnp_OrderType", "other");
            vnp_Params.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnp_Params.AddRequestData("vnp_TxnRef", MaGiaoDich);
            urlPayments = vnp_Params.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            try
            {
                db.GiaoDiches.InsertOnSubmit(giaoDich);
                db.SubmitChanges();
            }
            catch (Exception e)
            {

            }


            return Redirect(urlPayments);
        }

        public ActionResult NangCapVipPro ()
        {
            if (UserIsEmpty((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult QuenMatKhau ()
        {

            return View();
        }

        [HttpPost]
        public ActionResult QuenMatKhau (string email)
        {
            string newPass = "@" + GenerateRandomNumber() + "#";
            var nguoiDung = db.NguoiDungs.FirstOrDefault(n => n.Email.Equals(email));
            if (nguoiDung != null)
            {
                nguoiDung.MatKhau = newPass;
                db.SubmitChanges();
            }
            string subject = "Trắc nghiệm IT - Lấy lại mật khẩu";
            string body = "Mật khẩu mới của bạn là: " + newPass + ". Vui lòng đăng nhập với tên đăng nhâp là: " + nguoiDung.TaiKhoan;
            gmail.SendGmail(email, subject, body);
            return View();

        }

        [HttpGet]
        public ActionResult DoiMatKhau ()
        {
            if (UserIsEmpty((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult DoiMatKhau (string currentPassword, string newPassword)
        {
            var nguoiDung = db.NguoiDungs.FirstOrDefault(n => n.TaiKhoan.Equals(((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]).TaiKhoan));
            if (nguoiDung != null)
            {
                if (!currentPassword.Equals(nguoiDung.MatKhau))
                {
                    return HttpNotFound();
                }
                else
                {
                    nguoiDung.MatKhau = newPassword;
                    db.SubmitChanges();
                }
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        public int GenerateRandomNumber ()
        {
            Random random = new Random();
            return random.Next(100000, 1000000);
        }

        public static bool UserIsEmpty (NguoiDung user)
        {
            return user == null;
        }

        public NguoiDung CreateUser (string username, string password, string email, string name, bool trangThai, int vip, int quyen)
        {
            NguoiDung user = new NguoiDung();
            user.TaiKhoan = username;
            user.MatKhau = password;
            user.TrangThai = trangThai;
            user.Email = email;
            user.HoTen = name;
            user.QuyenId = quyen;
            user.Vip = vip;
            return user;
        }

        public GiaoDich CreateOrder (string MaGiaoDich, bool TrangThai, DateTime NgayTao, int NguoiDungId)
        {
            GiaoDich giaoDich = new GiaoDich();
            giaoDich.MaGiaoDich = MaGiaoDich;
            giaoDich.TrangThai = TrangThai;
            giaoDich.NgayTao = NgayTao;
            giaoDich.NguoiDungId = NguoiDungId;
            return giaoDich;
        }
    }
}