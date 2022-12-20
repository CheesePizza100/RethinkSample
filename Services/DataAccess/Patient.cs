using Microsoft.EntityFrameworkCore;

namespace RethinkSample.Services.DataAccess
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateOnly Birthday { get; set; }
    }
}
