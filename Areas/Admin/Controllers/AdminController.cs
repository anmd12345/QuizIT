using QUIZ_IT.Constant;
using QUIZ_IT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QUIZ_IT.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        QuizITOpenConnectionDataContext db = new QuizITOpenConnectionDataContext();
        public ActionResult Index ()
        {
            if (((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]) == null)
            {
                return Redirect("/NguoiDung/DangNhap");
            }

            var DanhSachBaiTracNghiem = db.BaiTracNghiems.ToList();
            return View(DanhSachBaiTracNghiem);
        }

        public ActionResult GiaoDich ()
        {
            if (((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]) == null)
            {
                return Redirect("/NguoiDung/DangNhap");
            }

            var GiaoDichs = db.GiaoDiches.OrderByDescending(d => d.NgayTao).ToList();

            return View(GiaoDichs);
        }

        public ActionResult TaiKhoan ()
        {
            if (((NguoiDung)Session[ApplicationConstant.SESSION.SESSION_LOGIN]) == null)
            {
                return Redirect("/NguoiDung/DangNhap");
            }

            var TaiKhoans = db.NguoiDungs.Where(n => n.QuyenId != 1).ToList();

            return View(TaiKhoans);
        }

        public ActionResult MenuPartial ()
        {
            return PartialView();
        }

        public ActionResult LoaderPartial ()
        {
            return PartialView();
        }


        public ActionResult NavBarPartial ()
        {
            return PartialView();
        }
    }
}