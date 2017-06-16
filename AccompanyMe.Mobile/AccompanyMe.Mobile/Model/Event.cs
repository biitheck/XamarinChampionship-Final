using System;
using Microsoft.WindowsAzure.MobileServices;

namespace AccompanyMe.Mobile.Model
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

		[Version]
		public string AzureVersion { get; set; }
    }
}
