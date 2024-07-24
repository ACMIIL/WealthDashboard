using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using WealthDashboard.Configuration;
using WealthDashboard.Models;
//using TWCLandingPage.Configuration;
//using TWCLandingPage.Models;

public class CommanModel
{
    private readonly ConnectionStrings _connectionStrings;

    #region constructor
    public CommanModel(IOptions<ConnectionStrings> connectionStrings)
    {
        _connectionStrings = connectionStrings.Value;
    }
    #endregion

    #region  MainManuSliders
    //__________________________________MainManuSliders_________________________________________________________
    public async Task<ResultModel> GetMainSliderValue()
    {
        try
        {
            string folderPath = @"D:\LandingPageImages\Sliders";
            List<string> imageUrls = new List<string>();

            // Get image URLs from the specified folder
            if (Directory.Exists(folderPath))
            {
                var images = Directory.GetFiles(folderPath, "*.*")
                                      .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".jpeg") || s.EndsWith(".PNG") || s.EndsWith(".JPG"))
                                      .ToList();

                foreach (var image in images)
                {
                    var fileName = Path.GetFileName(image);
                    var imageUrl = $"/LandingPageImages/Sliders/{fileName}";
                    imageUrls.Add(imageUrl);
                }
            }

            // Database operations to fetch slider data
            using (var connection = new SqlConnection(_connectionStrings.TWC))
            {
                var builder = new SqlConnectionStringBuilder(_connectionStrings.TWC)
                {
                    Encrypt = true,
                    TrustServerCertificate = true
                };
                connection.ConnectionString = builder.ConnectionString;

                await connection.OpenAsync();

                // Execute stored procedure to fetch data
                using (var command = new SqlCommand("[dbo].[ManageMainSliderData]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@option", 1);

                    var sliderData = new List<MainSliderData>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            MainSliderData mainSliderData = new MainSliderData();
                            mainSliderData.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            mainSliderData.Header = reader.IsDBNull(reader.GetOrdinal("header")) ? null : reader.GetString(reader.GetOrdinal("header"));
                            mainSliderData.Content = reader.IsDBNull(reader.GetOrdinal("content")) ? null : reader.GetString(reader.GetOrdinal("content"));
                            mainSliderData.Url = reader.IsDBNull(reader.GetOrdinal("Url")) ? null : reader.GetString(reader.GetOrdinal("Url"));
                            mainSliderData.SliderImages = reader.IsDBNull(reader.GetOrdinal("sliderimages")) ? null : reader.GetString(reader.GetOrdinal("sliderimages"));
                            mainSliderData.Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? false : reader.GetBoolean(reader.GetOrdinal("Active"));
                            mainSliderData.NumberingPosition = reader.IsDBNull(reader.GetOrdinal("NumberingPosition")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumberingPosition"));
                            sliderData.Add(mainSliderData);
                        }
                    }

                    // Return result model with combined data
                    return new ResultModel()
                    {
                        Code = HttpStatusCode.OK,
                        Message = "ok",
                        Data = new { Images = imageUrls, SliderData = sliderData }
                    };
                }
            }
        }
        catch (Exception ex)
        {
            return new ResultModel()
            {
                Code = HttpStatusCode.InternalServerError,
                Message = "Error fetching slider value: " + ex.Message,
                Data = null
            };
        }
    }


    #endregion

    #region MainManuContent1
    //__________________________________MainManuContent1_________________________________________________________
    public async Task<ResultModel> GetMainContent1Value()
    {
        try
        {

            using (var connection = new SqlConnection(_connectionStrings.TWC))
            {
                var builder = new SqlConnectionStringBuilder(_connectionStrings.TWC)
                {
                    Encrypt = true,
                    TrustServerCertificate = true
                };
                connection.ConnectionString = builder.ConnectionString;

                await connection.OpenAsync();

                // Execute stored procedure to fetch data
                using (var command = new SqlCommand("[dbo].[MainContentData]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@option", 1);

                    var sliderData = new List<MainContent1>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            MainContent1 mainContent1Data = new MainContent1();
                            mainContent1Data.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            mainContent1Data.Header = reader.IsDBNull(reader.GetOrdinal("header")) ? null : reader.GetString(reader.GetOrdinal("header"));
                            mainContent1Data.Content = reader.IsDBNull(reader.GetOrdinal("content")) ? null : reader.GetString(reader.GetOrdinal("content"));
                            mainContent1Data.Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? false : reader.GetBoolean(reader.GetOrdinal("Active"));
                            mainContent1Data.NumberingPosition = reader.IsDBNull(reader.GetOrdinal("NumberingPosition")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumberingPosition"));
                            sliderData.Add(mainContent1Data);
                        }
                    }

                    // Return result model with combined data
                    return new ResultModel()
                    {
                        Code = HttpStatusCode.OK,
                        Message = "ok",
                        Data = new { SliderData = sliderData }
                    };
                }
            }
        }
        catch (Exception ex)
        {
            return new ResultModel()
            {
                Code = HttpStatusCode.InternalServerError,
                Message = "Error fetching slider value: " + ex.Message,
                Data = null
            };
        }
    }


    #endregion

    #region MainManuContent2
    //__________________________________MainManuContent2_________________________________________________________
    public async Task<ResultModel> GetMainContent2Value()
    {
        try
        {

            using (var connection = new SqlConnection(_connectionStrings.TWC))
            {
                var builder = new SqlConnectionStringBuilder(_connectionStrings.TWC)
                {
                    Encrypt = true,
                    TrustServerCertificate = true
                };
                connection.ConnectionString = builder.ConnectionString;

                await connection.OpenAsync();

                // Execute stored procedure to fetch data
                using (var command = new SqlCommand("[dbo].[ManageContent2Data]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@option", 1);

                    var sliderData = new List<MainContent2>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            MainContent2 MainContent2Data = new MainContent2();
                            MainContent2Data.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            MainContent2Data.Content = reader.IsDBNull(reader.GetOrdinal("content")) ? null : reader.GetString(reader.GetOrdinal("content"));
                            MainContent2Data.Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? false : reader.GetBoolean(reader.GetOrdinal("Active"));
                            MainContent2Data.NumberingPosition = reader.IsDBNull(reader.GetOrdinal("NumberingPosition")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumberingPosition"));
                            sliderData.Add(MainContent2Data);
                        }
                    }

                    // Return result model with combined data
                    return new ResultModel()
                    {
                        Code = HttpStatusCode.OK,
                        Message = "ok",
                        Data = new { SliderData = sliderData }
                    };
                }
            }
        }
        catch (Exception ex)
        {
            return new ResultModel()
            {
                Code = HttpStatusCode.InternalServerError,
                Message = "Error fetching slider value: " + ex.Message,
                Data = null
            };
        }
    }


    #endregion

    #region How__TWC_Help
    //__________________________________How__TWC_Help _________________________________________________________
    public async Task<ResultModel> GetHValue()
    {
        try
        {
            string folderPath = @"D:\LandingPageImages\Sliders";
            List<string> imageUrls = new List<string>();

            // Get image URLs from the specified folder
            if (Directory.Exists(folderPath))
            {
                var images = Directory.GetFiles(folderPath, "*.*")
                                      .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".jpeg") || s.EndsWith(".PNG") || s.EndsWith(".JPG") || s.EndsWith(".gif") || s.EndsWith(".GIF"))
                                      .ToList();

                foreach (var image in images)
                {
                    var fileName = Path.GetFileName(image);
                    var imageUrl = $"/LandingPageImages/Sliders/{fileName}";
                    imageUrls.Add(imageUrl);
                }
            }

            // Database operations to fetch slider data
            using (var connection = new SqlConnection(_connectionStrings.TWC))
            {
                var builder = new SqlConnectionStringBuilder(_connectionStrings.TWC)
                {
                    Encrypt = true,
                    TrustServerCertificate = true
                };
                connection.ConnectionString = builder.ConnectionString;

                await connection.OpenAsync();

                // Execute stored procedure to fetch data
                using (var command = new SqlCommand("[dbo].[ManageHoTWCHelpData]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@option", 1);

                    var sliderData = new List<ModelHoTWCHelpData>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ModelHoTWCHelpData HowTWChelpData = new ModelHoTWCHelpData();
                            HowTWChelpData.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            HowTWChelpData.Header = reader.IsDBNull(reader.GetOrdinal("header")) ? null : reader.GetString(reader.GetOrdinal("header"));
                            HowTWChelpData.HeaderContent = reader.IsDBNull(reader.GetOrdinal("headercontent")) ? null : reader.GetString(reader.GetOrdinal("headercontent"));
                            HowTWChelpData.Content = reader.IsDBNull(reader.GetOrdinal("content")) ? null : reader.GetString(reader.GetOrdinal("content"));
                            HowTWChelpData.SliderImages = reader.IsDBNull(reader.GetOrdinal("sliderimages")) ? null : reader.GetString(reader.GetOrdinal("sliderimages"));
                            HowTWChelpData.Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? false : reader.GetBoolean(reader.GetOrdinal("Active"));
                            HowTWChelpData.NumberingPosition = reader.IsDBNull(reader.GetOrdinal("NumberingPosition")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumberingPosition"));
                            sliderData.Add(HowTWChelpData);
                        }
                    }

                    // Return result model with combined data
                    return new ResultModel()
                    {
                        Code = HttpStatusCode.OK,
                        Message = "ok",
                        Data = new { Images = imageUrls, SliderData = sliderData }
                    };
                }
            }
        }
        catch (Exception ex)
        {
            return new ResultModel()
            {
                Code = HttpStatusCode.InternalServerError,
                Message = "Error fetching slider value: " + ex.Message,
                Data = null
            };
        }
    }


    #endregion

    #region Range__OF_Products
    //__________________________________Range__OF_Products _________________________________________________________
    public async Task<ResultModel> GetDataRangeOfProducts()
    {
        try
        {
            string folderPath = @"D:\LandingPageImages\Sliders";
            List<string> imageUrls = new List<string>();

            // Get image URLs from the specified folder
            if (Directory.Exists(folderPath))
            {
                var images = Directory.GetFiles(folderPath, "*.*")
                                      .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".jpeg") || s.EndsWith(".PNG") || s.EndsWith(".JPG") || s.EndsWith(".gif") || s.EndsWith(".GIF"))
                                      .ToList();

                foreach (var image in images)
                {
                    var fileName = Path.GetFileName(image);
                    var imageUrl = $"/LandingPageImages/Sliders/{fileName}";
                    imageUrls.Add(imageUrl);
                }
            }

            // Database operations to fetch slider data
            using (var connection = new SqlConnection(_connectionStrings.TWC))
            {
                var builder = new SqlConnectionStringBuilder(_connectionStrings.TWC)
                {
                    Encrypt = true,
                    TrustServerCertificate = true
                };
                connection.ConnectionString = builder.ConnectionString;

                await connection.OpenAsync();

                // Execute stored procedure to fetch data
                using (var command = new SqlCommand("[dbo].[ManageRangeOfProductsData]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@option", 1);

                    var sliderData = new List<RangeOfProduct>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            RangeOfProduct RangeOfData = new RangeOfProduct();
                            RangeOfData.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            RangeOfData.Header = reader.IsDBNull(reader.GetOrdinal("Products")) ? null : reader.GetString(reader.GetOrdinal("Products"));
                            RangeOfData.Content = reader.IsDBNull(reader.GetOrdinal("content")) ? null : reader.GetString(reader.GetOrdinal("content"));
                            RangeOfData.SliderImages = reader.IsDBNull(reader.GetOrdinal("sliderimages")) ? null : reader.GetString(reader.GetOrdinal("sliderimages"));
                            RangeOfData.Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? false : reader.GetBoolean(reader.GetOrdinal("Active"));
                            RangeOfData.NumberingPosition = reader.IsDBNull(reader.GetOrdinal("NumberingPosition")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumberingPosition"));
                            sliderData.Add(RangeOfData);
                        }
                    }

                    // Return result model with combined data
                    return new ResultModel()
                    {
                        Code = HttpStatusCode.OK,
                        Message = "ok",
                        Data = new { Images = imageUrls, SliderData = sliderData }
                    };
                }
            }
        }
        catch (Exception ex)
        {
            return new ResultModel()
            {
                Code = HttpStatusCode.InternalServerError,
                Message = "Error fetching slider value: " + ex.Message,
                Data = null
            };
        }
    }


    #endregion


    #region Meet Our Team
    //__________________________________Meet Our Team _________________________________________________________
    public async Task<ResultModel> GetDataMeetTeam()
    {
        try
        {
            string folderPath = @"D:\LandingPageImages\Sliders";
            List<string> imageUrls = new List<string>();

            // Get image URLs from the specified folder
            if (Directory.Exists(folderPath))
            {
                var images = Directory.GetFiles(folderPath, "*.*")
                                      .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".jpeg") || s.EndsWith(".PNG") || s.EndsWith(".JPG") || s.EndsWith(".gif") || s.EndsWith(".GIF"))
                                      .ToList();

                foreach (var image in images)
                {
                    var fileName = Path.GetFileName(image);
                    var imageUrl = $"/LandingPageImages/Sliders/{fileName}";
                    imageUrls.Add(imageUrl);
                }
            }

            // Database operations to fetch slider data
            using (var connection = new SqlConnection(_connectionStrings.TWC))
            {
                var builder = new SqlConnectionStringBuilder(_connectionStrings.TWC)
                {
                    Encrypt = true,
                    TrustServerCertificate = true
                };
                connection.ConnectionString = builder.ConnectionString;

                await connection.OpenAsync();

                // Execute stored procedure to fetch data
                using (var command = new SqlCommand("[dbo].[ManageMeetOurTeamData]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@option", 1);

                    var sliderData = new List<MeetOurTeam>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            MeetOurTeam MeetOurTeamData = new MeetOurTeam();
                            MeetOurTeamData.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            MeetOurTeamData.Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name"));
                            MeetOurTeamData.Position = reader.IsDBNull(reader.GetOrdinal("Position")) ? null : reader.GetString(reader.GetOrdinal("Position"));
                            MeetOurTeamData.Content = reader.IsDBNull(reader.GetOrdinal("content")) ? null : reader.GetString(reader.GetOrdinal("content"));
                            MeetOurTeamData.SliderImages = reader.IsDBNull(reader.GetOrdinal("sliderimages")) ? null : reader.GetString(reader.GetOrdinal("sliderimages"));
                            MeetOurTeamData.Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? false : reader.GetBoolean(reader.GetOrdinal("Active"));
                            MeetOurTeamData.NumberingPosition = reader.IsDBNull(reader.GetOrdinal("NumberingPosition")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumberingPosition"));
                            sliderData.Add(MeetOurTeamData);
                        }
                    }

                    // Return result model with combined data
                    return new ResultModel()
                    {
                        Code = HttpStatusCode.OK,
                        Message = "ok",
                        Data = new { Images = imageUrls, SliderData = sliderData }
                    };
                }
            }
        }
        catch (Exception ex)
        {
            return new ResultModel()
            {
                Code = HttpStatusCode.InternalServerError,
                Message = "Error fetching slider value: " + ex.Message,
                Data = null
            };
        }
    }

    #endregion 
}
