﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WealthDashboard.Models;
using WealthDashboard.Models.InvestNowManager;

namespace WealthDashboard.Controllers
{
    public class InvestNowController : Controller
    {
        #region Global Variables
        private readonly IInvestNowManager _investNowManager;
        #endregion

        #region Ctor
        public InvestNowController(IInvestNowManager investNowManager)
        {
            _investNowManager = investNowManager;
        }
        #endregion
        [HttpPost]
        public async Task<JsonResult> InvestNowInsert([FromBody] InvestNowDetailInsertModel investNowDetailInsertModel)
        {
            var generatesrno = await _investNowManager.generatesrno(investNowDetailInsertModel.UCC);
            var getResult = JsonConvert.DeserializeObject<srModel>(Convert.ToString(generatesrno.Data));
            investNowDetailInsertModel.UserSrNo = getResult.uDsrno;
            //investNowDetailInsertModel.UserSrNo = generatesrno.ToString();
            var getCheckOutsideDPdata = await _investNowManager.InvestNowInsert(investNowDetailInsertModel);
            return Json(getCheckOutsideDPdata);
        }

    }
}