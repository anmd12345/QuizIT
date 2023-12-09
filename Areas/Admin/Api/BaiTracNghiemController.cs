using QUIZ_IT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace QUIZ_IT.Areas.Admin.Api
{

    public class BaiTracNghiemController : ApiController
    {
        QuizITOpenConnectionDataContext db = new QuizITOpenConnectionDataContext();

        [Route("api/BaiTracNghiem")]
        public HttpResponseMessage GetAll ()
        {
            var baiTracNghiems = db.BaiTracNghiems.ToList();
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            return Request.CreateResponse(HttpStatusCode.OK, baiTracNghiems, json);
        }


        [Route("api/BaiTracNghiem/SuaBaiTracNghiem")]
        [HttpGet]
        public HttpResponseMessage SuaBaiTracNghiem (int id, string tenBaiTracNghiemMoi, string moTaBaiTracNghiemMoi)
        {
            var BaiTracNghiem = db.BaiTracNghiems.FirstOrDefault(b => b.Id == id);
            if (BaiTracNghiem != null)
            {
                BaiTracNghiem.TenBaiTracNghiem = tenBaiTracNghiemMoi;
                BaiTracNghiem.MoTaBaiTracNghiem = moTaBaiTracNghiemMoi;
                db.SubmitChanges();
                var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
                json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                var response = Request.CreateResponse(HttpStatusCode.OK, BaiTracNghiem, json);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }


        [Route("api/BaiTracNghiem/XoaBaiTracNghiem")]
        [HttpGet]
        public HttpResponseMessage XoaBaiTracNghiem (int id)
        {
            var BaiTracNghiem = db.BaiTracNghiems.FirstOrDefault(b => b.Id == id);
            if (BaiTracNghiem != null)
            {
                BaiTracNghiem.TrangThai = !BaiTracNghiem.TrangThai;
                db.SubmitChanges();
                var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
                json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                var response = Request.CreateResponse(HttpStatusCode.OK, BaiTracNghiem, json);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }


        [Route("api/BaiTracNghiem/ThemBaiTracNghiem")]
        [HttpGet]
        public HttpResponseMessage ThemBaiTracNghiem (int NguoiTao, string TenBaiTracNghiem, string MoTaBaiTracNghiem)
        {
            var BaiTracNghiem = db.BaiTracNghiems.FirstOrDefault(b => b.TenBaiTracNghiem == TenBaiTracNghiem);
            if (BaiTracNghiem == null)
            {
                BaiTracNghiem = new BaiTracNghiem();
                BaiTracNghiem.TenBaiTracNghiem = TenBaiTracNghiem;
                BaiTracNghiem.MoTaBaiTracNghiem = MoTaBaiTracNghiem;
                BaiTracNghiem.TrangThai = false;
                BaiTracNghiem.NguoiDungId = NguoiTao;
                db.BaiTracNghiems.InsertOnSubmit(BaiTracNghiem);
                db.SubmitChanges();
                var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
                json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                var response = Request.CreateResponse(HttpStatusCode.OK, BaiTracNghiem, json);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }








    }
}
