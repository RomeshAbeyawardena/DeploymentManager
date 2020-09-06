using DNI.Core.Shared.Attributes;
using DNI.Core.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.AppDomains.Models
{
    public class TargetType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [GeneratedDefaultValue(Generators.DateTimeOffSetValueGenerator)]
        public DateTimeOffset Created { get; set; }

        [GeneratedDefaultValue(Generators.DateTimeOffSetValueGenerator, setOnUpdate: true)]
        public DateTimeOffset Modified { get; set; }
    }
}
