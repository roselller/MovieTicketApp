using System;
using System.IO;
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
    class Program
    {
        static void Main(string[] args)
        {
            //creating lists
            List<Movie> movieList = new List<Movie>();
            List<Cinema> cinemaList = new List<Cinema>();
            List<Screening> screeningList = new List<Screening>();
            List<Order> orderList = new List<Order>();

            //Initialise all the data
            InitMovieData(movieList);
            InitCinemaData(cinemaList);
            int screencount = InitScreeningData(screeningList, movieList, cinemaList);
            while (true)
            {
                try //input validation
                {
                    Mainmenu(screeningList, movieList, orderList, cinemaList);
                    int userinput = Convert.ToInt32(Console.ReadLine());
                    if (userinput == 1)
                    {
                        //display all information on all available movies
                        DisplayMovie(movieList);
                    }
                    else if (userinput == 2)
                    {
                        //list movie screenings
                        DisplayMovieScreenings(screeningList, movieList);
                    }
                    else if (userinput == 3)
                    {
                        //add userinput moving screening
                        screencount = AddMovieScreeningSession(screeningList, movieList, cinemaList, screencount);
                    }
                    else if (userinput == 4)
                    {
                        DelMovieScreeningSession(screeningList, movieList, cinemaList);
                    }
                    else if (userinput == 5)
                    {
                        OrderMovieTicket(orderList, movieList, screeningList);
                    }
                    else if (userinput == 6)
                    {
                        CancelMovieTicket(orderList);
                    }
                    else if (userinput == 0)
                    {
                        Console.WriteLine("Thank you for visiting us.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid option.\n");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid option.\n");
                }
            }
        }

        static void Mainmenu(List<Screening> screeningList, List<Movie> movieList, List<Order> orderList, List<Cinema> cinemaList) //function for main menu
        {
            Console.WriteLine("================ Welcome, please choose an option to continue ====================");
            Console.WriteLine("[0] Exit");
            Console.WriteLine("[1] Display Movies");
            Console.WriteLine("[2] Display Movie Screenings");
            Console.WriteLine("[3] Add Movie Screening Sessions");
            Console.WriteLine("[4] Delete Movie Screening Sessions");
            Console.WriteLine("[5] Order movie ticket(s)");
            Console.WriteLine("[6] Cancel order of ticket");
            Console.WriteLine("==================================================================================");
            Console.WriteLine("Top 3 recommended movies:");
            RecommendedMovies(screeningList, movieList, orderList, cinemaList);
            Console.WriteLine("==================================================================================");
            Console.Write("Option: ");

        }

        // ------------------------------------------Movie---------------------------------------------------
        static void InitMovieData(List<Movie> movieList) //initialising movie.csv data to be added into list
        {
            string[] movies = File.ReadAllLines("Movie.csv");
            for (int m = 1; m < movies.Length; m++)
            {
                string[] mList = movies[m].Split(',');
                string[] genreList = mList[2].Split('/');
                List<string> genres = new List<string>();
                foreach (string g in genreList)
                {
                    genres.Add(g);
                }
                movieList.Add(new Movie(mList[0], Convert.ToInt32(mList[1]), mList[3], Convert.ToDateTime(mList[4]), genres));
            }
        }

        public static void DisplayMovie(List<Movie> movieList) //display movies from list for option (1)
        {
            Console.WriteLine("==================================== Movies ======================================");
            Console.WriteLine("{0,-25} {1,-15} {2,-15} {3,-14} {4,-20}", "Title", "Duration(mins)", "Classification", "Opening Date", "Genres");

            foreach (Movie mo in movieList)
            {
                Console.WriteLine("{0,-25} {1,-15} {2,-15} {3,-14} {4,-20}", mo.Title, mo.Duration, mo.Classification, mo.OpeningDate.ToString("dd/MM/yyyy"), string.Join(" / ", mo.genreList));
            }
            Console.WriteLine("==================================================================================");
        }

        static void DisplayAvailableMovies(List<Movie> movieList) //displays movies from list for screening option (2)
        {
            Console.WriteLine("==================================================================================");
            Console.WriteLine("Movies available:");
            Console.WriteLine("{0,-25} {1,-15} {2,-15} {3,-14} {4,-20}", "Title", "Duration(mins)", "Classification", "Opening Date", "Genres");
            foreach (Movie mo in movieList)
            {
                bool availableMovie = false;
                foreach (Screening scr in mo.screeningList)
                {
                    if (scr.SeatsRemaining > 0) //checks for any available seating
                    {
                        availableMovie = true;
                    }
                }

                if (availableMovie == true) //displaying movies that only are available
                {
                    Console.WriteLine("{0,-25} {1,-15} {2,-15} {3,-14} {4,-20}", mo.Title, mo.Duration, mo.Classification, mo.OpeningDate.ToString("dd/MM/yyyy"), string.Join(" / ", mo.genreList));
                }
            }
            Console.WriteLine("==================================================================================");
        }

        //--------------------------------------------Cinema-----------------------------------------------------
        static void InitCinemaData(List<Cinema> cinemaList) //initialising cinema.csv data to be added into list
        {
            string[] cinemas = File.ReadAllLines("Cinema.csv");
            for (int c = 1; c < cinemas.Length; c++)
            {
                string[] cList = cinemas[c].Split(',');
                cinemaList.Add(new Cinema(cList[0], Convert.ToInt32(cList[1]), Convert.ToInt32(cList[2])));
            }
        }

        static void DisplayCinema(List<Cinema> cinemaList) //display cinema from list
        {
            Console.WriteLine("{0,-20} {1,-15} {2,-7}", "Name", "Hall Number", "Capacity");
            foreach (Cinema p in cinemaList)
            {
                Console.WriteLine("{0,-20} {1,-15} {2,-7}", p.Name, p.HallNo, p.Capacity);
            }
        }

        //-------------------------------------------Screening------------------------------------------------------
        static int InitScreeningData(List<Screening> screeningList, List<Movie> movieList, List<Cinema> cinemaList) //initialising screening.csv data into a list
        {
            int ScreeningNo = 1001;
            string[] screening = File.ReadAllLines("Screening.csv");
            for (int scr = 1; scr < screening.Length; scr++)
            {
                string[] scrList = screening[scr].Split(',');
                string cinemaName = scrList[2];
                int hallNo = Convert.ToInt32(scrList[3]);
                static Cinema CinemaSearch(List<Cinema> cinemaList, string cinemaName, int hallNo) //search if cinema is inside the list
                {
                    for (int i = 0; i < cinemaList.Count; i++)
                    {
                        Cinema c = cinemaList[i];
                        if (cinemaName == c.Name && hallNo == c.HallNo)
                        {
                            return c;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    return null;
                }
                Cinema cSearchresult = CinemaSearch(cinemaList, cinemaName, hallNo);

                string movieName = scrList[4];
                static Movie MovieSearch(List<Movie> movieList, string movieName) //search if movie is inside the list
                {
                    for (int x = 0; x < movieList.Count; x++)
                    {
                        Movie m = movieList[x];
                        if (movieName == m.Title)
                        {
                            return m;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    return null;
                }
                Movie mSearchResult = MovieSearch(movieList, movieName);
                Screening newScrList = new Screening(ScreeningNo, Convert.ToDateTime(scrList[0]), scrList[1], cSearchresult, mSearchResult); //new screening object when cinema and movie corressponds 
                newScrList.SeatsRemaining = cSearchresult.Capacity;
                screeningList.Add(newScrList);
                newScrList.Movie.screeningList.Add(newScrList);
                ScreeningNo++;
            }
            return ScreeningNo;
        }

        public static Movie DisplayMovieScreenings(List<Screening> screeningList, List<Movie> movieList) //display movie screenings option (3)
        {
            DisplayAvailableMovies(movieList);
            string uimovie = "";
            bool movieExist = false;
            Movie selectedMovie = null;

            while (movieExist == false)
            {
                //prompt user for movie
                Console.Write("Select a movie: ");
                uimovie = Console.ReadLine();

                foreach (Movie m in movieList)
                {
                    //finding movie from input
                    if (uimovie.ToLower() == m.Title.ToLower())
                    {
                        movieExist = true;
                        selectedMovie = m;
                        break;
                    }
                }

                //input validation
                if (movieExist == false)
                {
                    Console.WriteLine("Please enter a valid option.");
                }
            }

            Console.WriteLine("==================================================================================");
            Console.WriteLine("Screening sessions for {0}:", uimovie);
            Console.WriteLine("{0,-13}  {1,-25}  {2,-15} {3}", "Screening No", "Screening Date & Time", "Screen Type", "Seats Remaining");
            screeningList.Sort();
            foreach (Screening scr in screeningList)
            {
                if (selectedMovie == scr.Movie)
                {
                    Console.WriteLine("{0,-13}  {1,-25}  {2,-15} {3}", scr.ScreeningNo, scr.ScreeningDateTime, scr.ScreenType, scr.SeatsRemaining);
                }
            }
            Console.WriteLine("==================================================================================");

            return selectedMovie;
        }

        public static int AddMovieScreeningSession(List<Screening> screeningList, List<Movie> movieList, List<Cinema> cinemaList, int screencount) //option (4)
        {
            DisplayAvailableMovies(movieList); //display all movies first

            //while loop for user's input
            while (true)
            {
                //selection of movie
                Console.Write("Select a movie: ");
                string uimovie = Console.ReadLine();
                Movie movie = null;
                bool movieExist = false;
                foreach (Movie m in movieList)
                {
                    if (uimovie.ToLower() == m.Title.ToLower()) //validation
                    {
                        movie = m;
                        movieExist = true;
                        break;
                    }
                }

                if (movieExist == true)
                {
                    while (true)
                    {
                        DateTime uiDateTime = new DateTime();

                        //selection of screening type
                        Console.Write("Enter a screening type [2D/3D]: ");
                        string uiScrType = Console.ReadLine().ToUpper();
                        if (uiScrType == "2D" || uiScrType == "3D") //validation
                        {
                            while (true)
                            {
                                while (true)
                                {
                                    try
                                    {
                                        //selection of screening date and time
                                        Console.Write("Enter a screening date and time [yyyy/mm/dd,HH:MM]: ");
                                        string scrDateTime = Console.ReadLine();
                                        string[] s = scrDateTime.Split(','); //to correspond with formatting
                                        string[] s2 = s[0].Split('/'); //to correspond with formatting
                                        string[] s3 = s[1].Split(':'); //to correspond with formatting
                                        uiDateTime = new DateTime(Convert.ToInt32(s2[0]), Convert.ToInt32(s2[1]), Convert.ToInt32(s2[2]), Convert.ToInt32(s3[0]), Convert.ToInt32(s3[1]), 0);
                                        break;
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Please enter a valid option.");
                                    }
                                }

                                List<Screening> currentScreening = new List<Screening>();
                                while (true)
                                {
                                    //display cinema information
                                    Console.WriteLine("==================================================================================");
                                    Console.WriteLine("Available cinema halls:");
                                    DisplayCinema(cinemaList);
                                    Console.WriteLine("==================================================================================");

                                    bool existingCinema = false;
                                    string cName = null;
                                    int cHNo = 0;

                                    while (existingCinema == false)
                                    {
                                        //prompt user for cinema and hallno
                                        Console.Write("Enter a cinema name: ");
                                        cName = Console.ReadLine();

                                        Console.Write("Enter a cinema hall number: ");
                                        cHNo = Convert.ToInt32(Console.ReadLine());

                                        foreach (Cinema c in cinemaList)
                                        {
                                            if (cName.ToLower() == c.Name.ToLower() && cHNo == c.HallNo) //validation
                                            {
                                                existingCinema = true;
                                                break;
                                            }
                                        }

                                        if (existingCinema == false)
                                        {
                                            Console.WriteLine("Please enter a valid option.");
                                        }
                                        else
                                            break;
                                    }

                                    if (existingCinema == true)
                                    {

                                        bool flag = true;
                                        int moviecount = 0;
                                        foreach (Screening scr in screeningList)
                                        {
                                            if (scr.Cinema.Name.ToLower() == cName.ToLower() && scr.Cinema.HallNo == cHNo) //validation
                                            {
                                                Cinema retrieveCinema = scr.Cinema;
                                                foreach (Movie m in movieList)
                                                {
                                                    if (uimovie.ToLower() == m.Title.ToLower()) //validation
                                                    {
                                                        Screening NewScreening = new Screening(screencount + 1, uiDateTime, uiScrType, retrieveCinema, m); //user input screening
                                                        currentScreening.Add(NewScreening);
                                                        moviecount += 1;
                                                        //if screening time + 30 mins from cleaning is > 0 & new screening time > previous screening time, bool = true
                                                        if ((scr.ScreeningDateTime.AddMinutes(scr.Movie.Duration + 30).Subtract(NewScreening.ScreeningDateTime) >= TimeSpan.Zero) && NewScreening.ScreeningDateTime >= scr.ScreeningDateTime)
                                                        {
                                                            flag = false;
                                                            break;
                                                        }
                                                        //opposite of the above 
                                                        else if ((NewScreening.ScreeningDateTime.AddMinutes(NewScreening.Movie.Duration + 30)).Subtract(scr.ScreeningDateTime) >= TimeSpan.Zero && NewScreening.ScreeningDateTime <= scr.ScreeningDateTime)
                                                        {
                                                            flag = false;
                                                            break;
                                                        }
                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                        if (flag == true && moviecount != 0)
                                        {
                                            currentScreening[0].SeatsRemaining = currentScreening[0].Cinema.Capacity;
                                            Screening newscrList = currentScreening[0];
                                            screeningList.Add(newscrList);
                                            movie.screeningList.Add(newscrList); //make the movie object from user to be inside the screeningList
                                            Console.WriteLine("New movie screening has been successfully added!\n");
                                            return screencount + 1;
                                        }
                                        else
                                        {
                                            Console.WriteLine("The timeslot for this movie screening has been taken."); // Validation 3 - Screening Date & Time
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cinema doesn't exist");
                                    }
                                    break;
                                }
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid screening type."); //validation 2 - Screening Type
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Movie doesn't exist."); //validation 1 - Movie
                }
            }
        }

        public static void DelMovieScreeningSession(List<Screening> screeningList, List<Movie> movieList, List<Cinema> cinemaList) //removal of movie screening session option (4)
        {
            Console.WriteLine("==================================================================================");
            Console.WriteLine("Screening sessions that have not sold any tickets");
            Console.WriteLine("{0,-13}  {1,-25}  {2,-15} {3}", "Screening No", "Screening Date & Time", "Screen Type", "Seats Remaining");

            foreach (Screening s in screeningList)
            {
                if (s.SeatsRemaining == s.Cinema.Capacity) //validation
                {
                    Console.WriteLine("{0,-13}  {1,-25}  {2,-15} {3}", s.ScreeningNo, s.ScreeningDateTime, s.ScreenType, s.SeatsRemaining);
                }
            }
            Console.WriteLine("==================================================================================");

            bool screeningExist = false;

            while (true) //prompt user to select session with input validation
            {
                try
                {
                    //prompt user for screening number
                    Console.Write("Select a screening number to remove: ");
                    int delSession = Convert.ToInt32(Console.ReadLine());
                    for (int i = 0; i < screeningList.Count; i++)
                    {
                        Screening s = screeningList[i];
                        if (delSession == s.ScreeningNo) //validation
                        {
                            screeningList.RemoveAt(i);
                            screeningExist = true;
                            Console.WriteLine("Session number successfully removed.");
                        }
                    }

                    if (screeningExist == false)
                    {
                        Console.WriteLine("Please enter a valid option.");
                    }
                    else
                        break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid option.");
                }
            }
        }

        //------------------------------------------- Order ------------------------------------------------------
        public static void OrderMovieTicket(List<Order> orderList, List<Movie> movieList, List<Screening> screeningList) //user orders ticket
        {
            Movie selectedMovie = DisplayMovieScreenings(screeningList, movieList); //display movies and screenings
            bool screeningExist = false;
            Screening screening = null;
            int scrID = 0;

            while (screeningExist == false) //user input for screening number and input validation
            {
                try
                {
                    //prompt user for screening number
                    Console.Write("Select a screening number: ");
                    scrID = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    scrID = 0;
                }

                foreach (Screening s in selectedMovie.screeningList)
                {
                    if (scrID == s.ScreeningNo)
                    {
                        screening = s;

                        if (screening.ScreeningDateTime <= DateTime.Now)
                        {
                            Console.WriteLine("This screening session is not available."); //if the person chooses a screening that has already happened
                            screeningExist = false;
                        }
                        else
                        {
                            screeningExist = true;
                        }
                        break;
                    }
                }

                if (screeningExist == false)
                {
                    Console.WriteLine("Please enter a valid option.");
                }
            }

            //user input for number of movie tickets and input validation
            int tickets = 999;

            while (tickets == 999)
            {
                try
                {
                    //prompt user for number of tickets
                    Console.Write("Enter number of tickets ordering: ");
                    tickets = Convert.ToInt32(Console.ReadLine());

                    if (tickets == 0) //if user does not want to order any tickets
                    {
                        return;
                    }
                    else if (tickets > 0)
                    {
                        if (tickets > screening.SeatsRemaining) //check if there are seat availability
                        {
                            Console.WriteLine("There are insufficient seats available.");
                            tickets = 999;
                        }
                        else //there are available seats
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid option.");
                        tickets = 999;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid option.");
                }
            }

            while (true)
            {
                if (selectedMovie.Classification != "G") //age verification
                {
                    Console.Write("This movie is {0}, do all ticket holders meet the classification requirements? [Y/N]: ", selectedMovie.Classification);
                    string ageVerify = Console.ReadLine().ToUpper();

                    if (ageVerify == "Y")
                    {
                        break;
                    }
                    else if (ageVerify == "N")
                    {
                        Console.WriteLine("You do not meet the requirements to watch this movie.\n");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid option.");
                    }
                }
                else
                    break;
            }

            //new order object created
            Order order = new Order(orderList.Count + 1, DateTime.Now);
            orderList.Add(order);
            order.Status = "Unpaid";
            Console.WriteLine("Order {0} created.", order.OrderNo);
            Console.WriteLine("==================================================================================");

            List<Ticket> ticketList = new List<Ticket>();
            int yob = -1;
            for (int i = 1; i <= tickets; i++)
            {
                while (true)
                {
                    //prompt user for type of ticket
                    Console.Write("Type of ticket ({0}) [Student / Senior Citizen / adult]: ", i);
                    string type = Console.ReadLine().ToLower();

                    if (type == "student") //if user chose ticket type as student
                    {
                        while (true)
                        {
                            if (selectedMovie.Classification == "G")
                            {
                                Console.Write("Level of study [Primary / Secondary / Tertiary]: ");
                                string los = Console.ReadLine().ToLower();

                                if (los == "primary" || los == "secondary" || los == "tertiary")
                                {
                                    //create student ticket
                                    order.AddTicket(new Student(screening, los));
                                    break;
                                }
                                else
                                    Console.WriteLine("Please enter a valid option.");
                            }
                            else if (selectedMovie.Classification == "PG13" || selectedMovie.Classification == "NC16")
                            {
                                Console.Write("Level of study [Secondary / Tertiary]: ");
                                string los = Console.ReadLine().ToLower();

                                if (los == "secondary" || los == "tertiary")
                                {
                                    //create student ticket
                                    order.AddTicket(new Student(screening, los));
                                    break;
                                }
                                else
                                    Console.WriteLine("Please enter a valid option.");
                            }
                            else if (selectedMovie.Classification == "M18" || selectedMovie.Classification == "R21")
                            {
                                order.AddTicket(new Student(screening, "tertiary"));
                                break;
                            }
                        }
                        break;
                    }
                    else if (type == "senior citizen") //if user chose ticket type as senior citizen
                    {
                        int age = -1;

                        while (true) //input validation for year of birth
                        {
                            try
                            {
                                //prompt user for yob
                                Console.Write("Year of birth: ");
                                yob = Convert.ToInt32(Console.ReadLine());
                                age = DateTime.Now.Year - yob;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Please enter a valid option.");
                            }

                            if (age < 0 || age > 100)
                            {
                                Console.WriteLine("Please enter a valid option.");
                            }
                            else
                                break;
                        }

                        if (age < 55) //if user does not meet age requirement for senior citizen type
                        {
                            Console.WriteLine("You do not meet the requirements for a Senior Citizen ticket, charging you an Adult ticket instead.");
                            bool popoff;

                            while (true) //prompt for adult ticket type and input validation
                            {
                                Console.Write("Would you like to purchase popcorn for $3? [Y/N]: ");
                                string popcorn = Console.ReadLine().ToUpper();

                                if (popcorn == "Y")
                                {
                                    popoff = true;
                                    break;
                                }
                                else if (popcorn == "N")
                                {
                                    popoff = false;
                                    break;
                                }
                                else if (popcorn != "Y" || popcorn != "N")
                                {
                                    Console.WriteLine("Please enter a valid option.");
                                }
                            }

                            //create senior citizen ticket (adult)
                            order.AddTicket(new Adult(screening, popoff));
                            break;
                        }
                        else //if user meets the age requirement for senior citizezn type
                        {
                            //create senior citizen ticket
                            order.AddTicket(new SeniorCitizen(screening, yob));
                            break;
                        }
                    }
                    else if (type == "adult") //if user chose ticket type as adult
                    {
                        bool popoff;

                        while (true)
                        {
                            Console.Write("Would you like to purchase popcorn for $3? [Y/N]: ");
                            string popcorn = Console.ReadLine().ToUpper();

                            if (popcorn == "Y")
                            {
                                popoff = true;
                                break;
                            }
                            else if (popcorn == "N")
                            {
                                popoff = false;
                                break;
                            }
                            else if (popcorn != "Y" || popcorn != "N")
                            {
                                Console.WriteLine("Please enter a valid option.");
                            }
                        }

                        //create adult ticket
                        order.AddTicket(new Adult(screening, popoff));
                        break;
                    }
                    else
                        Console.WriteLine("Please enter a valid option.");
                }
            }

            //update seats remaining for the screening
            screening.SeatsRemaining -= tickets;

            //list amount payable
            double price = 0;
            double payable = 0;

            foreach (Ticket t in order.ticketList)
            {
                if (t is Student)
                {
                    Student s = (Student)t;
                    price = s.CalculatedPrice();
                }
                else if (t is SeniorCitizen)
                {
                    SeniorCitizen sc = (SeniorCitizen)t;
                    price = sc.CalculatedPrice();
                }
                else if (t is Adult)
                {
                    Adult a = (Adult)t;
                    price = a.CalculatedPrice();
                }
                payable += price;
            }
            Console.WriteLine("==================================================================================");
            Console.WriteLine("Amount payable: ${0}", payable);

            //prompt user to make payment
            Console.WriteLine("Press any key to make payment.");
            Console.ReadKey();

            //fill in new details to order entity
            order.Amount = payable;
            order.Status = "Paid";
        }

        public static void CancelMovieTicket(List<Order> orderList)
        {
            Order order = new Order();
            int orderNo = 0;
            bool orderExist = false;

            while (true)
            {
                try
                {
                    Console.Write("Enter order number: ");
                    orderNo = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid option.");
                }

                foreach (Order o in orderList) //retrieve selected order
                {
                    if (orderNo == o.OrderNo)
                    {
                        order = o;
                        orderExist = true;
                        break;
                    }
                }

                if (orderExist == false)
                {
                    Console.WriteLine("Please enter a valid option.");
                }
                else
                    break;
            }

            //check if screening has screened
            foreach (Ticket t in order.ticketList)
            {
                if (DateTime.Now > t.Screening.ScreeningDateTime)
                {
                    Console.WriteLine("The screening in the selected order has been screened. Unable to refund.");
                    Console.WriteLine("Cancellation unsuccessful.\n");
                    break;
                }
                else
                {
                    t.Screening.SeatsRemaining += order.ticketList.Count;
                    order.Status = "Cancelled";

                    if (order.Status == "Cancelled")
                    {
                        Console.WriteLine("{0} has been refunded.", order.Amount);
                        Console.WriteLine("Cancellation successful.\n");
                    }
                    else
                    {
                        Console.WriteLine("Cancellation unsuccessful, please try again.\n");
                    }
                }
            }
        }

        //------------------------------------------- Advanced (Top 3 movies recommended based on sale of tickets) ------------------------------------------------------
        public static void RecommendedMovies(List<Screening> screeningList, List<Movie> movieList, List<Order> orderList, List<Cinema> cinemaList)
        {
            List<Recommend> recommendedList = new List<Recommend>();
            int soldTickets = 0;

            foreach (Movie m in movieList) //finding out how many tickets are sold per movie
            {
                soldTickets = 0;

                foreach (Screening s in m.screeningList)
                {
                    if (s.SeatsRemaining < s.Cinema.Capacity) //check for number of ticket sold
                    {
                        soldTickets += (s.Cinema.Capacity - s.SeatsRemaining);
                    }
                }

                recommendedList.Add(new Recommend(m, soldTickets));
            }

            recommendedList.Sort(); //sort list by ticket sold
            int turns = 0;

            Console.WriteLine("{0,-25} {1,-5}", "Movie Title", "Number of Tickets Sold");
            foreach (Recommend r in recommendedList)
            {
                if (r.Sold > 0)
                {
                    Console.WriteLine("{0,-25} {1,-5}", r.Movie.Title, r.Sold);
                    turns++;
                }

                if (turns == 3)
                    break;
            }
        }
    }
}