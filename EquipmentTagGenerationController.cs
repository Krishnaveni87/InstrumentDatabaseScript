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
    public class EquipmentTagGenerationController : Controller
    {
        // GET: EquipmentTagGeneration
        string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EquipmentTagGeneration()
        {
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();

            DataTable DT_EquipmentData = new DataTable();
            //SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");


            SqlConnection con = new SqlConnection(ConnectionString);

            string EquipmentData_Str = "select SNo as SLNo,Area,Eq_Type,Tag,P_ID,Eq as EquipmentNumber,FLC_in_FLOC,Area2,Remarks,Created_on,Created_by,Updated_on,Updated_by from EquipmentTag order by created_on DESC;";
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
            
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            List<string> Area_Lst = new List<string>();


            SqlCommand cmd_AreaLst = new SqlCommand("select distinct Concat(area,'     ',area_description) as Area from EquipmentTag  where SNo is not null;", con);
            SqlDataAdapter daArea_Lst = new SqlDataAdapter(cmd_AreaLst);
            DataTable dtArea_Lst = new DataTable();
            daArea_Lst.Fill(dtArea_Lst);

            Area_Lst.Add("--Select--");
            for (int i = 0; i < dtArea_Lst.Rows.Count; i++)
            {
                Area_Lst.Add(dtArea_Lst.Rows[i]["Area"].ToString());

            }
            var Area_LstTargests = new SelectList(Area_Lst);
            ViewData["Area_LstTargests"] = Area_LstTargests;



            #region Area2_lst
            List<string> Area2_Lst = new List<string>();

            SqlCommand cmd_Area2Lst = new SqlCommand("select distinct area2 from EquipmentTag  where SNo is not null;", con);
            SqlDataAdapter daArea2_Lst = new SqlDataAdapter(cmd_Area2Lst);
            DataTable dtArea2_Lst = new DataTable();
            daArea2_Lst.Fill(dtArea2_Lst);

            Area2_Lst.Add("--Select--");
            for (int i = 0; i < dtArea2_Lst.Rows.Count; i++)
            {
                Area2_Lst.Add(dtArea2_Lst.Rows[i]["area2"].ToString());
            }

            var Area2_LstTargests = new SelectList(Area2_Lst);
            ViewData["Area2_LstTargests"] = Area2_LstTargests;

            #endregion

            #region Eq_Types
            List<string> Eq_Types = new List<string>();
            string EqType = "select distinct Concat(Eq_Type,'     ',Eq_TypeDescription) as EQ_Type from EquipmentTag  where   SNo is not null and eq_typedescription is not null;";
            SqlCommand cmd_EqType = new SqlCommand(EqType, con);
            SqlDataAdapter daEqType = new SqlDataAdapter(cmd_EqType);
            DataTable dtEqType = new DataTable();
            daEqType.Fill(dtEqType);
            Eq_Types.Add("--Select--");
            for (int i = 0; i < dtEqType.Rows.Count; i++)
            {
                Eq_Types.Add(dtEqType.Rows[i]["EQ_Type"].ToString());
            }

            var Eq_TypeDescriptionTarget = new SelectList(Eq_Types);
            ViewData["Eq_TypeDescriptionTarget"] = Eq_TypeDescriptionTarget;
            #endregion


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
            
            ViewBag.seq_no = 0;

            #endregion

            #region FLOC_Code
            List<string> Tag_Code = new List<string>();
            Tag_Code.Add("--Select--");
            var Tag_Codedrp = new SelectList(Tag_Code);
            ViewData["Tag_CodeTar"] = Tag_Codedrp;
            #endregion
            con.Close();
        }

        public ActionResult GetMaxSeq_NO(string SelectedArea, string SelectedEqType)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string eq_type = SelectedEqType;
            int area = Convert.ToInt32(SelectedArea.Substring(0, 2));
            eq_type = Convert.ToString(eq_type.Substring(0, 1));
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
                if (dtSequenceNo.Rows[0]["Sequence"].ToString() != "")
                {
                    Seq_no = Convert.ToInt32(dtSequenceNo.Rows[0]["Sequence"].ToString());
                }
                else
                {
                    Seq_no = 1000;
                }
            }
            Seq_no = Seq_no + 1;
            List<string> MaxSeq_no = new List<string>();

            MaxSeq_no.Add(Seq_no.ToString());
            //Eq_Types.Add("--Select--");
            con.Close();
            var MaxSeq_noEq = new SelectList(MaxSeq_no);
            ViewData["MaxSeq_noEq"] = MaxSeq_noEq;
            return Json(MaxSeq_noEq, JsonRequestBehavior.AllowGet);
            
        }


        public ActionResult AddEquipmentTypeDetails(string Area, string Eq_Type, string Seq_No, string Eq_name, string PIDNo, string Areas2, string Requestor, string Project_Name)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
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

        [HttpPost]
        public ActionResult GetEQCode(string TagNo)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string str_GetEQCode = "select  distinct EQ  from EquipmentTag where Tag like @Tag;";
            SqlCommand cmd_GetEQCode = new SqlCommand(str_GetEQCode, con);
            cmd_GetEQCode.Parameters.AddWithValue("@Tag", "%" + TagNo + "%");
            //cmd_FLCin_FLOC.Parameters.AddWithValue("@TagNo", TagNo);
            SqlDataAdapter da_GetEQCode = new SqlDataAdapter(cmd_GetEQCode);
            DataTable dt_GetEQCode = new DataTable();
            da_GetEQCode.Fill(dt_GetEQCode);
            var Tag_Code = "0";
            List<string> Tag_No = new List<string>();
            // Tag_No.Add("--Select--");

            if (dt_GetEQCode.Rows.Count != 0)
            {
                for (int i = 0; i <= dt_GetEQCode.Rows.Count - 1; i++)
                {
                    Tag_Code = dt_GetEQCode.Rows[i]["EQ"].ToString();
                    Tag_No.Add(Tag_Code);
                }
                //return Json(FLOC_CODE, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //return Json(FLOC_CODE, JsonRequestBehavior.AllowGet);
            }
            con.Close();
            var Tag_Nodrp = new SelectList(Tag_No);
            ViewData["Tag_CodeTar"] = Tag_Nodrp;
            return Json(Tag_Nodrp, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFLCin_FLOC(string TagNo, string EQCode)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            List<string> FLCin_FLOC = new List<string>();
            string str_FLCin_FLOC = "select distinct FLC_in_FLOC from EquipmentTag where eq=@EQ;";
            SqlCommand cmd_FLCin_FLOC = new SqlCommand(str_FLCin_FLOC, con);
            cmd_FLCin_FLOC.Parameters.AddWithValue("@EQ", EQCode);
            SqlDataAdapter da_FLCin_FLOC = new SqlDataAdapter(cmd_FLCin_FLOC);
            DataTable dt_FLCin_FLOC = new DataTable();
            da_FLCin_FLOC.Fill(dt_FLCin_FLOC);
            if (dt_FLCin_FLOC.Rows.Count != 0)
            {
                FLCin_FLOC.Add(dt_FLCin_FLOC.Rows[0]["FLC_in_FLOC"].ToString());
            }
            var FLCin_FLOCTar = new SelectList(FLCin_FLOC);
            con.Close();
            return Json(FLCin_FLOCTar, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveFLOCcode(string EQCode, string FLOCCode)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string str_UpdateFLOC = "update EquipmentTag set FLOC_Status=@FLOC_Status,FLC_in_FLOC=@FLC_in_FLOC,updated_on=@updated_on,updated_by=@updated_by where EQ=@EQ;";
            SqlCommand cmd_UpdateFLOC = new SqlCommand(str_UpdateFLOC, con);
            cmd_UpdateFLOC.Parameters.AddWithValue("@FLOC_Status", 1);
            cmd_UpdateFLOC.Parameters.AddWithValue("@FLC_in_FLOC", FLOCCode);
            cmd_UpdateFLOC.Parameters.AddWithValue("@EQ", EQCode);
            cmd_UpdateFLOC.Parameters.AddWithValue("@updated_on", DateTime.Now);
            cmd_UpdateFLOC.Parameters.AddWithValue("@updated_by", "Admin");
           
            int i = cmd_UpdateFLOC.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failure", JsonRequestBehavior.AllowGet);
            }

        }
        //public ActionResult SaveFLOCcode(string TagNo, string FLOCCode)
        //{
        //    string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
        //    SqlConnection con = new SqlConnection(ConnectionString);
        //    string str_UpdateFLOC = "update EquipmentTag set FLOC_Status=@FLOC_Status,FLC_in_FLOC=@FLC_in_FLOC,updated_on=@updated_on,updated_by=@updated_by where Tag=@Tag;";
        //    SqlCommand cmd_UpdateFLOC = new SqlCommand(str_UpdateFLOC, con);
        //    cmd_UpdateFLOC.Parameters.AddWithValue("@FLOC_Status", 1);
        //    cmd_UpdateFLOC.Parameters.AddWithValue("@FLC_in_FLOC", FLOCCode);
        //    cmd_UpdateFLOC.Parameters.AddWithValue("@Tag", TagNo);
        //    cmd_UpdateFLOC.Parameters.AddWithValue("@updated_on", DateTime.Now);
        //    cmd_UpdateFLOC.Parameters.AddWithValue("@updated_by", "Admin");
        //    con.Open();
        //    int i = cmd_UpdateFLOC.ExecuteNonQuery();
        //    if (i > 0)
        //    {
        //        return Json("Success", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("Failure", JsonRequestBehavior.AllowGet);
        //    }

        //}

    }
}