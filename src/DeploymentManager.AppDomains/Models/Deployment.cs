using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.AppDomains.Models
{
    public class Deployment
    {
        public int Id { get; set; }
        public int TargetId { get; set; }
        public int TransactionId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Scheduled { get; set; }
        public DateTimeOffset? Completed { get; set; }

        public Target Target { get; set; }
        public Transaction Transaction { get; set; }
    }
}
