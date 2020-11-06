using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Category: ObservableCollection<string>
    {
        public Category()
        {
            Add("Leverans");
            Add("Retur");
            Add("KöpVillkor");
        }
    }
}
