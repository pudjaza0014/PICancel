using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using PICancel.Conn;
using PICancel.Models;
using System.Net;
using ClosedXML.Excel;

namespace PICancel.Controllers
{
    public class HomeController : Controller
    {
        MgrCondition objrun = new MgrCondition();
        DataTable dt;
        DataSet ds;
        // GET: Home
        #region Login-Logout

        
        public ActionResult Login()
        {
            Session["OPID"] = null;
            Session["USERNAME"] = null;
            //string PCName1 = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName; 
            //Session["COMPUTERNAME"] = PCName1.Replace(".rist.local", "").ToUpper();
            Session["COMPUTERNAME"] = "";
            ViewBag.USERNAME = Session["COMPUTERNAME"]; 
            ViewBag.Title = "TRDI CANCEL CONTROL MENU";
            return View();
        }
        public JsonResult ChkLogin(string OPID,string Password)
        { 
            dt = new DataTable();
            OPeratorLogin OPResult = new OPeratorLogin();
            string _Result = "";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            OPResult = objrun.GetOperator(OPID, Password);
            if (OPResult.strResult == true && OPResult.OPeratordetail == null)
            {
                _Result = "warning";
                _DataResult = "Data not found";
                _ResultLabel = OPResult.strResult;
            }
            else if (OPResult.strResult == false && OPResult.OPeratordetail != null)
            {
                _Result = "error";
                _DataResult = OPResult.OPeratordetail.OPID;
                _ResultLabel = OPResult.strResult;
            }
            else
            {
                _Result = "OK";
                Session["USERNAME"] = OPResult.OPeratordetail.OPName;
                Session["OPID"] = OPResult.OPeratordetail.OPID;
                Session["OPType"] = OPResult.OPeratordetail.OPType;
                Session["AuthorityLV"] = OPResult.OPeratordetail.AuthorityLV.Trim();
                _ResultLabel = OPResult.strResult;

            }
            return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = OPResult.OPeratordetail }, JsonRequestBehavior.AllowGet);
           
        }
         
        public JsonResult GetLogOut()
        {


            dt = new DataTable();
            
            string _Result = "";
            string _DataResult = "";
            Boolean _ResultLabel = true;


            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            if (Session.Count  ==  0)
            {
                _Result = "OK";
            }



            return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region picancel     
        #region GetData initial PICancel
        public ActionResult Index()
        {
             if(Session["OPID"] == null)
            {
                return View("Login");
            }
            //string PCName1 = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;  
            //Session["COMPUTERNAME"] = PCName1.Replace(".rist.local", "").ToUpper(); 
            Session["COMPUTERNAME"] = "";
            ViewBag.USERNAME = Session["USERNAME"];
            ViewBag.COMPUTERNAME = Session["COMPUTERNAME"];
            ViewBag.Title = "TRDI CANCEL CONTROL MENU";
            return View();
        }
        public ActionResult LotCancel(string Event)
        {
           
            string PCName1 = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;
            //Session["USERNAME"] = PCName1.Replace(".rist.local", "").ToUpper();
            //Session["COMPUTERNAME"] = PCName1.Replace(".rist.local", "").ToUpper(); 
            ViewBag.USERNAME = Session["USERNAME"];
            ViewBag.COMPUTERNAME = Session["COMPUTERNAME"];


            Session["model"] = null;
            if (Session["OPID"] == null)
            {
                return View("Login");
            }else
            if (Event != "" && Event != null)
            {
                string TitleEvent = "";
                string _WKstatus = "";
                if (Event == "LotCancel")
                {
                    TitleEvent = "LOT CANCEL";
                    _WKstatus = "Lot";
                }
                else if (Event == "OrderCancel")
                {
                    TitleEvent = "ORDER CANCEL";
                    _WKstatus = "Order";
                }
                else if (Event == "LotCancelReinput")
                {
                    TitleEvent = "LOT CANCEL RE-INPUT";
                    _WKstatus = "ReInput";
                }
                else if (Event == "UndoCancel")
                {
                    TitleEvent = "UNDO CANCEL";
                    _WKstatus = "Undo";
                }
                else if (Event == "CancelInquiry")
                {
                    TitleEvent = "LOT CANCEL INQUIRY";
                    _WKstatus = "Inquiry";
                }
                else if (Event == "LotCancelReflow")
                {
                    TitleEvent = "LOT CANCEL RE-FLOW";
                    _WKstatus = "ReFlow";
                }
                ViewBag.Title = TitleEvent;
                ViewBag.Event = Event;
                ViewBag.WKstatus = _WKstatus;
                return View();
            }
            else
            {
                return View("Index");
            }
           

        }
        // Cancelcode to dropdownList
        public JsonResult getCancelCode(string Event)
        {

            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetCancelCode();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "SELECT CancelCode",
                    Value = ""
                });
                string strText = "";
                foreach (DataRow row in dt.Rows)
                {
                    strText = row["CancelCode"].ToString().Trim() + " | " + row["CancelReason"].ToString().Trim();
                    listItems.Add(new SelectListItem()
                    {
                        Text = strText,
                        Value = row["CancelCode"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));

        }
        //CancelReson after choose CancelCode 
        public JsonResult getCancelReason(string CancelCode)
        {
            try
            {
                string Result = objrun.GetCancelReason(CancelCode);
                return Json(new { data = Result }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json("Error:" + e);
            }
        }
        // TreatmentCode to dropdownList
        public JsonResult GetTreatmentCode(string Event)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetTreatmentCode();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "SELECT TreatmentCode",
                    Value = ""
                });
                string strText = "";
                foreach (DataRow row in dt.Rows)
                {
                    strText = row["TreatmentCode"].ToString().Trim() + " | " + row["TreatmentName"].ToString().Trim();
                    listItems.Add(new SelectListItem()
                    {
                        Text = strText,
                        Value = row["TreatmentCode"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }
        //TreatmentName after choose TreatmentCode 
        public JsonResult GetTreatmentName(string TreatmentCode)
        {
            try
            {
                string Result = objrun.GetTreatmentName(TreatmentCode);
                return Json(new { data = Result }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json("Error:" + e);
            }
        }
        #endregion

        #region PICancel Operation     
        //GetDetail of data cancel from database to control
        public JsonResult GetDataDetail(string strLotNo, string strWKstatus)
        {
            JsonResult test;
            if (strWKstatus == "Order")
            {
                test = funCancelOrderNoUpdate(strLotNo, strWKstatus);
            }
            else
            {
                test = funCancelLotNoUpdate(strLotNo, strWKstatus);
            }
            TempData["tdDetail"] = test;
            return Json(test.Data, JsonRequestBehavior.AllowGet);
        }
        //GetData by LotNo
        public JsonResult funCancelLotNoUpdate(string strLotNo, string strWKstatus)
        {
            dt = new DataTable();
            LotCancelDetail CancelDetailData = new LotCancelDetail();
            string _Result = "";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            if (strLotNo == "" || strLotNo == null)
            {
                _Result = null;
                _DataResult = null;
            }
            else
            {
                _Result = objrun.funCheckCancelTransaction(strLotNo, strWKstatus);
                if (_Result != "OK")
                {
                    _DataResult = "Data is mistake ,Please Undo Data Cancel";
                }
                else
                {
                    dt = objrun.funGetLotForCancel(strLotNo, strWKstatus);

                    if (dt.Rows.Count != 0)
                    {
                        CancelDetailData.OrderNo = dt.Rows[0]["OrderNo"].ToString();
                        CancelDetailData.ProductCateg = dt.Rows[0]["ProductCateg"].ToString();
                        CancelDetailData.InputDate = Convert.ToDateTime(dt.Rows[0]["InputDate"].ToString());
                        CancelDetailData.OutputDate = Convert.ToDateTime(dt.Rows[0]["outputDate"].ToString());
                        CancelDetailData.InputQTY = Convert.ToInt32(dt.Rows[0]["inputQty"].ToString());
                        CancelDetailData.OutputQTY = Convert.ToInt32(dt.Rows[0]["outputQty"].ToString());
                        CancelDetailData.CancelQTY = Convert.ToInt32(dt.Rows[0]["LotCancelQty"].ToString());
                        CancelDetailData.CompleteQTY = Convert.ToInt32(dt.Rows[0]["LotCompQty"].ToString());
                        CancelDetailData.Packunit = dt.Rows[0]["PackUnitQty"].ToString();
                        CancelDetailData.Type = dt.Rows[0]["Type"].ToString();
                        CancelDetailData.ProductCode = dt.Rows[0]["ProductCode"].ToString();

                        int OrigQty = Convert.ToInt32(dt.Rows[0]["OutputQty"].ToString()) - Convert.ToInt32(dt.Rows[0]["LotCompQty"].ToString()) + Convert.ToInt32(dt.Rows[0]["LotCancelQty"].ToString());


                        CancelDetailData.OriginalQty = OrigQty;
                        if (strWKstatus == "ReInput")
                        {
                            CancelDetailData.NeedCancelQty = Convert.ToInt32(dt.Rows[0]["OutputQty"].ToString());
                        }
                        else if (strWKstatus == "ReFlow")
                        {
                            CancelDetailData.NeedCancelQty = OrigQty;
                        }
                        else
                        {
                            CancelDetailData.NeedCancelQty = 0;
                        }
                        CancelDetailData.RemainQty = CancelDetailData.OriginalQty - CancelDetailData.NeedCancelQty;
                        if ((strWKstatus == "ReInput") && (OrigQty != Convert.ToInt32(dt.Rows[0]["OutputQty"].ToString())))
                        {
                            _DataResult = "Can not Reinput. Because some qty was canceled or completed.";
                            _ResultLabel = false;
                        }
                        else
                        {
                            _DataResult = "Lot condition is shown.";
                        }


                    }
                    else
                    {
                        _Result = "warning";
                        _DataResult = "Lot is not found.";
                    }
                }
            }

            return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = CancelDetailData }, JsonRequestBehavior.AllowGet);
        }
        //GetData by Order No
        public JsonResult funCancelOrderNoUpdate(string strLotNo, string strWKstatus)
        {
            dt = new DataTable();
            LotCancelDetail CancelDetailData = new LotCancelDetail();
            string _Result = "";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            if (strLotNo == "" || strLotNo == null)
            {
                _Result = null;
                _DataResult = null;

            }
            else
            {
                dt = objrun.funGetLotForCancel(strLotNo, strWKstatus);
                if (dt == null|| dt.Rows.Count == 0)
                {
                    dt = objrun.funGetOrderTransectionForCancel(strLotNo);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        _DataResult = "1.Order is not found in Order Master.";
                        _ResultLabel = false;
                    }
                    else
                    {
                        CancelDetailData.OrderNo = dt.Rows[0]["OrderNo"].ToString();
                        CancelDetailData.ProductCateg = dt.Rows[0]["ProductCateg"].ToString();
                        CancelDetailData.InputDate = Convert.ToDateTime(dt.Rows[0]["InputDate"].ToString());
                        CancelDetailData.OutputDate = Convert.ToDateTime(dt.Rows[0]["outputDate"].ToString());
                        CancelDetailData.OutputQTY = Convert.ToInt32(dt.Rows[0]["OrderQty"].ToString());
                        CancelDetailData.CancelQTY = Convert.ToInt32(dt.Rows[0]["LotCancelQty"].ToString());
                        CancelDetailData.OriginalQty = Convert.ToInt32(dt.Rows[0]["LotCancelQty"].ToString());
                        CancelDetailData.CompleteQTY = Convert.ToInt32(dt.Rows[0]["OrderQty"].ToString());
                        CancelDetailData.Packunit = dt.Rows[0]["PackUnitQty"].ToString();
                        CancelDetailData.Type = dt.Rows[0]["Type"].ToString();
                        CancelDetailData.ProductCode = dt.Rows[0]["ProductCode"].ToString();

                        CancelDetailData.NeedCancelQty = CancelDetailData.CompleteQTY;
                        CancelDetailData.RemainQty = CancelDetailData.OriginalQty - CancelDetailData.NeedCancelQty;
                        if (CancelDetailData.NeedCancelQty > 0)
                        {
                            _DataResult = "Order condition is shown.";
                            _ResultLabel = true;
                        }
                        else
                        {
                            _DataResult = "Nothing remain qty to cancel. Cancel disable.";
                            _ResultLabel = false;
                        }
                        _Result = "OK";
                    }
                }
                else
                {
                    CancelDetailData.OrderNo = dt.Rows[0]["OrderNo"].ToString();
                    CancelDetailData.Type = dt.Rows[0]["Type"].ToString();
                    CancelDetailData.InputDate = Convert.ToDateTime(dt.Rows[0]["InputDate"].ToString());
                    CancelDetailData.OutputDate = Convert.ToDateTime(dt.Rows[0]["outputDate"].ToString());
                    CancelDetailData.ProductCode = dt.Rows[0]["ProductCode"].ToString();
                    CancelDetailData.OutputQTY = Convert.ToInt32(dt.Rows[0]["OrderQty"].ToString());
                    CancelDetailData.CancelQTY = Convert.ToInt32(dt.Rows[0]["InstructionQty"].ToString());
                    CancelDetailData.CompleteQTY = Convert.ToInt32(dt.Rows[0]["CancelQty"].ToString());
                    CancelDetailData.OriginalQty = Convert.ToInt32(dt.Rows[0]["OrderQty"].ToString()) - Convert.ToInt32(dt.Rows[0]["CancelQty"].ToString());
                    CancelDetailData.NeedCancelQty = Convert.ToInt32(dt.Rows[0]["OrderQty"].ToString()) - Convert.ToInt32(dt.Rows[0]["InstructionQty"].ToString()) - Convert.ToInt32(dt.Rows[0]["CancelQty"].ToString());
                    CancelDetailData.RemainQty = CancelDetailData.OriginalQty - CancelDetailData.NeedCancelQty;
                    if (CancelDetailData.NeedCancelQty > 0)
                    {
                        _DataResult = "Order condition is shown.";
                        _ResultLabel = true;
                    }
                    else
                    {
                        _DataResult = "Nothing remain qty to cancel. Cancel disable.";
                        _ResultLabel = false;
                    }
                    _Result = "OK";
                }
            }

            return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = CancelDetailData }, JsonRequestBehavior.AllowGet);
        }
        //Detail data from control to Datatables
        public JsonResult TemporaryCondition(LotCancelTable dtTemp, string strWKstatus)
        {
            
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            if (strWKstatus != "Order")
            {

                if (dtTemp.LotNo == "" || dtTemp.LotNo == null)
                {
                    _DataResult = "Please enter LotNo.";
                    _ResultLabel = false;

                    return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel  }, JsonRequestBehavior.AllowGet);

                }
                if (Convert.ToInt32(dtTemp.RemainQty) < 0)
                {
                    _DataResult = "Qty Illegal, Disable to cancel.";
                    _ResultLabel = false;
                     
                    return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (dtTemp.OrderNo == "" || dtTemp.OrderNo == null)
            {
                _DataResult = "Please enter LotNo.";
                _ResultLabel = false;
                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel }, JsonRequestBehavior.AllowGet);
            } 
            if(strWKstatus == "Lot")
            {
                if(dtTemp.ProductCateg != "22" && dtTemp.ProductCateg != "23")
                {
                    if(Convert.ToInt32(dtTemp.NeedCancelQty) % Convert.ToInt32(dtTemp.Packunit) != 0)
                    { 
                        _DataResult = "Qty is not Change.";
                        _ResultLabel = false;
                        _Result = "Error";
                        return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel }, JsonRequestBehavior.AllowGet);
                    }
                }
            } 
            if(dtTemp.NeedCancelQty == 0 )
            {
                _DataResult = "Cancel Qty should be more than 0.";
                _ResultLabel = false;
                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel }, JsonRequestBehavior.AllowGet); 
            }else if( dtTemp.NeedCancelQty > dtTemp.OriginalQty)
            {
                _DataResult = "Must be less than CurrentQty in positive";
                _ResultLabel = false;
                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel }, JsonRequestBehavior.AllowGet);
            }
            if( dtTemp.CancelCode == null || dtTemp.CancelCode.Length == 0)
            {
                _DataResult = "Please select CancelCode.";
                _ResultLabel = false;
                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel }, JsonRequestBehavior.AllowGet);
            }
            if (  dtTemp.Treatment == null || dtTemp.Treatment.Length == 0 )
            {
                _DataResult = "Please select Treatment.";
                _ResultLabel = false;
                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel }, JsonRequestBehavior.AllowGet);
            } 
             
            List<OrderCancleTable> dtTemp2 = Session["model"] as List<OrderCancleTable>;
            if (dtTemp2 == null)
            {
                dtTemp2 = new List<OrderCancleTable>();
            }
            List<LotCancelTable> chkDataExist = dtTemp2.Where(m => m.LotNo == dtTemp.LotNo).Select(m => new LotCancelTable { LotNo = m.LotNo }).ToList();

            if (chkDataExist.Count > 0)
            { 

                if (strWKstatus != "Order")
                {
                    _DataResult = "LotNo";
                }
                else
                {
                    _DataResult = "OrderNo";
                }

                _DataResult += " has Exist !!!";
                _ResultLabel = false;
                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel }, JsonRequestBehavior.AllowGet);
            }
            else
            {


                //dtTemp2.Add(new OrderCancleTable()
                //{
                //    LotNo = dtTemp.LotNo,
                //    OrderNo = dtTemp.OrderNo,
                //    InputDate =  dtTemp.InputDate,
                //    OutputDate = dtTemp.OutputDate,
                //    InputQTY = dtTemp.InputQTY,
                //    OutputQTY = dtTemp.OutputQTY,
                //    CancelQTY = dtTemp.CancelQTY,
                //    CompleteQTY = dtTemp.CompleteQTY,
                //    ProductCateg = dtTemp.ProductCateg,
                //    Packunit = dtTemp.Packunit,
                //    Type = dtTemp.Type,
                //    ProductCode = dtTemp.ProductCode,
                //    OriginalQty = dtTemp.OriginalQty,
                //    NeedCancelQty = dtTemp.NeedCancelQty,
                //    RemainQty = dtTemp.RemainQty,
                //    CancelCode = dtTemp.CancelCode,
                //    Treatment = dtTemp.Treatment,
                //    RespProcess = dtTemp.RespProcess,
                //    RespMachine = dtTemp.RespMachine,
                //    OpeDate = dtTemp.OpeDate
                //}); 





                dtTemp2.Add(new OrderCancleTable()
                {
                    CancelBy = strWKstatus,
                    OrderNo = dtTemp.OrderNo,
                    LotNo = dtTemp.LotNo == null ? "" : dtTemp.LotNo,
                    Type = dtTemp.Type,
                    ProductCode = dtTemp.ProductCode,
                    CancelQty = dtTemp.NeedCancelQty,
                    RBCtgr = "0",
                    CancelCode = dtTemp.CancelCode,
                    treatmentCode = dtTemp.Treatment,
                    RespProcess = dtTemp.RespProcess,
                    RespMachine = dtTemp.RespMachine,
                    RespOpeDate = dtTemp.OpeDate
                });

                Session["model"] = dtTemp2;

                _DataResult = "";
                //_ResultLabel = false;


                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);
            }
        }
        //Update Lot / Order Cancel 
        public JsonResult CancelEntry()
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            DataSet dsCancel = new DataSet();
            DataTable dtCancel = new DataTable();
            List<OrderCancleTable> dtTemp2 = Session["model"] as List<OrderCancleTable>;
            //dtCancel = Session["model"] as DataTable;
            if (dtTemp2 == null || dtTemp2.Count == 0)
            {
                _DataResult = "Data cannot found to Cancel !!!";
                _Result = "Error";
                _ResultLabel = false;
                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);
            }
            string  UserName = Session["USERNAME"] as string; 
            string[] BResult = objrun.PICancel(dtTemp2, UserName); 

           if(BResult[0] == "True" && BResult[1]=="OK")
            {
                _DataResult = "Data Cancel Successful";
                Session["model"] = null;
            }
            else
            {
                _Result = "Error";
                _ResultLabel = Convert.ToBoolean(BResult[0]);
                _DataResult = BResult[1];

            }


            return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);
        }
        //GetDetail of data cancel from database to control
        public JsonResult GetUndoData(string strLotNo, string strWKstatus )
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            DataSet dsCancel = new DataSet();
            DataTable dtCancel = new DataTable();
            List<UndoDetailTable> dtTemp2 = new List<UndoDetailTable>();


            dtTemp2 = objrun.funUndoCancel(strLotNo, strWKstatus);

            if (dtTemp2.Count == 0)
            {
                _ResultLabel = false;
                _DataResult = strWKstatus + " is not found in Cancel History.";
            }
            else
            {
                _DataResult = strWKstatus + " cancel history is shown.";
            }


            return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);
        }
        //Update Lot / Order Undo Cancel 
        public JsonResult GetUndoDataEntry(string strLotNo, string strWKstatus, UndoDetailTable DataUndo)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            DataSet dsCancel = new DataSet();
            DataTable dtCancel = new DataTable();
            List<UndoDetailTable> dtTemp2 = new List<UndoDetailTable>();

            if(strLotNo == "")
            {
                _DataResult = "Please enter LotNo or OrderNo. ";
                _Result = "warning";
                _ResultLabel = false; 
                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);
            }
            if(DataUndo.OrderNo == null || DataUndo.OrderNo == "")
            {
                _DataResult = "Nothing data to Undo. ";
                _Result = "warning";
                _ResultLabel = false;
                return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);
            }
            string UserName = Session["USERNAME"] as string;
            string[] BResult = objrun.PICancelTransaction(DataUndo, UserName);
            if (BResult[0] == "True" && BResult[1] == "OK")
            {
                _DataResult = "Undo Cancel entry completed.";
                Session["model"] = null;
            }
            else
            {
                _Result = "Error";
                _ResultLabel = Convert.ToBoolean(BResult[0]);
                _DataResult = BResult[1];

            }


            return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
        #region PICancel inquiry

        // GetLine to dropdownList
        public JsonResult GetLine()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetLine();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "SELECT LINE",
                    Value = ""
                });
                string strText = "";
                foreach (DataRow row in dt.Rows)
                {
                    //strText = row["SLine"].ToString().Trim() + " | " + row["CancelReason"].ToString().Trim();
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["ALine"].ToString().Trim(),
                        Value = row["ALine"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }
        // Get Type to dropdownList
        public JsonResult GetType()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetType();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "SELECT TYPE",
                    Value = ""
                });
                string strText = "";
                foreach (DataRow row in dt.Rows)
                {
                    //strText = row["SLine"].ToString().Trim() + " | " + row["CancelReason"].ToString().Trim();
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["Type"].ToString().Trim(),
                        Value = row["Type"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }
        // Get ProductCode to dropdownList
        public JsonResult GetProductCode()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetProductCode();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "SELECT PRODUCTCODE",
                    Value = ""
                });
                string strText = "";
                foreach (DataRow row in dt.Rows)
                {
                    //strText = row["SLine"].ToString().Trim() + " | " + row["CancelReason"].ToString().Trim();
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["ProductCode"].ToString().Trim(),
                        Value = row["ProductCode"].ToString().Trim(),

                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }
        // Get Cancelby to dropdownList
        public JsonResult GetCancelBy()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            dt = new DataTable();
            dt = objrun.GetCancelBy();
            if (dt.Rows.Count != 0)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "SELECT CANCEl BY",
                    Value = ""
                });
                string strText = "";
                foreach (DataRow row in dt.Rows)
                { 
                    listItems.Add(new SelectListItem()
                    {
                        Text = row["CancelBy"].ToString().Trim(),
                        Value = row["CancelBy"].ToString().Trim(), 
                    });
                }
            }
            return Json(new MultiSelectList(listItems, "Value", "Text"));
        }
        //Get Data PICancel Detail to datatables 
        public JsonResult GetPICancelInquiry(PicancelInquiryControl Data)
        {
            string _Result = "OK";
            string _DataResult = "";
            Boolean _ResultLabel = true;
            DataSet dsCancel = new DataSet();
            DataTable dtCancel = new DataTable();
            List<PICancelInquiryData> dtTemp2 = new List<PICancelInquiryData>();


            dtTemp2 = objrun.GetPICancelInquiries(Data);
            if (dtTemp2[0].OrderNo.ToString() == "Error")
            {
                _Result = dtTemp2[0].OrderNo.ToString();
                _DataResult = dtTemp2[0].LotNo.ToString();
                _ResultLabel = false;
            }
            else
            {
                if (dtTemp2.Count() > 0)
                {
                    _DataResult = "Get Data Successful.";
                    Session["PIInquiry"] = Data;
                }
                else
                {
                    _ResultLabel = false;
                    _Result = "warning";
                    _DataResult = "Data Not Found ! !";
                }

            }
            // return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);


            var jsonResult = Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = dtTemp2 }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #region Data PiInquiry Export Excel
       
        public ActionResult GetDataExportToExcel() 
        {
            try
            { 
                PicancelInquiryControl Data = Session["PIInquiry"] as PicancelInquiryControl;
                DataSet ds = getDataSetExportToExcel(Data);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= PI_Cancel_Inquiry.xls");
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
        public DataSet getDataSetExportToExcel(PicancelInquiryControl Data)
        {
            DataSet ds = new DataSet();
            

            ds = objrun.GetdsInquiry(Data);

            int CountDs = ds.Tables.Count;
            var Checking = 0;
            string[] strNameSheets = { "PI_Cancel_Inquiry" };
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
        
    }
}