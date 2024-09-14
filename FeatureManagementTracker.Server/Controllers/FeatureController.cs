using FeatureManagementTracker.Data;
using FeatureManagementTracker.Server.Models;
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
    public async Task<ActionResult<IEnumerable<FeatureModel>>> GetAllFeaturesAsync()
    {
        var features = await _context.Features.ToListAsync();
        var featureModels = features.Select(f => new FeatureModel
        {
            Id = f.Id,
            Title = f.Title,
            Description = f.Description,
            Complexity = f.EstimatedComplexity,
            Status = f.Status,
            TargetDate = f.TargetCompletionDate,
            ActualDate = f.ActualCompletionDate
        }).ToList();

        return Ok(featureModels);
    }

    // GET: api/feature/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FeatureModel>> GetFeatureByIdAsync(int id)
    {
        var feature = await _context.Features.FindAsync(id);

        if (feature == null)
        {
            return NotFound($"Feature with ID {id} not found.");
        }

        var featureModel = new FeatureModel
        {
            Id = feature.Id,
            Title = feature.Title,
            Description = feature.Description,
            Complexity = feature.EstimatedComplexity.ToLower(),
            Status = feature.Status.ToLower(),
            TargetDate = feature.TargetCompletionDate,
            ActualDate = feature.ActualCompletionDate
        };

        return Ok(featureModel);
    }

    // POST: api/feature
    [HttpPost]
    public async Task<ActionResult<FeatureModel>> CreateFeatureAsync(FeatureModel feature)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Invalid Model State");
            return BadRequest(ModelState);
        }

        try
        {
            Console.WriteLine($"Feature Status: {feature.Status}");
            if (!Enum.TryParse(feature.Status, true, out Status status))
            {
                Console.WriteLine("Invalid Status");
                return BadRequest("Invalid status value.");
            }

            if (status == Status.Active && feature.TargetDate == null)
            {
                Console.WriteLine("Missing Target Completion Date");
                return BadRequest("TargetCompletionDate must be provided when Status is Active.");
            }

            if (status == Status.Closed && feature.ActualDate == null)
            {
                Console.WriteLine("Missing Actual Completion Date");
                return BadRequest("ActualCompletionDate must be provided when Status is Closed.");
            }

            Feature ftr = new Feature();
            ftr.Title = feature.Title;
            ftr.Description = feature.Description;
            ftr.EstimatedComplexity = feature.Complexity;
            ftr.Status = feature.Status;
            ftr.TargetCompletionDate = feature.TargetDate;
            ftr.ActualCompletionDate = feature.ActualDate;

            _context.Features.Add(ftr);
            await _context.SaveChangesAsync();

            return Ok(feature);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.ToString()}");
            return BadRequest(ex.ToString());
        }
    }


    // PUT: api/feature/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeatureAsync(int id, FeatureModel feature)
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

            if (status == Status.Active && feature.TargetDate == null)
            {
                return Ok(new { status = "0", message = "TargetCompletionDate must be provided when Status is Active." });
            }

            if (status == Status.Closed && feature.ActualDate == null)
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
            updateFeature.EstimatedComplexity = feature.Complexity;
            updateFeature.TargetCompletionDate = feature.TargetDate;
            updateFeature.ActualCompletionDate = feature.ActualDate;

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
