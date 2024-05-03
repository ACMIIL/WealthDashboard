namespace WealthDashboard.Areas.EKYC_MFJourney.Models
{
    public class ClientGeoTaggingDetailsModel
    {
        public int GeoTagId { get; set; }
        public string InwardNo { get; set; }
        public string IPAddress { get; set; }
        public string IPType { get; set; }
        public string ContinentName { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
