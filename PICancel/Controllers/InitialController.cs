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
using System.Text;

namespace PICancel.Controllers
{
    public class InitialController : Controller
    {
        MgrCondition objrun = new MgrCondition();
        DataTable dt;
        DataSet ds;
        // GET: Initial
        public JsonResult GetManuSide(string Event)
        {
            string _Result = "OK";
            string _DataResult = "";
            string [] strHTML ;
            Boolean _ResultLabel = true;
            DataSet dsCancel = new DataSet();
            DataTable dtCancel = new DataTable();
            try
            {
                dt = new DataTable();
                initial OPResult = new initial();
                string OPType = Session["OPType"] as string;


                OPResult = objrun.GetMenuSide(OPType);

                if (OPResult.strResult == "true")
                { 
                    strHTML =    GetHTMLMenuSide(OPResult.dtResult);
                    _ResultLabel = Convert.ToBoolean(OPResult.strResult);
                    _DataResult = OPResult.strErrMsg;   
                }
                else
                {
                    strHTML = null;
                    _Result = "Warning";
                    _ResultLabel = Convert.ToBoolean(OPResult.strResult);
                    _DataResult = OPResult.strErrMsg;
                }




            }
            catch (Exception e)
            {
                strHTML = null;
                _Result = "error";
                _ResultLabel = false;
                _DataResult = e.Message;
            }

            return Json(new { strResult = _Result, dataLabel = _DataResult, strboolbel = _ResultLabel, data = strHTML }, JsonRequestBehavior.AllowGet);
        }

        public string [] GetHTMLMenuSide(List<ListPICancelMenu> dt)
        {

            int i = 0;
            
            string[]  arrHTMl;
            string  strHTML = "";
            List<string> stringList = new List<string>(); 
            foreach (ListPICancelMenu tbs in dt)
            { 
                //strHTML = "";

                i++;
                int j = 0;

                foreach (PICancelMenu items in tbs.dtResult)
                {
                    j++;

                    if (items.ManuCateg == "subMenutitle")
                    {
                        strHTML += "<li class=_nav-item_><a class=_nav-link collapsed_ href=_#_ data-toggle=_collapse_ data-target=_#collapse"+i+ "_ aria-expanded=_true_ aria-controls=_collapse" + i + "_><i class=_fas fa-bars_></i><span>" + items.ManuName + "</span></a>";
                        strHTML += "<div id=_collapse" + i + "_ class=_collapse_ aria-labelledby=_headingTwo_ data-parent=_#accordionSidebar_>";
                        strHTML += "<div class=_bg-white py-2 collapse-inner_ > ";
                    //}

                    //if (items.ManuCateg == "subMenutitle")
                    //{
                        strHTML += "<h6 class=_collapse-header  bg-info text-white mb-1_>" + items.ManuName + "</h6> ";
                    }
                    else
                   if (items.ManuCateg == "SystemManu")
                    {
                        strHTML += " <a href=_../" + items.Controller + "/" + items.Action + "?Event=" + items.Param + "_class=_collapse-item_>" + items.ManuName + "</a>  ";
                    } 

                    if ( j == tbs.dtResult.Count)
                    {
                        strHTML += " </div></div></li> ";
                    }







                   
                } 
               

               
            }
            var sb = new StringBuilder(strHTML.Replace('_', '"'));
            string text = sb.ToString();
            stringList.Add(text);

            arrHTMl = stringList.ToArray();
                return arrHTMl;
        } 
        
       
    }
}