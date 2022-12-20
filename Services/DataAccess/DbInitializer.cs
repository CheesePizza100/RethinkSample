namespace RethinkSample.Services.DataAccess;

public class DbInitializer
{
    private readonly PatientContext _patientContext;

    public DbInitializer(PatientContext patientContext)
    {
        _patientContext = patientContext;
    }

    public void Run()
    {
        _patientContext.Database.EnsureDeleted();
        _patientContext.Database.EnsureCreated();

        _patientContext.Patients.AddRange(new List<Patient>()
        {
            new() { FirstName = "Clark", LastName = "Kent", Gender = "M", Birthday = new DateOnly(1982, 02, 28) },
            new() { FirstName = "Diana", LastName = "Prince", Gender = "F", Birthday = new DateOnly(1976, 03, 22) },
            new() { FirstName = "Tony", LastName = "Stark", Gender = "M", Birthday = new DateOnly(1970, 05, 29) },
            new() { FirstName = "Carol", LastName = "Danvers", Gender = "F", Birthday = new DateOnly(1968, 04, 24) }
        });

        _patientContext.SaveChanges();
    }
}