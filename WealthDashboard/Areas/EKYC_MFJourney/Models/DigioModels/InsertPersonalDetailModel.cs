namespace WealthDashboard.Areas.EKYC_MFJourney.Models.DigioModels
{
    public class InsertPersonalDetailModel
    {
        public int PersonalDetailsId { get; set; } = 0;
        public int RegistrationId { get; set; }
        public int Title { get; set; } = 0;
        public string Bacode { get; set; } = "";
        public string ClientFullName { get; set; } = "";
        public string ClientFirstName { get; set; } = "";
        public string ClientMiddleName { get; set; } = "";
        public string ClientLastName { get; set; } = "";
        public string ClientMotherName { get; set; } = "";
        public int ClientCategoryId { get; set; } = 0;
        public int ClientHolderId { get; set; } = 0;
        public DateTime DateOfBirth { get; set; }
        public string PAN { get; set; } = "";
        public string UID { get; set; } = "";
        public int Gender { get; set; } = 0;
        public int Education { get; set; } = 0;
        public int ClientsRelationId { get; set; } = 0;
        public int MaritalStatus { get; set; } = 0;
        public int OccupationType { get; set; } = 0;
        public string Telephone1 { get; set; } = "";
        public string Telephone2 { get; set; } = "";
        public string EmailId { get; set; } = "";
        public string MobileNo { get; set; } = "";
        // public int AddressDetailsId { get; set; }
        public bool SameAddress { get; set; }
        public string UserId { get; set; } = "";
        public int Pincode { get; set; } = 0;
        public string? Address { get; set; }
        public int investmentExperienceId { get; set; }
        public int annualIncomeId { get; set; }
        public string networth { get; set; }
        public string fatherfirstname { get; set; }
        public string fathermiddlename { get; set; }
        public string fatherlastname { get; set; }
    }
}
