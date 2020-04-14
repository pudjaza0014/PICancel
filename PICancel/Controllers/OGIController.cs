using PICancel.Conn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PICancel.Models;
using ClosedXML.Excel;
using System.IO;

namespace PICancel.Controllers
{
    public class OGIController : Controller
    {
        MgrCondition objrun = new MgrCondition();
        DataTable dt;
        DataSet ds;
        #region Get Initial OGICancel

        // GET: OGI
        public ActionResult OGICancel(string Event)
        {
            ViewBag.USERNAME = Session["USERNAME"];

            if (Session["OPID"] == null)
            {
                return View("~/Views/Home/Login.cshtml");
            }
            else if (Event != "" && Event != null)
            {
                string TitleEvent = "";
                string _WKstatus = "";
                if (Event == "CancelConfirm")
                {
                    TitleEvent = "OGI CANCEL CONFIRM";
                    _WKstatus = "Confirm";
                }
                else if (Event == "CancelInquiry")
                {
                    TitleEvent = "OGI CANCEL CONFIRM INQUERY";
                    _WKstatus = "Inquiry";
                }
                string AuthorityLV = Session["AuthorityLV"] as string;
                ViewBag.Title = TitleEvent;
                ViewBag.Event = Event;
                ViewBag.WKstatus = _WKstatus;
                ViewBag.AuthorityLV = AuthorityLV;
                return View();
            }
            else
            {
                return View("~/Views/Home/Login.cshtml");
            }
             
        }
        
        public JsonResult GetCancelCodeReason()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetOGICancelCode();
            if (dt.Rows.Count != 0)
            { 
                listItems.Add(new SelectListItem()
                {
                    Text = "All",
                    Value = ""
                });
               
                foreach (DataRow row in dt.Rows)
                { 
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["CancelList"].ToString().Trim(),
                        Value = row["CancelCode"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }

        public JsonResult GetOGICancelOperator()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetOGICancelOperator();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "All",
                    Value = ""
                });
                string strText = "";
                foreach (DataRow row in dt.Rows)
                { 
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["OperatorName"].ToString().Trim(),
                        Value = row["OperatorName"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }

        public JsonResult GetTypeOGI()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetOGIType();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "All",
                    Value = ""
                });
                string strText = "";
                foreach (DataRow row in dt.Rows)
                {
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["Type"].ToString().Trim(),
                        Value = row["Type"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }

        public JsonResult GetStatusOGI()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetStatusOGI();
            if (dt.Rows.Count != 0)
            {
                //listItems.Add(new SelectListItem()
                //{
                //    Text = "SELECT STATUS",
                //    Value = ""
                //});
                //string strText = "";
                foreach (DataRow row in dt.Rows)
                {
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["StatusName"].ToString().Trim(),
                        Value = row["StatusID"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }
        #endregion

        #region OGI Cancel Inquiry

        public JsonResult GetDataInquiry(OGIInquiryCondition DataCondition)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            DataSet dsCancel = new DataSet();
            DataTable dtCancel = new DataTable();
            List<OGICancelInquiryModel> dtTemp = new List<OGICancelInquiryModel>();

            OGIInquiryResult dtTemp2 = new OGIInquiryResult();
            dtTemp2 = objrun.GetDataOGIInquiry(DataCondition);

            if(dtTemp2.Result == true)
            {
                _Result = "OK"; 
                dtTemp = dtTemp2.DataList;
                Session["OGIInquiry"] = DataCondition;
            }
            else
            {
                _Result = "error";
                dtTemp = null;
            }
            _DataResult = dtTemp2.strResult;
            _ResultLabel = dtTemp2.Result;

            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        #endregion
 
        #region ExporttoExcel

        public ActionResult GetDataExportToExcel()
        { 
            try
            {
                OGIInquiryCondition Data = Session["OGIInquiry"] as OGIInquiryCondition;
                DataSet ds = getDataSetExportToExcel(Data);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds.Tables[0]); 
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; 
                    wb.Style.Font.Bold = true; 
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= OGI_Cancel_Inquiry.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                return Json("Error:" + e.Message);
            }

        }
        public DataSet getDataSetExportToExcel(OGIInquiryCondition Data)
        {
            DataSet ds = new DataSet();


            ds = objrun.GetdsOGIInquiry(Data);

            int CountDs = ds.Tables.Count;
            var Checking = 0;
            string[] strNameSheets = { "OGI_Cancel_Inquiry" };
            for (int i = 0; i < CountDs; i++)
            {
                ds.Tables[i].TableName = strNameSheets[i];
                Checking = Checking + 1;
            }

            var asdadasd = Checking;


            return ds;
        }

        #endregion


        #region OGI Confirm

        public JsonResult GetDataConfirm(OGIConfirmCondition DataCondition)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            DataSet dsCancel = new DataSet();
            DataTable dtCancel = new DataTable();
            List<OGICancelInquiryModel> dtTemp = new List<OGICancelInquiryModel>();

            OGIInquiryResult dtTemp2 = new OGIInquiryResult();
            dtTemp2 = objrun.GetDataOGIConfirm(DataCondition, Session["AuthorityLV"] as string); 
            if (dtTemp2.Result == true)
            {
                _Result = "OK";
                dtTemp = dtTemp2.DataList;
                Session["OGIInquiry"] = DataCondition;
            }
            else
            {
                _Result = "error";
                dtTemp = null;
            }
            _DataResult = dtTemp2.strResult;
            _ResultLabel = dtTemp2.Result;

            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult CanceltransactionTrans(string[] TrnID)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            DataSet dsCancel = new DataSet();
            DataTable dtCancel = new DataTable();
            OGICancelResult dtTemp = new OGICancelResult();
            
            dtTemp = objrun.CanceltransactionTrans(TrnID, Session["AuthorityLV"] as string, Session["OPID"] as string);

            _ResultLabel = dtTemp.Result;
            _DataResult = dtTemp.strResult;
            dtTemp = null;
           var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

    }
}