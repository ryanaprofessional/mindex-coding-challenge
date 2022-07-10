using System;
using System.ComponentModel.DataAnnotations;

namespace challenge.Models
{
    public class Compensation //Compensations class for task#2 - RMA20220710
    {
        [Key] 
        public String CompensationId {get;set;}
        public String Employee { get; set; }
        public float Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
