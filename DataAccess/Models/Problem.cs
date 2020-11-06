using DataAccessLibrary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Problem
    {
        private Task<IEnumerable<Problem>> task;

        public Problem()
        {
            
        }

        public Problem(Task<IEnumerable<Problem>> task)
        {
            this.task = task;
        }

        public Problem(int id, int customerId, string status, string category, string description, DateTime created)
        {
            Id = id;
            CustomerId = customerId;
            Status = status;
            Category = category;
            Description = description;
            Created = created;
        }

        public int Id { get; set; } 
        public int CustomerId { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Comment Comments { get; set; }
        

    }

    public class ViewModels
    {
        public ObservableCollection<Problem> problem = new ObservableCollection<Problem>();

        public ObservableCollection<Problem> Problems { get { return this.problem; }}

        public ViewModels()
        {
            this.problem.Add(new Problem()
            {
                Category = "",
                Description = "Nour"


            }); ;

        }


    }
}
