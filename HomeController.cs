using DataTableExample.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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



        public ActionResult DataTable()
        {
            return View();
        }
        public ActionResult TabsOverview()
        {
            DataTable dtHome = HomeTabTableView();

            DataTable dtInstrument = InstrumentsTableView();
            DataTable dtVendor = VendorTableView();
            DataTable dtManufacturer = ManufacturerTableView();
            DataTable dtTemplates = TemplatesTableView();
            DataTable dtInstType = InstTypeTableView();
            DataTable dtInfoType = InfoTypeTableView();
            DataTable dtInfoGroups = InfoGroupsTableView();
            DataTable dtLoopTable = LoopTableView();
            DataTable dt_inst = EquipmentData();

            dsFinalTable.Tables.Add(dtHome);
            dsFinalTable.Tables.Add(dtInstrument);
            dsFinalTable.Tables.Add(dtVendor);
            dsFinalTable.Tables.Add(dtManufacturer);
            dsFinalTable.Tables.Add(dtTemplates);
            dsFinalTable.Tables.Add(dtInstType);
            dsFinalTable.Tables.Add(dtInfoType);
            dsFinalTable.Tables.Add(dtInfoGroups);
            dsFinalTable.Tables.Add(dtLoopTable);
            dsFinalTable.Tables.Add(dt_inst);

            return View(dsFinalTable);
        }

        private DataTable InfoGroupsTableView()
        {
            string connectionString = string.Empty;
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Information_Group], [Group_Seq_No] FROM [InstrumentDatabase].[dbo].[InstrumentData] GROUP BY [Information_Group], [Group_Seq_No]", con);

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

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Information_Type],[Unit],[Min_Norm_Max_Field], count(*) as InfoRecordCount FROM [InstrumentDatabase].[dbo].[InstrumentData] GROUP BY [Information_Type],[Unit],[Min_Norm_Max_Field]", con);

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

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Instrument_Type], count(*) as InstrumentCount, [Instrument_Template_Line_Count] FROM [InstrumentDatabase].[dbo].[Instruments] GROUP BY [Instrument_Type],[Instrument_Template_Line_Count]", con);

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

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Seq_No],[Instrument_Type],[Information_Type],[Unit],[Information_Group],[Group_Seq_No], count(*) as InfoRecordCount FROM [InstrumentDatabase].[dbo].[InstrumentData] GROUP BY [Seq_No],[Instrument_Type],[Information_Type],[Unit],[Information_Group],[Group_Seq_No]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;
            con.Close();
            return FinalTable;
        }


        private DataTable LoopTableView()
        {
            string connectionString = string.Empty;
            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Vendor],[Vendor_Contact],[Vendor_PhoneNo], count(*) FROM [InstrumentDatabase].[dbo].[Instruments] GROUP BY [Vendor],[Vendor_Contact],[Vendor_PhoneNo]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;

            return FinalTable;
        }


        private DataTable ManufacturerTableView()
        {
            string connectionString = string.Empty;

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct [Manufacturer],[Manufacturer_PhoneNo] as PhoneNo, count(*) FROM [InstrumentDatabase].[dbo].[Instruments] GROUP BY [Manufacturer],[Manufacturer_PhoneNo]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;

            return FinalTable;
        }

        private DataTable VendorTableView()
        {
            string connectionString = string.Empty;

            string ConnectionString = ConfigurationManager.ConnectionStrings["cons"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);

            //SqlCommand cmdInstrumentCount = new SqlCommand("select Instrument_type,Area ,count(*) as Instrument_Count from Instruments group by Area,Instrument_Type order by Instrument_Type,Area", con);
            SqlCommand cmdInstrumentCount = new SqlCommand("select distinct[Vendor],[Vendor_Contact],[Vendor_PhoneNo], count(*) FROM[InstrumentDatabase].[dbo].[Instruments] GROUP BY[Vendor],[Vendor_Contact],[Vendor_PhoneNo]", con);

            con.Open();
            SqlDataAdapter daInstrumentCount = new SqlDataAdapter(cmdInstrumentCount);
            DataTable dtInstrumentCount = new DataTable();
            daInstrumentCount.Fill(dtInstrumentCount);
            DataTable FinalTable = new DataTable();

            FinalTable = dtInstrumentCount;

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
            SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");
            SqlCommand cmd_AreaLst = new SqlCommand("select * from EquipmentTag  where SNo is not null;", con);
            SqlDataAdapter daArea_Lst = new SqlDataAdapter(cmd_AreaLst);
            DataTable dtArea_Lst = new DataTable();
            daArea_Lst.Fill(dtArea_Lst);
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

            return FinalTable;
        }
        public ActionResult Homepage()
        {
            return View();
        }
        public ActionResult Equipment()
        {
            DataTable DT_Equipment = GetEquipmentData();
            return View(DT_Equipment);
        }

        private DataTable GetEquipmentData()
        {
            DataTable DT_EquipmentData = new DataTable();
            SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");
            string EquipmentData_Str = "select SNo,Area,Eq_Type,Tag,P_ID,Eq,FLC_as_in_Eq_List,FLC_in_FLOC,Area2,Remarks from EquipmentTag  where SNo is not null;";
            SqlCommand cmdEquipmentData = new SqlCommand(EquipmentData_Str, con);
            SqlDataAdapter daEquipmentData = new SqlDataAdapter(cmdEquipmentData);
            daEquipmentData.Fill(DT_EquipmentData);
            return DT_EquipmentData;
        }

        public ActionResult Instrument()
        {
            DataTable DT_InstrumentData = new DataTable();
            SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");
            SqlCommand cmdInstrumentData = new SqlCommand("select *from InstrumentData", con);
            SqlDataAdapter daInstrumentData = new SqlDataAdapter(cmdInstrumentData);
            daInstrumentData.Fill(DT_InstrumentData);
            return View(DT_InstrumentData);
        }

        public static AreaModel cMainareamodel;
        public ActionResult EquipForm()
        {

            AreaModel objAreamodel = new AreaModel();
            cMainareamodel = new AreaModel();
            objAreamodel.Areas = cMainareamodel.Areas = FillList();
            //ViewBag.lblArea = "";


            List<string> Area_Lst = new List<string>();
            SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");
            SqlCommand cmd_AreaLst = new SqlCommand("select distinct area from EquipmentTag  where SNo is not null;", con);
            SqlDataAdapter daArea_Lst = new SqlDataAdapter(cmd_AreaLst);
            DataTable dtArea_Lst = new DataTable();
            daArea_Lst.Fill(dtArea_Lst);
            for (int i = 0; i < dtArea_Lst.Rows.Count; i++)
            {
                Area_Lst.Add(dtArea_Lst.Rows[i]["Area"].ToString());
            }
            ViewData["Area"] = Area_Lst;
            ViewBag.Area_Lst = Area_Lst;
            //MyEntities e = new MyEntities();


            #region EquipmentType

            int Area = 0;
            string EqType = "select distinct Eq_Type from EquipmentTag where area=" + Area;
            SqlCommand cmd_EqType = new SqlCommand(EqType, con);
            SqlDataAdapter daEqType = new SqlDataAdapter(cmd_EqType);
            DataTable dtEqType = new DataTable();
            daEqType.Fill(dtEqType);

            #endregion

            #region SequenceNo

            string str_SequenceNo = "SELECT MAX(Tag) as Sequence FROM EquipmentTag where SNo is not null";
            SqlCommand cmdSequenceNo = new SqlCommand(str_SequenceNo, con);
            SqlDataAdapter daSequenceNo = new SqlDataAdapter(cmdSequenceNo);
            DataTable dtSequenceNo = new DataTable();
            daSequenceNo.Fill(dtSequenceNo);
            #endregion

            //return View(Area_Lst);

            return View(objAreamodel);
        }


        [HttpPost]
        public ActionResult Index(AreaModel areamodel)
        {
            var g = cMainareamodel.Areas;
            var selectedCountry = g.Find(p => p.Value == areamodel.Areaid.ToString());
            areamodel.Areas = FillList();
            ViewBag.LblCountry = "You selected " + selectedCountry.Text.ToString();
            return View(areamodel);
        }

        private List<SelectListItem> FillList()
        {
            var list = new List<SelectListItem>();
            SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = true");
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

        public ActionResult InstForm()
        {
            #region GetInst_type

            SqlConnection con = new SqlConnection("data source =.; database = InstrumentDatabase; integrated security = SSPI");
            SqlCommand cmdInst_type = new SqlCommand("select distinct Instrument_Type from InstrumentData order by Instrument_Type ASC;", con);
            SqlDataAdapter daInst_type = new SqlDataAdapter(cmdInst_type);
            DataTable dtInst_type = new DataTable();
            daInst_type.Fill(dtInst_type);
            List<string> Lst_Insttype = new List<string>();
            for (int i = 0; i < dtInst_type.Rows.Count; i++)
            {
                Lst_Insttype.Add(dtInst_type.Rows[i]["Instrument_Type"].ToString());
            }
            #endregion

            #region GetSeq_No

            SqlCommand cmdSeq_No = new SqlCommand("select max(Seq_no)  from InstrumentData", con);
            SqlDataAdapter daSeq_No = new SqlDataAdapter(cmdSeq_No);
            DataTable dtSeq_No = new DataTable();
            daInst_type.Fill(dtSeq_No);
            int seq_no = Convert.ToInt16(dtSeq_No.Rows[0]["Max_Seqno"].ToString());

            #endregion

            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }
    }
}
