﻿using System.Globalization;

namespace LibraryManagementSystem.DTOs.StudentDTOs
{
    public class BookingHistoryDTO
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public string BorrowStatus { get; set; }

        public DateOnly? BorrowDate { get; set; }

        public string ReturnStatus { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public int Fine {  get; set; }
    }
}
