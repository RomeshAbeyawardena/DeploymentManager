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
    public class Credential
    {
        [Key]
        public int Id { get; set; }
        public string UniqueReference { get; set; }

        public string PublicKey { get; set; }

        [GeneratedDefaultValue(Generators.DateTimeOffSetValueGenerator)]
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Expires { get; set; }
    }
}
