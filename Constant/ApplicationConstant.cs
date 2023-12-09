using System.Configuration;

namespace QUIZ_IT.Constant
{
    public class ApplicationConstant
    {
        //Google developer info
        public class GOOGLE
        {
            public static string CLIENT_ID = ConfigurationManager.AppSettings["GoogleClientId"].ToString();
            public static string CLIENT_SECRET = ConfigurationManager.AppSettings["GoogleClientSecret"].ToString();
            public static string REDIRECT_URL = ConfigurationManager.AppSettings["GoogleRedirectUri"].ToString();
        }

        //Github develop info
        public class GITHUB
        {
            public static string CLIENT_ID = ConfigurationManager.AppSettings["GithubClientId"].ToString();
            public static string CLIENT_SECRET = ConfigurationManager.AppSettings["GithubClientSecret"].ToString();
            public static string REDIRECT_URL = ConfigurationManager.AppSettings["GithubRedirectUri"].ToString();
            public static string TOKEN_URL = ConfigurationManager.AppSettings["GithubToken"].ToString();
            public static string PROJECT = ConfigurationManager.AppSettings["GithubProject"].ToString();
        }

        public static class VNPAY
        {
            public static string VNP_RETURN_URL = ConfigurationManager.AppSettings["vnp_Returnurl"].ToString();
            public static string VNP_URL = ConfigurationManager.AppSettings["vnp_Url"].ToString();
            public static string VNP_TMN_CODE = ConfigurationManager.AppSettings["vnp_TmnCode"].ToString();
            public static string VNP_HASH_SECRET = ConfigurationManager.AppSettings["vnp_HashSecret"].ToString();
            public static string VNP_API = ConfigurationManager.AppSettings["vnp_Api"].ToString();
        }

        //Session key info
        public static class SESSION
        {
            public static string SESSION_LOGIN = "NguoiDung";
        }
    }
}