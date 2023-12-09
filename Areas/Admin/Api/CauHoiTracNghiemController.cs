using Newtonsoft.Json;
using QUIZ_IT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QUIZ_IT.Areas.Admin.Api
{
    public class CauHoiTracNghiemController : ApiController
    {
        QuizITOpenConnectionDataContext db = new QuizITOpenConnectionDataContext();

        [Route("api/CauHoiTracNghiem")]
        [HttpGet]
        public HttpResponseMessage GetAll (int BaiTracNghiemId)
        {
            var DanhSachCauHoiTracNghiem = db.CauHoiTracNghiems.Where(c => c.BaiTracNghiemId == BaiTracNghiemId).ToList();

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            return Request.CreateResponse(HttpStatusCode.OK, DanhSachCauHoiTracNghiem, json);
        }

        [Route("api/CauHoiTracNghiem/CauHoiSau")]
        [HttpGet]
        public HttpResponseMessage LayCauHoiSau (int BaiTracNghiemId, int CauHoiHienTai)
        {
            var CheckCauHoiHienTai = db.CauHoiTracNghiems.FirstOrDefault(c => c.Id == CauHoiHienTai && c.BaiTracNghiemId == BaiTracNghiemId);
            if (CheckCauHoiHienTai == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }


            var CauHoiSau = db.CauHoiTracNghiems.Where(c => c.BaiTracNghiemId == BaiTracNghiemId && c.Id > CauHoiHienTai).FirstOrDefault();

            if (CauHoiSau == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }


            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            return Request.CreateResponse(HttpStatusCode.OK, CauHoiSau, json);
        }

        [Route("api/CauHoiTracNghiem/CauHoiTruoc")]
        [HttpGet]
        public HttpResponseMessage LayCauHoiTruoc (int BaiTracNghiemId, int CauHoiHienTai)
        {
            var CheckCauHoiHienTai = db.CauHoiTracNghiems.FirstOrDefault(c => c.Id == CauHoiHienTai && c.BaiTracNghiemId == BaiTracNghiemId);
            if (CheckCauHoiHienTai == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }


            var CauHoiTruoc = db.CauHoiTracNghiems.Where(c => c.BaiTracNghiemId == BaiTracNghiemId && c.Id < CauHoiHienTai).OrderByDescending(c => c.Id).FirstOrDefault();

            if (CauHoiTruoc == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }


            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            return Request.CreateResponse(HttpStatusCode.OK, CauHoiTruoc, json);
        }






        [Route("api/CauHoiTracNghiem/ThemCauHoiMoi")]
        [HttpGet]
        public HttpResponseMessage ThemCauHoiMoi (int BaiTracNghiemId, string CauHoiMoi)
        {
            var CauHoi = db.CauHoiTracNghiems.FirstOrDefault(c => c.CauHoi == CauHoiMoi && c.BaiTracNghiemId == BaiTracNghiemId);

            if (CauHoi != null || CauHoiMoi == "" || CauHoiMoi == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                CauHoi = new CauHoiTracNghiem();
                CauHoi.CauHoi = CauHoiMoi;
                CauHoi.BaiTracNghiemId = BaiTracNghiemId;
                db.CauHoiTracNghiems.InsertOnSubmit(CauHoi);
                db.SubmitChanges();
                var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
                json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                return Request.CreateResponse(HttpStatusCode.OK, CauHoi, json);
            }
        }



        [Route("api/CauHoiTracNghiem/SuaCauHoi")]
        [HttpGet]
        public HttpResponseMessage SuaCauHoi (int id, string CauHoiMoi)
        {
            var CauHoi = db.CauHoiTracNghiems.FirstOrDefault(c => c.Id == id);

            if (CauHoi == null || CauHoiMoi == "" || CauHoiMoi == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                if (CauHoi.CauHoi == CauHoiMoi)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    CauHoi.CauHoi = CauHoiMoi;
                    db.SubmitChanges();
                    var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
                    json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                    return Request.CreateResponse(HttpStatusCode.OK, CauHoi, json);
                }

            }
        }


        [Route("api/CauHoiTracNghiem/XoaCauHoi")]
        [HttpGet]
        public HttpResponseMessage XoaCauHoi (int BaiTracNghiemId, int CauHoiId)
        {
            var CauHoi = db.CauHoiTracNghiems.FirstOrDefault(c => c.Id == CauHoiId && c.BaiTracNghiemId == BaiTracNghiemId);

            if (CauHoi == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                CauHoi.BaiTracNghiemId = null;
                db.SubmitChanges();

                var CauHoiDeleted = db.CauHoiTracNghiems.FirstOrDefault(c => c.Id == CauHoiId);

                db.CauHoiTracNghiems.DeleteOnSubmit(CauHoiDeleted);
                db.SubmitChanges();

                var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
                json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                return Request.CreateResponse(HttpStatusCode.OK, CauHoiDeleted, json);
            }
        }
    }
}
