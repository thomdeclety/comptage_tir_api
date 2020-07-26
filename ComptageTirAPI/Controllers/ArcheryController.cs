using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComptageTirAPI.Models;
using ComptageTirAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComptageTirAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class ArcheryController : ControllerBase
    {
        private readonly ArcheryService _archeryService;

        public ArcheryController(ArcheryService archeryService)
        {
            _archeryService = archeryService;
        }

        #region Account
        [HttpGet("{username}/{password}/connect")]
        public string Connect(string username, string password)
        {
            return _archeryService.Connect(username, password);
        }

        [HttpDelete("DeleteAccount")]
        public void DeleteAccount([FromBody] string id)
        {
            _archeryService.DeleteAccount(id);
        }

        #endregion

        #region Spec

        [HttpGet("{username}/getspec")]
        public List<ArrowSpec> GetSpecs(string username)
        {
            return _archeryService.GetSpecs(username);
        }

        [HttpPost("addspec")]
        public void AddSpec([FromBody]ArrowSpec spec)
        {
            _archeryService.AddSpec(spec);
        }

        [HttpPost("modifyspec")]
        public void ModifySpec([FromBody] ArrowSpec spec)
        {
            _archeryService.ModifySpec(spec);
        }

        [HttpDelete("deletespec/{id}")]
        public void DeleteSpec(string id)
        {
            _archeryService.DeleteSpec(id);
        }

        #endregion

        #region Serie
        [HttpGet("GetSeries/{username}/{range}/{location}/{date}/{competition}")]
        public List<Result> GetSeries(string username, string range, string location, string date, bool competition)
        {
            return _archeryService.GetResults(username, range, location, date, competition);
        }

        [HttpGet("GetSummary/{username}/{range}/{location}/{date}/{competition}")]
        public List<int> GetSummary(string username, string range, string location, string date, bool competition)
        {
            return _archeryService.GetSummary(_archeryService.GetResults(username, range, location, date, competition));
        }

        [HttpGet("GetLastSerie/{username}")]
        public Result GetLastSerie(string username)
        {
            return _archeryService.GetLastResult(username);
        }

        [HttpPost("AddSerie")]
        public void AddSerie([FromBody] Result result)
        {
            _archeryService.AddResult(result);
        }

        [HttpDelete("DeleteSerie/{id}")]
        public void DeleteSerie(string id)
        {
            _archeryService.DeleteResult(id);
        }
        #endregion
    }
}
