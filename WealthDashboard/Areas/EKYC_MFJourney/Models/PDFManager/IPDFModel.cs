using iTextSharp.text.pdf;
using System.Data;

namespace WealthDashboard.Areas.EKYC_MFJourney.Models.PDFManager
{
    public interface IPDFModel
    {
        Task<MainModel> GetDataFromDatabase(int RegistrationId);

        Task<string> CreateDirectory(int RegistrationId, string BAcode, string PDFSelect, string EsignType, MainModel model);

        Task<bool> DocUploadDetails(MainModel model, DataTable DtUplaodDetails, PdfReader pdfReader, int addAttchmentPageNo,
                       PdfStamper stamper, PersonalDetailsModel mPersonalDetailsModel,
                       PennyDropDetailsModel mPennyDropDetailsModel, ClientPermanentAddressModel mClientPermanentAddressModel,
                       int RegistrationId, string PDFType, string EsignType, UploadGEOTag uploadGEO);


        Task<DataTable> ConvertListToDataTable(List<UploadDocDetailsModel> upload);
        Task<DataTable> ConvertListToDataTable(UploadPhotoDocs photo);
        Task<DataTable> ConvertListToDataTable(UploadSignDocs sign);

        Task<bool> ClientPhoto(MainModel model, int PageAddedCount, PdfReader pdfReader, PdfStamper stamper, string PDFType);

        Task<DataTable> GetClientPhoto(MainModel model, int RegistraionId);
        Task<DataTable> GetClientSign(MainModel model, int RegistraionId);
        Task<bool> ClientSignature(MainModel model, DataTable DtClientSign, int PageNo, PdfStamper stamper, string PDFType);
        Task<bool> AuthEmpSign(int PageNo, PdfStamper stamper, string PDFType);
        Task<bool> ClientNomineeEsign_BOI(PersonalDetailsModel mPersonalDetailsModel, ClientPermanentAddressModel mClientPermanentAddressModel,
                   int PageAddedCount, PdfStamper stamper, string PDFType, string EsignType);
        Task<bool> ClientNomineeEsign(MainModel model, int PageAddedCount, PdfStamper stamper, string PDFType, string EsignType);
    }
}
