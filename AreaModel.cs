using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstrumentDatabase.Model
{
    public class AreaModel
    {
        
        [Required(ErrorMessage = "Area is required.")]
        public IEnumerable<SelectListItem> Areas { get; set; }


       // [Required(ErrorMessage = "Area2 is required.")]
        //public string Area { get; set; }

        //public string Areaid { get; set; }


        [Required(ErrorMessage = "Area2 is required.")]
        public IEnumerable<SelectListItem> Areas2 { get; set; }



        //public string Area2 { get; set; }

        //public string Areaid2 { get; set; }


        [Required(ErrorMessage = "Equipment is required.")]
        public IEnumerable<SelectListItem> Eq_Types { get; set; }

        //public string Eq_Type { get; set; }

        //public string Eq_Typeid { get; set; }

        [Required(ErrorMessage = "SequenceNo is required.")]
        public int Seq_No { get; set; }

        [Required(ErrorMessage = "EquipmentName is required.")]
        public string Eq_name { set; get; }

        [Required(ErrorMessage = "PIDNo is required.")]
        public string PIDNo { set; get; }

        [Required(ErrorMessage = "Requestor is required.")]
        public string Requestor { set; get; }

        [Required(ErrorMessage = "Project Name is required.")]
        public string Project_Name { set; get; }
    }
}