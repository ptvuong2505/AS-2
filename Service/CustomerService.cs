using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repository;

namespace Service
{
    public class CustomerService
    {
        private readonly CustomerRepository repository;

        public CustomerService()
        {
            repository = new CustomerRepository();
        }

        public List<Customer> GetAll() => (List<Customer>)repository.GetAll();
        public Customer FindById(int id) => (Customer)repository.GetById(id);
    }
}
