using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using PICancel.Models;
using System.Web.Mvc;
using System.Configuration;
//using MCMonitoring.Models;

namespace PICancel.Conn
{
    public class MgrCondition
    {
        Connection objRun = new Connection();
        DataTable dt;
        DataSet ds;
        string conTRPI = ConfigurationManager.ConnectionStrings["PIDbContext1"].ConnectionString;
        string conTRPICancel = ConfigurationManager.ConnectionStrings["OGIDbContext"].ConnectionString;
        #region Common
        //Login
        public OPeratorLogin GetOperator(string OPID, string Password)
        {
            dt = new DataTable();
            OperatorModel OPDetail = new OperatorModel();
            OPeratorLogin OPResult = new OPeratorLogin();
            string SQL;
            try
            {

                SQL = "SELECT * FROM [TRPICancel].[dbo].[vewOperator] ";
                SQL += "WHERE OPCode ='" + OPID + "' ";
                SQL += "AND OPPassword = '" + Password + "';";
                dt = objRun.GetDatatables(SQL, conTRPI);

                if (dt.Rows.Count == 0)
                {
                    OPResult.strResult = true;
                    OPResult.OPeratordetail = null;
                }
                else
                {
                    OPDetail.OPID = dt.Rows[0]["OPCode"].ToString();
                    OPDetail.Password = dt.Rows[0]["OPPassword"].ToString();
                    OPDetail.OPName = dt.Rows[0]["OPName"].ToString();
                    OPDetail.OPLevel = dt.Rows[0]["OPLevel"].ToString();
                    OPDetail.OPType = dt.Rows[0]["OPType"].ToString();
                    OPDetail.AuthorityLV = dt.Rows[0]["AuthorityLV"].ToString();
                    OPResult.strResult = true;
                    OPResult.OPeratordetail = OPDetail;
                }
            }
            catch (Exception e)
            {
                OPResult.strResult = false;
                OPDetail.OPID = e.Message;
                OPResult.OPeratordetail = OPDetail;
            }

            return OPResult;
        }
        //MenuSide
        public initial GetMenuSide(string OPType)
        {
            dt = new DataTable();
            initial OPDetail = new initial();
            List<ListPICancelMenu> ListOPResult = new List<ListPICancelMenu>();
            List<PICancelMenu> OPResult = new List<PICancelMenu>();
            string SQL;
            try
            {





                SQL = "exec [TRPICancel].[dbo].[sprzGETPIMenu] '" + OPType + "';";


                ds = objRun.GetDataSet(SQL, conTRPI);
                OPDetail.strResult = "true";
                OPDetail.strErrMsg = "OK";
                if (ds.Tables.Count != 0)
                {

                    int CountDs = ds.Tables.Count;
                    var Checking = 0;
                    string[] strNameSheets = { "MachineSupplement" };
                    for (int i = 0; i < CountDs; i++)
                    {
                        OPResult = new List<PICancelMenu>();
                        foreach (DataRow row in ds.Tables[i].Rows)
                        {




                            OPResult.Add(new PICancelMenu()
                            {
                                SystemID = row["SystemID"].ToString().Trim(),
                                SystemName = row["SystemName"].ToString().Trim(),
                                ManuID = row["ManuID"].ToString().Trim(),
                                ManuName = row["ManuName"].ToString().Trim(),
                                ManuCateg = row["ManuCateg"].ToString().Trim(),
                                Action = row["Action"].ToString().Trim(),
                                Controller = row["Controller"].ToString().Trim(),
                                Param = row["Param"].ToString().Trim(),
                            });
                        }

                        ListOPResult.Add(new ListPICancelMenu()
                        {
                            dtResult = OPResult
                        });
                        Checking = Checking + 1;
                    }

                    OPDetail.dtResult = ListOPResult;
                }
                else
                {
                    OPDetail.dtResult = null;
                }



            }
            catch (Exception e)
            {
                OPDetail.strResult = "false";
                OPDetail.strErrMsg = e.Message;
                OPDetail.dtResult = null;
            }

            return OPDetail;
        }
        #endregion
        #region PICancel
        //CancelCode to ddl
        public DataTable GetCancelCode()
        {
            string SQL = "";

            try
            {
                SQL = "SELECT TOP (100) [CancelCode],[CancelReason]   FROM [TRPI].[dbo].[CancelCode]  order by [CancelCode]";
                dt = objRun.GetDatatables(SQL, conTRPI);

                return dt;

            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;


            }

            return dt;
        }
        public string GetCancelReason(string CancalCode)
        {
            string SQL = "";
            string strResult = "";
            try
            {
                SQL = "SELECT  Top 1 [CancelCode],[CancelReason]   FROM [TRPI].[dbo].[CancelCode]  where CancelCode = '" + CancalCode + "' order by [CancelCode]";
                dt = objRun.GetDatatables(SQL, conTRPI);

                strResult = dt.Rows[0]["CancelReason"].ToString();

                return strResult;
            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;

                return ErrMgs;
            }


        }
        //Treatment to ddl
        public DataTable GetTreatmentCode()
        {
            string SQL = "";

            try
            {
                SQL = "SELECT TOP (100) [TreatmentCode] ,[TreatmentName]  FROM [TRPI].[dbo].[TreatmentCode] order by [TreatmentCode] asc";
                dt = objRun.GetDatatables(SQL, conTRPI);

                return dt;

            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;


            }

            return dt;
        }
        public string GetTreatmentName(string TreatmentCode)
        {
            string SQL = "";
            string strResult = "";
            try
            {
                SQL = "SELECT  Top 1 [TreatmentCode] ,[TreatmentName]  FROM [TRPI].[dbo].[TreatmentCode]  where TreatmentCode = '" + TreatmentCode + "' Order By [TreatmentCode]";
                dt = objRun.GetDatatables(SQL, conTRPI);

                strResult = dt.Rows[0]["TreatmentName"].ToString();

                return strResult;
            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;

                return ErrMgs;
            }


        }
        //Line to ddl
        public DataTable GetLine()
        {
            string SQL = "";

            try
            {
                SQL = "Select ALine From vewSType Group by ALine Order by ALine";
                dt = objRun.GetDatatables(SQL, conTRPI);

                return dt;

            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;


            }

            return dt;
        }
        //Type to ddl
        public DataTable GetType()
        {
            string SQL = "";

            try
            {
                SQL = "Select Type From vewSType Group by Type Order by Type";
                dt = objRun.GetDatatables(SQL, conTRPI);

                return dt;

            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;


            }

            return dt;
        }
        //ProductCode to ddl
        public DataTable GetProductCode()
        {
            string SQL = "";

            try
            {
                SQL = "Select ProductCode From vewProduct Group by ProductCode Order by ProductCode";
                dt = objRun.GetDatatables(SQL, conTRPI);

                return dt;

            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;


            }

            return dt;
        }
        //CancelBy to ddl
        public DataTable GetCancelBy()
        {
            string SQL = "";
            try
            {
                SQL = "Select CancelBy From vewCancelBy Order by CancelBy";
                dt = objRun.GetDatatables(SQL, conTRPI);
                return dt;
            }
            catch (Exception e)
            {
                string ErrMgs;
                ErrMgs = e.Message;
            }
            return dt;
        }
        //for PIcancel
        public string funCheckCancelTransaction(string strLotNo, string strPrevCtrl)
        {
            string SQL = "";
            string strResult = "";
            try
            {
                SQL = " Exec sprCheckCancelTransaction '" + strLotNo + "' ,'" + strPrevCtrl + "'";
                dt = objRun.GetDatatables(SQL, conTRPI);

                strResult = dt.Rows[0][0].ToString();

                return strResult;
            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;

                strResult = ErrMgs;
                return strResult;

            }
        }
        public DataTable funGetLotForCancel(string strLotNo, string strWKstatus)
        {
            string SQL = "";
            if (strWKstatus == "Order")
            {
                SQL = "SELECT * FROM vewOrderForCancel Where OrderNo = '" + strLotNo + "'";
            }
            else
            {
                SQL = "SELECT * FROM vewLotForCancel Where LotNo = '" + strLotNo + "'";
            }

            dt = objRun.GetDatatables(SQL, conTRPI);

            return dt;
        }
        public DataTable funGetOrderTransectionForCancel(string strLotNo)
        {
            string SQL = "";
            SQL = "SELECT * FROM [TRPI].[dbo].[vewCheckOrderTransection] Where OrderNo = '" + strLotNo + "'";
            dt = objRun.GetDatatables(SQL, conTRPI);
            return dt;
        }
        //
        public List<LotCancelDetail> AddDataDetailToTempTable(LotCancelDetail dtTemp)
        {
            List<LotCancelDetail> dtTemp1 = new List<LotCancelDetail>();

            dtTemp1.Add(new LotCancelDetail()
            {
                LotNo = dtTemp.LotNo,
                OrderNo = dtTemp.OrderNo,
                InputDate = dtTemp.InputDate,
                OutputDate = dtTemp.OutputDate,
                InputQTY = dtTemp.InputQTY,
                OutputQTY = dtTemp.OutputQTY,
                CancelQTY = dtTemp.CancelQTY,
                CompleteQTY = dtTemp.CompleteQTY,
                Type = dtTemp.Type,
                ProductCode = dtTemp.ProductCode,
                OriginalQty = dtTemp.OriginalQty,
                NeedCancelQty = dtTemp.NeedCancelQty,
                RemainQty = dtTemp.RemainQty,
                CancelCode = dtTemp.CancelCode,
                Treatment = dtTemp.Treatment,
                RespProcess = dtTemp.RespProcess,
                RespMachine = dtTemp.RespMachine,
                OpeDate = dtTemp.OpeDate
            });



            return dtTemp1;
        }
        //PICancel Entry
        public string[] PICancel(List<OrderCancleTable> Datatemp, string prmOPname)
        {
            string Result = "true";
            string[] Results;
            Boolean BoolResults;
            string SQL = "";
            string _strResult;
            string status = "PICancel";
            try
            {
                Datatemp.ForEach(delegate (OrderCancleTable i)
                {
                    SQL += "Exec sprCancelEntry '";
                    SQL += i.CancelBy.Trim() + "', '";
                    SQL += i.OrderNo.Trim() + "', '";
                    SQL += i.CancelQty + "', '";
                    SQL += i.RBCtgr.Trim() + "', '";
                    SQL += i.CancelCode.Trim() + "', '";
                    SQL += i.treatmentCode.Trim() + "', '";
                    SQL += i.LotNo.Trim() + "', '";
                    SQL += (i.RespProcess == null ? "" : i.RespProcess.Trim()) + "', '";
                    SQL += (i.RespMachine == null ? "" : i.RespMachine.Trim()) + "', '";
                    SQL += Convert.ToDateTime(i.RespOpeDate).ToString("yyyyMMdd") + "', '";
                    SQL += prmOPname.Trim() + "', '";
                    SQL += status.Trim() + "';";
                });

                //BoolResults = objRun.GetDataExcute(SQL);
                Results = objRun.GetDataExcute(SQL, conTRPI);
                //_strResult = BoolResults==true ?"OK":"Bad";
            }
            catch (Exception e)
            {
                _strResult = e.Message;
                Result = "false";
                Results = new string[] { Result, _strResult };
            }

            return Results;


        }
        //Getdata detail for Undo
        public List<UndoDetailTable> funUndoCancel(string strLotNo, string strWKstatus)
        {
            List<UndoDetailTable> DataResult = new List<UndoDetailTable>();
            string SQL = "";
            try
            {
                SQL += "Exec sprSelectUndoCancelTransaction  '" + strLotNo + "','" + strWKstatus + "'";
                dt = objRun.GetDatatables(SQL, conTRPI);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DataResult.Add(new UndoDetailTable()
                        {
                            CancelBy = row["CancelBy"].ToString().Trim(),
                            LotNo = row["LotNo"].ToString().Trim(),
                            OrderNo = row["OrderNo"].ToString().Trim(),
                            ProdFamily = row["ProdFamily"].ToString().Trim(),
                            Type = row["Type"].ToString().Trim(),
                            ProductCode = row["ProductCode"].ToString().Trim(),
                            CancelQty = Convert.ToInt32(row["CancelQty"].ToString().Trim()),
                            CancelCode = row["CancelCode"].ToString().Trim(),
                            CancelReason = row["CancelReason"].ToString().Trim(),
                            TreatmentCode = row["TreatmentCode"].ToString().Trim(),
                            TreatmentName = row["TreatmentName"].ToString().Trim(),
                            RespProcess = row["RespProcess"].ToString().Trim(),
                            RespMachine = row["RespMachine"].ToString().Trim(),
                            RespOpeDate = Convert.ToDateTime(row["RespOpeDate"].ToString().Trim()).ToString("dd-MMM-yyyy"),
                            SentCateg = row["SentCateg"].ToString().Trim(),
                            UpdDate = Convert.ToDateTime(row["UpdDate"].ToString().Trim()).ToString("dd-MMM-yyyy"),
                            Operator = row["Operator"].ToString().Trim(),
                            OperatorName = row["OperatorName"].ToString().Trim(),
                            CancelSeq = row["CancelSeq"].ToString().Trim(),
                            RBCTGR = row["RBCTGR"].ToString().Trim(),
                        });





                    }
                }


            }
            catch (Exception e)
            {

            }



            return DataResult;
        }
        //PICancel Undo
        public string[] PICancelTransaction(UndoDetailTable Datatemp, string prmOPname)
        {
            string Result = "true";
            string[] Results;
            Boolean BoolResults;
            string SQL = "";
            string _strResult;
            string status = "PICancel";
            try
            {
                SQL += "Exec sprCancelEntry '";
                SQL += Datatemp.CancelBy.Trim() + "', '";
                SQL += Datatemp.OrderNo.Trim() + "', '";
                SQL += Datatemp.CancelQty + "', '";
                SQL += "1', '";
                SQL += Datatemp.CancelCode.Trim() + "', '";
                SQL += Datatemp.TreatmentCode.Trim() + "', '";
                SQL += Datatemp.LotNo + "', '";
                SQL += (Datatemp.RespProcess == null ? "" : Datatemp.RespProcess.Trim()) + "', '";
                SQL += (Datatemp.RespMachine == null ? "" : Datatemp.RespMachine.Trim()) + "', '";
                SQL += Convert.ToDateTime(Datatemp.RespOpeDate).ToString("yyyyMMdd") + "', '";
                SQL += prmOPname.Trim() + "', '";
                SQL += status.Trim() + "';";

                Results = objRun.GetDataExcute(SQL, conTRPI);

            }
            catch (Exception e)
            {
                _strResult = e.Message;
                Result = "false";
                Results = new string[] { Result, _strResult };
            }

            return Results;


        }
        #endregion
        #region PICancel Inquiry
        //PICancel Inquiry
        public List<PICancelInquiryData> GetPICancelInquiries(PicancelInquiryControl _Data)
        {
            List<PICancelInquiryData> DataResult = new List<PICancelInquiryData>();


            ds = new DataSet();
            try
            {
                ds = GetdsInquiry(_Data);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataResult.Add(new PICancelInquiryData()
                        {
                            CancelDate = row["CancelDate"].ToString().Trim(),
                            CancelBy = row["CancelBy"].ToString().Trim(),
                            LotNo = row["LotNo"].ToString().Trim(),
                            CancelQty = row["CancelQty"].ToString().Trim(),
                            OrderNo = row["OrderNo"].ToString().Trim(),
                            Type = row["Type"].ToString().Trim(),
                            SLine = row["SLine"].ToString().Trim(),
                            ALine = row["ALine"].ToString().Trim(),
                            ProductCode = row["ProductCode"].ToString().Trim(),
                            CancelCode = row["CancelCode"].ToString().Trim(),
                            RBCTGR = row["RBCTGR"].ToString().Trim(),
                            CancelReason = row["CancelReason"].ToString().Trim(),
                            Operator = row["Operator"].ToString().Trim(),
                            UpdDate = row["UpdDate"].ToString().Trim(),
                            OperatorName = row["OperatorName"].ToString().Trim(),
                        });
                    }
                }
            }
            catch (Exception e)
            {
                DataResult.Add(new PICancelInquiryData()
                {
                    OrderNo = "Error",
                    LotNo = e.Message
                });
            }
            return DataResult;

        }
        public string getinquiryString(PicancelInquiryControl _Data)
        {
            string SQL = "";
            int Chkint = 0;
            SQL += "Select * From VewCancelInquiry";
            if (_Data.Lotno != "" && _Data.Lotno != null)                 //Lotno
            {
                SQL += Chkint == 0 ? " Where " : " And ";
                SQL += "LotNo = '" + _Data.Lotno + "'";
                Chkint++;
            }
            if (_Data.Line != "" && _Data.Line != null)             //Line
            {
                SQL += Chkint == 0 ? " Where " : " And ";
                SQL += "ALine = '" + _Data.Line + "'";
                Chkint++;
            }
            if (_Data.type != "" && _Data.type != null)             //type
            {
                SQL += Chkint == 0 ? " Where " : " And ";
                SQL += "Type = '" + _Data.type + "'";
                Chkint++;
            }
            if (_Data.ProductCode != "" && _Data.ProductCode != null)//ProductCode
            {
                SQL += Chkint == 0 ? " Where " : " And ";
                SQL += "ProductCode = '" + _Data.ProductCode + "'";
                Chkint++;
            }
            if (_Data.StartTime != null && _Data.EndTime != null)    //StartTime - EndTime
            {
                SQL += Chkint == 0 ? " Where " : " And ";
                SQL += "CancelDate BETWEEN '" + Convert.ToDateTime(_Data.StartTime).ToString("yyyyMMdd") + "'AND '" + Convert.ToDateTime(_Data.EndTime).ToString("yyyyMMdd") + "'";
                Chkint++;
            }
            else if (_Data.StartTime != null && _Data.EndTime == null)   //StartTime
            {
                SQL += Chkint == 0 ? " Where " : " And ";
                SQL += "CancelDate >= '" + Convert.ToDateTime(_Data.StartTime).ToString("yyyyMMdd") + "'";
                Chkint++;
            }
            else if (_Data.StartTime == null && _Data.EndTime != null)      //EndTime
            {
                SQL += Chkint == 0 ? " Where " : " And ";
                SQL += "CancelDate <='" + Convert.ToDateTime(_Data.EndTime).ToString("yyyyMMdd") + "'";
                Chkint++;
            }
            if (_Data.CancelBy != "" && _Data.CancelBy != null)     //CancelBy
            {
                SQL += Chkint == 0 ? " Where " : " And ";
                SQL += "Cancelby = '" + _Data.CancelBy + "'";
                Chkint++;
            }
            if (_Data.cancelCode != "" && _Data.cancelCode != null)//cancelCode
            {
                SQL += Chkint == 0 ? " Where " : " And ";
                SQL += "cancelCode = '" + _Data.cancelCode + "'";
                Chkint++;
            }
            return SQL;
        }
        public DataSet GetdsInquiry(PicancelInquiryControl _Data)
        {
            ds = new DataSet();
            ds = objRun.GetDataSet(getinquiryString(_Data), conTRPI);
            return ds;
        }
        #endregion
         

        #region OGIcancelOrders

        #region OGI initial Data

        public DataTable GetOGICancelCode()
        {
            string SQL = "";
            try
            {
                SQL = "SELECT [CancelCode]      ,[CancelReason]      ,[CancelList]  FROM[TRPICancel].[dbo].[vewCancelReason]";
                dt = objRun.GetDatatables(SQL, conTRPICancel);

                return dt;

            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;


            }

            return dt;
        }

        public DataTable GetOGICancelOperator()
        {
            string SQL = "";
            try
            {
                SQL = "SELECT  [OperatorName]  FROM[TRPICancel].[dbo].[vewCancelTransactionTransOperator]";
                dt = objRun.GetDatatables(SQL, conTRPICancel);

                return dt;

            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;


            }

            return dt;
        }

        public DataTable GetOGIType()
        {
            string SQL = "";
            try
            {
                SQL = "SELECT [Type]  FROM [TRPICancel].[dbo].[vewCancelTransactionTransType]";
                dt = objRun.GetDatatables(SQL, conTRPICancel);

                return dt;

            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;


            }

            return dt;
        }

        public DataTable GetStatusOGI()
        {
            string SQL = "";
            try
            {
                SQL = "SELECT   [StatusID] ,[StatusName]  FROM [TRPICancel].[dbo].[OGICancelStatus] Order by [StatusName] asc";
                dt = objRun.GetDatatables(SQL, conTRPICancel);

                return dt;

            }
            catch (Exception e)
            {
                string ErrMgs;

                ErrMgs = e.Message;


            }

            return dt;
        }
        #endregion

        #region GOIInquiry
        public OGIInquiryResult GetDataOGIInquiry(OGIInquiryCondition DataCondition)
        {
            dt = new DataTable();
            List<OGICancelInquiryModel> dtTemp2 = new List<OGICancelInquiryModel>();
            OGIInquiryResult dataResult = new OGIInquiryResult();


            try
            {

                ds = objRun.GetDataSet(getOGIinquiryString(DataCondition), conTRPICancel);

                if (ds.Tables[0].Rows.Count != 0)
                {

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dtTemp2.Add(new OGICancelInquiryModel()
                        {
                            No = row["CancelSeq"].ToString().Trim(),
                            LotNo = row["LotNo"].ToString().Trim(),
                            OrderNo = row["OrderNo"].ToString().Trim(),
                            ProdFamily = row["ProdFamily"].ToString().Trim(),
                            Type = row["Type"].ToString().Trim(),
                            ProductCode = row["ProductCode"].ToString().Trim(),
                            RBCTGR = row["RBCTGR"].ToString().Trim(),
                            CancelQty = row["CancelQty"].ToString().Trim(),
                            CancelCode = row["CancelCode"].ToString().Trim(),
                            CancelReason = row["CancelReason"].ToString().Trim(),
                            cancelDate = row["cancelDate"].ToString().Trim(),
                            OperatorName = row["OperatorName"].ToString().Trim(),

                        });
                    }
                    dataResult.Result = true;
                    dataResult.strResult = "Get DataSuccess!";
                    dataResult.DataList = dtTemp2;
                }
                else
                {
                    dataResult.Result = false;
                    dataResult.strResult = "Data not found";
                    dataResult.DataList = dtTemp2;
                }
            }
            catch (Exception e)
            {
                dataResult.Result = false;
                dataResult.strResult = e.Message;
                dataResult.DataList = null;
            }

            return dataResult;
        }
        public DataSet GetdsOGIInquiry(OGIInquiryCondition _Data)
        {
            ds = new DataSet();
            ds = objRun.GetDataSet(getOGIinquiryString(_Data), conTRPICancel);
            return ds;
        }
        public string getOGIinquiryString(OGIInquiryCondition DataCondition)
        {
            string SQL = "";
            int i = 0;
            SQL += "select  CancelSeq,LotNo,OrderNo,ProdFamily,Type,ProductCode,RBCTGR,CancelQty,CancelCode,RTRIM(CancelReason) CancelReason,cancelDate,OperatorName from [dbo].[vewCancelTransactionTransInquiry]  ";

            if (DataCondition.CancelReason != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " CancelCode ='" + DataCondition.CancelReason + "' ";
                i++;
            };
            if (DataCondition.LotNo != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " LotNo ='" + DataCondition.LotNo + "' ";
                i++;
            };
            if (DataCondition.Type != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " Type ='" + DataCondition.Type + "' ";
                i++;
            };
            if (DataCondition.OrderNo != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " OrderNo ='" + DataCondition.OrderNo + "' ";
                i++;
            };
            if (DataCondition.Status != null && DataCondition.Status != "999")
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " RBCTGR ='" + DataCondition.Status + "' ";
                i++;
            };
            if (DataCondition.StartDate != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " CancelDate >='" + DataCondition.StartDate + "' ";
                i++;
            };
            if (DataCondition.EndDate != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " CancelDate <='" + DataCondition.EndDate + "' ";
                i++;
            };
            return SQL;
        }
        #endregion

        #region OGIConfirm

        public OGIInquiryResult GetDataOGIConfirm(OGIConfirmCondition DataCondition, string AuthorityLV)
        {
            dt = new DataTable();
            List<OGICancelInquiryModel> dtTemp2 = new List<OGICancelInquiryModel>();
            OGIInquiryResult dataResult = new OGIInquiryResult();


            try
            {
                ds = objRun.GetDataSet(GetdsOGIConfirmString(DataCondition, AuthorityLV), conTRPICancel);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dtTemp2.Add(new OGICancelInquiryModel()
                        {
                            No = row["CancelSeq"].ToString().Trim(),
                            LotNo = row["LotNo"].ToString().Trim(),
                            OrderNo = row["OrderNo"].ToString().Trim(),
                            ProdFamily = row["ProdFamily"].ToString().Trim(),
                            Type = row["Type"].ToString().Trim(),
                            ProductCode = row["ProductCode"].ToString().Trim(),
                            RBCTGR = row["RBCTGR"].ToString().Trim(),
                            CancelQty = row["CancelQty"].ToString().Trim(),
                            CancelCode = row["CancelCode"].ToString().Trim(),
                            CancelReason = row["CancelReason"].ToString().Trim(),
                            cancelDate = row["cancelDate"].ToString().Trim(),
                            OperatorName = row["OperatorName"].ToString().Trim(),
                        });
                    }
                    dataResult.Result = true;
                    dataResult.strResult = "Get DataSuccess!";
                    dataResult.DataList = dtTemp2;
                }
                else
                {
                    dataResult.Result = false;
                    dataResult.strResult = "Data not found";
                    dataResult.DataList = dtTemp2;
                }
            }
            catch (Exception e)
            {
                dataResult.Result = false;
                dataResult.strResult = e.Message;
                dataResult.DataList = null;
            }

            return dataResult;
        }
        public DataSet GetdsOGIConfirm(OGIConfirmCondition _Data, string AuthorityLV)
        {
            ds = new DataSet();
            ds = objRun.GetDataSet(GetdsOGIConfirmString(_Data, AuthorityLV), conTRPICancel);
            return ds;
        }
        public string GetdsOGIConfirmString(OGIConfirmCondition DataCondition, string AuthorityLV)
        {
            string SQL = "";
            int i = 0;
            SQL += "select  CancelSeq,LotNo,OrderNo,ProdFamily,Type,ProductCode,RBCTGR,CancelQty,CancelCode,CancelReason,cancelDate,OperatorName from [dbo].[vewCancelTransactionTrans] ";

            if (DataCondition.CancelReason != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " CancelCode ='" + DataCondition.CancelReason + "' ";
                i++;
            };
            if (DataCondition.LotNo != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " LotNo ='" + DataCondition.LotNo + "' ";
                i++;
            };
            if (DataCondition.StartDate != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " CancelDate >='" + DataCondition.StartDate + "' ";
                i++;
            };
            if (DataCondition.EndDate != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " CancelDate <='" + DataCondition.EndDate + "' ";
                i++;
            };
            if (DataCondition.Operator != null)
            {
                SQL += i == 0 ? " WHERE " : " And ";
                SQL += " OperatorName ='" + DataCondition.Operator + "' ";
                i++;
            };
            SQL += i == 0 ? " WHERE " : " And ";
            SQL += " RBCTGR ='" + AuthorityLV + "' ";
            i++;
            return SQL;
        }


        public OGICancelResult CanceltransactionTrans(string[] TrnID, string AuthorityLV, string OPID)
        {

            ds = new DataSet();
            OGICancelResult dataResult = new OGICancelResult();
            string SQL = "";
            try
            {
                foreach (string i in TrnID)
                {
                    SQL = "sprCancelTransactionTrans '" + i + "','" + AuthorityLV + "','" + OPID + "'";

                    ds = objRun.GetDataSet(SQL, conTRPICancel);
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() != "0")
                    {
                        dataResult.Result = false;
                        dataResult.strResult = "Cancel Error!!";
                        dataResult.DataList = null;
                        break;
                    }
                    else
                    {
                        dataResult.Result = true;
                        dataResult.strResult = "Cancel Successful☺";
                        dataResult.DataList = ds;
                    }
                }



            }
            catch (Exception e)
            {
                dataResult.Result = false;
                dataResult.strResult = e.Message.ToString();
                dataResult.DataList = null;
            }
            return dataResult;
        }
        #endregion

        #endregion


        #region Rohm order 
        #region Rohm Order initial
        public DataTable GetNoChipErrorGrp(string _WKstatus)
        {
            string SQL = "";

            try
            {
                SQL = "select Type from vew" + _WKstatus + "ChangeRohmGrp";
                dt = objRun.GetDatatables(SQL, conTRPICancel); 
                return dt; 
            }
            catch (Exception e)
            {
                string ErrMgs; 
                ErrMgs = e.Message; 
            }

            return dt;
        }
        public RohmOrderModel GetDataDetail(RohmOrderControl dataArr,string _WKstatus)
        {
            string SQL = "";
            int Chkint = 0;
            RohmOrderModel grpListDataDetail = new RohmOrderModel();
            //List<RohmOrderError> OrderErrorList = new List<RohmOrderError>();
            List<RohmOrderNOChip> OrderNoChipList = new List<RohmOrderNOChip>();
            try
            {
                SQL = "select ROW_NUMBER() OVER(ORDER BY ORDERNo ASC) AS Rows,* from vew" + _WKstatus + "ChangeRohm";

                if (dataArr.Type != "" && dataArr.Type != null)                 //Lotno
                {
                    SQL += Chkint == 0 ? " Where " : " And ";
                    SQL += "Type = '" + dataArr.Type + "'";
                    Chkint++;
                }
                if (dataArr.OrderNo != "" && dataArr.OrderNo != null)             //Line
                {
                    SQL += Chkint == 0 ? " Where " : " And ";
                    SQL += "OrderNo = '" + dataArr.OrderNo + "'";
                    Chkint++;
                } 

                dt = objRun.GetDatatables(SQL, conTRPICancel);








                if(dt.Rows.Count != 0)
                {
                    if(_WKstatus == "NoChip")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            OrderNoChipList.Add(new RohmOrderNOChip()
                            {
                                No = row["Rows"].ToString().Trim(),
                                OrderNo = row["OrderNo"].ToString().Trim(),
                                ProductCode = row["ProductCode"].ToString().Trim(),
                                TRNo = row["TRNo"].ToString().Trim(),
                                Spec = row["Spec"].ToString().Trim(),
                                Pack = row["Pack"].ToString().Trim(),
                                hFERank = row["hFERank"].ToString().Trim(),
                                RohmLine = row["RohmLine"].ToString().Trim(),
                                Type = row["Type"].ToString().Trim(),
                                InputDate = row["InputDate"].ToString().Trim(),
                                OrderQtyOrg = row["OrderQty"].ToString().Trim(),
                                OrderQty = row["OrderQty"].ToString().Trim(),
                                Message = row["Message"].ToString().Trim(),  
                            });
                        }

                    }
                    else if(_WKstatus == "OrderError")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            OrderNoChipList.Add(new RohmOrderNOChip()
                            {
                                No = row["Rows"].ToString().Trim(),
                                OrderNo = row["OrderNo"].ToString().Trim(),
                                ProductCode = row["ProductCode"].ToString().Trim(),
                                TRNo = row["TRNo"].ToString().Trim(),
                                Spec = row["Spec"].ToString().Trim(),
                                Pack = row["Pack"].ToString().Trim(),
                                hFERank = row["hFERank"].ToString().Trim(),
                                RohmLine = row["RohmLine"].ToString().Trim(),
                                Type = row["Type"].ToString().Trim(),
                                InputDate = row["InputDate"].ToString().Trim(), 
                                OrderQty = row["OrderQty"].ToString().Trim(),
                                Message = row["Message"].ToString().Trim(),
                            });
                        }
                    } 
                } 
                if (OrderNoChipList.Count() != 0)
                {
                    grpListDataDetail.strResult = "OK";
                    grpListDataDetail.blResult = true;
                    grpListDataDetail.ListOrder = OrderNoChipList.ToList();
                }
                else
                {
                    grpListDataDetail.strResult = "Data not found";
                    grpListDataDetail.blResult = false;
                    grpListDataDetail.ListOrder = null;
                }
            }
            catch (Exception e)
            { 
                grpListDataDetail.strResult = e.Message;
                grpListDataDetail.blResult = false;
                grpListDataDetail.ListOrder = null;
            }

            return grpListDataDetail;
        }
        public DataTable GetRunningNo(string Type)
        {
            dt = new DataTable();

            string SQL = "";


            SQL += "SELECT * FROM vewChangRohmRunningNo WHERE YYYYMMDD = '"+ DateTime.Now.ToString("yyyyMMdd")+ "' AND Type = '"+Type+ "' order by RunningNo desc"; 
            dt = objRun.GetDatatables(SQL, conTRPICancel);

            return dt;
        }
        #endregion


        #region Rohm Order No Chip

        public string OrderDeleteChangeRohm(string orderNo ,string DocNo ,string strCateg, int Condition, int OrderQty,int SeqNo, string OPID)
        {
            DataTable dt = new DataTable();
            string Result = "";
            string SQL = "";

            try
            {
                SQL += " sprOrderDeleteChangRohm '"+ orderNo+ "','" + DocNo + "','" + strCateg + "','" + Condition + "','" + OrderQty + "','" + SeqNo + "','" + OPID + "';";
                dt = objRun.GetDatatables(SQL,conTRPICancel);
                
                    Result = dt.Rows.Count == 0? "1": dt.Rows[0]["Msg"].ToString();
                 
            }
            catch (Exception e)
            {
                Result = e.Message;
            }

            return Result;
        }

        public string OrderHideSimulationtrans(string orderNo)
        {
            DataTable dt = new DataTable();
            string Result = "";
            string SQL = "";

            try
            {
                SQL += " sprSimulationResultTrans '" + orderNo + "';";
                dt = objRun.GetDatatables(SQL, conTRPICancel);

                Result = dt.Rows.Count == 0 ? "1" : dt.Rows[0]["Msg"].ToString();

            }
            catch (Exception e)
            {
                Result = e.Message;
            }

            return Result;
        }



        public int GetCountCondition( string DocNo , int Condition)
        {
            DataTable dt = new DataTable();
            int Result;
            string SQL = ""; 
            try
            {
                SQL += " select * from vewChangeRequestSheet Where DocNo = '"+ DocNo + "' and Condittion = '" + Condition + "' ";
                dt = objRun.GetDatatables(SQL, conTRPICancel);

                Result = dt.Rows.Count;

            }
            catch (Exception e)
            {
                Result = 0;
            }

            return Result;
        }

        public string getPurpose(string GroupCode ,string Code)
        {
            DataTable dt = new DataTable();
            string strValue;
            string SQL = "";
            try
            {
                SQL += "select * from purpose where GroupCode = '" + GroupCode + "' and Code = '" + Code + "'";

                dt = objRun.GetDatatables(SQL, conTRPICancel);

                strValue = dt.Rows[0]["Value1"].ToString();
            }
            catch (Exception e)
            {
                strValue = e.Message;
            }
            return strValue;
        }

        #endregion
        #endregion


    }
}
