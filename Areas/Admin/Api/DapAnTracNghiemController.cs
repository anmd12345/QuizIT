using QUIZ_IT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QUIZ_IT.Areas.Admin.Api
{
    public class DapAnTracNghiemController : ApiController
    {
        QuizITOpenConnectionDataContext db = new QuizITOpenConnectionDataContext();

        [Route("api/DapAnTracNghiem")]
        [HttpGet]
        public HttpResponseMessage GetAll (int CauHoiTracNghiemId)
        {
            var DanhSachDapAn = db.DapAnTracNghiems.Where(d => d.CauHoiTracNghiemId == CauHoiTracNghiemId).ToList();

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            return Request.CreateResponse(HttpStatusCode.OK, DanhSachDapAn, json);
        }


        [Route("api/DapAnTracNghiem/KiemTraDapAn")]
        [HttpGet]
        public HttpResponseMessage KiemTraDapAn (string CauHoi, string DapAn)
        {
            var CauHoiTracNghiem = db.CauHoiTracNghiems.Where(c => c.CauHoi == CauHoi).FirstOrDefault();
            if (CauHoiTracNghiem != null)
            {
                var DapAnDung = db.DapAnTracNghiems.Where(d => d.CauHoiTracNghiemId == CauHoiTracNghiem.Id && d.CauTraLoi == DapAn).ToList().FirstOrDefault();
                if (DapAnDung == null || DapAnDung.DapAn == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
                    json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                    return Request.CreateResponse(HttpStatusCode.OK, DapAnDung, json);
                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }


        [Route("api/DapAnTracNghiem/ThemDapAn")]
        [HttpGet]
        public HttpResponseMessage ThemDapAn (int CauHoiTracNghiemId, string DapAnMoi, bool KetQua)
        {
            var DapAn = db.DapAnTracNghiems.FirstOrDefault(d => d.CauTraLoi == DapAnMoi && d.CauHoiTracNghiemId == CauHoiTracNghiemId);

            if (DapAn == null)
            {
                DapAn = new DapAnTracNghiem();
                DapAn.CauTraLoi = DapAnMoi;
                DapAn.DapAn = KetQua;
                DapAn.CauHoiTracNghiemId = CauHoiTracNghiemId;
                db.DapAnTracNghiems.InsertOnSubmit(DapAn);
                db.SubmitChanges();
                var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
                json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

                return Request.CreateResponse(HttpStatusCode.OK, DapAn, json);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }


        [Route("api/DapAnTracNghiem/XoaDapAn")]
        [HttpGet]
        public HttpResponseMessage XoaDapAn (int CauHoiTracNghiemId)
        {
            var listDapAn = db.DapAnTracNghiems.Where(d => d.CauHoiTracNghiemId == CauHoiTracNghiemId).ToList();
            listDapAn.ForEach(item =>
            {
                var DapAn = db.DapAnTracNghiems.FirstOrDefault(i => i.Id == item.Id);
                db.DapAnTracNghiems.DeleteOnSubmit(DapAn);
                db.SubmitChanges();
            });
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return Request.CreateResponse(HttpStatusCode.OK, listDapAn, json);
        }



        [Route("api/DapAnTracNghiem/XoaMotDapAn")]
        [HttpGet]
        public HttpResponseMessage XoaMotDapAn (int id)
        {
            var DapAn = db.DapAnTracNghiems.FirstOrDefault(d => d.Id == id);

            if (DapAn == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            DapAn.CauHoiTracNghiemId = null;
            db.SubmitChanges();


            db.DapAnTracNghiems.DeleteOnSubmit(DapAn);
            db.SubmitChanges();

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return Request.CreateResponse(HttpStatusCode.OK, DapAn, json);
        }


        [Route("api/DapAnTracNghiem/SuaMotDapAn")]
        [HttpGet]
        public HttpResponseMessage SuaMotDapAn (int id, string dapAnMoi, bool ketQua)
        {
            var DapAn = db.DapAnTracNghiems.FirstOrDefault(d => d.Id == id);

            if (DapAn == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            DapAn.CauTraLoi = dapAnMoi;
            DapAn.DapAn = ketQua;
            db.SubmitChanges();

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return Request.CreateResponse(HttpStatusCode.OK, DapAn, json);
        }
    }
}
