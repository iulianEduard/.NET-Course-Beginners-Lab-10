using DAL.DataSets;
using DAL.EF;
using DAL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab13
{
    class Program
    {
        static void Main(string[] args)
        {
            EFExample();
        }

        public static void AdoNetExample()
        {
            var cnStr = ConfigurationManager.ConnectionStrings["DBConn"];

            var customer = new CustomerModel(cnStr.ConnectionString);
            customer.DisconectedExample();

            Console.ReadKey();
        }

        public static void EFExample()
        {
            var customerEntity = new CustomerEntity();

            customerEntity.GetAll();
        }

        public static void DapperExample()
        {
            var customerRepository = new CustomerRepository();

            customerRepository.GetAllCustomers();
        }

        public static void DisconnectedExample()
        {
            CustomerDS customerDS = new CustomerDS();
            DataSet ds = new DataSet("In memory db");

            customerDS.FillDataSet(ds);
            customerDS.PrintDataSet(ds);

            Console.ReadKey();
        }
    }
}
