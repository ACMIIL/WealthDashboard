using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Net;
using WealthDashboard.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WealthDashboard.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OTPVerify()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SendOtp()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken] // Uncomment this line if you're using AntiForgeryToken
        public IActionResult SendOtp(string phoneNumber)
        {
            try
            {
                // Store phone number for OTP verification
                TempData["PhoneNumber"] = phoneNumber;

                // Generate and store OTP only in TempData
                TempData["GeneratedOtp"] = Verification.GenerateOTP(_configuration);

                // Call the SendingOtp method to send the generated OTP
                var sendingOtpResult = Verification.SendingOtp(phoneNumber, _configuration, TempData);

                // Check if sending OTP was successful
                if (sendingOtpResult.Status == enResultStatuslogin.Success)
                {
                    // Optionally, you can log or handle the success case
                    var successMessage = "OTP sent successfully!";
                    return Json(new { success = true, message = successMessage });
                }
                else
                {
                    // Handle the case where sending OTP failed
                    ModelState.AddModelError(string.Empty, sendingOtpResult.Message);
                    return Json(new { success = false, message = sendingOtpResult.Message });
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine(ex.ToString());

                // Add an error message to ModelState if needed
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");

                // Return a JSON response with the error message
                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult VerifyOtp(string otp, string Mobile)
        {
            try
            {
                // Get the stored phone number and generated OTP from TempData
                var phoneNumber = TempData["PhoneNumber"] as string;

                // Validate the phone number and generated OTP
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    ModelState.AddModelError(string.Empty, "Invalid request");
                    return Json(new { success = false, message = "Invalid request" });
                }

                // Call the Verifyotp method from the Verification class
                var result = Verification.Verifyotp(phoneNumber, otp, _configuration);

                if (result.Status == enResultStatuslogin.Success)
                {
                    // OTP verification successful, proceed to the dashboard or another page
                    return Json(new { success = true, message = "OTP verification successful" });
                }
                else
                {
                    // Handle the case where OTP verification failed
                    ModelState.AddModelError(string.Empty, result.Message);
                    return Json(new { success = false, message = result.Message });
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine(ex.ToString());

                // Add an error message to ModelState if needed
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");

                // Return a JSON response with the error message
                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public class Verification
        {
            private readonly IConfiguration _configuration;

            public Verification(IConfiguration configuration)
            {
                _configuration = configuration;
            }
            public class Verify
            {
                public string Mobileno { get; set; }
                public int id { get; set; }
                public HttpStatusCode code { get; set; }
            }
            public static WebApiMethodResult SendingOtp(string mobileNumber, IConfiguration configuration, ITempDataDictionary tempData)
            {
                WebApiMethodResult result = new WebApiMethodResult();
                VerificationModel verificationModel = new VerificationModel();

                try
                {
                    string constring = configuration.GetConnectionString("DefaultConnection");

                    using (SqlConnection connection = new SqlConnection(constring))
                    {
                        connection.Open();

                        // Modify the stored procedure or query to retrieve details based on mobile number
                        using (SqlCommand command = new SqlCommand("[dbo].[USP_CheckUserByMobile]", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Mobile", mobileNumber);

                            // Add the output parameter for the found mobile number
                            SqlParameter foundMobileParameter = new SqlParameter("@FoundMobile", SqlDbType.VarChar, 12);
                            foundMobileParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(foundMobileParameter);

                            // Add the output parameter for @UserFound
                            SqlParameter userFoundParameter = new SqlParameter("@UserFound", SqlDbType.Bit);
                            userFoundParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(userFoundParameter);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Check if the user is found based on the @UserFound output parameter
                                bool userFound = Convert.ToBoolean(userFoundParameter.Value);

                                if (userFound)
                                {
                                    // Retrieve the found mobile number from the output parameter
                                    verificationModel.Mobileno = foundMobileParameter.Value.ToString();
                                }
                                else
                                {
                                    // Handle the case where the user is not registered with the provided mobile number
                                    result.SetStatus(enResultStatuslogin.Error, "User not registered with the provided mobile number");
                                    return result;
                                }
                            }
                        }
                    }

                    // Generate and store OTP only in TempData
                    tempData["GeneratedOtp"] = GenerateOTP(configuration);

                    // Continue with the rest of your logic...
                    SendMobileOTP(tempData["GeneratedOtp"].ToString(), verificationModel.Mobileno);
                    int id = Saveotpdetails(mobileNumber, tempData["GeneratedOtp"].ToString(), configuration);
                    Verify verify = new Verify();
                    verify.Mobileno = verificationModel.Mobileno;
                    verify.id = id;
                    result.SetStatus(enResultStatuslogin.Success, verify, "OTP Send Successfully");
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    Console.WriteLine(ex.ToString());
                    result.SetStatus(enResultStatuslogin.Exception, ex.ToString());
                }

                return result;
            }

            public static string GenerateOTP(IConfiguration configuration)
            {
                string otp = "";

                try
                {
                    string constring = configuration.GetConnectionString("DefaultConnection");

                    using (SqlConnection connection = new SqlConnection(constring))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("[dbo].[GenerateRandomOtpForLogin]", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    otp = reader["OTP"].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    Console.WriteLine(ex.ToString());
                }

                return otp;
            }

            public static void SendMobileOTP(string MobileOTP, string MobileNo)
            {
                try
                {
                    string OTPMsg = MobileOTP + " is your OTP. This OTP can be used only once. - www.Investmentz.com";

                    using (var client = new System.Net.WebClient())
                    {
                        client.Headers.Add("Content-Type:application/json");
                        client.Headers.Add("Accept:application/json");
                        var result = client.DownloadString("https://push3.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=acmiil&pass=acmiil&appid=acmiil&subappid=acmiil&contenttype=1&to=" + MobileNo + "&from=ACMIIL&text=" + OTPMsg + "&selfid=true&alert=1&dlrreq=true");
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            public static int Saveotpdetails(string mobileNumber, string OTP, IConfiguration configuration)
            {
                int msg = 0;

                try
                {
                    string constring = configuration.GetConnectionString("DefaultConnection");

                    using (SqlConnection connection = new SqlConnection(constring))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("[dbo].[UpdateOTP]", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Mobile", mobileNumber);
                            command.Parameters.AddWithValue("@OTP", OTP);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    msg = Convert.ToInt32(reader[0]);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    Console.WriteLine(ex.ToString());
                }

                return msg;
            }

            public static WebApiMethodResult Verifyotp(string phoneNumber, string enteredOTP, IConfiguration configuration)
            {
                bool Verify = false;
                WebApiMethodResult result = new WebApiMethodResult();

                try
                {
                    // Add logic to retrieve the stored OTP for the given UCC from your database
                    string storedOTP = GetStoredOTP(phoneNumber, configuration);

                    // Compare the stored OTP with the entered OTP
                    Verify = storedOTP == enteredOTP;

                    if (Verify)
                    {
                        result.SetStatus(enResultStatuslogin.Success, Verify, "OTP Verify Successfully");
                    }
                    else
                    {
                        result.SetStatus(enResultStatuslogin.Error, Verify, "OTP is not match");
                    }
                }
                catch (Exception ex)
                {
                    result.SetStatus(enResultStatuslogin.Exception, ex.ToString());
                }

                return result;
            }

            // Add a method to retrieve the stored OTP from the database based on UCC
            private static string GetStoredOTP(string Mobile, IConfiguration configuration)
            {
                string storedOTP = "";

                try
                {
                    string constring = configuration.GetConnectionString("DefaultConnection");

                    using (SqlConnection connection = new SqlConnection(constring))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("[dbo].[GetStoredOTP]", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Mobile", Mobile);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    storedOTP = reader["StoredOTP"].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    Console.WriteLine(ex.ToString());
                }

                return storedOTP;
            }

            private static void CheckUser(string Mobile, IConfiguration configuration, out bool userFound, out string foundMobile)
            {
                userFound = false;
                foundMobile = "";

                try
                {
                    string constring = configuration.GetConnectionString("DefaultConnection");

                    using (SqlConnection connection = new SqlConnection(constring))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("USP_CheckUserByMobile", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Mobile", Mobile);

                            // Output parameter for userFound
                            SqlParameter userFoundParameter = new SqlParameter("@UserFound", SqlDbType.Bit);
                            userFoundParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(userFoundParameter);

                            // Output parameter for foundMobile
                            SqlParameter foundMobileParameter = new SqlParameter("@FoundMobile", SqlDbType.VarChar, 12);
                            foundMobileParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(foundMobileParameter);

                            // Execute the stored procedure
                            command.ExecuteNonQuery();

                            // Retrieve the output values
                            userFound = Convert.ToBoolean(userFoundParameter.Value);
                            foundMobile = foundMobileParameter.Value.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    Console.WriteLine(ex.ToString());
                }
            }

        }
    }
}
