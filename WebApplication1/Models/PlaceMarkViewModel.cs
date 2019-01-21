namespace WebApplication1.Models
{
    public class PlaceMarkViewModel
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public string SymbolRow { get; set; }

        public PlaceMarkViewModel()
        {

        }

        public PlaceMarkViewModel(Point point)
        {
            Name = point.Name;
            Longitude = point.Longitude;
            Latitude = point.Latitude;
            Altitude = point.Altitude;
            SymbolRow = point.SymbolRow;
        }

        public bool AreEqual(PlaceMarkViewModel secondMark)
        {
            if (!Altitude.Equals(secondMark.Altitude))
                return false;

            if (!Longitude.Equals(secondMark.Longitude))
                return false;

            if (!Latitude.Equals(secondMark.Latitude))
                return false;

            return true;
        }
    }
}