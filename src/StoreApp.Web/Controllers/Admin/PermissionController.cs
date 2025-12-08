using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Dtos.Account;
using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities.User;

namespace StoreApp.Web.Controllers.Admin
{
   
    public class PermissionController : AdminApiBaseController
    {
        private readonly StoreAppDbContext db;
        private readonly IPermissionService permissionService;
        private readonly IMapper mapper;

        public PermissionController(StoreAppDbContext db, IPermissionService permissionService, IMapper mapper)
        {
            this.db = db;
            this.permissionService = permissionService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var list = await permissionService.GetAllAsync(ct);
            return Ok(mapper.Map<List<PermissionDto>>(list));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct)
        {
            var item = await permissionService.GetByIdAsync(id, ct);
            if (item == null) return NotFound();
            return Ok(mapper.Map<PermissionDto>(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionDto dto, CancellationToken ct)
        {
            var p = await permissionService.CreateAsync(dto, ct);
            return Ok(mapper.Map<PermissionDto>(p));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionDto dto, CancellationToken ct)
        {
            if (id != dto.Id) return BadRequest();
            var ok = await permissionService.UpdateAsync(dto, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ok = await permissionService.DeleteAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }

        // Assign / Remove role permissions
        [HttpPost("role/{roleId}/assign")]
        public async Task<IActionResult> AssignToRole(string roleId, [FromBody] int permissionId, CancellationToken ct)
        {
            await permissionService.AssignPermissionToRoleAsync(roleId, permissionId, ct);
            return NoContent();
        }

        [HttpPost("role/{roleId}/remove")]
        public async Task<IActionResult> RemoveFromRole(string roleId, [FromBody] int permissionId, CancellationToken ct)
        {
            await permissionService.RemovePermissionFromRoleAsync(roleId, permissionId, ct);
            return NoContent();
        }
    }

    public class AssignPermissionDto
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
