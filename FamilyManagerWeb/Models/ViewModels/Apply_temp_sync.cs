﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyManagerWeb.Models.ViewModels
{
    class Apply_temp_sync_VM
    {
        public int ID { get; set; }
        public int userID { get; set; }
        public System.DateTime applyDate { get; set; }
        public string keepType { get; set; }
        public int flowTypeID { get; set; }
        public string flowTypeName { get; set; }
        public string inOutType { get; set; }
        public int feeItemID { get; set; }
        public string feeItemName { get; set; }
        public decimal imoney { get; set; }
        public int inUserBankID { get; set; }
        public int outUserBankID { get; set; }
        public string cAdd { get; set; }
    }
}
