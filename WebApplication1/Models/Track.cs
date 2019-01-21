using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DownloadTime { get; set; }
        public ICollection<Point> Points { get; set; }

        public Track()
        {
        }

        public Track(string name)
        {
            Name = name;
            DownloadTime = DateTime.Now;
        }
    }
}
