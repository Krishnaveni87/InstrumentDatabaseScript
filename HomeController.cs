using InstrumentDatabase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace InstrumentDatabase.Controllers
{
    public class HomeController : Controller
    {

        DataSet dsFinalTable = new DataSet();
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        public ActionResult Homepage()
        {
            return View();
        }
        public ActionResult Equipment()
        {
            DataTable DT_Equipment = GetEquipmentData();
            return View(DT_Equipment);
        }

        //public ActionResult InstMultiForm()
        //{
        //    return View();
        //}
        public ActionResult InstType()
        {
            return View();
        }

        public ActionResult InstDescription()
        {
            return View();
        }

        public ViewResult DataTable()
        {
            return View();
        }
        public ViewResult TabsOverview()
        {
            DataTable dtHome = HomeTabTableView();

            DataTable dtInstrument = InstrumentsTableView();
            DataTable dtVendor = VendorTableView();
            DataTable dtManufacturer = ManufacturerTableView();
            DataTable dtTemplates = TemplatesTableView();
            DataTable dtInstType = InstTypeTableView();
            DataTable dtInfoType = InfoTypeTableView();
            DataTable dtInfoGroups = InfoGroupsTableView();
            //DataTable dtLoopTable = LoopTableView();
            DataTable dt_inst = EquipmentData();

            dsFinalTable.Tables.Add(dtHome);
            dsFinalTable.Tables.Add(dtInstrument);
            dsFinalTable.Tables.Add(dtVendor);
            dsFinalTable.Tables.Add(dtManufacturer);
            dsFinalTable.Tables.Add(dtTemplates);
            dsFinalTable.Tables.Add(dtInstType);
            dsFinalTable.Tables.Add(dtInfoType);
            dsFinalTable.Tables.Add(dtInfoGroups);
            //dsFinalTable.Tables.Add(dtLoopTable);
            //dsFinalTable.Tables.Add(dt_inst);

            //string JSONString = JsonConvert.SerializeObject(dsFinalTable);

            //JObject json = JObject.Parse(JSONString);

            return View(dsFinalTable);


        }

        private DataTable InfoGroupsTableView()
        {
            string connectionString = string.Empty;
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("  select distinct [Information_Group], [Group_Seq_No] FROM  [InstrumentTable] GROUP BY [Information_Group], [Group_Seq_No]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;
            con.Close();
            return FinalTable;
        }

        private DataTable InfoTypeTableView()
        {
            string connectionString = string.Empty;

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Information_Type],[Unit],[Min_Norm_Max_Field], count(*) as InfoRecordCount FROM [InstrumentTable] GROUP BY [Information_Type],[Unit],[Min_Norm_Max_Field]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;
            con.Close();
            return FinalTable;
        }

        private DataTable InstTypeTableView()
        {
            string connectionString = string.Empty;

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Instrument_Type], count(*) as InstrumentCount, [Instrument_Template_Line_Count] FROM [Instruments] GROUP BY [Instrument_Type],[Instrument_Template_Line_Count]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;

            con.Close();
            return FinalTable;
        }

        private DataTable TemplatesTableView()
        {
            string connectionString = string.Empty;

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Seq_No],[Instrument_Type],[Information_Type],[Unit],[Information_Group],[Group_Seq_No], count(*) as InfoRecordCount FROM  [InstrumentTable] GROUP BY [Seq_No],[Instrument_Type],[Information_Type],[Unit],[Information_Group],[Group_Seq_No]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;
            con.Close();
            return FinalTable;
        }

       
        public ActionResult InstrumentDetails(string Countrow, string Instrument_Type,string Area)
        {
            string str_InstrumentDetails = string.Empty;
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            if (Instrument_Type == "(empty)" && Area == "(empty)")
            {
                str_InstrumentDetails = "select Instrument_Type,TagName,Loop_No,Description,Type,Area,PID_No,Model_No,Needs_Updating,Spec_Info from Instruments where  Instrument_Type is null and  Area is null ;";
            }
            else if(Instrument_Type != "(empty)" && Area == "(empty)")
            {
                str_InstrumentDetails = "select  Instrument_Type,TagName,Loop_No,Description,Type,Area,PID_No,Model_No,Needs_Updating,Spec_Info from Instruments where  Instrument_Type =@Instrument_Type and  Area is null ;";
            }
            else if (Instrument_Type == "(empty)" && Area != "(empty)")
            {
                str_InstrumentDetails = "select Instrument_Type,TagName,Loop_No,Description,Type,Area,PID_No,Model_No,Needs_Updating,Spec_Info from Instruments where  Instrument_Type  is null and  Area=@Area ;";
            }
            //else if(Countrow=="-")
            //{

            //}
            //else if(Instrument_Type=="")
            //{

            //}
            //else if(Area=="")
            //{

            //}
            else
            {
                str_InstrumentDetails = "select Instrument_Type,TagName,Loop_No,Description,Type,Area,PID_No,Model_No,Needs_Updating,Spec_Info  from Instruments where  Instrument_Type=@Instrument_Type and  Area=@Area ;";
            }
            con.Open();
            SqlCommand cmdInstrumentDetails = new SqlCommand(str_InstrumentDetails, con);
            cmdInstrumentDetails.Parameters.AddWithValue("@Instrument_Type", Instrument_Type);
            cmdInstrumentDetails.Parameters.AddWithValue("@Area", Area);
            SqlDataAdapter daInstrumentDetails = new SqlDataAdapter(cmdInstrumentDetails);
            DataTable dtInstrumentDetails = new DataTable();
            daInstrumentDetails.Fill(dtInstrumentDetails);
            if(dtInstrumentDetails.Rows.Count==0 && Countrow=="-")
            {
                return Json("Page is Empty", JsonRequestBehavior.AllowGet);
            }
            //dsFinalTable.Tables.Add(dtInstrumentDetails);
            //return dtInstrumentDetails;
            ViewData.Model = dtInstrumentDetails.AsEnumerable();
           String STR_DT= ConvertDataTabletoString(dtInstrumentDetails);
            return Json(STR_DT, JsonRequestBehavior.AllowGet);
            //var DatatableInstDetails = new SelectList((System.Collections.IEnumerable)dtInstrumentDetails);
           // return View((, JsonRequestBehavior.AllowGet);
        }

        public string ConvertDataTabletoString(DataTable dt)
        {
            //DataTable dt = new DataTable();
           
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return serializer.Serialize(rows);
              
        }

        private DataTable LoopTableView()
        {
            string connectionString = string.Empty;
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Vendor],[Vendor_Contact],[Vendor_PhoneNo], count(*) as VendorCount FROM  [Instruments] GROUP BY [Vendor],[Vendor_Contact],[Vendor_PhoneNo]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;

            con.Close();
            return FinalTable;
        }


        private DataTable ManufacturerTableView()
        {
            string connectionString = string.Empty;

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Manufacturer],[Manufacturer_PhoneNo] as PhoneNo, count(*) as InstrumentCount FROM  [Instruments] GROUP BY [Manufacturer],[Manufacturer_PhoneNo]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;

            con.Close();
            return FinalTable;
        }

        private DataTable VendorTableView()
        {
            string connectionString = string.Empty;

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            //SqlCommand cmdInstrumentCount = new SqlCommand("select Instrument_type,Area ,count(*) as Instrument_Count from Instruments group by Area,Instrument_Type order by Instrument_Type,Area", con);
            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct[Vendor],[Vendor_Contact],[Vendor_PhoneNo], count(*) as InstrumentCount FROM [Instruments] GROUP BY[Vendor],[Vendor_Contact],[Vendor_PhoneNo]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;

            con.Close();
            return FinalTable;
        }


        /// <summary>
        /// This method is used for to fetch the data for the InstrumentTab in InstrumentDatabase
        /// Implemented by Harsha M A
        /// </summary>

        private DataTable InstrumentsTableView()
        {
            string connectionString = string.Empty;

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("SELECT [Instrument_Type],[TagName],[Loop_No],[Description],[Type],[Area],[PID_No],[Model_No],[Needs_Updating],[Spec_Info] FROM [Instruments]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;
            con.Close();
            return FinalTable;
        }

        /// <summary>
        /// This method is used for to fetch the data for the Equipment Generation Page in Equipment Number Generation Tab
        /// Implemented by Krishnaveni B
        /// </summary>
        private DataTable EquipmentData()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);


            //SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = True");
            SqlCommand cmd_AreaLst = new SqlCommand("select * from EquipmentTag  where SNo is not null;", con);
            SqlDataAdapter daArea_Lst = new SqlDataAdapter(cmd_AreaLst);
            DataTable dtArea_Lst = new DataTable();
            daArea_Lst.Fill(dtArea_Lst);
            con.Close();
            return dtArea_Lst;
        }




        /// <summary>
        /// This method is used for to fetch the data for the HomeTab in InstrumentDatabase
        /// Implemented by Krishnaveni B
        /// </summary>
        private DataTable HomeTabTableView()
        {

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("select Instrument_type,Area ,count(*) as Instrument_Count from Instruments group by Area,Instrument_Type order by Instrument_Type,Area", con);
            SqlCommand cmdarea = new SqlCommand("Select distinct area from instruments order by area asc", con);
            SqlCommand cmdInstrument_Type = new SqlCommand("select distinct Instrument_Type from Instruments order by Instrument_Type ASC", con);
            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            SqlDataAdapter daarea = new SqlDataAdapter(cmdarea);
            SqlDataAdapter daInstrument_Type = new SqlDataAdapter(cmdInstrument_Type);
            DataTable dtInstrumentCount = new DataTable();
            DataTable dtarea = new DataTable();
            DataTable dtInstrument_Type = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            daarea.Fill(dtarea);
            daInstrument_Type.Fill(dtInstrument_Type);
            DataTable FinalTable = new DataTable();

            FinalTable.Columns.Add("Area");
            foreach (DataRow row in dtarea.Rows)
            {
                string Column_Text = row["area"].ToString();
                if (Column_Text == "")
                {
                    FinalTable.Columns.Add("(empty)");
                }
                else
                {
                    FinalTable.Columns.Add(Column_Text);
                }
            }
            DataRow newRowFinalTable = FinalTable.NewRow();
            for (int i = 0; i <= FinalTable.Columns.Count - 1; i++)
            {
                if (i == 0)
                {
                    newRowFinalTable[i] = "Instrument Type";
                }
                else
                {
                    newRowFinalTable[i] = "Number of Instruments";
                }

            }
            FinalTable.Rows.InsertAt(newRowFinalTable, 1);

            #region CommentPreviousCode
            //for(int i=0;i<=dtInstrumentCount.Rows.Count;i++)
            //{
            //    DataRow newRowFinalTable_inst = FinalTable.NewRow();
            //    int Column_no = 0;
            //    int Column_Count = 0;
            //    for (int Table_column_cnt = 0; Table_column_cnt <= FinalTable.Columns.Count - 1; Table_column_cnt++)
            //    {

            //        if (Column_no == 0)
            //        {
            //            newRowFinalTable_inst[Table_column_cnt] = dtInstrumentCount.Rows[Table_column_cnt]["Instrument_type"].ToString();

            //            if(dtInstrumentCount.Rows[Table_column_cnt]["Instrument_type"].ToString()=="")
            //            {
            //                newRowFinalTable_inst[Table_column_cnt] = "(empty)";
            //            }

            //            Column_no++;

            //            if (dtInstrumentCount.Rows[Table_column_cnt]["Instrument_Count"].ToString() == "")
            //            {
            //                string Str_InstCount = "select count(*) as Instrument_Count from Instruments where Area=@Area and Instrument_Type=@Instrument_Type group by Area,Instrument_Type order by Instrument_Type, Area;";
            //                SqlCommand cmdStr_InstCount = new SqlCommand(Str_InstCount, con);
            //                SqlParameter[] param = new SqlParameter[2];
            //                param[0] = new SqlParameter("@Area", dtInstrumentCount.Rows[Table_column_cnt]["Area"].ToString());
            //                param[1] = new SqlParameter("@Instrument_Type", dtInstrumentCount.Rows[Table_column_cnt]["Instrument_type"].ToString());
            //                cmdStr_InstCount.Parameters.Add(param[0]);
            //                cmdStr_InstCount.Parameters.Add(param[1]);
            //                int count = Convert.ToInt32(cmdStr_InstCount.ExecuteScalar());
            //                if (count.ToString() == "")
            //                {
            //                    newRowFinalTable_inst[Table_column_cnt + Column_no] = "-";
            //                }
            //            }
            //            else
            //            {
            //                newRowFinalTable_inst[Table_column_cnt + Column_no] = dtInstrumentCount.Rows[Table_column_cnt]["Instrument_Count"].ToString();
            //            }
            //        }
            //        else
            //        {
            //            Column_Count = Table_column_cnt + Column_no;
            //            if (Column_Count<12)
            //            {
            //                if (dtInstrumentCount.Rows[Table_column_cnt]["Instrument_Count"].ToString() == "")
            //                {
            //                    newRowFinalTable_inst[Table_column_cnt + Column_no] = "-";
            //                }
            //                else
            //                {
            //                    newRowFinalTable_inst[Table_column_cnt + Column_no] = dtInstrumentCount.Rows[Table_column_cnt]["Instrument_Count"].ToString();
            //                }
            //            }
            //        }
            //    }
            //    FinalTable.Rows.Add(newRowFinalTable_inst);
            //}
            #endregion

            for (int i = 0; i < dtInstrument_Type.Rows.Count; i++)
            {
                DataRow newRowFinalTable_inst = null;
                int first_column = 0;
                newRowFinalTable_inst = FinalTable.NewRow();
                for (int j = 1; j < dtarea.Rows.Count + 1; j++)
                {

                    string Str_Query = string.Empty;
                    SqlCommand cmdInstrument_Count = null;
                    string strInstrument_Type = dtInstrument_Type.Rows[i]["Instrument_Type"].ToString();
                    string strArea = dtarea.Rows[j - 1]["Area"].ToString();

                    if (strInstrument_Type == "" && j == 1)
                    {
                        newRowFinalTable_inst[0] = "(empty)";
                        first_column++;
                    }
                    else
                    {
                        if (first_column == 0)
                        {
                            newRowFinalTable_inst[0] = strInstrument_Type;
                        }
                    }
                    if (strInstrument_Type == "" && strArea == "")
                    {
                        Str_Query = "select  count(*) as Instrument_Count from Instruments where Instrument_Type  is null and Area  is null ";
                    }
                    else if (strInstrument_Type != "" && strArea == "")
                    {
                        Str_Query = "select  count(*) as Instrument_Count from Instruments where Instrument_Type= '" + strInstrument_Type + "'  and Area  is null ";
                    }
                    else if (strInstrument_Type == "" && strArea != "")
                    {
                        Str_Query = "select  count(*) as Instrument_Count from Instruments where Instrument_Type is null  and Area= '" + strArea + "'";
                    }
                    else
                    {
                        Str_Query = "select  count(*) as Instrument_Count from Instruments where Instrument_Type='" + strInstrument_Type + "' and Area= '" + strArea + "'";
                    }
                    cmdInstrument_Count = new SqlCommand(Str_Query, con);

                    #region CommentSection
                    //cmdInstrument_Count.Parameters.Add("@Instrument_Type", "");
                    //cmdInstrument_Count.Parameters.Add("@Area", strArea);

                    //SqlParameter[] param = new SqlParameter[2];
                    //if (strInstrument_Type == "")
                    //{
                    //    param[0] = new SqlParameter("@Instrument_Type", System.DBNull.Value);
                    //}
                    //else
                    //{
                    //    param[0] = new SqlParameter("@Instrument_Type", strInstrument_Type);
                    //}
                    //if (strArea == "")
                    //{
                    //    param[1] = new SqlParameter("@Area", System.DBNull.Value);
                    //}
                    //else
                    //{
                    //    param[1] = new SqlParameter("@Area", strArea);
                    //}

                    //cmdInstrument_Count.Parameters.Add(param[0]);
                    //cmdInstrument_Count.Parameters.Add(param[1]);
                    #endregion


                    SqlDataAdapter daInstrument_Count = new SqlDataAdapter(cmdInstrument_Count);
                    DataTable dtInstrument_Count = new DataTable();
                    daInstrument_Count.Fill(dtInstrument_Count);

                    if (Convert.ToInt32(dtInstrument_Count.Rows[0]["Instrument_Count"]) == 0)
                    {
                        newRowFinalTable_inst[j] = "-";
                    }
                    else
                    {
                        newRowFinalTable_inst[j] = dtInstrument_Count.Rows[0]["Instrument_Count"].ToString();
                    }


                }
                FinalTable.Rows.Add(newRowFinalTable_inst);
            }

            //FinalTable.Columns.Add("Total");
            DataRow NewFinalTable = FinalTable.NewRow();
            NewFinalTable[0] = "Total";
            FinalTable.Rows.Add(NewFinalTable);

            DataTable dt_temp = new DataTable();
            FinalTable.TableName = "TempTable";
            dt_temp = FinalTable.Copy();

            for (int i = 0; i <= dt_temp.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dt_temp.Columns.Count; j++)
                {
                    dt_temp.Rows[i][j] = dt_temp.Rows[i][j].ToString().Replace("-", "0");
                }

            }


            dt_temp.Columns.Add("Total", typeof(decimal));
            foreach (DataRow row in dt_temp.Rows)
            {
                decimal rowSum = 0;
                foreach (DataColumn col in dt_temp.Columns)
                {
                    if (!row.IsNull(col))
                    {
                        string stringValue = row[col].ToString();
                        decimal d;
                        if (decimal.TryParse(stringValue, out d))
                            rowSum += d;
                    }
                }
                row.SetField("Total", rowSum);
            }

            dt_temp.Rows.Add("Total", typeof(decimal));
            dt_temp.Rows.RemoveAt(0);

            //object sumObject;
            //sumObject = dt_temp.Compute("Sum(BoilerHouse)", string.Empty);

            int sum = 0;

            int Count_no = 0;


            for (int i = 1; i < dt_temp.Columns.Count - 1; i++)
            {
                sum = 0;
                for (int j = 0; j < dt_temp.Rows.Count - 2; j++)
                {
                    Count_no = Convert.ToInt32(dt_temp.Rows[j][i]);
                    sum += Count_no;
                }
                dt_temp.Rows[dt_temp.Rows.Count - 2][i] = sum;
            }

            dt_temp.Rows.RemoveAt(dt_temp.Rows.Count - 1);
            SqlCommand cmd_GrandInstCount = new SqlCommand("select count(*) as Instrument_Count from Instruments", con);
            SqlDataAdapter daGrandInstCount = new SqlDataAdapter(cmd_GrandInstCount);
            DataTable dtGrandInstCount = new DataTable();
            daGrandInstCount.Fill(dtGrandInstCount);
            dt_temp.Rows[dt_temp.Rows.Count - 1][dt_temp.Columns.Count - 1] = dtGrandInstCount.Rows[0]["Instrument_Count"];

            for (int i = 0; i <= dt_temp.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dt_temp.Columns.Count; j++)
                {
                    if (dt_temp.Rows[i][j].ToString().Length == 1 && dt_temp.Rows[i][j].ToString() == "0")
                    {
                        dt_temp.Rows[i][j] = dt_temp.Rows[i][j].ToString().Replace("0", "-");
                    }
                }

            }

            FinalTable = dt_temp.Copy();

            DataRow newRowFinalTable1 = FinalTable.NewRow();
            for (int i = 0; i <= FinalTable.Columns.Count - 2; i++)
            {
                if (i == 0)
                {
                    newRowFinalTable1[i] = "Instrument Type";
                }
                else
                {
                    newRowFinalTable1[i] = "Number of Instruments";
                }

            }
            FinalTable.Rows.InsertAt(newRowFinalTable1, 0);

            con.Close();
            return FinalTable;
        }

        private DataTable GetEquipmentData()
        {
            DataTable DT_EquipmentData = new DataTable();
            //SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            string EquipmentData_Str = "select SNo,Area,Eq_Type,Tag,P_ID,Eq,FLC_as_in_Eq_List,FLC_in_FLOC,Area2,Remarks from EquipmentTag;";
            SqlCommand cmdEquipmentData = new SqlCommand(EquipmentData_Str, con);
            SqlDataAdapter daEquipmentData = new SqlDataAdapter(cmdEquipmentData);
            daEquipmentData.Fill(DT_EquipmentData);
            con.Close();
            return DT_EquipmentData;
        }

        public ActionResult Instrument()
        {
            DataTable DT_InstrumentData = new DataTable();
            //SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);


            SqlCommand cmdInstrumentData = new SqlCommand("select * from InstrumentData order by Seq_No DESC", con);
            SqlDataAdapter daInstrumentData = new SqlDataAdapter(cmdInstrumentData);
            daInstrumentData.Fill(DT_InstrumentData);


            #region GetInst_type

            //SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");

            SqlCommand cmdInst_type = new SqlCommand("select distinct Instrument_Type from InstrumentData order by Instrument_Type ASC;", con);
            SqlDataAdapter daInst_type = new SqlDataAdapter(cmdInst_type);
            DataTable dtInst_type = new DataTable();
            daInst_type.Fill(dtInst_type);
            List<string> Lst_Insttype = new List<string>();
            for (int i = 0; i < dtInst_type.Rows.Count; i++)
            {
                Lst_Insttype.Add(dtInst_type.Rows[i]["Instrument_Type"].ToString());
            }

            var conqTargets = new SelectList(Lst_Insttype);
            ViewData["conqTargets"] = conqTargets;
            //ViewBag.Title = "ManageTargets";


            #endregion

            #region GetSeq_No

            SqlCommand cmdSeq_No = new SqlCommand("select max(Seq_no) as Max_Seqno from InstrumentData", con);
            SqlDataAdapter daSeq_No = new SqlDataAdapter(cmdSeq_No);
            DataTable dtSeq_No = new DataTable();
            daSeq_No.Fill(dtSeq_No);
            int seq_no = Convert.ToInt16(dtSeq_No.Rows[0]["Max_Seqno"].ToString());
            seq_no = seq_no + 1;
            //            var conqTargets = new SelectList(Lst_Insttype);
            ViewData["SeqNo"] = seq_no;
            //ViewBag.Title = "ManageTargets";


            #endregion

            #region GetSeq_No

            SqlCommand cmdMultiSeq_No = new SqlCommand("select max(Seq_no) as Max_Seqno from InstrumentData", con);
            SqlDataAdapter daMultiSeq_No = new SqlDataAdapter(cmdSeq_No);
            DataTable dtMultiSeq_No = new DataTable();
            daSeq_No.Fill(dtMultiSeq_No);
            int Multiseq_no = Convert.ToInt16(dtMultiSeq_No.Rows[0]["Max_Seqno"].ToString());
            Multiseq_no = Multiseq_no + 1;
            //            var conqTargets = new SelectList(Lst_Insttype);
            ViewData["SeqNoFrom"] = Multiseq_no;
            //ViewBag.Title = "ManageTargets";


            ViewData["SeqNoTo"] = Multiseq_no;

            #endregion

            con.Close();

            return View(DT_InstrumentData);
        }

        //public static AreaModel cMainareamodel;
        //public ActionResult EquipForm()
        //{

        //    AreaModel objAreamodel = new AreaModel();
        //    cMainareamodel = new AreaModel();
        //    objAreamodel.Areas = cMainareamodel.Areas = FillList();
        //    //ViewBag.lblArea = "";


        //    List<string> Area_Lst = new List<string>();
        //    SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");
        //    SqlCommand cmd_AreaLst = new SqlCommand("select distinct area from EquipmentTag  where SNo is not null;", con);
        //    SqlDataAdapter daArea_Lst = new SqlDataAdapter(cmd_AreaLst);
        //    DataTable dtArea_Lst = new DataTable();
        //    daArea_Lst.Fill(dtArea_Lst);
        //    for (int i = 0; i < dtArea_Lst.Rows.Count; i++)
        //    {
        //        Area_Lst.Add(dtArea_Lst.Rows[i]["Area"].ToString());
        //    }
        //    ViewData["Area"] = Area_Lst;
        //    ViewBag.Area_Lst = Area_Lst;
        //    //MyEntities e = new MyEntities();


        //    #region EquipmentType

        //    int Area = 0;
        //    string EqType = "select distinct Eq_Type from EquipmentTag where area=" + Area;
        //    SqlCommand cmd_EqType = new SqlCommand(EqType, con);
        //    SqlDataAdapter daEqType = new SqlDataAdapter(cmd_EqType);
        //    DataTable dtEqType = new DataTable();
        //    daEqType.Fill(dtEqType);

        //    #endregion

        //    #region SequenceNo

        //    string str_SequenceNo = "SELECT MAX(Tag) as Sequence FROM EquipmentTag where SNo is not null";
        //    SqlCommand cmdSequenceNo = new SqlCommand(str_SequenceNo, con);
        //    SqlDataAdapter daSequenceNo = new SqlDataAdapter(cmdSequenceNo);
        //    DataTable dtSequenceNo = new DataTable();
        //    daSequenceNo.Fill(dtSequenceNo);
        //    #endregion

        //    //return View(Area_Lst);

        //    return View(objAreamodel);
        //}


        //[HttpPost]
        //public ActionResult Index(AreaModel areamodel)
        //{
        //    var g = cMainareamodel.Areas;
        //    var selectedCountry = g.Find(p => p.Value == areamodel.Areaid.ToString());
        //    areamodel.Areas = FillList();
        //    ViewBag.LblCountry = "You selected " + selectedCountry.Text.ToString();
        //    return View(areamodel);
        //}

        public ActionResult InstForm()
        {
            #region GetInst_type

            //SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInst_type = new SqlCommand("select distinct Instrument_Type from InstrumentData order by Instrument_Type ASC;", con);
            SqlDataAdapter daInst_type = new SqlDataAdapter(cmdInst_type);
            DataTable dtInst_type = new DataTable();
            daInst_type.Fill(dtInst_type);
            List<string> Lst_Insttype = new List<string>();
            for (int i = 0; i < dtInst_type.Rows.Count; i++)
            {
                Lst_Insttype.Add(dtInst_type.Rows[i]["Instrument_Type"].ToString());
            }

            var conqTargets = new SelectList(Lst_Insttype);
            ViewData["conqTargets"] = conqTargets;
            //ViewBag.Title = "ManageTargets";


            #endregion

            #region GetSeq_No

            SqlCommand cmdSeq_No = new SqlCommand("select max(Seq_no) as Max_Seqno from InstrumentData", con);
            SqlDataAdapter daSeq_No = new SqlDataAdapter(cmdSeq_No);
            DataTable dtSeq_No = new DataTable();
            daSeq_No.Fill(dtSeq_No);
            int seq_no = Convert.ToInt16(dtSeq_No.Rows[0]["Max_Seqno"].ToString());
            seq_no = seq_no + 1;
            //            var conqTargets = new SelectList(Lst_Insttype);
            ViewData["SeqNo"] = seq_no;
            //ViewBag.Title = "ManageTargets";


            #endregion

            con.Close();

            return View();
        }

        public ActionResult InstDataSave(string InstType, int SeqNo, string InstName, string PnIdNo, string Requestor, string Project)
        {
            var instType = InstType;
            int seqNo = SeqNo;
            var instName = InstName;
            var pnIdNo = PnIdNo;
            var requestor = Requestor;
            var project = Project;


            try
            {
                //string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

                //SqlConnection con = new SqlConnection(ConnectionString);

                //SqlCommand cmdSeq_No = new SqlCommand("select max(Seq_no) as Max_Seqno from InstrumentData", con);
                //SqlDataAdapter daSeq_No = new SqlDataAdapter(cmdSeq_No);

                //DataTable dtSeq_No = new DataTable();
                //daSeq_No.Fill(dtSeq_No);
                //int seq_no = Convert.ToInt16(dtSeq_No.Rows[0]["Max_Seqno"].ToString());
                //con.Close();

                //if()

                string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

                SqlConnection con = new SqlConnection(ConnectionString);

                SqlCommand cmd_InsertEqDetails = new SqlCommand("InsertInstrumentData", con);
                cmd_InsertEqDetails.CommandType = CommandType.StoredProcedure;
                cmd_InsertEqDetails.Parameters.AddWithValue("@InstType", instType);
                cmd_InsertEqDetails.Parameters.AddWithValue("@SeqNo", seqNo);
                cmd_InsertEqDetails.Parameters.AddWithValue("@InstName", instName);
                cmd_InsertEqDetails.Parameters.AddWithValue("@PnIdNo", pnIdNo);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Requestor", requestor);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Project", project);

                con.Open();
                int i = cmd_InsertEqDetails.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {
                    ViewBag.Message = "Saved Successfully";
                    return View();

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

        [HttpGet]
        public ActionResult InstMultiForm()
        {
            #region GetSeq_No
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdSeq_No = new SqlCommand("select max(Seq_no) as Max_Seqno from InstrumentData", con);
            SqlDataAdapter daSeq_No = new SqlDataAdapter(cmdSeq_No);
            DataTable dtSeq_No = new DataTable();
            daSeq_No.Fill(dtSeq_No);
            int seq_no = Convert.ToInt16(dtSeq_No.Rows[0]["Max_Seqno"].ToString());
            seq_no = seq_no + 1;
            //            var conqTargets = new SelectList(Lst_Insttype);
            ViewData["SeqNoFrom"] = seq_no;
            //ViewBag.Title = "ManageTargets";


            ViewData["SeqNoTo"] = seq_no;

            #endregion

            con.Close();

            return View();
        }

        [HttpPost]
        public ActionResult getSeqNoRange(int NoOfTag)
        {
            int noOfTag = NoOfTag;

            #region GetSeq_No
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdSeq_No = new SqlCommand("select max(Seq_no) as Max_Seqno from InstrumentData", con);
            SqlDataAdapter daSeq_No = new SqlDataAdapter(cmdSeq_No);
            DataTable dtSeq_No = new DataTable();
            daSeq_No.Fill(dtSeq_No);
            int seq_no = Convert.ToInt16(dtSeq_No.Rows[0]["Max_Seqno"].ToString());
            int seq_noFrom = seq_no + 1;
            //            var conqTargets = new SelectList(Lst_Insttype);
            ViewData["SeqNoFrom"] = seq_noFrom;
            //ViewBag.Title = "ManageTargets";


            int seq_noTo = seq_noFrom + (noOfTag - 1);
            ViewData["SeqNoTo"] = seq_noTo;
            #endregion


            //return View("InstMultiForm");
            return Json(seq_noTo, JsonRequestBehavior.AllowGet);

            //    }
            //    else
            //    {

            //        return Json("Failure", JsonRequestBehavior.AllowGet);
            //    }

            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}


        }

        public ActionResult InstMultiDataSave(int SeqNoFrom, int SeqNoTo, string Requestor, string Project)
        {
            int seqNoFrom = SeqNoFrom;
            var seqNoTo = SeqNoTo;
            var requestor = Requestor;
            var project = Project;

            int i = 1;

            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

                SqlConnection con = new SqlConnection(ConnectionString);


                for (int j = SeqNoFrom; j <= SeqNoTo + 1; j++)
                {
                    SqlCommand cmd_InsertEqDetails = new SqlCommand("InsertInstrumentMultiData", con);
                    cmd_InsertEqDetails.CommandType = CommandType.StoredProcedure;


                    cmd_InsertEqDetails.Parameters.AddWithValue("@SeqNo", j);

                    cmd_InsertEqDetails.Parameters.AddWithValue("@Requestor", requestor);
                    cmd_InsertEqDetails.Parameters.AddWithValue("@Project", project);

                    con.Open();
                    cmd_InsertEqDetails.ExecuteNonQuery();

                    con.Close();
                }
                if (i >= 1)
                {
                    //ViewBag.Message = "Saved Successfully";
                    //return Json("Success", JsonRequestBehavior.AllowGet);

                    return RedirectToAction("Instrument", "Home");
                    //Instrument();

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

        public ActionResult Index2()
        {
            return View();
        }


        [HttpGet]
        public ActionResult EquipForm()
        {
            GetDetailsEquipment();

            return View();
        }

        [HttpPost]
        public ActionResult EquipForm(AreaModel areamdl)
        {
            try
            {
                GetDetailsEquipment();
                areamdl.Areas = ViewBag.areas;
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                string Areas = string.Empty;
                string Eq_Types = string.Empty;
                string Seq_No = string.Empty;
                string Eq_name = string.Empty;
                string PIDNo = string.Empty;
                string Areas2 = string.Empty;
                string Requestor = string.Empty;
                string Project_Name = string.Empty;
                foreach (var key in ViewData.ModelState.Keys)
                {
                    string modelStateVal = ViewData.ModelState[key].ToString();
                    var currentKeyValue = ModelState[key].Value.AttemptedValue;
                    switch (key)
                    {
                        case "Areas":
                            Areas = currentKeyValue;
                            break;
                        case "Eq_Types":
                            Eq_Types = currentKeyValue;
                            break;
                        case "Seq_No":
                            Seq_No = currentKeyValue;
                            break;
                        case "Eq_name":
                            Eq_name = currentKeyValue;
                            break;
                        case "PIDNo":
                            PIDNo = currentKeyValue;
                            break;
                        case "Areas2":
                            Areas2 = currentKeyValue;
                            break;
                        case "Requestor":
                            Requestor = currentKeyValue;
                            break;
                        case "Project_Name":
                            Project_Name = currentKeyValue;
                            break;
                        default:
                            Console.WriteLine("Unknown value");
                            break;
                    }

                }
                if (AddEquipmentTypeDetails(Areas, Eq_Types, Seq_No, Eq_name, PIDNo, Areas2, Requestor, Project_Name))
                {
                    ViewBag.Message = "Equipment  details added successfully";
                }


                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        public ActionResult DropDownAreaList()
        {
            AreaModel cMainareamodel = new AreaModel();
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
            SqlConnection con = new SqlConnection(ConnectionString);
            List<SelectListItem> Eq_Types = new List<SelectListItem>();
            //int Area = Convert.ToInt32(Request.Form["ddlArea"].ToString());
            string EqType = "select distinct Eq_Type from EquipmentTag";
            SqlCommand cmd_EqType = new SqlCommand(EqType, con);
            SqlDataAdapter daEqType = new SqlDataAdapter(cmd_EqType);
            DataTable dtEqType = new DataTable();
            daEqType.Fill(dtEqType);
            for (int i = 0; i < dtEqType.Rows.Count; i++)
            {
                //cMainareamodel.Eq_Types.Add(new SelectListItem { Text = dtEqType.Rows[i]["Eq_Type"].ToString(), Value = dtEqType.Rows[i]["Eq_Type"].ToString() });
                Eq_Types.Add(new SelectListItem { Text = dtEqType.Rows[i]["Eq_Type"].ToString(), Value = dtEqType.Rows[i]["Eq_Type"].ToString() });
            }
            cMainareamodel.Eq_Types = Eq_Types;
            //ViewData["EqTypeListItems"] = LstEqType;
            ViewBag.Eq_Types = Eq_Types;




            return View();

        }

        //[HttpPost]
        //public ActionResult Index(AreaModel areamodel)
        //{
        //    AreaModel cMainareamodel = new AreaModel();
        //    var g = cMainareamodel.Areas;
        //    var selectedCountry = g.Find(p => p.Value == areamodel.Areaid.ToString());
        //    areamodel.Areas = FillList();
        //    ViewBag.LblCountry = "You selected " + selectedCountry.Text.ToString();
        //    return View(areamodel);
        //}

        private List<SelectListItem> FillList()
        {
            var list = new List<SelectListItem>();
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmd_AreaLst = new SqlCommand("select distinct area from EquipmentTag  where SNo is not null;", con);

            SqlDataReader drAreaLst = cmd_AreaLst.ExecuteReader();
            if (drAreaLst.Read())
            {
                while (drAreaLst.Read())
                {
                    list.Add(new SelectListItem { Text = drAreaLst["area"].ToString(), Value = drAreaLst["area"].ToString() });
                }
            }
            return list;
        }

        [HttpGet]
        public ActionResult EquipmentNumberGeneration()
        {
            GetDetailsEquipment();

            return View();
        }

        private void GetDetailsEquipment()
        {
            AreaModel cMainareamodel = new AreaModel();
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
            SqlConnection con = new SqlConnection(ConnectionString);

            List<SelectListItem> Area_Lst = new List<SelectListItem>();

            SqlCommand cmd_AreaLst = new SqlCommand("select distinct area from EquipmentTag  where SNo is not null;", con);
            SqlDataAdapter daArea_Lst = new SqlDataAdapter(cmd_AreaLst);
            DataTable dtArea_Lst = new DataTable();
            daArea_Lst.Fill(dtArea_Lst);

            for (int i = 0; i < dtArea_Lst.Rows.Count; i++)
            {
                //cMainareamodel.Areas.Add(new SelectListItem { Text = dtArea_Lst.Rows[i]["area"].ToString(), Value = dtArea_Lst.Rows[i]["area"].ToString() });
                Area_Lst.Add(new SelectListItem { Text = dtArea_Lst.Rows[i]["area"].ToString(), Value = dtArea_Lst.Rows[i]["area"].ToString() });
                //cMainareamodel.Area = dtArea_Lst.Rows[i]["area"].ToString();
                //cMainareamodel.Areaid = dtArea_Lst.Rows[i]["area"].ToString();
            }
            //ViewData["ListItems"] = Area_Lst;
            cMainareamodel.Areas = Area_Lst;
            ViewBag.areas = Area_Lst;


            List<SelectListItem> Area2_Lst = new List<SelectListItem>();

            SqlCommand cmd_Area2Lst = new SqlCommand("select distinct area2 from EquipmentTag  where SNo is not null;", con);
            SqlDataAdapter daArea2_Lst = new SqlDataAdapter(cmd_Area2Lst);
            DataTable dtArea2_Lst = new DataTable();
            daArea2_Lst.Fill(dtArea2_Lst);

            for (int i = 0; i < dtArea2_Lst.Rows.Count; i++)
            {
                //cMainareamodel.Areas.Add(new SelectListItem { Text = dtArea_Lst.Rows[i]["area"].ToString(), Value = dtArea_Lst.Rows[i]["area"].ToString() });
                Area2_Lst.Add(new SelectListItem { Text = dtArea2_Lst.Rows[i]["area2"].ToString(), Value = dtArea2_Lst.Rows[i]["area2"].ToString() });
            }
            //ViewData["ListItems"] = Area_Lst;
            cMainareamodel.Areas2 = Area2_Lst;
            ViewBag.Areas2 = Area2_Lst;




            List<SelectListItem> EQ_Lst = new List<SelectListItem>();

            // string RawUrl = Request.RawUrl;

            //string substr = RawUrl.Substring(RawUrl.Length - 2);
            DropDownAreaList();



            EQ_Lst.Add(new SelectListItem { Text = "--select--", Value = "--select--" });
            cMainareamodel.Eq_Types = EQ_Lst;
            //ViewBag.Eq_Types = EQ_Lst;
            //ViewData["EqTypeListItems"] = EQ_Lst;

            //return View(Area_Lst);


            #region SequenceNo

            string str_SequenceNo = "SELECT MAX(Tag) as Sequence FROM EquipmentTag";
            SqlCommand cmdSequenceNo = new SqlCommand(str_SequenceNo, con);
            SqlDataAdapter daSequenceNo = new SqlDataAdapter(cmdSequenceNo);
            DataTable dtSequenceNo = new DataTable();
            daSequenceNo.Fill(dtSequenceNo);
            int Seq_no = Convert.ToInt32(dtSequenceNo.Rows[0]["Sequence"].ToString());
            Seq_no = Seq_no + 1;
            cMainareamodel.Seq_No = Seq_no;
            ViewBag.seq_no = Seq_no;

            #endregion
        }

        [HttpPost]
        public ActionResult EquipmentNumberGeneration(AreaModel areamdl)
        {
            try
            {
                GetDetailsEquipment();
                areamdl.Areas = ViewBag.areas;
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                string Areas = string.Empty;
                string Eq_Types = string.Empty;
                string Seq_No = string.Empty;
                string Eq_name = string.Empty;
                string PIDNo = string.Empty;
                string Areas2 = string.Empty;
                string Requestor = string.Empty;
                string Project_Name = string.Empty;
                foreach (var key in ViewData.ModelState.Keys)
                {
                    string modelStateVal = ViewData.ModelState[key].ToString();
                    var currentKeyValue = ModelState[key].Value.AttemptedValue;
                    switch (key)
                    {
                        case "Areas":
                            Areas = currentKeyValue;
                            break;
                        case "Eq_Types":
                            Eq_Types = currentKeyValue;
                            break;
                        case "Seq_No":
                            Seq_No = currentKeyValue;
                            break;
                        case "Eq_name":
                            Eq_name = currentKeyValue;
                            break;
                        case "PIDNo":
                            PIDNo = currentKeyValue;
                            break;
                        case "Areas2":
                            Areas2 = currentKeyValue;
                            break;
                        case "Requestor":
                            Requestor = currentKeyValue;
                            break;
                        case "Project_Name":
                            Project_Name = currentKeyValue;
                            break;
                        default:
                            Console.WriteLine("Unknown value");
                            break;
                    }

                }
                if (AddEquipmentTypeDetails(Areas, Eq_Types, Seq_No, Eq_name, PIDNo, Areas2, Requestor, Project_Name))
                {
                    ViewBag.Message = "Equipment  details added successfully";
                }


                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult getValues(string RowValues)
        {

            string ValRow = RowValues;

            //string[] authorsList = ValRow.Split(", ");
            List<string> names = ValRow.Split(',').ToList<string>();
            //foreach (string author in authorsList)
            //Console.WriteLine(author);
            string InstrumentType = names[0];
            if (InstrumentType == "")
            {
                ViewBag.InstrumentType = "N/A";
            }
            else
            {
                ViewBag.InstrumentType = InstrumentType;
            }
            string TagName = names[1];
            if (TagName == "")
            {
                ViewBag.TagName = "N/A";
            }
            else
            {
                ViewBag.TagName = TagName;
            }
            string LoopNo = names[2];
            if (LoopNo == "")
            {
                ViewBag.LoopNo = "N/A";
            }
            else
            {
                ViewBag.LoopNo = LoopNo;
            }
            string Description = names[3];
            if (Description == "")
            {
                ViewBag.Description = "N/A";
            }
            else
            {
                ViewBag.Description = Description;
            }
            string Type = names[4];
            if (Type == "")
            {
                ViewBag.Type = "N/A";
            }
            else
            {
                ViewBag.Type = Type;
            }
            string Area = names[5];
            if (Area == "")
            {
                ViewBag.Area = "N/A";
            }
            else
            {
                ViewBag.Area = Area;
            }
            string PID = names[6];
            if (PID == "")
            {
                ViewBag.PID = "N/A";
            }
            else
            {
                ViewBag.PID = PID;
            }
            string ModelNo = names[7];
            if (ModelNo == "")
            {
                ViewBag.ModelNo = "N/A";
            }
            else
            {
                ViewBag.ModelNo = ModelNo;
            }
            string NeedsUpdate = names[8];
            if (NeedsUpdate == "")
            {
                ViewBag.NeedsUpdate = "N/A";
            }
            else
            {
                ViewBag.NeedsUpdate = NeedsUpdate;
            }
            string SpecInfo = names[9];
            if (SpecInfo == "")
            {
                ViewBag.SpecInfo = "N/A";
            }
            else
            {
                ViewBag.SpecInfo = SpecInfo;
            }
            //return RedirectToAction("InstDescription", "Home");
            //return View("InstDescription");

            return Json("Success", JsonRequestBehavior.AllowGet);
            //if(ValRow=="")
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }


        //To Add Equipment details
        public bool AddEquipmentTypeDetails(string Areas, string Eq_Types, string Seq_No, string Eq_name, string PIDNo, string Areas2, string Requestor, string Project_Name)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;
                SqlConnection con = new SqlConnection(ConnectionString);

                //15-C-5564
                List<SelectListItem> Area = ViewBag.Area;

                string EQ = Areas + "-" + Eq_Types + "-" + Seq_No;
                SqlCommand cmd_InsertEqDetails = new SqlCommand("InsertEquipmentType", con);
                cmd_InsertEqDetails.CommandType = CommandType.StoredProcedure;
                cmd_InsertEqDetails.Parameters.AddWithValue("@Area", Areas);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Eq_Type", Eq_Types);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Tag", Seq_No);
                cmd_InsertEqDetails.Parameters.AddWithValue("@P_ID", PIDNo);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Eq", EQ);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Area2", Areas2);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Equipment_Name", Eq_name);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Requestor", Requestor);
                cmd_InsertEqDetails.Parameters.AddWithValue("@Project_Name", Project_Name);

                con.Open();
                int i = cmd_InsertEqDetails.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {

                    return true;

                }
                else
                {

                    return false;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
