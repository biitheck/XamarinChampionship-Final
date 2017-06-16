using Microsoft.Azure.Mobile.Server;
using System;

namespace AccompanyMe.Azure.Backend.DataObjects
{
    public class Event : EntityData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}