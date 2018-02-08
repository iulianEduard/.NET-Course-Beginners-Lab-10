using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class CustomerEntity
    {
        public void GetAll()
        {
            using (IntelMarkEntities1 context = new IntelMarkEntities1())
            {
                var customerDetailsSet = context.CustomerDetails;
                var customerSet = context.Customers;

                foreach(var customer in customerSet)
                {
                    Console.WriteLine(customer.Name + " " + customer.Email);
                }

                var dbResponse = context.usp_GetCustomerById(1, null);

                Console.ReadKey();
            }
        }

        public void Add()
        {
            using (IntelMarkEntities1 context = new IntelMarkEntities1())
            {
                context.Customers.Add(new Customer { Age = 21, Name = "Claudia", Email = "claudia@gmail.com" });
                context.SaveChanges();
            }
        }
    }
}
