using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuantoEuCobro.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace QuantoEuCobro.Controllers;

[ApiController]
[Route("api/templates")]
public class TemplatesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TemplatesController(AppDbContext context)
    {
        _context = context;
    }

    // 🔹 LISTAR TODOS OS TEMPLATES
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var templates = await _context.Templates
            .AsNoTracking()
            .Select(t => new
            {
                t.Id,
                t.Nome,
                t.Categoria,
                t.CorPrimaria,
                t.CorSecundaria,
                t.Fonte,
                t.Premium,
                t.Thumbnail
            })
            .ToListAsync(ct);

        return Ok(templates);
    }

    // 🔹 DETALHE DO TEMPLATE (para preview)
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Obter(int id, CancellationToken ct)
    {
        var template = await _context.Templates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        if (template == null)
            return NotFound();

        return Ok(template);
    }
}
