using QUIZ_IT.Constant;
using QUIZ_IT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QUIZ_IT.Controllers
{
    public class HomeController : Controller
    {
        QuizITOpenConnectionDataContext db = new QuizITOpenConnectionDataContext();

        public ActionResult Index ()
        {
            var dsBaiTracNghiem = db.BaiTracNghiems.Where(b => b.TrangThai == false).ToList();
            return View(dsBaiTracNghiem);
        }


        public ActionResult BatDauLamBai (int? id)
        {
            try
            {
                if(id == null)
                {
                    return Redirect("https://localhost:44318/");
                }
                if (((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]) == null)
                {
                    return RedirectToAction("DangNhap", "NguoiDung");
                }
                var soLuongCauHoi = db.CauHoiTracNghiems.Where(c => c.BaiTracNghiemId == id);
                if (soLuongCauHoi != null)
                {
                    ViewBag.SoLuongCauHoi = soLuongCauHoi.Count();
                    var dsCauHoi = db.CauHoiTracNghiems.FirstOrDefault(c => c.BaiTracNghiemId == id);
                    if (dsCauHoi != null)
                    {
                        var CauHoiVaDapAn = new Dictionary<CauHoiTracNghiem, List<DapAnTracNghiem>>();

                        CauHoiVaDapAn.Add(dsCauHoi, db.DapAnTracNghiems.Where(d => d.CauHoiTracNghiemId == dsCauHoi.Id).ToList());
                        return View(CauHoiVaDapAn);
                    }
                }

                ViewBag.KhongCoCauHoi = "Hiện bài trắc nghiệm này chưa có câu hỏi";
                return View();
            }
            catch(Exception ex)
            {
                return Redirect("https://localhost:44318/");
            }
            
        }

        public ActionResult KetThucLamBai (int id)
        {
            var soLuongCauHoi = db.CauHoiTracNghiems.Where(c => c.BaiTracNghiemId == id);
            ViewBag.SoLuongCauHoi = soLuongCauHoi.Count();
            var dsCauHoi = db.CauHoiTracNghiems.FirstOrDefault(c => c.BaiTracNghiemId == id);
            if (dsCauHoi != null)
            {
                var CauHoiVaDapAn = new Dictionary<CauHoiTracNghiem, List<DapAnTracNghiem>>();

                CauHoiVaDapAn.Add(dsCauHoi, db.DapAnTracNghiems.Where(d => d.CauHoiTracNghiemId == dsCauHoi.Id).ToList());
                return View(CauHoiVaDapAn);
            }
            ViewBag.KhongCoCauHoi = "Hiện bài trắc nghiệm này chưa có câu hỏi";
            return View();
        }

        public ActionResult HeadPartial ()
        {
            return PartialView();
        }

        public ActionResult MetaPartial ()
        {
            return PartialView();
        }

        public ActionResult LoaderPartial ()
        {
            return PartialView();
        }

        public ActionResult MenuPartial ()
        {
            return PartialView();
        }

        public ActionResult FooterPartial ()
        {
            return PartialView();
        }
    }
}