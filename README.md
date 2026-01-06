# TCP-Quiz-System
A Client-Server Quiz Application built using C# and Socket Programming. This system allows multiple clients to connect to a central server, undergo an approval process, and participate in a real-time timed quiz.
# Multi-Client TCP Quiz System (C#)

A robust **Client-Server Quiz Application** built using C# and Socket Programming. This system allows multiple clients to connect to a central server, undergo an approval process, and participate in a real-time timed quiz.

## 🚀 Features
* **TCP/IP Networking:** Reliable communication between server and clients using `TcpListener` and `TcpClient`.
* **Multi-Threading:** The server can handle multiple clients simultaneously.
* **Admin Approval:** The server administrator can manually Accept (`y`) or Reject (`n`) a client's join request.
* **Timed Challenges:** Each question comes with a **30-second countdown**. If the client fails to answer, a timeout is triggered.
* **Instant Feedback:** After every question, the system reveals if the answer was correct or displays the right answer.
* **Final Scoring:** A comprehensive result is displayed to the client at the end of the 10-question set.

## 🛠️ Technologies Used
* **Language:** C#
* **Framework:** .NET Console Application
* **Protocol:** TCP/IP
* **Namespace:** `System.Net.Sockets`, `System.Threading`

## 📂 Project Structure
* **QuizServer:** Manages client connections, handles logic, and sends questions.
* **QuizClient:** User interface for participants to enter names and submit answers.

## 📝 How to Run
1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/YourUsername/TCP-Quiz-System.git](https://github.com/YourUsername/TCP-Quiz-System.git)
    ```
2.  **Run the Server:** Open the `QuizServer` project in Visual Studio and run it.
3.  **Run the Client(s):** Open the `QuizClient` project and run one or more instances.
4.  **Flow:** * Enter name on Client.
    * Approve on Server.
    * Answer 10 questions within the time limit!

## 📸 Demo
Check out my [LinkedIn Profile](PASTE_YOUR_LINKEDIN_VIDEO_POST_LINK_HERE) to see the video demonstration of this project in action!
