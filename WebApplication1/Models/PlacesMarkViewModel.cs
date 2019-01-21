using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Models
{
    public class PlacesMarkViewModel
    {
        public List<PlaceMarkViewModel> FirstPlaceMark { get; set; }
        public List<PlaceMarkViewModel> SecondPlaceMark { get; set; }
        public List<PlaceMarkViewModel> MutualPlaceMarks { get; set; }

        public PlacesMarkViewModel()
        {
            FirstPlaceMark = new List<PlaceMarkViewModel>();
            SecondPlaceMark = new List<PlaceMarkViewModel>();
            MutualPlaceMarks = new List<PlaceMarkViewModel>();
        }

        public bool AreNotEmpty()
        {
            if (FirstPlaceMark != null && FirstPlaceMark.Any())
                return true;

            if (SecondPlaceMark != null && SecondPlaceMark.Any())
                return true;

            if (MutualPlaceMarks != null && MutualPlaceMarks.Any())
                return true;

            return false;
        }

        public void CompareMarks()
        {
            SetMutualMarks();
            FirstPlaceMark = GetMarksWithoutMutualMarks(FirstPlaceMark);
            SecondPlaceMark = GetMarksWithoutMutualMarks(SecondPlaceMark);
        }

        private void SetMutualMarks()
        {
            foreach (var firstPlaceMarkViewModel in FirstPlaceMark)
            {
                foreach (var secondPlaceMarkViewModel in SecondPlaceMark)
                {
                    if (firstPlaceMarkViewModel.AreEqual(secondPlaceMarkViewModel))
                    {
                        MutualPlaceMarks.Add(secondPlaceMarkViewModel);
                    }
                }
            }
        }

        private List<PlaceMarkViewModel> GetMarksWithoutMutualMarks(IEnumerable<PlaceMarkViewModel> marks)
        {
            var result = marks
                .Where(mark => !MutualPlaceMarks.Any(x => x.AreEqual(mark)))
                .ToList();
            return result;
        }
    }

}