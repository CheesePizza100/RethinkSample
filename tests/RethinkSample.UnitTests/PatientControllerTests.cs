using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;
using RethinkSample.Controllers;
using RethinkSample.Models;
using RethinkSample.Services.DataAccess;

namespace RethinkSample.UnitTests;

public class PatientControllerTests
{
    [Fact]
    public void GetPatient_Returns_One_Patient()
    {
        // Arrange
        var mockDbContext = new Mock<PatientContext>();
        mockDbContext.Setup(x => x.Patients).ReturnsDbSet(GetPatients());

        var controller = new PatientController(mockDbContext.Object);

        // Act
        IActionResult result = controller.GetPatient(1);

        // Assert
        OkObjectResult viewResult = Assert.IsType<OkObjectResult>(result);
        var patient = viewResult.Value as PatientViewModel;

        Assert.Equal("Bruce", patient!.FirstName);
    }

    [Fact]
    public void GetPatients_Returns_All_Patients()
    {
        // Arrange
        var mockDbContext = new Mock<PatientContext>();
        mockDbContext.Setup(x => x.Patients).ReturnsDbSet(GetPatients());

        var controller = new PatientController(mockDbContext.Object);

        // Act
        IActionResult result = controller.GetPatients();

        // Assert
        OkObjectResult viewResult = Assert.IsType<OkObjectResult>(result);
        var patients = viewResult.Value as List<PatientViewModel>;

        Assert.Equal(3, patients!.Count);
    }

    [Fact]
    public void SavePatient_Returns_BadResult()
    {
        // Arrange
        var mockDbContext = new Mock<PatientContext>();
        mockDbContext.Setup(x => x.Patients).ReturnsDbSet(GetPatients());

        var controller = new PatientController(mockDbContext.Object);
        controller.ModelState.AddModelError("FirstName", "Required");

        // Act
        IActionResult result = controller.SavePatient(new NewPatientViewModel());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void SavePatient_Returns_RedirectToAction()
    {
        // Arrange
        var mockDbContext = new Mock<PatientContext>();
        mockDbContext.Setup(x => x.Patients).ReturnsDbSet(GetPatients());
        mockDbContext.Setup(x => x.SaveChanges()).Returns(5);

        var controller = new PatientController(mockDbContext.Object);

        // Act
        IActionResult result = controller.SavePatient(new NewPatientViewModel()
        {
            FirstName = "Barry",
            LastName = "Allen",
            Gender = "M",
            Birthday = new DateOnly(1990, 12, 20)
        });

        // Assert
        var createdActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Null(createdActionResult.ControllerName);
        Assert.Equal("GetPatient", createdActionResult.ActionName);
    }

    [Fact]
    public void SavePatientCsv_Returns_Ok()
    {
        // Arrange
        var mockDbContext = new Mock<PatientContext>();
        mockDbContext.Setup(x => x.Patients).ReturnsDbSet(GetPatients());
        mockDbContext.Setup(x => x.SaveChanges()).Returns(5);

        var content = "First Name,Last Name,Birthday,Gender\r\nClark,Kent,2/29/1984,M\r\nDiana,Prince,3/22/1976,F\r\nTony,Stark,5/29/1970,M\r\nCarol,Denvers,4/24/1968,F";
        var fileName = "Patients.csv";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        var controller = new PatientController(mockDbContext.Object);

        // Act
        IActionResult result = controller.SavePatientCsv(file);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    private static List<Patient> GetPatients() => new()
    {
        new() { Id = 1, FirstName = "Bruce", LastName = "Wayne", Gender = "M", Birthday = new DateOnly(1990, 12, 20) },
        new () { Id = 2, FirstName = "Barry", LastName = "Allen", Gender = "M", Birthday = new DateOnly(1990, 12, 20) },
        new () { Id = 3, FirstName = "Carter", LastName = "Hall", Gender = "M", Birthday = new DateOnly(1990, 12, 20) },
    };
}