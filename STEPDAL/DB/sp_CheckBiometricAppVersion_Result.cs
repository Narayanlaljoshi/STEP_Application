//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STEPDAL.DB
{
    using System;
    
    public partial class sp_CheckBiometricAppVersion_Result
    {
        public string Machine_Name { get; set; }
        public string Current_Version { get; set; }
        public System.DateTime LastUpdatedOn { get; set; }
        public string LatestVersion { get; set; }
        public Nullable<bool> IsCurrentVersionIsLatest { get; set; }
        public string Link { get; set; }
    }
}
