using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.AppDomains.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int SortOrderId { get; set; }
        public string Payload { get; set; }
        public string Payload_File { get; set; }
        public int NextTransactionId { get; set; }
    }
}
