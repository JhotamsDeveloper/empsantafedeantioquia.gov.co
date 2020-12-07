using System;
using System.Collections.Generic;
using System.Text;

namespace model
{
    public class Employee
    {
        public Employee() 
        {
            DateCreate = DateTime.Now;
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string CoverPage { get; set; }
        public string Occupation { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
