using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STEPDAL.DB;
using System.Web;

namespace STEPDAL.CustomDAL
{
    public class BiometricReports
    {
        public static void ExportExcel_RegistrationData()
        {
            string Url = GeneraetExcel_Biometric();
            string Body = "<html><body><h3>Dear San,</h3><b>Greetings for the day!!</b>";
            Body += "<p>Please find the attachment for the biometric report.  </p>";
            //Body = Body + @"<table border=\"" +1+\""style=\""text- align:center; \""><thead><tr><th>#</th><th>Agency Code</th><th>MSPIN</th></tr></thead>";
            Body += "<p>Thank You.</p><p> Regards </p><p> STEP Portal </p>";
            Body += "<p>** This is an auto generated mail, please do not reply.</p></body></html>";

            Email.sendEmail_WithReports("Biometric Report as on - "+ DateTime.Now.ToString("dd-MMM-yyyy"), Body, Url);
        }

        private static string GeneraetExcel_Biometric()
        {

            List<sp_GetBiometricReport_Result> ReportData = new List<sp_GetBiometricReport_Result>();
            using (var Context = new CEIDBEntities())
            {
                ReportData = Context.sp_GetBiometricReport().ToList();
            }

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            //Header of table  
            //
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[1, 1].Value = "Agency Code";
            workSheet.Cells[1, 2].Value = "Faculty Code";
            workSheet.Cells[1, 3].Value = "MSPIN";
            workSheet.Cells[1, 4].Value = "Registration Date";
            //Body of table  
            //
            int recordIndex = 2;
            foreach (var item in ReportData)
            {
                workSheet.Cells[recordIndex, 1].Value = item.AgencyCode;
                workSheet.Cells[recordIndex, 2].Value = item.FAcultyCode;
                workSheet.Cells[recordIndex, 3].Value = item.MSPIN;
                workSheet.Cells[recordIndex, 4].Value = item.CreationDate.ToString("dd-MMM-yyyy HH:mm tt");
                recordIndex++;
            }
            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();
            workSheet.Column(4).AutoFit();
            
            string FileName =HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Reports"])+"BiometricReport_" + DateTime.Now.ToString("dd-MMM-yyyy")+".xlsx";
            string FolderPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Reports"]);
            //if()
            excel.SaveAs(new FileInfo(FileName));
            excel.Dispose();

            return FileName;

        }
    }
}
