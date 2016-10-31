using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateManager.Entity
{
    public class DataGridEntity
    {
        public DataGridEntity(Driver driver)
        {
            isCheck = false;
            this.driver = driver;
        }
        public Boolean isCheck { get; set; }   
        public Driver driver { get; set; }
    }
}
