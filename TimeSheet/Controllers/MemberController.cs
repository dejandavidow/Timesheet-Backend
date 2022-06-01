using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System.Threading.Tasks;
using System.Threading;
using System;
using Contracts;
using Domain;

[ApiController]
[Route("api/Member")]
public class MemberController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    public MemberController(IServiceManager serviceManager)
    {
        _serviceManager=serviceManager;
    }
    [HttpGet("search-count")]
    public async Task<IActionResult> CountSearchMembers([FromQuery] string search)
    {
        var members = await _serviceManager.MemberService.SearchCountAsync(search);
        return Ok(members);
    }
    [HttpGet("filter-count")]
    public async Task<IActionResult> CountFilterMembers([FromQuery] string letter)
    {
        var members = await _serviceManager.MemberService.FilterCountAsync(letter);
        return Ok(members);
    }
    [HttpGet("filter")]
    public async Task<IActionResult> FilterMembers([FromQuery] MemberParams memberParams, string letter)
    {
        var members = await _serviceManager.MemberService.FilterAsync(memberParams, letter);
        return Ok(members);
    }
    [HttpGet("search")]
    public async Task<IActionResult> SearchMembers([FromQuery] MemberParams memberParams,string search)
    {
        var members = await _serviceManager.MemberService.SearchAsync(memberParams,search);
        return Ok(members);
    }
    [HttpGet]
    public async Task<IActionResult> GetMembers([FromQuery] MemberParams memberParams,CancellationToken cancellationToken)
    {
        var members = await _serviceManager.MemberService.GetAllAsync(memberParams,cancellationToken);
        return Ok(members);
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetMemberById(Guid id,CancellationToken cancellationToken)
    {
        var member = await _serviceManager.MemberService.GetByIdAsync(id,cancellationToken);
        return Ok(member);
    }
    [HttpPost]
    public async Task<IActionResult> PostMember([FromBody] MemberDTO memberDTO,CancellationToken cancellationToken)
    {
        await _serviceManager.MemberService.CreateAsync(memberDTO,cancellationToken);
        return Ok();
    }
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteMember(Guid id,CancellationToken cancellationToken)
    {
        await _serviceManager.MemberService.DeleteAsync(id,cancellationToken);
        return NoContent();
    }
    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateMember(Guid id,[FromBody] MemberDTO memberDTO, CancellationToken cancellationToken)
    {
        await _serviceManager.MemberService.UpdateAsync(id,memberDTO,cancellationToken);
        return NoContent();
    }
}