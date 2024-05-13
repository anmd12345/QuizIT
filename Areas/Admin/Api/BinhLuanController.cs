using Microsoft.AspNet.SignalR;
using QUIZ_IT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.SessionState;

namespace QUIZ_IT.Areas.Admin.Api
{
    public class BinhLuanController : ApiController
    {

        QuizITOpenConnectionDataContext db = new QuizITOpenConnectionDataContext();

        [Route("api/BinhLuan")]
        public HttpResponseMessage GetAll (int CauHoiId)
        {

            var dsBinhLuan = db.BinhLuans.Where(x => x.CauHoiId == CauHoiId).OrderByDescending(x => x.ThoiGianDang).ToList();
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return Request.CreateResponse(HttpStatusCode.OK, dsBinhLuan, json);
        }


        [Route("api/DemLike")]
        [HttpGet]
        public HttpResponseMessage DemSoLike (int BinhLuanId)
        {
            var dsBinhLuanChiTiet = db.BinhLuanChiTiets.Where(x => x.BinhLuanId == BinhLuanId && x.DaThich == true).ToList();
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return Request.CreateResponse(HttpStatusCode.OK, dsBinhLuanChiTiet.Count, json);
        }

        [Route("api/ThemBinhLuan")]
        [HttpGet]
        public HttpResponseMessage ThemBinhLuan (string noiDung, int cauHoiId, int nguoiDungId)
        {
            if (noiDung == "" || Regex.IsMatch(noiDung, @"<script[^>]*>|<\/script>|<[^>]+>|on\w+="))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            var NguoiDung = db.NguoiDungs.FirstOrDefault(n => n.Id == nguoiDungId);

            var BinhLuanMoi = new BinhLuan();
            BinhLuanMoi.NoiDung = noiDung;
            BinhLuanMoi.ThoiGianDang = DateTime.Now;
            BinhLuanMoi.NguoiDungId = nguoiDungId;
            BinhLuanMoi.CauHoiId = cauHoiId;
            db.BinhLuans.InsertOnSubmit(BinhLuanMoi);
            db.SubmitChanges();
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return Request.CreateResponse(HttpStatusCode.OK, BinhLuanMoi, json);
        }


        [Route("api/ThemBinhLuanChiTiet")]
        [HttpGet]
        public HttpResponseMessage ThemBinhLuanChiTiet (int BinhLuanId, int NguoiDungId)
        {
            var BinhLuanChiTietMoi = new BinhLuanChiTiet();
            BinhLuanChiTietMoi.BinhLuanId = BinhLuanId;
            BinhLuanChiTietMoi.NguoiDungId = NguoiDungId;
            db.BinhLuanChiTiets.InsertOnSubmit(BinhLuanChiTietMoi);
            db.SubmitChanges();
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return Request.CreateResponse(HttpStatusCode.OK, BinhLuanChiTietMoi, json);
        }


        [Route("api/ChiTietBinhLuan")]
        [HttpGet]
        public HttpResponseMessage ChiTietBinhLuan (int BinhLuanId, int NguoiDungId)
        {
            var BinhLuanChiTiet = db.BinhLuanChiTiets.FirstOrDefault(b => b.BinhLuanId == BinhLuanId && b.NguoiDungId == NguoiDungId);
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return Request.CreateResponse(HttpStatusCode.OK, BinhLuanChiTiet, json);
        }





        [Route("api/ThichBinhLuan")]
        [HttpGet]
        public HttpResponseMessage ThichBinhLuan (int BinhLuanId, int NguoiDungId)
        {
            var BinhLuanChiTiet = db.BinhLuanChiTiets.FirstOrDefault(b => b.BinhLuanId == BinhLuanId && b.NguoiDungId == NguoiDungId);

            if (BinhLuanChiTiet != null)
            {
                BinhLuanChiTiet.DaThich = (BinhLuanChiTiet.DaThich == null || BinhLuanChiTiet.DaThich == false) ? true : false;
                db.SubmitChanges();
            }
            else
            {
                BinhLuanChiTiet = new BinhLuanChiTiet();
                BinhLuanChiTiet.DaThich = true;
                BinhLuanChiTiet.NguoiDungId = NguoiDungId;
                BinhLuanChiTiet.BinhLuanId = BinhLuanId;
                db.BinhLuanChiTiets.InsertOnSubmit(BinhLuanChiTiet);
                db.SubmitChanges();
            }


            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return Request.CreateResponse(HttpStatusCode.OK, BinhLuanChiTiet, json);
        }
    }
}
