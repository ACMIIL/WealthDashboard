namespace WealthDashboard.Configuration
{
    public class Appsetting
    {
        #region MF EKYC
        public string DigioBaseUrl { get; set; }
        public string CntrlzDigiSuccRespoBaseurl { get; set; }
        public string DigioAuthorization { get; set; }
        public string sourceType { get; set; }
        public string DigioFileDownload { get; set; }
        public string PDFNewFilePath { get; set; }
        public string IsLiveEnvironment { get; set; }
        public string EKYC_apiBaseUrl { get; set; }
        public string ClientFilePath { get; set; }
        public string FromEmailId { get; set; }
        public string smtpClientEmail { get; set; }
        public string EmailLoginId { get; set; }
        public string EmailPassword { get; set; }
        public string AccountOpenSubject { get; set; }
        public string CommonPageURL { get; set; }
        public string DirectClient { get; set; }
        public string GetDPBaseURL { get; set; }
        public string GetDPDataURL { get; set; }
        public string Incomeprooffilepath { get; set; }
        public string UplodDocument { get; set; }
        public string EsignBaseurl { get; set; }
        public string PagestaticCount { get; set; }
        public string PagestaticBOICount { get; set; }

        //CVLKRA URL

        public string CentralizeCVLKRAUrl { get; set; }

        // PDF Generation
        public string BaCode { get; set; }
        public string BOID { get; set; }
        public string EmpFullName { get; set; }
        public string EmpACMCode { get; set; }
        public string EmpDesignation { get; set; }
        public string CompName { get; set; }
        public string CompCode { get; set; }
        public string CompBranch { get; set; }
        public string PDFBlank_BOI { get; set; }
        public string addAttchmentPageNo { get; set; }
        public string ReasonForEsign { get; set; }
        public string EsignYCor { get; set; }

        public string EsignHeight { get; set; }

        public string PDFBlank_Normal { get; set; }
        public string AuthorizedSignPath { get; set; }
        public string EsignStamp { get; set; }
        public string AuthorizedSameer { get; set; }
        public string AuthorizedEmpSignPath { get; set; }
        public string PDFCreateFolder { get; set; }


        public string MFAPIBaseURL { get; set; }
        #endregion
    }
}
