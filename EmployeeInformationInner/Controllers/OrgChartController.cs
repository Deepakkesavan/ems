using EmpInfoInner.Middleware;
using EmpInfoService.Common;
using EmpInfoService.Model;
using EmpInfoService.Services.ServiceImpl;
using Microsoft.AspNetCore.Mvc;

namespace EmpInfoInner.Controllers
{
    [Route("api/[controller]")]
    public class OrgChartController : ControllerBase
    {
        private readonly OrgChartService _orgChartService;
        private readonly CommonService _commonService;
        public OrgChartController(OrgChartService orgChartService, CommonService commonService)
        {
            _orgChartService = orgChartService;
            _commonService = commonService;
        }

        [HttpGet("GetOrgChart")]
        public async Task<List<OrgChartDto>> GetOrgChart()
        {
            return await _orgChartService.GetOrgChart();
        }
        [HttpGet("GetCurrentUser")]
        public async Task<UserDto> GetUser()
        {
            return await _commonService.GetUserDtoAsync();
            //UserDto user = await _orgChartService.GetCurrentUser("1081");
            //return user;
        }

        [HttpGet("GetOrgChartMyTeam")]
        public async Task<OrgChartDto> GetOrgChartMyTeam()
        {
            var user = await _commonService.GetUserDtoAsync();
            //var user= await _orgChartService.GetCurrentUser("1081");
            return await _orgChartService.GetOrgChartMyTeam(user.EmpId);
        }
    }
}
