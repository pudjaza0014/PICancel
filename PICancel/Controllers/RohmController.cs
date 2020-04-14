using PICancel.Conn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PICancel.Models;
using ClosedXML.Excel;
using System.IO;
using System.Configuration;

namespace PICancel.Controllers
{
    public class RohmController : Controller
    {
        MgrCondition objrun = new MgrCondition();
        DataTable dt;
        DataSet ds;
        // GET: Rohm
        public ActionResult OrderCancel(string Event)
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
                if (Event == "OrderError")
                {
                    TitleEvent = "ORDER ERROR RETURN";
                    _WKstatus = "OrderError";
                }
                else if (Event == "NoChip")
                {
                    TitleEvent = "NO CHIP RETURN";
                    _WKstatus = "NoChip";
                }
                else if (Event == "Inquiry")
                {
                    TitleEvent = "ORDER RETURN INQUIRY";
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

        public JsonResult GetNOChipErrorgrp(string _WKstatus)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetNoChipErrorGrp(_WKstatus);
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "Select ",
                    Value = ""
                });

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



        public JsonResult GetDataListNoChip(RohmOrderControl dataArr, string _WKstatus)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            //DataSet dsCancel = new DataSet();
            //DataTable dtCancel = new DataTable();
            string strDocNo = "";
            string strRunNo = "";
            List<RohmOrderNOChip> OrderNoChipList = new List<RohmOrderNOChip>();
            //List<OGICancelInquiryModel> dtTemp = new List<OGICancelInquiryModel>();
            RohmOrderModel dtTemp = objrun.GetDataDetail(dataArr, _WKstatus);
            

            OrderNoChipList = dtTemp.ListOrder;
            _DataResult = dtTemp.strResult;
            _Result = dtTemp.blResult == true ? "OK" : "Error";
            _ResultLabel = dtTemp.blResult = true;


            strDocNo = DateTime.Now.ToString("yyyyMMdd") + "-" + dataArr.Type + "-";
            dt = objrun.GetRunningNo(dataArr.Type);

            if (dt.Rows.Count == 0)
            {
                strRunNo = "001";
            }
            else
            {
                strRunNo = Convert.ToString( Convert.ToInt32(dt.Rows[0][""].ToString())+1).PadLeft(3, '0');
            }
            


            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = OrderNoChipList,DocNo = strDocNo ,RunNo = strRunNo }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        public JsonResult EntryNoChip(List<RohmOrderQty> dataRohmOrderQty,string strRunNo)
        {
            string _Result = "OK";
            string _strRunNo = strRunNo;
            string _DataResult = "";
            Boolean _ResultLabel = true;  
            List<RohmOrderNOChip> OrderNoChipList = new List<RohmOrderNOChip>();

            try
            {
                _strRunNo = Convert.ToString(Convert.ToInt32(strRunNo.Trim())).PadLeft(3,'0');






            }
            catch(Exception e)
            {

            }

            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = OrderNoChipList  }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}