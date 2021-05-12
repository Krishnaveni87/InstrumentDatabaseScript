using InstrumentDatabase.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DataTableExample.Controllers
{
    public class EquipmentTagController : Controller
    {
        // GET: EquipmentTag

        string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public ActionResult EquipmentTag()
        {

            List<SelectListItem> listSelectListItems = new List<SelectListItem>();

            DataTable DT_EquipmentData = new DataTable();
            //SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");
            

            SqlConnection con = new SqlConnection(ConnectionString);

            string EquipmentData_Str = "select SNo,Area,Eq_Type,Tag,P_ID,Eq,FLC_as_in_EqList,FLC_in_FLOC,FLOC_Status,Area2,Remarks,created_on,Created_by,updated_on,updated_by from EquipmentTag order by created_on desc;";
            SqlCommand cmdEquipmentData = new SqlCommand(EquipmentData_Str, con);
            SqlDataAdapter daEquipmentData = new SqlDataAdapter(cmdEquipmentData);
            daEquipmentData.Fill(DT_EquipmentData);
            con.Close();
            //return DT_EquipmentData;
            GetDetailsEquipment();
            return View(DT_EquipmentData);
        }

        private void GetDetailsEquipment()
        {
            AreaModel cMainareamodel = new AreaModel();
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
            SqlConnection con = new SqlConnection(ConnectionString);

            List<string> Area_Lst = new List<string>();

            //List<> Area_Lst = new List<SelectListItem>();

            SqlCommand cmd_AreaLst = new SqlCommand("select distinct area,area_description from EquipmentTag  where SNo is not null;", con);
            SqlDataAdapter daArea_Lst = new SqlDataAdapter(cmd_AreaLst);
            DataTable dtArea_Lst = new DataTable();
            daArea_Lst.Fill(dtArea_Lst);


            for (int i = 0; i < dtArea_Lst.Rows.Count; i++)
            {
                // SelectListItem selectList = new SelectListItem();

                Area_Lst.Add(dtArea_Lst.Rows[i]["area"].ToString()  + "   "+ dtArea_Lst.Rows[i]["area_description"].ToString());

                //Area_Lst.Add(new string { Text = dtArea_Lst.Rows[i]["area"].ToString(), Value = dtArea_Lst.Rows[i]["area"].ToString() });
                //Area_Lst.Add(selectList);
            }
            var Area_LstTargests = new SelectList(Area_Lst);
            ViewData["Area_LstTargests"] = Area_LstTargests;
            //cMainareamodel.Areas = Area_Lst;

            


            #region Area2_lst
            List<string> Area2_Lst = new List<string>();

            SqlCommand cmd_Area2Lst = new SqlCommand("select distinct area2 from EquipmentTag  where SNo is not null;", con);
            SqlDataAdapter daArea2_Lst = new SqlDataAdapter(cmd_Area2Lst);
            DataTable dtArea2_Lst = new DataTable();
            daArea2_Lst.Fill(dtArea2_Lst);

            for (int i = 0; i < dtArea2_Lst.Rows.Count; i++)
            {
                //SelectListItem selectList = new SelectListItem();
                Area2_Lst.Add(dtArea2_Lst.Rows[i]["area2"].ToString());
                //Area2_Lst.Add(new SelectListItem { Text = dtArea2_Lst.Rows[i]["area2"].ToString(), Value = dtArea2_Lst.Rows[i]["area2"].ToString() });
               // Area2_Lst.Add(selectList);
            }

            var Area2_LstTargests = new SelectList(Area2_Lst);
            ViewData["Area2_LstTargests"] = Area2_LstTargests;

            #endregion

            List<SelectListItem> EQ_Lst = new List<SelectListItem>();

            // string RawUrl = Request.RawUrl;

            //string substr = RawUrl.Substring(RawUrl.Length - 2);
            //DropDownAreaList();



            //EQ_Lst.Add(new SelectListItem { Text = "--select--", Value = "--select--" });
            cMainareamodel.Eq_Types = EQ_Lst;
            //ViewBag.Eq_Types = EQ_Lst;
            //ViewData["EqTypeListItems"] = EQ_Lst;

            //return View(Area_Lst);


            #region SequenceNo
            string eq_type = "C";
            int area = 15;
            string str_SequenceNo = "select max(Tag) as Sequence from EquipmentTag where area=" + area + "  and eq_type='" + eq_type + "'";
            SqlCommand cmdSequenceNo = new SqlCommand(str_SequenceNo, con);
            SqlDataAdapter daSequenceNo = new SqlDataAdapter(cmdSequenceNo);
            DataTable dtSequenceNo = new DataTable();
            daSequenceNo.Fill(dtSequenceNo);
            int Seq_no = Convert.ToInt32(dtSequenceNo.Rows[0]["Sequence"].ToString());
            Seq_no = Seq_no + 1;
            cMainareamodel.Seq_No = Seq_no;
            ViewBag.seq_no = Seq_no;

            #endregion
            #region Eq_Types
            List<string> Eq_Types = new List<string>();

            Eq_Types.Add("--Select--");
                //Eq_Types.Add("--Select--");
            var Eq_TypeDescriptionTarget = new SelectList(Eq_Types);
            ViewData["Eq_TypeDescriptionTarget"] = Eq_TypeDescriptionTarget;
            #endregion
        }

        //[HttpPost]
        public ActionResult GetEq_Types(string SelectedArea)
        {
            #region Eq_Types
            List<string> Eq_Types = new List<string>();
            SqlConnection con = new SqlConnection(ConnectionString);
            //int Area = Convert.ToInt32(Request.Form["ddlArea"].ToString());
            string EqType = "select distinct Eq_Type,Eq_TypeDescription from EquipmentTag  where Area=@Area and  SNo is not null and eq_typedescription is not null;";
            SqlCommand cmd_EqType = new SqlCommand(EqType, con);
            cmd_EqType.Parameters.AddWithValue("@Area", Convert.ToInt32(SelectedArea.Substring(0, 2)));
            SqlDataAdapter daEqType = new SqlDataAdapter(cmd_EqType);
            DataTable dtEqType = new DataTable();
            daEqType.Fill(dtEqType);
            for (int i = 0; i < dtEqType.Rows.Count; i++)
            {
                //SelectListItem selectList = new SelectListItem();
                //Eq_Types.Add(new SelectListItem { Text = dtEqType.Rows[i]["Eq_Type"].ToString(), Value = dtEqType.Rows[i]["eq_typedescription"].ToString() });
                //Eq_Types.Add(selectList);
                Eq_Types.Add(dtEqType.Rows[i]["Eq_Type"].ToString() + "       " + dtEqType.Rows[i]["Eq_TypeDescription"].ToString());
            }
            // cMainareamodel.Eq_Types = Eq_Types;
            var Eq_TypeDescriptionTarget =  new SelectList( Eq_Types);
            ViewData["Eq_TypeDescriptionTarget"] = Eq_TypeDescriptionTarget;
            return Json(Eq_TypeDescriptionTarget, JsonRequestBehavior.AllowGet);
            #endregion
        }

        public ActionResult GetMaxSeq_NO(string SelectedArea, string SelectedEqType)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            string eq_type = SelectedEqType;
            int area = Convert.ToInt32(SelectedArea.Substring(0, 2));
            string str_SequenceNo = "select max(Tag) as Sequence from EquipmentTag where area=" + area + "  and eq_type='" + eq_type + "'";
            SqlCommand cmdSequenceNo = new SqlCommand(str_SequenceNo, con);
            SqlDataAdapter daSequenceNo = new SqlDataAdapter(cmdSequenceNo);
            DataTable dtSequenceNo = new DataTable();
            daSequenceNo.Fill(dtSequenceNo);
            int Seq_no = 0;
            int errorCounter = Regex.Matches(dtSequenceNo.Rows[0]["Sequence"].ToString(), @"[a-zA-Z]").Count;
            if (errorCounter > 0)
            {
                string value = Regex.Replace(dtSequenceNo.Rows[0]["Sequence"].ToString(), "[A-Za-z ]", "");
                Seq_no = Convert.ToInt32(value);
            }
            else
            {
                Seq_no = Convert.ToInt32(dtSequenceNo.Rows[0]["Sequence"].ToString());
            }
            Seq_no = Seq_no + 1;
            List<string> MaxSeq_no = new List<string>();

            MaxSeq_no.Add(Seq_no.ToString());
            //Eq_Types.Add("--Select--");
            var MaxSeq_noEq = new SelectList(MaxSeq_no);
            ViewData["MaxSeq_noEq"] = MaxSeq_noEq;
            return Json(MaxSeq_noEq, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSeqNo_Count(string Seqno)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            string str_CountSeqno = "select count(*) as Rows_Count from EquipmentTag   where  Tag=@Tag;";
            SqlCommand cmd_CountSeqno = new SqlCommand(str_CountSeqno, con);
            cmd_CountSeqno.Parameters.AddWithValue("@Tag", Seqno);
            SqlDataAdapter da_CountSeqno = new SqlDataAdapter(cmd_CountSeqno);
            DataTable dt_CountSeqno = new DataTable();
            da_CountSeqno.Fill(dt_CountSeqno);
            var Seq_noCount = dt_CountSeqno.Rows[0]["Rows_Count"].ToString();
            return Json(Seq_noCount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFLOCCode(string TagNo)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            string str_FLCin_FLOC = "select FLC_in_FLOC from EquipmentTag where Tag=@TagNo";
            SqlCommand cmd_FLCin_FLOC = new SqlCommand(str_FLCin_FLOC, con);
            cmd_FLCin_FLOC.Parameters.AddWithValue("@TagNo", TagNo);
            SqlDataAdapter da_FLCin_FLOC = new SqlDataAdapter(cmd_FLCin_FLOC);
            DataTable dt_FLCin_FLOC = new DataTable();
            da_FLCin_FLOC.Fill(dt_FLCin_FLOC);
            var FLOC_CODE="0";
            if (dt_FLCin_FLOC.Rows.Count!=0)
            {
                FLOC_CODE = dt_FLCin_FLOC.Rows[0]["FLC_in_FLOC"].ToString();
                return Json(FLOC_CODE, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(FLOC_CODE, JsonRequestBehavior.AllowGet);
            }
            
            
        }

        public ActionResult SaveFLOCcode(string TagNo,string FLOCCode)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
            SqlConnection con = new SqlConnection(ConnectionString);
            string str_UpdateFLOC = "update EquipmentTag set FLOC_Status=@FLOC_Status,FLC_in_FLOC=@FLC_in_FLOC,updated_on=@updated_on,updated_by=@updated_by where Tag=@Tag;";
            SqlCommand cmd_UpdateFLOC = new SqlCommand(str_UpdateFLOC, con);
            cmd_UpdateFLOC.Parameters.AddWithValue("@FLOC_Status", 1);
            cmd_UpdateFLOC.Parameters.AddWithValue("@FLC_in_FLOC", FLOCCode);
            cmd_UpdateFLOC.Parameters.AddWithValue("@Tag", TagNo);
            cmd_UpdateFLOC.Parameters.AddWithValue("@updated_on", DateTime.Now);
            cmd_UpdateFLOC.Parameters.AddWithValue("@updated_by", "Admin");
            con.Open();
            int i = cmd_UpdateFLOC.ExecuteNonQuery();
            if (i > 0)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failure", JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult AddEquipmentTypeDetails(string Area, string Eq_Type, string Seq_No, string Eq_name, string PIDNo, string Areas2, string Requestor, string Project_Name)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
                SqlConnection con = new SqlConnection(ConnectionString);

                //15-C-5564
                string Area_Param = Area.Substring(0, 2);
                string Area_DescParam = Area.Substring(2, Area.Length - 2);
                string EqType_Param = Eq_Type.Substring(0, 1);
                string EqTypeDesc_Param = Eq_Type.Substring(1, Eq_Type.Length - 1).Trim();

                string EQ = Area_Param + "-" + EqType_Param + "-" + Seq_No;
                SqlCommand cmd_InsertEqDetails = new SqlCommand("InsertEquipmentType", con);
                cmd_InsertEqDetails.CommandType = CommandType.StoredProcedure;
                cmd_InsertEqDetails.Parameters.AddWithValue("@Area", Area_Param);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Area_Description", Area_DescParam);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Eq_Type", EqType_Param);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Eq_TypeDescription", EqTypeDesc_Param);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Tag", Seq_No);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Equipment_Name", Eq_name);
                cmd_InsertEqDetails.Parameters.AddWithValue("@P_ID", PIDNo);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Eq", EQ);
                cmd_InsertEqDetails.Parameters.AddWithValue("@FLOC_Status", 0);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Area2", Areas2);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Requestor", Requestor);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Project_Name", Project_Name);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Created_on", DateTime.Now);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Created_by", "Admin");
                cmd_InsertEqDetails.Parameters.AddWithValue("@Updated_on", DateTime.Now);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Updated_by", "Admin");

                con.Open();
                int i = cmd_InsertEqDetails.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failure", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}