﻿using Microsoft.AspNetCore.Mvc;
using WealthDashboard.Areas.EKYC_MFJourney.Models.Encryption;
using WealthDashboard.Models;

namespace WealthDashboard.Controllers
{
    public class MutualFundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("MutualFund/Main")]
        public IActionResult Mutual_Fund()
        {
            return View();
        }
        [Route("MutualFund/Event")]
        public IActionResult Mf_sub_event_mf()
        {
            return View();
        }

        [Route("MutualFund/Screener")]
        public IActionResult Mf_event_mf_screener()
        {
            return View();
        }

        [Route("MutualFund/AdvicedFund")]
        public IActionResult Mf_event_mf_Advised_Fund()
        {
            return View();
        }

        [Route("MutualFund/GlobalSearch")]
        public IActionResult MFGlobalSearch()
        {
            return View();
        }

        [Route("MutualFund/LumpsumFund")]
        public IActionResult MFadvisedfundLumpsum()
        {
            return View();
        }

        [Route("MutualFund/InvestNow")]
        public IActionResult MFadvisedfundSIP()
        {

            return View();
        }

        [Route("MutualFund/PortfolioSummary")]
        public IActionResult MFportfoliosummary()
        {

            return View();
        }

        [Route("MutualFund/Transaction")]
        public IActionResult MFtransaction()
        {

            return View();
        }

        [Route("MutualFund/NFO")]
        public IActionResult MFNFO()
        {
            return View();
        }
        
        [Route("MutualFund/FundDetails")]
        public IActionResult FundDetails()
        {
            return View();
        }
        [Route("MutualFund/CartDetail")]
        public IActionResult CartDetail()
        {
            return View();
        }
        [Route("MutualFund/Authentication")]
        public IActionResult Authentication(string encucc)
        {
            string decucc = Encryption.Decrypt(encucc.Replace(' ', '+').Replace(' ', '+'));
            ENCUCCM eNCUCCM = new ENCUCCM();
            eNCUCCM.ucc = decucc;
            return View(eNCUCCM);
        }
        public IActionResult Redeemswp()
        {
            return View();
        }
    }
}
