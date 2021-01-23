using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextWinForm.Public
{
    public class WP_ORDERS
    {
        public int ORDERS_ID { get; set; }
        public string JOB_STATUS { get; set; }
        public string JOB_EDGE { get; set; }
        public Nullable<System.DateTime> JOB_DATE { get; set; }
        public string JOB_RX_NUM { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_ADDRESS_STREET { get; set; }
        public string PATIENT_ADDRESS_CITY { get; set; }
        public string PATIENT_ADDRESS_STATE { get; set; }
        public string PATIENT_ADDRESS_ZIP { get; set; }
        public string PATIENT_ADDRESS_PHONE { get; set; }
        public string SHIP_VIA_CARRIER { get; set; }
        public string SHIP_VIA_METHOD { get; set; }
        public string RX_R_SPHERE { get; set; }
        public string RX_R_CYLINDER { get; set; }
        public string RX_R_AXIS { get; set; }
        public string RX_R_PD { get; set; }
        public string RX_R_OC_FROM { get; set; }
        public string RX_R_OC { get; set; }
        public string RX_R_ADD_POWER { get; set; }
        public string RX_R_ADD_NEAR_PD { get; set; }
        public string RX_R_ADD_SEG_HEIGHT { get; set; }
        public string RX_L_SPHERE { get; set; }
        public string RX_L_CYLINDER { get; set; }
        public string RX_L_AXIS { get; set; }
        public string RX_L_PD { get; set; }
        public string RX_L_OC_FROM { get; set; }
        public string RX_L_OC { get; set; }
        public string RX_L_ADD_POWER { get; set; }
        public string RX_L_ADD_NEAR_PD { get; set; }
        public string RX_L_ADD_SEG_HEIGHT { get; set; }
        public string LENS_R_MAT { get; set; }
        public string LENS_R_STYLE { get; set; }
        public string LENS_R_COLOR { get; set; }
        public string LENS_L_MAT { get; set; }
        public string LENS_L_STYLE { get; set; }
        public string LENS_L_COLOR { get; set; }
        public string COAT { get; set; }
        public string FRAME_STATUS { get; set; }
        public string FRAME_MFR { get; set; }
        public string FRAME_STYLE { get; set; }
        public string FRAME_COLOR { get; set; }
        public string FRAME_MAT { get; set; }
        public string FRAME_EDGE { get; set; }
        public string FRAME_EYE_SIZE { get; set; }
        public string FRAME_BRIDGE { get; set; }
        public string FRAME_TEMPLE_LENGTH { get; set; }
        public string FRAME_TEMPLE_TYPE { get; set; }
        public string BARCODE { get; set; }
        public System.DateTime DELIVERY_DATE { get; set; }
        public string CUSTOMER_NUMBER { get; set; }
        public string R_CL_LENS { get; set; }
        public Nullable<int> R_SPH { get; set; }
        public Nullable<int> R_CYL { get; set; }
        public Nullable<int> R_ADD { get; set; }
        public Nullable<int> R_AXIS { get; set; }
        public Nullable<int> R_QTY { get; set; }
        public string L_CL_LENS { get; set; }
        public Nullable<int> L_SPH { get; set; }
        public Nullable<int> L_CYL { get; set; }
        public Nullable<int> L_ADD { get; set; }
        public Nullable<int> L_AXIS { get; set; }
        public Nullable<int> L_QTY { get; set; }
        public string TINT_CODE { get; set; }
        public string TINT_DESC { get; set; }
        public Nullable<bool> UV { get; set; }
        public Nullable<bool> HARD { get; set; }
        public string MC { get; set; }
        public Nullable<bool> POLISH { get; set; }
        public string EDGE_CODE { get; set; }
        public string SUPE_CODE { get; set; }
        public string BIN_LOC { get; set; }
        public string REMARK { get; set; }
        public Nullable<System.DateTime> CREATION_DATE { get; set; }
        public string CREATED_BY_COMPUTER { get; set; }
        public Nullable<bool> EXPORTED { get; set; }
        public Nullable<decimal> CALC_R_SIZE { get; set; }
        public Nullable<decimal> CALC_L_SIZE { get; set; }
        public string ATTRIBUTE1 { get; set; }
        public string ATTRIBUTE2 { get; set; }
        public string ATTRIBUTE3 { get; set; }
        public string ATTRIBUTE4 { get; set; }
        public string ATTRIBUTE5 { get; set; }
        public string ATTRIBUTE6 { get; set; }
        public string ATTRIBUTE7 { get; set; }
        public string ATTRIBUTE8 { get; set; }
        public string ATTRIBUTE9 { get; set; }
        public string ATTRIBUTE10 { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string R_PRISMI { get; set; }
        public string R_PRISMO { get; set; }
        public string R_PRISMU { get; set; }
        public string R_PRISMD { get; set; }
        public string L_PRISMI { get; set; }
        public string L_PRISMO { get; set; }
        public string L_PRISMU { get; set; }
        public string L_PRISMD { get; set; }
    }
}
