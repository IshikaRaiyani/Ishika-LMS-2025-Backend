# ?? Library Management System (LMS)

---

## ?? Project Overview
A full-stack **Library Management System (LMS)** web application where:
- **Students** can browse and borrow books.
- **Librarians** manage borrow/return approvals and book records.
- **Admins** oversee users and system data.

## ??? Tech Stack

| Layer               | Technology                          |
|--------------------|--------------------------------------|
| Frontend           | Angular v18+                         |
| Backend API        | .NET Core Web API v6+                |
| ORM                | Entity Framework Core (Code-First)  |
| Database           | Microsoft SQL Server                |
| Authentication     | JWT Token-Based Authentication       |
| API Testing        | Swagger / Postman                   |
| Version Control    | Git + GitHub                        |

## ?? Core Functionalities

### ?? Admin Panel
Admins hold the highest authority and can manage the entire ecosystem.

- ?? **User Management**  
  - Add new users (Students or Librarians)  
  - Block/Unblock user access  
  - Assign or revoke Librarian roles  

- ?? **Books & Transactions**  
  - View all books and their availability  
  - Monitor borrow and return transactions system-wide  

- ?? **Dashboard Overview**  
  - Get insights on active users, total books, and system health  
  - Centralized control panel for navigating across module


### ?? Librarian Console

- ? **Approval System**  
  - Review borrow and return requests  
  - Approve or reject with a single click  

- ?? **Book Management**  
  - Add new books to the catalog  
  - Edit book details (title, author, genre, quantity)  
  - *Note: Deleting books is restricted to Admins*  

- ????? **Student Records (Read-Only)**  
  - Access student profiles to validate borrowers  
  - No editing rights to ensure privacy and security

### ????? Student Dashboard

Students are the primary users who engage with the library resources.

- ?? **Explore & Discover**  
  - Search books by title, author, or genre  
  - Filter by availability to easily find borrowable items  

- ?? **Borrowing System**  
  - Request to borrow available books  
  - Track borrowed books and due dates  

- ?? **Return Requests**  
  - Initiate return for borrowed books  
  - Track approval status and penalties (if any)  

- ?? **Borrowing History**  
  - View all past and active transactions in a personal log  

- ?? **Wishlist & Notifications**  
  - Add unavailable books to wishlist  
  - Get notified via email + in-app when they become available

## ? Conclusion

The Library Management System (LMS) is a comprehensive web-based application that streamlines the management of library operations for Students, Librarians, and Admins. By implementing modern technologies like Angular and .NET Core along with role-based JWT authentication, the system ensures secure, efficient, and user-friendly access to library resources.

This project not only emphasizes clean code architecture and layered separation of concerns but also showcases real-world application flow, including:

- User management and authorization
- Book inventory control
- Borrow and return lifecycle with validations
- Notifications and wishlist integration

With features designed for scalability, security, and performance, this system can serve as a strong foundation for any educational or institutional library seeking digital transformation.