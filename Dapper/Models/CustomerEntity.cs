using Dapper.Contrib.Extensions;

namespace Dapper.Models
{
    [Table("dbo.Customer")]
    public class CustomerEntity
    {
        [ExplicitKey]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }
    }
}
