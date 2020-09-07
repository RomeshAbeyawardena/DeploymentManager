using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.AppDomains.Models
{
    [MessagePack.MessagePackObject((true))]
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public int TargetTypeId { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string FullyQualifiedTargetReference { get; set; }

        public TargetType Type { get; set; }
        public string Reference { get; set; }
    }
}
