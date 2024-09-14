using FeatureManagementTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FeatureManagementTracker.Server.Utilities.Enum;

[ApiController]
[Route("api/[controller]")]
public class FeatureController : ControllerBase
{
    private readonly FeatureDBContext _context;

    public FeatureController(FeatureDBContext context)
    {
        _context = context;
    }

    // GET: api/feature
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Feature>>> GetAllFeaturesAsync()
    {
        var features = await _context.Features.ToListAsync();
        return Ok(features);
    }

    // GET: api/feature/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Feature>> GetFeatureByIdAsync(int id)
    {
        var feature = await _context.Features.FindAsync(id);

        if (feature == null)
        {
            return NotFound($"Feature with ID {id} not found.");
        }

        return Ok(feature);
    }

    // POST: api/feature
    [HttpPost]
    public async Task<ActionResult<Feature>> CreateFeatureAsync(Feature feature)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!Enum.TryParse(feature.Status, true, out Status status))
        {
            return BadRequest("Invalid status value.");
        }

        if (status == Status.Active && feature.TargetCompletionDate == null)
        {
            return BadRequest("TargetCompletionDate must be provided when Status is Active.");
        }

        if (status == Status.Closed && feature.ActualCompletionDate == null)
        {
            return BadRequest("ActualCompletionDate must be provided when Status is Closed.");
        }

        _context.Features.Add(feature);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFeatureByIdAsync), new { id = feature.Id }, feature);
    }

    // PUT: api/feature/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeatureAsync(int id, Feature feature)
    {
        if (id != feature.Id)
        {
            return BadRequest("Feature ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!Enum.TryParse(feature.Status, true, out Status status))
        {
            return BadRequest("Invalid status value.");
        }

        if (status == Status.Active && feature.TargetCompletionDate == null)
        {
            return BadRequest("TargetCompletionDate must be provided when Status is Active.");
        }

        if (status == Status.Closed && feature.ActualCompletionDate == null)
        {
            return BadRequest("ActualCompletionDate must be provided when Status is Closed.");
        }

        _context.Entry(feature).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FeatureExists(id))
            {
                return NotFound($"Feature with ID {id} not found.");
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/feature/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeatureAsync(int id)
    {
        var feature = await _context.Features.FindAsync(id);
        if (feature == null)
        {
            return NotFound($"Feature with ID {id} not found.");
        }

        _context.Features.Remove(feature);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FeatureExists(int id)
    {
        return _context.Features.Any(e => e.Id == id);
    }
}
