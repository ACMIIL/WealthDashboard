using System.Net;

namespace WealthDashboard.Models
{
    public class ResultModel
    {
        public HttpStatusCode Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
   
    public class MainSliderData
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public int NumberingPosition { get; set; }
        public string SliderImages { get; set; }

    }


    public class DeleteData
    {
        public int id { get; set; }
        public string ImagePath { get; set; }


    }

    public class addrow
    {

        public string newHeader { get; set; }
        public string newContent { get; set; }
        public string newUrl { get; set; }
        public string newimagesName { get; set; }
        public IFormFile newSliderImages { get; set; }


    }

    public class UpdateRowData
    {
        public int idToUpdate { get; set; }
        public string newHeader { get; set; }
        public string newContent { get; set; }
        public string newUrl { get; set; }
        public string newimagesName { get; set; }

        public IFormFile newSliderImages { get; set; }


    }
    public class MainContent1
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public bool Active { get; set; }
        public int NumberingPosition { get; set; }


    }

    public class MainContent2
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public bool Active { get; set; }
        public int NumberingPosition { get; set; }


    }

    public class ModelHoTWCHelpData
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string HeaderContent { get; set; }
        public string Content { get; set; }
        public string SliderImages { get; set; }
        public bool Active { get; set; }
        public int NumberingPosition { get; set; }

    }

    public class RangeOfProduct
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string HeaderContent { get; set; }
        public string Content { get; set; }
        public string SliderImages { get; set; }
        public bool Active { get; set; }
        public int NumberingPosition { get; set; }

    }


    public class UpdateRowHoTWCHelpData
    {
        public int idToUpdate { get; set; }
        public string header { get; set; }
        public string headercontent { get; set; }
        public string Content { get; set; }
        public string newimagesName { get; set; }

        public IFormFile newSliderImages { get; set; }


    }
    public class addrowTWCHelpData
    {

        public string Header { get; set; }
        public string HeaderContent { get; set; }
        public string content { get; set; }
        public string newimagesName { get; set; }
        public IFormFile newSliderImages { get; set; }


    }
    public class addrowRangeOfProductData
    {

        public string Header { get; set; }

        public string content { get; set; }
        public string newimagesName { get; set; }
        public IFormFile newSliderImages { get; set; }


    }
    public class MeetOurTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Content { get; set; }
        public string SliderImages { get; set; }

        public bool Active { get; set; }
        public int NumberingPosition { get; set; }

    }
    public class UpdateMeetOurTeamData
    {
        public int idToUpdate { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Content { get; set; }
        public string newimagesName { get; set; }

        public IFormFile newSliderImages { get; set; }


    }
}
