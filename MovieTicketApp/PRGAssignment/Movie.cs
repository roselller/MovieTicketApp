using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGAssignment
{
    //==================================================================
    // Student Number : S10222137, S10223630
    // Student Name : Chen Junrui Justin, Armirola Roseller Iii Tumolva
    // Module Group : T04
    //==================================================================
    class Movie
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Classification { get; set; }
        public DateTime OpeningDate { get; set; }
        public List<string> genreList = new List<string>();
        public List<Screening> screeningList = new List<Screening>();

        public Movie() { }
        public Movie(string t, int d, string cl, DateTime od, List<string> gl)
        {
            Title = t;
            Duration = d;
            Classification = cl;
            OpeningDate = od;
            genreList = gl;
        }

        public void AddGenre(string addg)
        {
            string[] genres = addg.Split("/");
            foreach (string g in genres)
            {
                genreList.Add(g);
            }
        }

        public void AddScreening(Screening scr)
        {
            screeningList.Add(scr);
        }

        public override string ToString()
        {
            return "Title: " + Title + "Duration: " + Duration + "Classification: " + Classification + "OpeningDate: " + OpeningDate;
        }
    }
}
