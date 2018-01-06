using System;
using System.ComponentModel.DataAnnotations;

namespace aureliadotnetcore.Model
{
    public class AthleteItem
    {
        public string athlete { get; set; }
        public int age { get; set; }
        public string country { get; set; }
        public int year { get; set; }
        public string date { get; set; }
        public string sport { get; set; }
        public int? gold { get; set; }
        public int? silver { get; set; }
        public int? bronze { get; set; }
        public int? total { get; set; }
        [Key]
        public int id { get; set; }
    }
}
