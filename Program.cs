using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace EpicLibrary3 {
    class Accounts {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public Accounts(string _username, string _password, bool _isadmin) {
            Username = _username;
            Password = _password;
            IsAdmin = _isadmin;
        }
    }

    class Book {
        public string Title { get; set; }
        public string Author { get; set; }
        public int ID { get; set; }
        public bool IsAvailable { get; set; }
        public string CheckedOutBy { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }

        public Book(string _title, string _author, int _id, string _genre, int _year, string _description) {
            Title = _title;
            Author = _author;
            ID = _id;
            IsAvailable = true;
            CheckedOutBy = "None";
            Genre = _genre;
            Year = _year;
            Description = _description;
        }
    }

    class Program {
        static List<Accounts> libraryAccounts = new List<Accounts>();
        static List<Book> libraryBooks = new List<Book>();

        static void Main(string[] args) {
            LoadDataFromFile();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "epiclibrary.com";
            Console.WriteLine(@"  _____ ____ ___ ____   _     ___ ____  ____      _    ______   __  _____ ");
            Console.WriteLine(@" | ____|  _ \_ _/ ___| | |   |_ _| __ )|  _ \    / \  |  _ \ \ / / |___ / ");
            Console.WriteLine(@" |  _| | |_) | | |     | |    | ||  _ \| |_) |  / _ \ | |_) \ V /    |_ \ ");
            Console.WriteLine(@" | |___|  __/| | |___  | |___ | || |_) |  _ <  / ___ \|  _ < | |    ___) |");
            Console.WriteLine(@" |_____|_|  |___\____| |_____|___|____/|_| \_\/_/   \_\_| \_\|_|   |____/ ");
            Loading("Loading program . ", 15);
            Console.WriteLine("");
            Wait("continue");

            bool error = false;
            while (true) {
                Console.Title = "epiclibrary.com";
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(" · Epic Library 3 ·");
                Console.ForegroundColor = ConsoleColor.White;
                if (error == true) {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(" Unknown Command.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    error = false;
                }
                else {
                    Console.WriteLine("\n");
                }
                Console.WriteLine(" Commands:\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("     · /log");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("in");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("     · /reg");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("ister");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("     · /a");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("bout");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("     · /e");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("xit");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n > ");
                string choice = Console.ReadLine()!.Trim();
                if (string.IsNullOrWhiteSpace(choice)) {continue;}
                switch (choice) {
                    case "/register":
                    case "/reg":
                    case "/r":
                        Register();
                        break;
                    case "/login":
                    case "/log":
                    case "/l":
                        Login();
                        break;
                    case "/about":
                    case "/a":
                        About();
                        break;
                    case "/exit":
                    case "/ex":
                    case "/e":
                        Environment.Exit(0);
                        break;
                    default:
                        error = true;
                        break;
                }
            }
        }

        static void LoadDataFromFile() {
            if (File.Exists(@"accounts.txt")) {
                var accountLines = File.ReadAllLines(@"accounts.txt");
                foreach (var line in accountLines) {
                    var parts = line.Split('|');
                    if (parts.Length == 3) {
                        string username = parts[0];
                        string password = parts[1];
                        bool isAdmin = bool.Parse(parts[2]);
                        libraryAccounts.Add(new Accounts(username, password, isAdmin));
                    }
                }
            }

            if (File.Exists(@"books.txt")) {
                var bookLines = File.ReadAllLines(@"books.txt");
                foreach (var line in bookLines) {
                    var parts = line.Split('|');
                    if (parts.Length >= 7) {
                        string title = parts[0];
                        string author = parts[1];
                        int id = int.Parse(parts[2]);
                        bool isAvailable = bool.Parse(parts[3]);
                        string checkedOutBy = parts[4];
                        string genre = parts[5];
                        int year = int.Parse(parts[6]);
                        string description = string.Join("|", parts, 7, parts.Length - 7);

                        var book = new Book(title, author, id, genre, year, description) {
                            IsAvailable = isAvailable,
                            CheckedOutBy = checkedOutBy
                        };
                        libraryBooks.Add(book);
                    }
                }
            }
        }

        static void SaveDataToFile() {
            var accountLines = libraryAccounts.Select(account => $"{account.Username}|{account.Password}|{account.IsAdmin}");
            File.WriteAllLines(@"accounts.txt", accountLines);

            var bookLines = libraryBooks.Select(book =>
                $"{book.Title}|{book.Author}|{book.ID}|{book.IsAvailable}|{book.CheckedOutBy}|{book.Genre}|{book.Year}|{book.Description.Replace("\n", " ")}");
            File.WriteAllLines(@"books.txt", bookLines);
        }

        static void About() {
            Console.Title = "epiclibrary.com/about";
            Console.Clear();
            Console.WriteLine(" · About ·\n");
            Console.WriteLine(" · Christian G. Canturias BSCS-I · Copyright 2024");
            Console.WriteLine(" · Library Management System 2 · Intermediate Programming (CC103)");
            System.Console.WriteLine(" · Mr. Edgardo Olmoguez II · M–W (16:00–17:30)");
            Wait("continue");
        }

        static void Wait(string type) {
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"\n (Press 'enter' to {type}...)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey(true);
            Console.Clear();
        }

        static void Loading(string message, int steps) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"\n {message}");
            for (int i = 0; i < steps; i++) {
                Console.Write(". ");
                Thread.Sleep(200);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" Done!");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(250);
        }

        static void Register() {
            Console.Title = "epiclibrary.com/register";
            Console.Clear();
            Console.WriteLine(" · Create an account ·\n");
            Console.Write(" Username: @");
            string newUsername = Console.ReadLine()!;
            if (!Regex.IsMatch(newUsername, @"^[a-zA-Z0-9._]+$")) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Usernames can only contain letters, numbers, periods, and underscores.");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            if (libraryAccounts.Exists(account => account.Username.Equals(newUsername, StringComparison.OrdinalIgnoreCase))) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n This username is already taken. Please choose a different username.");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            Console.Write(" Password: ");
            string newPassword = Console.ReadLine()!;
            if (newPassword.Length < 8) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Password must be at least 8 characters long.");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            else if (string.IsNullOrWhiteSpace(newPassword)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Passwords can't be blank.");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            else if (Regex.IsMatch(newPassword, @"\s")) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Passwords can't have spaces.");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            Console.Write(" Admin code: ");
            bool newIsAdmin = Console.ReadLine()!.ToLower() == "qwerty";
            if (!newIsAdmin) {
                Console.Write("\n Incorrect admin code. Your account will be a standard user.\n");
            }
            else {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n Correct admin code. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Your account will be an ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("admin ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("user.");
            }
            Accounts newAccount = new Accounts(newUsername, newPassword, newIsAdmin);
            libraryAccounts.Add(newAccount);
            SaveDataToFile();
            Loading("Creating account . ", 15);
        }

        static void Login() {
            Console.Title = "epiclibrary.com/login";
            Console.Clear();
            Console.WriteLine(" · Sign in to your account ·\n");
            Console.Write(" Username: @");
            string username = Console.ReadLine()!;
            Accounts? account = libraryAccounts.Find(account => account.Username == username);
            if (account == null) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Invalid username.");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }

            Console.Write(" Password: ");
            string password = Console.ReadLine()!;
            if (account.Password == password) {
                bool isAdmin = account.IsAdmin;
                if (account != null) {
                    Loading("Logging in . ", 15);
                    Console.Clear();
                    EpicLibrary2(isAdmin, username);
                } 
            } 
            else {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Invalid password.");
                Wait("return");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void EpicLibrary2(bool isAdmin, string username) {
            Console.Title = "epiclibrary.com/home";
            bool error = false;
            bool exit = false;
            while (!exit) {
                Console.Title = "epiclibrary.com/home";
                if (isAdmin) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($" Welcome @{username}");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (error == true) {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(" · Unknown Command.\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        error = false;
                    }
                    else {
                        Console.WriteLine("\n");
                    }
                }
                else {
                    Console.Write($" Welcome @{username}");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (error == true) {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(" · Unknown Command.\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        error = false;
                    }
                    else {
                        Console.WriteLine("\n");
                    }
                }

                Console.WriteLine(" Commands:");
                Console.Write("\n     · /displaybooks\n     · /checkout\n     · /checkin\n     · /inventory");

                if (isAdmin == true) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n     · /addbook\n     · /removebook");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("     · /logout");
                    Console.Write($"\n @{username} > ");
                    string choice = Console.ReadLine()!;
                    Console.Clear();
                    switch(choice) {
                        case "/displaybooks":
                        case "/display":
                        case "/db":
                            DisplayBooks();
                            Wait("continue");
                            break;
                        case "/checkout":
                        case "/out":
                        case "/co":
                            DisplayBooks();
                            CheckOutBook(username);
                            break;
                        case "/checkin":
                        case "/in":
                        case "/ci":
                            DisplayBooks();
                            CheckInBook(username);
                            break;
                        case "/inventory":
                        case "/inv":
                        case "/i":
                            DisplayInventory(username);
                            break;
                        case "/addbook":
                        case "/add":
                        case "/ab":
                            DisplayBooks();
                            AddBook(username);
                            break;
                        case "/removebook":
                        case "/remove":
                        case "/rb":
                            DisplayBooks();
                            RemoveBook(username);
                            break;
                        case "/logout":
                        case "/lo":
                        exit = true; //logout
                            break;
                        default:
                            error = true;
                            Console.WriteLine(" Invalid input!");
                            break;
                    }
                }
                else {
                    Console.WriteLine("\n     · /logout");
                    Console.Write($"\n @{username} > ");
                    string choice = Console.ReadLine()!;
                    Console.Clear();
                    switch(choice) {
                        case "/displaybooks":
                        case "/display":
                        case "/db":
                            DisplayBooks();
                            Wait("continue");
                            break;
                        case "/checkout":
                        case "/out":
                        case "/co":
                            DisplayBooks();
                            CheckOutBook(username);
                            break;
                        case "/checkin":
                        case "/in":
                        case "/ci":
                            DisplayBooks();
                            CheckInBook(username);
                            break;
                        case "/inventory":
                        case "/inv":
                        case "/i":
                            DisplayInventory(username);
                            break;
                        case "/logout":
                        case "/lo":
                        exit = true; //logout
                            break;
                        default:
                            error = true;
                            Console.WriteLine(" Invalid input!");
                            break;
                    }
                } Console.Clear();
            }     
        }
        static void DisplayBooks() {
            Console.Title = "epiclibrary.com/books";
            Console.WriteLine($" Number of Books: {libraryBooks.Count}\n");
            Console.WriteLine(" List of Books:");
            libraryBooks = libraryBooks.OrderBy(book => book.ID).ToList(); //Sorts books by ID number
            for (int i = 0; i < libraryBooks.Count; i++) {
                var book = libraryBooks[i];
                Console.Write($"\n {i + 1}.)\t'{book.Title}'");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($" by {book.Author}");
                Console.WriteLine($"\t'{book.Description}'");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"\t Genre: {book.Genre}");
                Console.WriteLine($" · Year: {book.Year}");
                Console.Write($"\t Book ID: {book.ID} ");
                Console.ForegroundColor = book.IsAvailable ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write(book.IsAvailable ? "(Available)" : "(Unavailable)");
                Console.ForegroundColor = ConsoleColor.White;
                if (book.CheckedOutBy != "None") {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($" · Checked Out By: @{book.CheckedOutBy}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("");
            }
        }
        static void CheckOutBook(string username) {
            Console.Title = "epiclibrary.com/checkout";
            Console.Write("\n Enter the ID of the book to check out.");
            Console.Write($"\n @{username} > ");
            string input = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(input)) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n Invalid input.");
            Console.ForegroundColor = ConsoleColor.White;
            Wait("return");
            return;
            }

            if (!int.TryParse(input, out int parsedID)) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n Invalid input. Please enter a valid book ID (a whole number).");
            Console.ForegroundColor = ConsoleColor.White;
            Wait("return");
            return;
            }
            Book? bookToCheckout = libraryBooks.Find(book => book.ID == parsedID);
            if (bookToCheckout != null) {
                if (bookToCheckout.IsAvailable) {
                    bookToCheckout.IsAvailable = false;
                    bookToCheckout.CheckedOutBy = username;
                    SaveDataToFile();
                    Loading("Checking out . ", 15);
                    Console.WriteLine("\n Book checked out successfully!");
                }
                else {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n Book is currently checked out and is not available!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Book not found.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Wait("return");
        }
        static void CheckInBook(string username) {
            Console.Title = "epiclibrary.com/checkin";
            Console.Write("\n Enter the ID of the book to check in.");
            Console.Write($"\n @{username} > ");
            string input = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(input)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Invalid input.");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            if (!int.TryParse(input, out int parsedId)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Invalid input. Please enter a valid book ID (a whole number).");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }

            Book? bookToCheckIn = libraryBooks.Find(book => book.Title == input || book.ID == parsedId);

            if (bookToCheckIn != null) {
                if (!bookToCheckIn.IsAvailable) {
                    if (bookToCheckIn.CheckedOutBy == username) {
                        bookToCheckIn.IsAvailable = true;
                        bookToCheckIn.CheckedOutBy = "None";
                        SaveDataToFile();
                        Loading("Checking in . ", 15);
                        Console.WriteLine("\n Book checked in successfully!");
                    }
                    else {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("\n This book is not in your inventory.");
                    }
                }
                else {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n This book is not in your inventory.");
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Book not found.");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Wait("return");
        }
        static void DisplayInventory(string username) {
            Console.Title = $"epiclibrary.com/@{username}";
            Console.WriteLine($" Your inventory @{username}");
            var userBooks = libraryBooks.Where(book => book.CheckedOutBy == username).ToList();
            if (userBooks.Count > 0) {
                for (int i = 0; i < userBooks.Count; i++) {
                    var book = userBooks[i];
                    Console.Write($"\n {i + 1}.)\t'{book.Title}'");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($" by {book.Author}");
                    Console.WriteLine($"\t'{book.Description}'");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"\t Genre: {book.Genre}");
                    Console.WriteLine($" · Year: {book.Year}");
                    Console.Write($"\t Book ID: {book.ID}");
                }
                Console.WriteLine("");
            }
            else {
                Console.WriteLine("\n You have no books currently checked out.");
            }
            Wait("return");
        }
        static void AddBook(string username) {
            Console.Title = "epiclibrary.com/addbook";
            Console.WriteLine("\n Enter book details.\n");
            Console.Write(" Title: ");
            string newTitle = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(newTitle)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Title cannot be blank!");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            Console.Write(" Author: ");
            string newAuthor = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(newAuthor)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Author cannot be blank!");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            Console.Write(" Genre: ");
            string newGenre = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(newGenre)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Genre cannot be blank!");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            Console.Write(" Year: ");
            if (!int.TryParse(Console.ReadLine(), out int newYear)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Year must be a valid number!");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            Console.Write(" Description: ");
            string newDescription = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(newDescription)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Description cannot be blank!");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            Console.Write(" ID: ");
            if (!int.TryParse(Console.ReadLine(), out int newId)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Book ID must be a whole number!");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            if (libraryBooks.Exists(book => book.ID == newId)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n A book with the same ID already exists. Please enter a unique ID.");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            libraryBooks.Add(new Book(newTitle, newAuthor, newId, newGenre, newYear, newDescription));
            SaveDataToFile();
            Loading("Adding book . ", 15);
            Console.WriteLine("\n Book added successfully!");
            Wait("continue");
        }

        static void RemoveBook(string username) {
            Console.Title = "epiclibrary.com/removebook";
            Console.Write("\n Enter the ID of the book to remove.");
            Console.Write($"\n @{username} > ");
            if (!int.TryParse(Console.ReadLine(), out int input)) {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Invalid input! Please enter a valid book ID (a whole number).");
                Console.ForegroundColor = ConsoleColor.White;
                Wait("return");
                return;
            }
            Book? bookToRemove = libraryBooks.Find(book => book.ID == input);
            if (bookToRemove != null) {
                if (bookToRemove.IsAvailable) {
                    libraryBooks.Remove(bookToRemove);
                    SaveDataToFile();
                    Loading("Removing book . ", 15);
                    Console.WriteLine("\n Book removed successfully!");
                }
                else {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n This book is currently checked out. Try again later.");
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n Book not found!");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Wait("return");
        }
    }
}

