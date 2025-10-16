using System.Text;
using Microsoft.AspNetCore.Mvc;
using John.SocialClub.Application;
using John.SocialClub.Domain;

namespace John.SocialClub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _service;
        public MembersController(IMemberService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetAll(CancellationToken ct)
        {
            var list = await _service.ListAsync(ct);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Member>> Get(int id, CancellationToken ct)
        {
            var member = await _service.GetAsync(id, ct);
            return member is null ? NotFound() : Ok(member);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Member>>> Search([FromQuery] int? occupation, [FromQuery] int? maritalStatus, [FromQuery] bool and, CancellationToken ct)
        {
            Occupation? occ = occupation.HasValue ? (Occupation?)occupation.Value : null;
            MaritalStatus? mar = maritalStatus.HasValue ? (MaritalStatus?)maritalStatus.Value : null;
            var list = await _service.SearchAsync(occ, mar, and, ct);
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<Member>> Create([FromBody] Member member, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var created = await _service.CreateAsync(member, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Member member, CancellationToken ct)
        {
            if (id != member.Id) return BadRequest("ID mismatch");
            var ok = await _service.UpdateAsync(member, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await _service.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export(CancellationToken ct)
        {
            var list = await _service.ListAsync(ct);
            var sb = new StringBuilder();
            sb.AppendLine("Id,Name,DateOfBirth,Occupation,MaritalStatus,HealthStatus,Salary,NumberOfChildren");
            foreach (var m in list)
            {
                var dob = m.DateOfBirth.ToString("dd-MM-yyyy");
                sb.AppendLine($"{m.Id},\"{m.Name}\",{dob},{(int)m.Occupation},{(int)m.MaritalStatus},{(int)m.HealthStatus},{m.Salary},{m.NumberOfChildren}");
            }
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "Members.csv");
        }
    }
}