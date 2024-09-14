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

        try
        {
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

            return Ok(feature);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            return BadRequest(ex.ToString());
        }
    }

    // PUT: api/feature/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeatureAsync(int id, Feature feature)
    {
        if (id != feature.Id)
        {
            return Ok(new { status = "0", message = "Feature ID mismatch." });
        }

        if (!ModelState.IsValid)
        {
            return Ok(new { status = "0", message = "Invalid model state.", errors = ModelState });
        }

        try
        {
            if (!Enum.TryParse(feature.Status, true, out Status status))
            {
                return Ok(new { status = "0", message = "Invalid status value." });
            }

            if (status == Status.Active && feature.TargetCompletionDate == null)
            {
                return Ok(new { status = "0", message = "TargetCompletionDate must be provided when Status is Active." });
            }

            if (status == Status.Closed && feature.ActualCompletionDate == null)
            {
                return Ok(new { status = "0", message = "ActualCompletionDate must be provided when Status is Closed." });
            }

            Feature updateFeature = await _context.Features.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (updateFeature == null)
            {
                return Ok(new { status = "0", message = $"Feature with ID {id} not found." });
            }

            updateFeature.Title = feature.Title;
            updateFeature.Description = feature.Description;
            updateFeature.Status = feature.Status;
            updateFeature.TargetCompletionDate = feature.TargetCompletionDate;
            updateFeature.ActualCompletionDate = feature.ActualCompletionDate;

            await _context.SaveChangesAsync();

            return Ok(new { status = "1", message = "Feature updated successfully.", feature = updateFeature });
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FeatureExists(id))
            {
                return Ok(new { status = "0", message = $"Feature with ID {id} not found." });
            }
            return Ok(new { status = "0", message = "A concurrency error occurred while updating the feature." });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Ok(new { status = "0", message = "An unexpected error occurred." });
        }
        finally
        {
            Console.WriteLine("Update operation completed.");
        }
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
