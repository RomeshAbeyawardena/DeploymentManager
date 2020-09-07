using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNI.Core.Shared.Attributes;
using DNI.Core.Shared.Constants;

namespace DeploymentManager.AppDomains.Models
{
    [MessagePack.MessagePackObject((true))]
    public class Deployment
    {
        [Key]
        public int Id { get; set; }
        public int TargetId { get; set; }
        public int TransactionId { get; set; }

        [GeneratedDefaultValue(Generators.DateTimeOffSetValueGenerator)]
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Scheduled { get; set; }
        public DateTimeOffset? Completed { get; set; }

        public Target Target { get; set; }
        public Transaction Transaction { get; set; }
        public string Reference { get; set; }
    }
}
