using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;  

namespace Repository
{
    public class CustomerRepository : GenericRepository<Customer>, IRepository<Customer>
    {
        public CustomerRepository() { }
        
        // You can add any specific methods for CustomerRepository here if needed
        
    }
    
}
