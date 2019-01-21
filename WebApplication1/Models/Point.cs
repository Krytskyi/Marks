using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Point
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public string SymbolRow { get; set; }

        public int? TrackId { get; set; }
        public Track Track { get; set; }

        public Point()
        {
        }

        public Point(string name, double longitude, double latitude, double altitude, string symbolRow)
        {
            Name = name;
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
            SymbolRow = symbolRow;
        }
    }
}
