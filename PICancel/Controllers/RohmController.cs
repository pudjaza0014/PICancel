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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

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
                strRunNo = Convert.ToString(Convert.ToInt32(dt.Rows[0]["RunningNo"].ToString()) + 1).PadLeft(3, '0');
            }



            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = OrderNoChipList, DocNo = strDocNo, RunNo = strRunNo }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
         
        public JsonResult EntryNoChip(List<RohmOrderQty> dataRohmOrderQty)
        {
            string _Result = "OK";
            string _strRunNo = "";
            string strDocNo = dataRohmOrderQty[0].strRunNo1 + dataRohmOrderQty[0].strRunNo2;
            string _DataResult = "";
            Boolean _ResultLabel = true;
            int Seqno = 1;
            dt = new DataTable();
            List<RohmOrderNOChip> OrderNoChipList = new List<RohmOrderNOChip>();
            try
            {
                string DataResult;
                foreach (RohmOrderQty Dataitem in dataRohmOrderQty)
                {
                    _strRunNo = Convert.ToString(Convert.ToInt32(Dataitem.strRunNo2.Trim())).PadLeft(3, '0');
                    string _orderNo = Dataitem.OrderNo;
                    int _orderQty = Dataitem.OrderQty;
                    int _originQty = Dataitem.OrderQtyOrg;
                    string strRunNo1 = Dataitem.strRunNo1;
                    string strCagte = Dataitem.CategType;
                    int Condition = 2;
                    if (_originQty != _orderQty)
                    {
                        Condition = 3;
                    }
                    else
                    {
                        _orderQty = 0;
                    }

                    DataResult = objrun.OrderDeleteChangeRohm(_orderNo, strRunNo1 + _strRunNo, strCagte, Condition, _orderQty, Seqno, Session["OPID"] as string);

                    if (DataResult != "0")
                    {
                        _ResultLabel = false;
                        _Result = "Error";
                        _DataResult = (DataResult == "1" ? "Error sprOrderDeleteChangRohm '" + _orderNo + "' " : DataResult) + "Please Contact IS. 4611 , 4612";
                        break;
                    }
                    else
                    {
                        Seqno++;
                    }

                }

                _DataResult = Seqno - 1 == dataRohmOrderQty.Count ? "Success" : "Error";

            }
            catch (Exception e)
            {

            }
            string Export = ExportReport(strDocNo);
            if (Export != "")
            {
                _ResultLabel = false;
                _Result = "Error";
                _DataResult = Export;
            }
            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = Export }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult DeleteNoChip(List<RohmOrderQty> dataRohmOrderQty)
        {
            string _Result = "OK";
            string _strRunNo = "";
            string strDocNo = dataRohmOrderQty[0].strRunNo1 + dataRohmOrderQty[0].strRunNo2;
            string _DataResult = "";
            Boolean _ResultLabel = true;
            int Seqno = 1;
            dt = new DataTable();
            List<RohmOrderNOChip> OrderNoChipList = new List<RohmOrderNOChip>();
            try
            {
                string DataResult;
                foreach (RohmOrderQty Dataitem in dataRohmOrderQty)
                {
                    //_strRunNo = Convert.ToString(Convert.ToInt32(Dataitem.strRunNo2.Trim())).PadLeft(3, '0');
                    string _orderNo = Dataitem.OrderNo;
                    int _orderQty = Dataitem.OrderQty;
                    int _originQty = Dataitem.OrderQtyOrg;
                    string strRunNo1 = Dataitem.strRunNo1;
                    string strCagte = Dataitem.CategType;
                    int Condition = 2;
                    if (_originQty != _orderQty)
                    {
                        Condition = 3;
                    }
                    else
                    {
                        _orderQty = 0;
                    }

                    DataResult = objrun.OrderHideSimulationtrans(_orderNo);

                    if (DataResult != "0")
                    {
                        _ResultLabel = false;
                        _Result = "Error";
                        _DataResult = (DataResult == "1" ? "Error sprSimulationResultTrans '" + _orderNo + "' " : DataResult) + "Please Contact IS. 4611 , 4612";
                        break;
                    }
                    else
                    {
                        Seqno++;
                    }

                }

                _DataResult = Seqno - 1 == dataRohmOrderQty.Count ? "Success" : "Error";

            }
            catch (Exception e)
            {

            }
            string Export = "";//ExportReport(strDocNo);
            if (Export != "")
            {
                _ResultLabel = false;
                _Result = "Error";
                _DataResult = Export;
            }
            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = Export }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
 
        public string ExportReport(string DocNo)
        {
            try
            {
                ConnectionInfo myconnectioninfo = new ConnectionInfo();
                ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings["TRPIConnectionString"];
                var connectionStringPieces = ConnectionStringSettings.ConnectionString.Split(';');
                foreach (string connectionStringPiece in connectionStringPieces)
                {
                    string[] connectionSubPieces = connectionStringPiece.Split('=');

                    string key = connectionSubPieces[0].ToLower();
                    string value = connectionSubPieces[1];
                    switch (key)
                    {
                        case "data source":
                            myconnectioninfo.ServerName = value;
                            break;
                        case "server":
                            myconnectioninfo.ServerName = value;
                            break;
                        case "initial catalog":
                            myconnectioninfo.DatabaseName = value;
                            break;
                        case "user id":
                            myconnectioninfo.UserID = value;
                            break;
                        case "password":
                            myconnectioninfo.Password = value;
                            break;
                        case "pwd":
                            myconnectioninfo.Password = value;
                            break;
                    }
                }

                ReportDocument cryRpt = new ReportDocument();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;
                // cryRpt.Load(Path.Combine(Server.MapPath("~/Report"), "RptOrderChang.rpt"));
                cryRpt.Load(Path.Combine(Server.MapPath("~/Report"), "RptOrderChang.rpt"));

                cryRpt.SetParameterValue("DocNo", DocNo);
                cryRpt.SetParameterValue("DocNo", DocNo.Trim());
                cryRpt.SetParameterValue("Condition-2", objrun.GetCountCondition(DocNo.Trim(), 2));
                cryRpt.SetParameterValue("Condition-3", objrun.GetCountCondition(DocNo.Trim(), 3));

                crConnectionInfo.ServerName = myconnectioninfo.ServerName;
                crConnectionInfo.DatabaseName = myconnectioninfo.DatabaseName;
                crConnectionInfo.UserID = myconnectioninfo.UserID;
                crConnectionInfo.Password = myconnectioninfo.Password;

                CrTables = cryRpt.Database.Tables;


                foreach (Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }


                string path = objrun.getPurpose("path", "PTH0001");

                //string path =(@"\\10.29.7.124\Sharing\");

                //bool exists = Directory.Exists(Server.MapPath(path));
                //if (!exists)
                //{
                //    Directory.CreateDirectory(Server.MapPath(path));
                //};
                bool exists = Directory.Exists(path);
                if (!exists)
                {
                    Directory.CreateDirectory(path);
                };
                cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path + "NO_CHIP-" + DocNo.Trim() + ".pdf");
                cryRpt.Close();
                return "";
            }
            catch (Exception e) { return "ExportReport Error : " + e.Message; }
            //Response.Buffer = false;
            //Response.ClearContent();
            //Response.ClearHeaders();

            //Stream stream = cryRpt.ExportToStream(ExportFormatType.PortableDocFormat);
            //stream.Seek(0, SeekOrigin.Begin);

            //return File(stream, "application/pdf", "NO_CHIP-" + DocNo.Trim() + ".pdf");
        }
    }
}