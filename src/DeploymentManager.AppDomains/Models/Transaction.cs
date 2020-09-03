using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.AppDomains.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int SortOrderId { get; set; }
        public string Payload { get; set; }
        public string Payload_File { get; set; }
        public int NextTransactionId { get; set; }
        //       [Id] INT NOT NULL
        //	CONSTRAINT PK_Transaction PRIMARY KEY
        //,[SortOrderId] INT NULL
        //,[Payload] NVARCHAR(2000) NULL
        //,[Payload_File] VARCHAR(200) NULL
        //,[NextTransactionId] INT NULL
        //	REFERENCES [dbo].[Transaction]
    }
}
