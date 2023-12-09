using QUIZ_IT.Constant;
using QUIZ_IT.Models;
using QUIZ_IT.Models.Payments;
using QUIZ_IT.Models.SendGmail;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QUIZ_IT.Controllers
{
    public class PaymentsController : Controller
    {
        QuizITOpenConnectionDataContext db = new QuizITOpenConnectionDataContext();

        public static bool UserIsEmpty (NguoiDung user)
        {
            return user == null;
        }

        Gmail Gmail = new Gmail();

        public ActionResult Payments ()
        {


            if (UserIsEmpty((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Request.QueryString.Count > 0)
                {
                    string vnp_HashSecret = ApplicationConstant.VNPAY.VNP_HASH_SECRET;
                    var vnpayData = Request.QueryString;
                    VnPayLibrary vnpay = new VnPayLibrary();

                    foreach (string s in vnpayData)
                    {
                        if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                        {
                            vnpay.AddResponseData(s, vnpayData[s]);
                        }
                    }

                    string MaGiaoDich = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                    long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                    string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                    string vnp_BankCode = vnpay.GetResponseData("vnp_BankCode");
                    string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                    String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                    String TerminalID = Request.QueryString["vnp_TmnCode"];
                    long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                    String bankCode = Request.QueryString["vnp_BankCode"];
                    GiaoDich giaoDich = db.GiaoDiches.FirstOrDefault(g => g.MaGiaoDich == MaGiaoDich);


                    bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                    if (checkSignature)
                    {
                        if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                        {
                            var nguoiDung = db.NguoiDungs.FirstOrDefault(n => n.Id == ((NguoiDung)Session["NguoiDung"]).Id);
                            string toEmail = ((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]).Email;
                            string subject = "Trắc nghiệm IT - Nâng cấp tài khoản";
                            string body = "Chúc mừng bạn đã nâng cấp thành công tài khoản. Và chúng tôi xin chân thành cảm ơn bạn rất nhiều!\n" + "<!DOCTYPE html><html><body><h2>Chi tiết giao dịch</h2><table style=\"background-color: grey; color: white; font-weight: bold\" class=\"table table-bordered table - dark\"><thead> <tr><th scope = \"col\"> Mã Giao Dịch</th><th scope = \"col\"> Số Tiền Giao Dịch </th> <th scope = \"col\"> Thời Gian Giao Dịch </th><th scope = \"col\"> Nội Dung Giao Dịch </th><th scope = \"col\"> Trạng Thái </th></tr></thead><tbody ><tr><th scope = \"row\">" + giaoDich.MaGiaoDich + "</th><td>" + giaoDich.GiaTien + " vnđ</td><td>" + giaoDich.NgayTao + "</td><td>Nâng cấp tài khoản</td><td>Giao dịch thành công</td ></tr> </tbody></table></body></html>";
                            giaoDich.TrangThai = true;
                            if (giaoDich.GiaTien == 25000)
                            {
                                nguoiDung.Vip = 1;
                            }
                            else if (giaoDich.GiaTien == 100000)
                            {
                                nguoiDung.Vip = 2;
                            }

                            Gmail.SendGmail(toEmail, subject, body);
                            Session[ApplicationConstant.SESSION.SESSION_LOGIN] = nguoiDung;
                            ViewBag.NganHang = vnp_BankCode;
                            db.SubmitChanges();
                        }
                        else
                        {

                            ViewBag.InnerText = "Đã hủy thanh toán thành công!";
                            return View(giaoDich);
                        }
                    }
                    return View(giaoDich);
                }
                return null;
            }
        }
    }
}