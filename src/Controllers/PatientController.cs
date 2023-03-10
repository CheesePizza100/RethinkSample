using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using RethinkSample.Extensions;
using RethinkSample.Mapping;
using RethinkSample.Models;
using RethinkSample.Services.DataAccess;

namespace RethinkSample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly PatientContext _context;

    public PatientController(PatientContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("get")]
    public IActionResult GetPatients()
    {
        return Ok(_context.Patients.Select(x => new PatientViewModel()
        {
            Id = x.Id, 
            FirstName = x.FirstName, 
            LastName = x.LastName, 
            Gender = x.Gender, 
            Birthday = x.Birthday
        }).ToList());
    }

    [HttpGet]
    [Route("get/{id}")]
    public IActionResult GetPatient(int id)
    {
        if (id <= 0) return BadRequest();

        var patient = _context.Patients.First(x => x.Id == id);

        return Ok(new PatientViewModel()
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Gender = patient.Gender,
            Birthday = patient.Birthday
        });
    }


    [HttpPost]
    [Route("/post/patient")]
    public IActionResult SavePatient(NewPatientViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Patient patient = new ()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Gender = model.Gender,
            Birthday = model.Birthday
        };

        _context.Patients.Add(patient);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetPatient), new { id = patient.Id });
    }

    [HttpPost]
    [Route("/post/patients")]
    public IActionResult SavePatientCsv(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<NewPatientViewModelMap>();
        var patientRecords = csv.GetRecords<NewPatientViewModel>().Select(x => new Patient()
        {
            FirstName = x.FirstName,
            LastName = x.LastName,
            Gender = x.Gender,
            Birthday = x.Birthday
        }).ToList();

        _context.AddRange(patientRecords);
        _context.SaveChanges();

        return Ok();
    }
}