using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var trackIds = db.Tracks.Select(x => x.Id).ToList();
            ViewBag.TrackIds = trackIds;
            var placeMarkViewModelLis = new PlacesMarkViewModel();
            return View(placeMarkViewModelLis);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, int? accurancy, string trackName, bool? uploadFile, int? firstTrackId, int? secondTrackId)
        {
            var trackIds = db.Tracks.Select(x => x.Id).ToList();
            ViewBag.TrackIds = trackIds;

            if (accurancy == null || accurancy < 1)
                accurancy = 10;

            if (string.IsNullOrEmpty(trackName))
                trackName = file?.FileName ?? "";

            var placeMarkViewModelLis = new PlacesMarkViewModel();
            if (uploadFile ?? false)
            {
                InitPlaceMarkAfterUpload(placeMarkViewModelLis, file, accurancy, trackName);
            }
            else
            {
                InitPlaceMarkAfterComparison(placeMarkViewModelLis, file, accurancy, firstTrackId, secondTrackId);
            }

            return View(placeMarkViewModelLis);
        }

        private void InitPlaceMarkAfterComparison(PlacesMarkViewModel placeMarkViewModelLis, HttpPostedFileBase file, int? accurancy, int? firstTrackId, int? secondTrackId)
        {
            Track firstTrack = null;
            Track secondTrack = null;

            if (firstTrackId != null)
            {
                firstTrack = db.Tracks.Find(firstTrackId);
                firstTrack.Points = db.Points.Where(x => x.TrackId == firstTrack.Id).ToList();
            }

            if (secondTrackId != null)
            {
                secondTrack = db.Tracks.Find(secondTrackId);
                secondTrack.Points = db.Points.Where(x => x.TrackId == secondTrack.Id).ToList();
            }

            placeMarkViewModelLis.FirstPlaceMark = firstTrack != null ? InitPlaceMark(firstTrack) : new List<PlaceMarkViewModel>();
            placeMarkViewModelLis.SecondPlaceMark = secondTrack != null ? InitPlaceMark(secondTrack) : new List<PlaceMarkViewModel>();
            placeMarkViewModelLis.CompareMarks();
        }

        private List<PlaceMarkViewModel> InitPlaceMark(Track track)
        {
            var firstPlaceMark = new List<PlaceMarkViewModel>();
            foreach (var point in track.Points)
            {
                var placeMarc = new PlaceMarkViewModel(point);
                firstPlaceMark.Add(placeMarc);
            }

            return firstPlaceMark;
        }

        private void InitPlaceMarkAfterUpload(PlacesMarkViewModel placeMarkViewModelLis, HttpPostedFileBase file, int? accurancy, string trackName)
        {
            List<PlaceMarkViewModel> firstPlaceMarkViewModelLis = new List<PlaceMarkViewModel>();
            if (file != null && file?.ContentLength > 0 && file?.ContentType == "application/octet-stream")
                try
                {
                    string path = Path.Combine(Server.MapPath("/"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(path);
                    XmlElement xRoot = xDoc.DocumentElement;
                    XmlNodeList childnodes = xRoot.GetElementsByTagName("trkpt");

                    var track = new Track(trackName);
                    List<Point> points = new List<Point>();

                    foreach (XmlNode xnode in childnodes)
                    {
                        var longitude = double.Parse(xnode.Attributes["lon"].InnerText, System.Globalization.CultureInfo.InvariantCulture);
                        var latitude = double.Parse(xnode.Attributes["lat"].InnerText, System.Globalization.CultureInfo.InvariantCulture);
                        var altitude = double.Parse(xnode.FirstChild.InnerText, System.Globalization.CultureInfo.InvariantCulture);
                        var symbolRow = GetSymbolRow(longitude, latitude, accurancy);

                        var point = new Point(xnode.InnerText, longitude, latitude, altitude, symbolRow);

                        points.Add(point);

                        var placeMarkViewModel = new PlaceMarkViewModel()
                        {
                            Name = xnode.InnerText,
                            Longitude = longitude,
                            Latitude = latitude,
                            Altitude = altitude,
                            SymbolRow = symbolRow
                        };
                        firstPlaceMarkViewModelLis.Add(placeMarkViewModel);
                    }

                    track.Points = points;
                    db.Tracks.Add(track);

                    placeMarkViewModelLis.FirstPlaceMark = firstPlaceMarkViewModelLis;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file or file format is wrong";
            }
            db.SaveChanges();
        }

        private static string GetSymbolRow(double Longitude, double Latitude, int? accuracy)
        {

            double leftWidth = 0;
            double topHeight = 0;
            double righttWidth = 360;
            double bottomHeight = 360;
            string SymbolRow = "";

            for (int i = 0; i < accuracy; i++)
            {
                double averageWidth = (leftWidth + righttWidth) / 2;
                double averageHeight = (topHeight + bottomHeight) / 2;

                if (Longitude <= averageWidth)
                {
                    righttWidth = averageWidth;
                    if (Latitude <= averageHeight)
                    {
                        bottomHeight = averageHeight;
                        SymbolRow += "p";
                    }
                    else
                    {
                        SymbolRow += "s";
                        topHeight = averageHeight;
                    }
                }
                else
                {
                    leftWidth = averageWidth;
                    if (Latitude <= averageHeight)
                    {
                        SymbolRow += "q";
                        bottomHeight = averageHeight;
                    }
                    else{
                        SymbolRow += "t";
                        topHeight = averageHeight;
                    }
                }
            }
            return SymbolRow;
        }
    }
}