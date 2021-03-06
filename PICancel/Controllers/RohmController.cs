﻿using PICancel.Conn;
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
using System.Web;
using System.Collections;

namespace PICancel.Controllers
{
    public class RohmController : Controller
    {
        MgrCondition objrun = new MgrCondition();
        DataTable dt;
        DataSet ds;
        // GET: Rohm


        #region Rohm Order Return

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

        #endregion

        #region RohmInquiry

        public JsonResult GetCategType()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetRohmCatgType();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "ALL",
                    Value = ""
                });

                foreach (DataRow row in dt.Rows)
                {
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["CategType"].ToString().Trim().ToUpper(),
                        Value = row["CategType"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }

        public JsonResult GetRohmIquiryType()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetRohmIquiryType();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "ALL",
                    Value = ""
                });

                foreach (DataRow row in dt.Rows)
                {
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["Type"].ToString().Trim().ToUpper(),
                        Value = row["Type"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }

        public JsonResult GetRohmInquiryList(RohmInquiryControl dataArr)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true; 
            
            List<RohmOrderInquiry> OrderNoChipList = new List<RohmOrderInquiry>(); 
            RohmInqGrp dtTemp = objrun.GetDataInquiryList(dataArr);

            Session["dataControl"] = dataArr;

            OrderNoChipList = dtTemp.DataList;
            _DataResult = dtTemp.strresult;
            _Result = dtTemp.blStatus == true ? "OK" : "Error";
            _ResultLabel = dtTemp.blStatus = true;
             

            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = OrderNoChipList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult ExportReportInquiry(string DocNo)
        {
            //try
            //{
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


               // string path = objrun.getPurpose("path", "PTH0001");

                //string path =(@"\\10.29.7.124\Sharing\");

                //bool exists = Directory.Exists(Server.MapPath(path));
                //if (!exists)
                //{
                //    Directory.CreateDirectory(Server.MapPath(path));
                //};
                //bool exists = Directory.Exists(path);
                //if (!exists)
                //{
                //    Directory.CreateDirectory(path);
                //};
                //cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path + "NO_CHIP-" + DocNo.Trim() + ".pdf");
                //cryRpt.Close();
                //return "";
            //}
            //catch (Exception e) { return "ExportReport Error : " + e.Message; }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = cryRpt.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf", "NO_CHIP-" + DocNo.Trim() + ".pdf");
        }


        #region ExporttoExcel

        public ActionResult GetDataExportToExcel()
        {
            try
            {
                RohmInquiryControl Data = Session["dataControl"] as RohmInquiryControl;
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
        public DataSet getDataSetExportToExcel(RohmInquiryControl Data)
        {
             DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            dt = objrun.GetdtInquiryList(Data);
            ds.Tables.Add(dt); 
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



        #endregion

        #region Product plan Maintenance

        public ActionResult ProductPlan(string Event)
        {
            Session["model"] = null;
            ViewBag.USERNAME = Session["USERNAME"];
            if (Session["OPID"] == null)
            {
                return View("~/Views/Home/Login.cshtml");
            }
            else if (Event != "" && Event != null)
            {
                string TitleEvent = "";
                string _WKstatus = "";

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

        public JsonResult GetTypeCode()
        {

            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetTypeCode();
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
                        Text = row["TypeCode"].ToString().Trim() + " : " + row["Name"].ToString().Trim(),
                        Value = row["TypeCode"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }

        public JsonResult AddTempTable(ProductPlan ArrData)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            DataSet dsCancel = new DataSet();
            DataTable dtCancel = new DataTable();
            //List<ProductPlan> dtTemp = new List<ProductPlan>();

            List<ProductPlan> dtTemp2 = Session["model"] as List<ProductPlan>;
            if (dtTemp2 == null)
            {
                dtTemp2 = new List<ProductPlan>();
            }

            dt = new DataTable();
            dt = objrun.GetProductPlanType(ArrData.TypeCode);


            dtTemp2.Add(new ProductPlan() {
                InputDate = ArrData.InputDate,
                Type = dt.Rows.Count != 0 ? dt.Rows[0]["Type"].ToString() : "" ,
                TypeCode= ArrData.TypeCode,
                DeliveryPlanQty = Convert.ToInt32(ArrData.DeliveryPlanQty),
                UserName = Session["OPID"] as string 
            });

            Session["model"] = dtTemp2;

            _DataResult = "";

            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult UploadExcel()
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true; 
            HttpPostedFileBase file = Request.Files[0]; 
            HttpPostedFileBase strReturn = Request.Files["FileUpload1"];
            DataTable dt;
            DataSet ds;
            //bool flag = true;
            //string responseMessage = string.Empty;
            string Results = "";
            try
            {
                if (file != null && file.ContentLength > 0  )
                //if (file != null && file.ContentLength > 0 && (Path.GetExtension(file.FileName).ToLower() == ".csv"))
                {
                    try
                    {
                        string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                        string query = null;
                        string connString = "";
                        string fileName = Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                        string[] arrResult; 
                        if (System.IO.File.Exists(filePath)) {
                            System.IO.File.Delete(filePath);
                        }; 
                        file.SaveAs(filePath);
                        //SautinSoft.UseOffice u = new SautinSoft.UseOffice();

                        //string inpFile = Path.GetFullPath(filePath);
                        //string outFile = Path.GetFullPath(filePath.Replace("xlsx","csv"));

                        //int ret = u.InitExcel();
                        //if (ret == 1)
                        //{

                        //    Console.WriteLine("Error! Can't load MS Excel library in memory");
                            
                        //}

                        //ret = u.ConvertFile(inpFile, outFile, SautinSoft.UseOffice.eDirection.XLSX_to_CSV);

                        //filePath = filePath.Replace("xlsx", "csv");
                        //extension = ".csv";
                        //u.CloseExcel();

                        //if (ret == 0)
                        //{
                        //    // Open the result.
                        //   // System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
                        //}


                        _ResultLabel = true;

                        if (extension == ".csv")
                        {
                            ds = Utility.ConvertCSVtoDataSet(filePath);
                            //responseMessage = objrun.UpdTargetMCgroup(dt, "");
                            //ViewBag.Data = dt;
                            if (ds.Tables.Count > 0)
                            {

                                arrResult = dsProductplanEntry(ds);


                                _DataResult = arrResult[0];
                                Results = _DataResult;
                                _ResultLabel = Convert.ToBoolean(arrResult[1]);
                            }
                            else
                            {
                                _DataResult = "datatable nothing ";
                                if (System.IO.File.Exists(filePath))
                                {
                                    _DataResult += filePath;
                                }
                            }
                        }
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            dt = Utility.ConvertXSLXtoDataTable(filePath, connString);
                            DataTable dtCopy = dt.Copy();
                            //ViewBag.Data = dt;
                            ds = new DataSet();
                            ds.Tables.Add(dtCopy);
                            // ViewBag.Data = dt;
                            if (ds.Tables.Count > 0)
                            {

                                arrResult = dsProductplanEntry(ds);


                                _DataResult = arrResult[0];
                                Results = _DataResult;
                                _ResultLabel = Convert.ToBoolean(arrResult[1]);
                            }
                            else
                            {
                                _DataResult = "datatable nothing ";
                                if (System.IO.File.Exists(filePath))
                                {
                                    _DataResult += filePath;
                                }
                            }
                        }
                        else
                        if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            dt = Utility.ConvertXSLXtoDataTable(filePath, connString);
                            DataTable dtCopy = dt.Copy();
                            //ViewBag.Data = dt;
                            ds = new DataSet();
                            ds.Tables.Add(dtCopy);
                            if (ds.Tables.Count > 0)
                            { 
                                arrResult = dsProductplanEntry(ds); 
                                _DataResult = arrResult[0];
                                Results = _DataResult;
                                _ResultLabel = Convert.ToBoolean(arrResult[1]);
                            }
                            else
                            {
                                _DataResult = "datatable nothing ";
                                if (System.IO.File.Exists(filePath))
                                {
                                    _DataResult += filePath;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _ResultLabel = false;
                        _DataResult = "Upload Failed with error: " + ex.Message;
                    }
                }
                else
                {
                    _ResultLabel = false;
                    _DataResult = "Please Upload Files .csv format";
                }
            } 
            catch (Exception e)
            {
                _Result = "ERROR";
                _ResultLabel = false;
                _DataResult = e.Message;
            }
            //return Json(new { data = responseMessage, flags = flag }, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = Results }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        



        public string[] dsProductplanEntry(DataSet ds)
        {
            string result;
            string _Result = "1";
            string _DataResult = "OK";
            Boolean _ResultLabel = true;

            string[] arrResult;
            

            List<ProductPlan> listItems = new List<ProductPlan>();
            int chkCol = 0;
            string strInputDate;
            try
            {
                foreach (DataColumn i in ds.Tables[0].Columns)
                {
                    if (chkCol >= 2 && i.ColumnName.ToString() != "ProductPlanTemplate")
                    { 
                        strInputDate = i.ColumnName.ToString(); 
                        foreach (DataRow row in ds.Tables[0].Rows)
                        { 
                            listItems.Add(new ProductPlan()
                            {
                                Type = row["Type"].ToString(),
                                TypeCode = row["TypeCode"].ToString(),
                                DeliveryPlanQty = Convert.ToInt32(row[strInputDate].ToString()),
                                InputDate = strInputDate,
                                UserName = Session["OPID"] as string
                            });  
                        }
                    }
                    chkCol++;
                } 
                _DataResult = objrun.ProductPlanUpdate(listItems); 
                _Result = _DataResult == "OK" ? "1" : "0"; 
            }
            catch (Exception e)
            {
                _DataResult = e.Message;
                _Result = "0";
            } 
            arrResult = new string[] { _DataResult, _Result }; 
            return arrResult; 
        }



        public JsonResult ProductplanEntry()
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;

            _DataResult = objrun.ProductPlanUpdate(Session["model"] as List<ProductPlan>);
            _Result = _DataResult == "OK" ? "1" : "0";

            if(_DataResult == "OK")
            {

                _Result = "OK";
                Session["model"] = null;


            }
            else
            {
                _Result = "Error"; 
            }


            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = _DataResult }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult ProdmainClearTemp()
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;

            Session["model"] = null;
            if (Session["model"] != null)
            {
                _ResultLabel = false;
                _Result = "Error";
            } 
            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = _DataResult }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }


        public JsonResult getOrderPlanInquiry(OrderPlanControl arrData)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            List<OrderPlanSummary> DataList = new List<OrderPlanSummary>();
            OrderPlanInqGrp dtTemp = objrun.GetOrderPlanSummary(arrData);

            DataList = dtTemp.DataList;
            _DataResult = dtTemp.strresult;
            _Result = dtTemp.blStatus == true ? "OK" : "Error";
            _ResultLabel = dtTemp.blStatus = true;


            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = DataList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }



        public ActionResult OrderplanDetail(string strInputDate, string strTypeCode)
        {
            ViewBag.USERNAME = Session["USERNAME"];

            if (Session["OPID"] == null)
            {

                return View("~/Views/Home/Login.cshtml");
            }
            ViewBag.Title = "Order Plan Detail : " + strInputDate + " TypeCode : " + strTypeCode;

            ViewBag.strInputDate = strInputDate;
            ViewBag.strTypeCode = strTypeCode;

            return View();
        }

        public JsonResult GetOrderdetails(string strInputDate, string strTypeCode)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            List<OrderPlanDetail> DataList = new List<OrderPlanDetail>();

            DataList = objrun.GetdtOrderPlanDetail(strInputDate, strTypeCode);

            ViewBag.DataList = DataList;

            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = DataList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
    }
}