using System.Globalization;
using CsvHelper.Configuration;
using RethinkSample.Models;

namespace RethinkSample.Mapping;

public class NewPatientViewModelMap : ClassMap<NewPatientViewModel>
{
    public NewPatientViewModelMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(x => x.FirstName).Name("First Name");
        Map(x => x.LastName).Name("Last Name");
    }
}