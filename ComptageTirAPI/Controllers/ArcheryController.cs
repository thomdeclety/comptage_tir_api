using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComptageTirAPI.Models;
using ComptageTirAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComptageTirAPI.Controllers
{
    [Route("api")]
    public class ArcheryController : Controller
    {
        private readonly ArcheryService _archeryService;

        public ArcheryController(ArcheryService feminaService)
        {
            _archeryService = feminaService;
        }

        #region Account
        [HttpGet("{username}/{password}/connect")]
        public string Connect(string username, string password)
        {
            return _archeryService.Connect(username, password);
        }
        #endregion

        #region Serie
        [HttpGet("GetSeries/{username}/{filter}")]
        public List<Result> GetSeries(string username, string filter)
        {
            return _archeryService.GetResults(username, filter);
        }

        [HttpGet("GetLastSerie/{username}")]
        public Result GetLastSerie(string username)
        {
            return _archeryService.GetLastResult(username);
        }

        [HttpGet("test")]
        public string Test(string username)
        {
            return "yessssssss";
        }

        [HttpPost("AddSerie")]
        public void GetSeries([FromBody] Result result)
        {
            _archeryService.AddResult(result);
        }
        #endregion
    }
}
