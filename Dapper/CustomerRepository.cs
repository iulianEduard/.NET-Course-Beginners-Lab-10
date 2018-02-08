using Dapper.Models;
using Dapper.Ports;
using System;

namespace Dapper
{
    public class CustomerRepository
    {
        #region Attributes

        private readonly IRepository<CustomerEntity> _customerRepository;

        #endregion Attributes

        #region Constructor

        public CustomerRepository()
        {
            _customerRepository = new Repository<CustomerEntity>();
        }

        #endregion Constructor

        #region Public Methods

        public void GetAllCustomers()
        {
            var customerList =  _customerRepository.GetAll();

            foreach(var customer in customerList)
            {
                Console.WriteLine($"{customer.Id} {customer.Name} {customer.Gender} {customer.Age}");
            }
        }

        #endregion Public Methods
    }
}
