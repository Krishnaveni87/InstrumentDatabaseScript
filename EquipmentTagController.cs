using InstrumentDatabase.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

            string EquipmentData_Str = "select SNo,Area,Eq_Type,Tag,P_ID,Eq,FLC_as_in_EqList,FLC_in_FLOC,FLOC_Status,Area2,Remarks from EquipmentTag;";
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
            string eq_type = SelectedEqType.Substring(0, 2);
            int area = Convert.ToInt32(SelectedArea.Substring(0, 2));
            string str_SequenceNo = "select max(Tag) as Sequence from EquipmentTag where area=" + area + "  and eq_type='" + eq_type + "'";
            SqlCommand cmdSequenceNo = new SqlCommand(str_SequenceNo, con);
            SqlDataAdapter daSequenceNo = new SqlDataAdapter(cmdSequenceNo);
            DataTable dtSequenceNo = new DataTable();
            daSequenceNo.Fill(dtSequenceNo);
            int Seq_no = Convert.ToInt32(dtSequenceNo.Rows[0]["Sequence"].ToString());
            Seq_no = Seq_no + 1;
            List<string> MaxSeq_no = new List<string>();

            MaxSeq_no.Add(Seq_no.ToString());
            //Eq_Types.Add("--Select--");
            var MaxSeq_noEq = new SelectList(MaxSeq_no);
            ViewData["MaxSeq_noEq"] = MaxSeq_noEq;
            return Json(MaxSeq_noEq, JsonRequestBehavior.AllowGet);
        }
    }
}