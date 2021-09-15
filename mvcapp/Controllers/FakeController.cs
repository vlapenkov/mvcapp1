using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvcapp.Controllers
{
    /// <summary>
    /// Класс который эмулирует работу endpoint
    /// </summary>
    [Authorize]
    //[AllowAnonymous]
    public class FakeController : Controller
    {


        public async Task<IEnumerable<AccrualsPaymentsNodeDto>> Index(DateTime? sDate, int? id)
        {

            //  if(Request.Headers["User-Agent"][0].Contains("Chrome"))
            //     throw new NotImplementedException();

            var path = "Reports/Data/DataByPeriod.json";
            if (sDate.HasValue && sDate.Value.Year == 2019) path = "Reports/Data/DataByPeriod2.json";



            IEnumerable<AccrualsPaymentsNodeDto> data;
            var pathToFile = StiNetCoreHelper.MapPath(this, path);
            var json = System.IO.File.ReadAllText(path);

            data = JsonConvert.DeserializeObject<IEnumerable<AccrualsPaymentsNodeDto>>(json);
            return data;
        }




    }
}
