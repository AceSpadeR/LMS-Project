using Microsoft.EntityFrameworkCore;
using LMS.Data;
using System;

namespace LMS.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LMSContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<LMSContext>>()))
            {
                if (context == null || context.User == null)
                {
                    throw new ArgumentNullException("Null LMSContext");
                }

                #region Users
                if (!context.User.Any())
                {
                    context.User.AddRange(
                        new User
                        {
                            FirstName = "Super",
                            LastName = "Admin",
                            Email = "super@admin.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Super Admin",
                            Password = "C8A78F0EF3C507B3416A71671437F229BF79A5299019D4830D304EA1FF2836F3A886254ABDC36B249E29F0F767F1EAD9F6DBFFC4E4115ACEC218BB92B8D0C6C1",
                            ConfirmPassword = "C8A78F0EF3C507B3416A71671437F229BF79A5299019D4830D304EA1FF2836F3A886254ABDC36B249E29F0F767F1EAD9F6DBFFC4E4115ACEC218BB92B8D0C6C1"
                            // Password: SuperAdmin
                        },
                        new User
                        {
                            FirstName = "TeacherFirst",
                            LastName = "TeacherLast",
                            Email = "teacher@teacher.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Instructor",
                            Password = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B",
                            ConfirmPassword = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B"
                            // Password: Teacher
                        },
                        new User
                        {
                            FirstName = "StudentFirst",
                            LastName = "StudentLast",
                            Email = "student@student.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "John",
                            LastName = "Smith",
                            Email = "john.smith@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Instructor",
                            Password = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B",
                            ConfirmPassword = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B"
                            // Password: Teacher
                        },
                        new User
                        {
                            FirstName = "Amber",
                            LastName = "Frizzel",
                            Email = "amber.frizzel@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Instructor",
                            Password = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B",
                            ConfirmPassword = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B"
                            // Password: Teacher
                        },
                        new User
                        {
                            FirstName = "Bill",
                            LastName = "Niegh",
                            Email = "bill.niegh@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Instructor",
                            Password = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B",
                            ConfirmPassword = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B"
                            // Password: Teacher
                        },

                        // Students
                        new User
                        {
                            FirstName = "Clara",
                            LastName = "Adams",
                            Email = "clara.adams@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "Willa",
                            LastName = "Bennett",
                            Email = "willa.bennett@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "William",
                            LastName = "Carter",
                            Email = "william.carter@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "Simon",
                            LastName = "Davis",
                            Email = "simon.davis@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "Thomas",
                            LastName = "Edwards",
                            Email = "thomas.Edwards@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "Rose",
                            LastName = "Foster",
                            Email = "rose.Foster@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "Andrew",
                            LastName = "Gray",
                            Email = "andrew.gray@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "Emerson",
                            LastName = "Hayes",
                            Email = "emerson.hayes@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "Harvey",
                            LastName = "Ingram",
                            Email = "harvey.ingram@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        },
                        new User
                        {
                            FirstName = "Reed",
                            LastName = "Jensen",
                            Email = "reed.jensen@stellarlms.com",
                            BirthDate = DateTime.Parse("2000-1-1"),
                            Role = "Student",
                            Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F",
                            ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F"
                            // Password: Student
                        }


                    );

                    context.SaveChanges();
                }
                #endregion

                #region Classes
                if (!context.Class.Any())
                {
                    var prof = context.User.Where(u => u.Email == "teacher@teacher.com").First();
                    
                    context.Class.AddRange(
                        new Class
                        {
                            DepartmentName = "Computer Science",
                            CourseNumber = "CS 3750",
                            Title = "Sofware Engineering II",
                            Location = "Noorda 318",
                            CreditHours = 4,
                            StartTime = DateTime.Parse("9:30 AM"),
                            EndTime = DateTime.Parse("11:20 AM"),
                            MeetingDays = new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Thursday },
                            ProfId = prof.Id
                        },

                        new Class
                        {
                            DepartmentName = "Computer Science",
                            CourseNumber = "CS 2350",
                            Title = "Client Side Web Development",
                            Location = "Noorda 215",
                            CreditHours = 4,
                            StartTime = DateTime.Parse("12:30 PM"),
                            EndTime = DateTime.Parse("1:20 PM"),
                            MeetingDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                            ProfId = prof.Id
                        }
                    );

                    prof = context.User.Where(u => u.Email == "john.smith@stellarlms.com").First();
                    context.Class.AddRange(
                        new Class
                        {
                            DepartmentName = "Math",
                            CourseNumber = "MATH 1010",
                            Title = "Intermediate Algebra",
                            Location = "Noorda 118",
                            CreditHours = 4,
                            StartTime = DateTime.Parse("11:30 AM"),
                            EndTime = DateTime.Parse("2:20 PM"),
                            MeetingDays = new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Thursday },
                            ProfId = prof.Id
                        },

                        new Class
                        {
                            DepartmentName = "Math",
                            CourseNumber = "MATH 3020",
                            Title = "Geometry and Statistics",
                            Location = "Noorda 110",
                            CreditHours = 3,
                            StartTime = DateTime.Parse("4:00 PM"),
                            EndTime = DateTime.Parse("4:50 PM"),
                            MeetingDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                            ProfId = prof.Id
                        }
                    );

                    prof = context.User.Where(u => u.Email == "amber.frizzel@stellarlms.com").First();
                    context.Class.AddRange(
                        new Class
                        {
                            DepartmentName = "Chemistry",
                            CourseNumber = "CHEM 2040",
                            Title = "Organic Chemistry I",
                            Location = "SRA 108",
                            CreditHours = 5,
                            StartTime = DateTime.Parse("8:30 AM"),
                            EndTime = DateTime.Parse("12:20 PM"),
                            MeetingDays = new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Thursday },
                            ProfId = prof.Id
                        },

                        new Class
                        {
                            DepartmentName = "Chemistry",
                            CourseNumber = "CHEM 1010",
                            Title = "Environmental Chemistry",
                            Location = "SRA 121",
                            CreditHours = 4,
                            StartTime = DateTime.Parse("12:30 PM"),
                            EndTime = DateTime.Parse("2:20 PM"),
                            MeetingDays = new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Thursday },
                            ProfId = prof.Id
                        }
                    );

                    prof = context.User.Where(u => u.Email == "bill.niegh@stellarlms.com").First();
                    context.Class.AddRange(
                        new Class
                        {
                            DepartmentName = "Physics",
                            CourseNumber = "PHYS 1020",
                            Title = "Fundamentals of Physics",
                            Location = "RMU 301",
                            CreditHours = 3,
                            StartTime = DateTime.Parse("3:30 PM"),
                            EndTime = DateTime.Parse("4:20 PM"),
                            MeetingDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                            ProfId = prof.Id
                        },

                        new Class
                        {
                            DepartmentName = "Physics",
                            CourseNumber = "PHYS 3100",
                            Title = "Introduction to Modern Physics",
                            Location = "RMU 222",
                            CreditHours = 4,
                            StartTime = DateTime.Parse("4:30 PM"),
                            EndTime = DateTime.Parse("6:20 PM"),
                            MeetingDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday},
                            ProfId = prof.Id
                        }
                    );

                    context.SaveChanges();
                }
                #endregion

                #region Events
                if (!context.Event.Any())
                {
                    List<User> profs = context.User.Where(u => u.Role == "Instructor").ToList();
                    foreach (var p in profs)
                    {
                        var prof = p;
                        var profClasses = context.Class.Where(c => c.ProfId == prof.Id).ToList();

                        foreach (var profClass in profClasses)
                        {
                            context.Event.Add(
                                new Event
                                {
                                    EventType = profClass.CourseNumber,
                                    EventDescription = "This is a description of the event.",
                                    UserId = prof.Id,
                                    ClassId = profClass.Id,

                                }
                            );
                        }
                    }

                }
                #endregion

                #region Assignments
                if (!context.Assignment.Any() ) 
                {
                    var tClass = context.Class.Where(u => u.CourseNumber == "CS 3750").First();
                    context.Assignment.AddRange(
                        new Assignment
                        {
                            Title = "Process Synchronization and Deadlock Handling II",
                            Description = "In this assignment, you’ll explore fundamental concepts related to process synchronization and deadlock prevention in operating systems. Your task is to implement a simple simulation of concurrent processes using semaphores or mutexes. Here are the key components of the assignment:\r\n\r\nProcess Simulation: Create a program that simulates multiple concurrent processes (threads) running in parallel. Each process represents a task (e.g., reading/writing to a shared resource, performing calculations, etc.).\r\nShared Resource Access: Design a critical section where processes compete for access to a shared resource (e.g., a printer, a file, or a database). Use semaphores or mutexes to ensure mutual exclusion and prevent race conditions.\r\nDeadlock Handling: Introduce scenarios where processes can potentially deadlock (e.g., circular wait, resource allocation graph). Implement deadlock detection and recovery mechanisms (e.g., timeout-based termination, resource preemption).\r\nTesting and Documentation: Thoroughly test your program with various input scenarios. Provide clear documentation explaining your design choices, how you handled synchronization, and your deadlock prevention strategies.\r\nRemember to adhere to good programming practices, such as proper error handling, efficient resource utilization, and clear code organization. Bonus points will be awarded for creative deadlock prevention techniques!",
                            DueDate = DateTime.Parse("05/01/2024 23:59:00"),
                            MaxPoints = 250,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        },

                        new Assignment
                        {
                            Title = "Secure File Encryption System II",
                            Description = "Objective: Design and implement a secure file encryption system using modern cryptographic techniques. Your system should allow users to encrypt and decrypt files, ensuring confidentiality and integrity. Consider key management, algorithm selection, and performance trade-offs. You must also provide a user-friendly command-line interface (CLI) for interacting with the system.\r\n\r\nRequirements:\r\n\r\nEncryption Algorithm: Choose an appropriate symmetric encryption algorithm (e.g., AES) and justify your selection.\r\nKey Generation and Management: Implement a secure key generation process and explore key storage options.\r\nFile I/O: Read files, encrypt their contents, and save the encrypted data to a new file. Decrypt files back to their original form.\r\nError Handling: Handle exceptions gracefully, especially during file I/O and key management.\r\nPerformance Analysis: Evaluate the system’s performance in terms of encryption/decryption speed and memory usage.\r\nDocumentation: Provide clear instructions on how to use your system, including examples.\r\nSubmission: Submit your code, a brief report explaining your design choices, and a demonstration video showcasing your system’s functionality.\r\n\r\n",
                            DueDate = DateTime.Parse("05/08/2024 23:59:00"),
                            MaxPoints = 150,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        },

                        new Assignment
                        {
                            Title = "Distributed Chat Application II",
                            Description = "Objective: Develop a distributed chat application that allows users to communicate securely over a network. Your system should support multiple clients connecting to a central server, exchanging messages, and ensuring confidentiality and integrity. Consider scalability, fault tolerance, and security aspects.\r\n\r\nRequirements:\r\n\r\nClient-Server Architecture: Design a client-server model where clients connect to a central server for message exchange.\r\nUser Authentication: Implement user authentication using secure protocols (e.g., OAuth, JWT).\r\nMessage Encryption: Encrypt chat messages using a hybrid encryption scheme (e.g., RSA for key exchange and AES for message encryption).\r\nKey Management: Explore key distribution and management techniques for secure communication.\r\nScalability: Consider how your system will handle a large number of concurrent users.\r\nError Handling: Handle network failures, disconnections, and other exceptions gracefully.\r\nDocumentation: Provide clear instructions on setting up and using your chat application.\r\nSubmission: Submit your code, a detailed design document explaining your architecture, and a demo video showcasing your chat application in action.",
                            DueDate = DateTime.Parse("05/15/2024 23:59:00"),
                            MaxPoints = 100,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        },

                        new Assignment
                        {
                            Title = "Blockchain-Based Voting System II",
                            Description = "Objective: Create a secure and transparent voting system using blockchain technology. Your system should allow voters to cast their ballots securely, prevent double voting, and ensure the integrity of election results. Consider consensus algorithms, smart contracts, and user experience.\r\n\r\nRequirements:\r\n\r\nBlockchain Implementation: Choose a suitable blockchain framework (e.g., Ethereum, Hyperledger Fabric) and set up a private blockchain network.\r\nVoter Registration: Develop a process for voter registration and identity verification.\r\nBallot Creation: Design a smart contract to create and manage election ballots.\r\nVote Casting: Implement a user-friendly interface for voters to cast their votes securely.\r\nConsensus Mechanism: Explore different consensus algorithms (e.g., Proof of Work, Proof of Stake) and justify your choice.\r\nSecurity Measures: Address potential attacks (e.g., Sybil attacks, 51% attacks) and propose countermeasures.\r\nAudit Trail: Ensure that all transactions are recorded on the blockchain for transparency.\r\nDocumentation: Provide clear instructions for deploying and using your voting system.\r\nSubmission: Submit your code, a detailed report explaining your design decisions, and a demonstration video showcasing the end-to-end functionality of your blockchain-based voting system.",
                            DueDate = DateTime.Parse("05/22/2024 23:59:00"),
                            MaxPoints = 400,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        }
                        );

                    tClass = context.Class.Where(u => u.CourseNumber == "CS 2350").First();
                    context.Assignment.AddRange(
                        new Assignment
                        {
                            Title = "Process Synchronization and Deadlock Handling",
                            Description = "In this assignment, you’ll explore fundamental concepts related to process synchronization and deadlock prevention in operating systems. Your task is to implement a simple simulation of concurrent processes using semaphores or mutexes. Here are the key components of the assignment:\r\n\r\nProcess Simulation: Create a program that simulates multiple concurrent processes (threads) running in parallel. Each process represents a task (e.g., reading/writing to a shared resource, performing calculations, etc.).\r\nShared Resource Access: Design a critical section where processes compete for access to a shared resource (e.g., a printer, a file, or a database). Use semaphores or mutexes to ensure mutual exclusion and prevent race conditions.\r\nDeadlock Handling: Introduce scenarios where processes can potentially deadlock (e.g., circular wait, resource allocation graph). Implement deadlock detection and recovery mechanisms (e.g., timeout-based termination, resource preemption).\r\nTesting and Documentation: Thoroughly test your program with various input scenarios. Provide clear documentation explaining your design choices, how you handled synchronization, and your deadlock prevention strategies.\r\nRemember to adhere to good programming practices, such as proper error handling, efficient resource utilization, and clear code organization. Bonus points will be awarded for creative deadlock prevention techniques!",
                            DueDate = DateTime.Parse("04/01/2024 23:59:00"),
                            MaxPoints = 250,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        },

                        new Assignment
                        {
                            Title = "Secure File Encryption System",
                            Description = "Objective: Design and implement a secure file encryption system using modern cryptographic techniques. Your system should allow users to encrypt and decrypt files, ensuring confidentiality and integrity. Consider key management, algorithm selection, and performance trade-offs. You must also provide a user-friendly command-line interface (CLI) for interacting with the system.\r\n\r\nRequirements:\r\n\r\nEncryption Algorithm: Choose an appropriate symmetric encryption algorithm (e.g., AES) and justify your selection.\r\nKey Generation and Management: Implement a secure key generation process and explore key storage options.\r\nFile I/O: Read files, encrypt their contents, and save the encrypted data to a new file. Decrypt files back to their original form.\r\nError Handling: Handle exceptions gracefully, especially during file I/O and key management.\r\nPerformance Analysis: Evaluate the system’s performance in terms of encryption/decryption speed and memory usage.\r\nDocumentation: Provide clear instructions on how to use your system, including examples.\r\nSubmission: Submit your code, a brief report explaining your design choices, and a demonstration video showcasing your system’s functionality.\r\n\r\n",
                            DueDate = DateTime.Parse("05/08/2024 23:59:00"),
                            MaxPoints = 150,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        },

                        new Assignment
                        {
                            Title = "Distributed Chat Application",
                            Description = "Objective: Develop a distributed chat application that allows users to communicate securely over a network. Your system should support multiple clients connecting to a central server, exchanging messages, and ensuring confidentiality and integrity. Consider scalability, fault tolerance, and security aspects.\r\n\r\nRequirements:\r\n\r\nClient-Server Architecture: Design a client-server model where clients connect to a central server for message exchange.\r\nUser Authentication: Implement user authentication using secure protocols (e.g., OAuth, JWT).\r\nMessage Encryption: Encrypt chat messages using a hybrid encryption scheme (e.g., RSA for key exchange and AES for message encryption).\r\nKey Management: Explore key distribution and management techniques for secure communication.\r\nScalability: Consider how your system will handle a large number of concurrent users.\r\nError Handling: Handle network failures, disconnections, and other exceptions gracefully.\r\nDocumentation: Provide clear instructions on setting up and using your chat application.\r\nSubmission: Submit your code, a detailed design document explaining your architecture, and a demo video showcasing your chat application in action.",
                            DueDate = DateTime.Parse("05/15/2024 23:59:00"),
                            MaxPoints = 100,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        },

                        new Assignment
                        {
                            Title = "Blockchain-Based Voting System",
                            Description = "Objective: Create a secure and transparent voting system using blockchain technology. Your system should allow voters to cast their ballots securely, prevent double voting, and ensure the integrity of election results. Consider consensus algorithms, smart contracts, and user experience.\r\n\r\nRequirements:\r\n\r\nBlockchain Implementation: Choose a suitable blockchain framework (e.g., Ethereum, Hyperledger Fabric) and set up a private blockchain network.\r\nVoter Registration: Develop a process for voter registration and identity verification.\r\nBallot Creation: Design a smart contract to create and manage election ballots.\r\nVote Casting: Implement a user-friendly interface for voters to cast their votes securely.\r\nConsensus Mechanism: Explore different consensus algorithms (e.g., Proof of Work, Proof of Stake) and justify your choice.\r\nSecurity Measures: Address potential attacks (e.g., Sybil attacks, 51% attacks) and propose countermeasures.\r\nAudit Trail: Ensure that all transactions are recorded on the blockchain for transparency.\r\nDocumentation: Provide clear instructions for deploying and using your voting system.\r\nSubmission: Submit your code, a detailed report explaining your design decisions, and a demonstration video showcasing the end-to-end functionality of your blockchain-based voting system.",
                            DueDate = DateTime.Parse("05/22/2024 23:59:00"),
                            MaxPoints = 400,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        }
                        );
                    tClass = context.Class.Where(u => u.CourseNumber == "MATH 1010").First();
                    context.Assignment.AddRange(
                        new Assignment
                        {
                            Title = "Syllabus: The Dynamics of Change: A Cross-Disciplinary Exploration",
                            Description = "Consider the concept of ‘change’ as a driving force in both natural and social phenomena. In a few sentences, explain how the idea of change is relevant to a subject you are currently studying, and provide an example of this change in action.",
                            DueDate = DateTime.Parse("05/11/2024 23:59:00"),
                            MaxPoints = 50,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        }
                        );
                    tClass = context.Class.Where(u => u.CourseNumber == "MATH 3020").First();
                    context.Assignment.AddRange(
                        new Assignment
                        {
                            Title = "Syllabus: The Dynamics of Change: A Cross-Disciplinary Exploration",
                            Description = "Consider the concept of ‘change’ as a driving force in both natural and social phenomena. In a few sentences, explain how the idea of change is relevant to a subject you are currently studying, and provide an example of this change in action.",
                            DueDate = DateTime.Parse("05/13/2024 23:59:00"),
                            MaxPoints = 50,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        }
                        );
                    tClass = context.Class.Where(u => u.CourseNumber == "CHEM 2040").First();
                    context.Assignment.AddRange(
                        new Assignment
                        {
                            Title = "Syllabus: The Dynamics of Change: A Cross-Disciplinary Exploration",
                            Description = "Consider the concept of ‘change’ as a driving force in both natural and social phenomena. In a few sentences, explain how the idea of change is relevant to a subject you are currently studying, and provide an example of this change in action.",
                            DueDate = DateTime.Parse("05/12/2024 23:59:00"),
                            MaxPoints = 50,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        }
                        );
                    tClass = context.Class.Where(u => u.CourseNumber == "CHEM 1010").First();
                    context.Assignment.AddRange(
                        new Assignment
                        {
                            Title = "Syllabus: The Dynamics of Change: A Cross-Disciplinary Exploration",
                            Description = "Consider the concept of ‘change’ as a driving force in both natural and social phenomena. In a few sentences, explain how the idea of change is relevant to a subject you are currently studying, and provide an example of this change in action.",
                            DueDate = DateTime.Parse("05/12/2024 23:59:00"),
                            MaxPoints = 50,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        }
                        );
                    tClass = context.Class.Where(u => u.CourseNumber == "PHYS 1020").First();
                    context.Assignment.AddRange(
                        new Assignment
                        {
                            Title = "Syllabus: The Dynamics of Change: A Cross-Disciplinary Exploration",
                            Description = "Consider the concept of ‘change’ as a driving force in both natural and social phenomena. In a few sentences, explain how the idea of change is relevant to a subject you are currently studying, and provide an example of this change in action.",
                            DueDate = DateTime.Parse("05/12/2024 23:59:00"),
                            MaxPoints = 50,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        }
                        );
                    tClass = context.Class.Where(u => u.CourseNumber == "PHYS 3100").First();
                    context.Assignment.AddRange(
                        new Assignment
                        {
                            Title = "Syllabus: The Dynamics of Change: A Cross-Disciplinary Exploration",
                            Description = "Consider the concept of ‘change’ as a driving force in both natural and social phenomena. In a few sentences, explain how the idea of change is relevant to a subject you are currently studying, and provide an example of this change in action.",
                            DueDate = DateTime.Parse("05/12/2024 23:59:00"),
                            MaxPoints = 50,
                            SubmissionType = "Text",
                            ClassId = tClass.Id,
                            Classes = tClass,
                        }
                        );
                    context.SaveChanges();
                }
                #endregion

                #region Registration
                if(!context.Registration.Any())
                {
                    var tClasses = context.Class.ToList();
                    var tStudents = context.User.Where(u => u.Email.EndsWith("stellarlms.com") && u.Role == "Student").ToList();
                    foreach(var s in tStudents)
                    {
                        foreach (var c in tClasses)
                        {
                            context.Registration.Add(
                                new Registration
                                {
                                    Users = s,
                                    UserId = s.Id,
                                    Classes = c,
                                    ClassId = c.Id

                                }
                            );
                        }
                    }
                }
                context.SaveChanges();
                #endregion

                #region Submission
                if (!context.Submission.Any())
                {
                    Random rand = new Random();
                    var tStudents = context.User.Where(u => u.Email.EndsWith("stellarlms.com") && u.Role == "Student").ToList();
                    var tAssignment = context.Assignment.Where(a => a.Title == "Process Synchronization and Deadlock Handling").First();
                    foreach (var s in tStudents)
                    {
                        
                        var tClasses = context.Registration.Where(r => r.UserId == s.Id).ToList();
                        if (tClasses.Exists(c => c.ClassId == tAssignment.ClassId))
                        {
                            context.Submission.Add(
                                new Submission
                                {
                                    AssignmentId = tAssignment.Id,
                                    Assignment = tAssignment,
                                    StudentId = s.Id,
                                    Student = s,
                                    TurnInTime = DateTime.Parse("04/01/2024 23:56:00"),
                                    SubmissionText = "For the Process Simulation, I would create a multithreaded program where each thread represents a process. These threads would run concurrently, simulating parallel execution of processes.\r\n\r\nFor Shared Resource Access, I would design a critical section where the threads compete for access to a shared resource. I would use semaphores or mutexes to ensure mutual exclusion, preventing race conditions. This would involve locking the resource before a process can access it and unlocking it once the process is done.\r\n\r\nFor Deadlock Handling, I would introduce scenarios that could potentially lead to a deadlock, such as circular wait or a resource allocation graph. To handle this, I would implement deadlock detection mechanisms, such as the Banker’s algorithm, and recovery mechanisms, such as timeout-based termination or resource preemption.\r\n\r\nFor Testing and Documentation, I would thoroughly test the program with various input scenarios to ensure it works as expected. I would also provide clear documentation explaining my design choices, how I handled synchronization, and my deadlock prevention strategies.\r\n\r\nIn terms of Good Programming Practices, I would ensure proper error handling, efficient resource utilization, and clear code organization. For bonus points, I might explore creative deadlock prevention techniques, such as priority inheritance or the Ostrich algorithm.",
                                    Points = ((int)(rand.NextDouble() * 125)) + 125
                                }
                            );
                        }
                    }
                    tAssignment = context.Assignment.Where(a => a.Title == "Secure File Encryption System").First();
                    foreach (var s in tStudents)
                    {
                        var tClasses = context.Registration.Where(r => r.UserId == s.Id).ToList();
                        if (tClasses.Exists(c => c.ClassId == tAssignment.ClassId))
                        {
                            context.Submission.Add(
                                new Submission
                                {
                                    AssignmentId = tAssignment.Id,
                                    Assignment = tAssignment,
                                    StudentId = s.Id,
                                    Student = s,
                                    TurnInTime = DateTime.Parse("05/0" + ((int)(Double.Round(rand.NextDouble() * 3) + 6)).ToString() + "/2024 23:58:00"),
                                    SubmissionText = "For the Encryption Algorithm, I’ve chosen AES (Advanced Encryption Standard) because it’s widely accepted, secure, and efficient. It’s a symmetric encryption algorithm that provides strong security and is used in many security protocols worldwide.\r\n\r\nIn terms of Key Generation and Management, I’ve implemented a secure key generation process using a cryptographically secure pseudorandom number generator (CSPRNG). I’ve also explored various key storage options to ensure the keys are stored securely.\r\n\r\nFor File I/O, I’ve developed functions to read files, encrypt their contents using the chosen encryption algorithm and key, and then write the encrypted data to a new file. I’ve also implemented functions to read encrypted files and decrypt them back to their original form using the corresponding decryption key.\r\n\r\nI’ve made sure to handle exceptions gracefully for Error Handling, especially those related to file I/O and key management. This involves catching exceptions, providing meaningful error messages, and failing securely.\r\n\r\nFor Performance Analysis, I’ve evaluated the system’s performance in terms of encryption/decryption speed and memory usage. This involves measuring the time taken to encrypt/decrypt files of various sizes and monitoring the memory usage during these operations.\r\n\r\nIn terms of Documentation, I’ve provided clear instructions on how to use the system, including examples of encrypting and decrypting files. This will help users understand how to interact with the system effectively.\r\n\r\nFinally, for the Submission, I’m preparing to submit the code, a brief report explaining my design choices, and a demonstration video showcasing the system’s functionality. The report will detail why I chose the specific encryption algorithm, how I implemented key generation and management, how I handled file I/O and errors, and the results of the performance analysis.",
                                    Points = ((int)(rand.NextDouble()*60) + 90)
                                }
                            );
                        }
                    }
                    tAssignment = context.Assignment.Where(a => a.Title == "Distributed Chat Application").First();
                    foreach (var s in tStudents)
                    {
                        var tClasses = context.Registration.Where(r => r.UserId == s.Id).ToList();
                        if (tClasses.Exists(c => c.ClassId == tAssignment.ClassId))
                        {
                            context.Submission.Add(
                                new Submission
                                {
                                    AssignmentId = tAssignment.Id,
                                    Assignment = tAssignment,
                                    StudentId = s.Id,
                                    Student = s,
                                    TurnInTime = DateTime.Parse("05/" + ((int)(Double.Round(rand.NextDouble() * 3) + 13)).ToString() + "/2024 23:59:00"),
                                    SubmissionText = "For the Client-Server Architecture, I’ve designed a model where multiple clients can connect to a central server for message exchange. This design allows for efficient communication and easy management of connections.\r\n\r\nFor User Authentication, I’ve implemented secure protocols such as OAuth and JWT. These protocols ensure that only authenticated users can access the chat application, providing an additional layer of security.\r\n\r\nFor Message Encryption, I’ve used a hybrid encryption scheme. I’ve used RSA for key exchange, which is secure for transmitting encryption keys over a network, and AES for message encryption, which provides strong security and is efficient for encrypting the actual chat messages.\r\n\r\nIn terms of Key Management, I’ve explored various techniques for distributing and managing keys securely. This is crucial for ensuring secure communication in the chat application.\r\n\r\nFor Scalability, I’ve considered how the system will handle a large number of concurrent users. I’ve used techniques such as load balancing and efficient resource management to ensure the system remains responsive even under heavy load.\r\n\r\nFor Error Handling, I’ve implemented mechanisms to handle network failures, disconnections, and other exceptions gracefully. This ensures that the application remains robust and reliable.\r\n\r\nIn terms of Documentation, I’ve provided clear instructions on setting up and using the chat application. This will help users to easily understand how to use the application.\r\n\r\nFinally, for the Submission, I’m preparing to submit the code, a detailed design document explaining the architecture, and a demo video showcasing the chat application in action. The design document will detail the client-server architecture, user authentication mechanism, message encryption scheme, key management techniques, scalability considerations, and error handling mechanisms."
                                }
                            );
                        }
                    }
                    tAssignment = context.Assignment.Where(a => a.Title == "Process Synchronization and Deadlock Handling II").First();
                    foreach (var s in tStudents)
                    {

                        var tClasses = context.Registration.Where(r => r.UserId == s.Id).ToList();
                        if (tClasses.Exists(c => c.ClassId == tAssignment.ClassId))
                        {
                            context.Submission.Add(
                                new Submission
                                {
                                    AssignmentId = tAssignment.Id,
                                    Assignment = tAssignment,
                                    StudentId = s.Id,
                                    Student = s,
                                    TurnInTime = DateTime.Parse("04/01/2024 23:56:00"),
                                    SubmissionText = "For the Process Simulation, I would create a multithreaded program where each thread represents a process. These threads would run concurrently, simulating parallel execution of processes.\r\n\r\nFor Shared Resource Access, I would design a critical section where the threads compete for access to a shared resource. I would use semaphores or mutexes to ensure mutual exclusion, preventing race conditions. This would involve locking the resource before a process can access it and unlocking it once the process is done.\r\n\r\nFor Deadlock Handling, I would introduce scenarios that could potentially lead to a deadlock, such as circular wait or a resource allocation graph. To handle this, I would implement deadlock detection mechanisms, such as the Banker’s algorithm, and recovery mechanisms, such as timeout-based termination or resource preemption.\r\n\r\nFor Testing and Documentation, I would thoroughly test the program with various input scenarios to ensure it works as expected. I would also provide clear documentation explaining my design choices, how I handled synchronization, and my deadlock prevention strategies.\r\n\r\nIn terms of Good Programming Practices, I would ensure proper error handling, efficient resource utilization, and clear code organization. For bonus points, I might explore creative deadlock prevention techniques, such as priority inheritance or the Ostrich algorithm.",
                                    Points = ((int)(rand.NextDouble() * 125)) + 125
                                }
                            );
                        }
                    }
                    tAssignment = context.Assignment.Where(a => a.Title == "Secure File Encryption System II").First();
                    foreach (var s in tStudents)
                    {
                        var tClasses = context.Registration.Where(r => r.UserId == s.Id).ToList();
                        if (tClasses.Exists(c => c.ClassId == tAssignment.ClassId))
                        {
                            context.Submission.Add(
                                new Submission
                                {
                                    AssignmentId = tAssignment.Id,
                                    Assignment = tAssignment,
                                    StudentId = s.Id,
                                    Student = s,
                                    TurnInTime = DateTime.Parse("05/0" + ((int)(Double.Round(rand.NextDouble() * 3) + 6)).ToString() + "/2024 23:58:00"),
                                    SubmissionText = "For the Encryption Algorithm, I’ve chosen AES (Advanced Encryption Standard) because it’s widely accepted, secure, and efficient. It’s a symmetric encryption algorithm that provides strong security and is used in many security protocols worldwide.\r\n\r\nIn terms of Key Generation and Management, I’ve implemented a secure key generation process using a cryptographically secure pseudorandom number generator (CSPRNG). I’ve also explored various key storage options to ensure the keys are stored securely.\r\n\r\nFor File I/O, I’ve developed functions to read files, encrypt their contents using the chosen encryption algorithm and key, and then write the encrypted data to a new file. I’ve also implemented functions to read encrypted files and decrypt them back to their original form using the corresponding decryption key.\r\n\r\nI’ve made sure to handle exceptions gracefully for Error Handling, especially those related to file I/O and key management. This involves catching exceptions, providing meaningful error messages, and failing securely.\r\n\r\nFor Performance Analysis, I’ve evaluated the system’s performance in terms of encryption/decryption speed and memory usage. This involves measuring the time taken to encrypt/decrypt files of various sizes and monitoring the memory usage during these operations.\r\n\r\nIn terms of Documentation, I’ve provided clear instructions on how to use the system, including examples of encrypting and decrypting files. This will help users understand how to interact with the system effectively.\r\n\r\nFinally, for the Submission, I’m preparing to submit the code, a brief report explaining my design choices, and a demonstration video showcasing the system’s functionality. The report will detail why I chose the specific encryption algorithm, how I implemented key generation and management, how I handled file I/O and errors, and the results of the performance analysis.",
                                    Points = ((int)(rand.NextDouble() * 60) + 90)
                                }
                            );
                        }
                    }
                    tAssignment = context.Assignment.Where(a => a.Title == "Distributed Chat Application II").First();
                    foreach (var s in tStudents)
                    {
                        var tClasses = context.Registration.Where(r => r.UserId == s.Id).ToList();
                        if (tClasses.Exists(c => c.ClassId == tAssignment.ClassId))
                        {
                            context.Submission.Add(
                                new Submission
                                {
                                    AssignmentId = tAssignment.Id,
                                    Assignment = tAssignment,
                                    StudentId = s.Id,
                                    Student = s,
                                    TurnInTime = DateTime.Parse("05/" + ((int)(Double.Round(rand.NextDouble() * 3) + 13)).ToString() + "/2024 23:59:00"),
                                    SubmissionText = "For the Client-Server Architecture, I’ve designed a model where multiple clients can connect to a central server for message exchange. This design allows for efficient communication and easy management of connections.\r\n\r\nFor User Authentication, I’ve implemented secure protocols such as OAuth and JWT. These protocols ensure that only authenticated users can access the chat application, providing an additional layer of security.\r\n\r\nFor Message Encryption, I’ve used a hybrid encryption scheme. I’ve used RSA for key exchange, which is secure for transmitting encryption keys over a network, and AES for message encryption, which provides strong security and is efficient for encrypting the actual chat messages.\r\n\r\nIn terms of Key Management, I’ve explored various techniques for distributing and managing keys securely. This is crucial for ensuring secure communication in the chat application.\r\n\r\nFor Scalability, I’ve considered how the system will handle a large number of concurrent users. I’ve used techniques such as load balancing and efficient resource management to ensure the system remains responsive even under heavy load.\r\n\r\nFor Error Handling, I’ve implemented mechanisms to handle network failures, disconnections, and other exceptions gracefully. This ensures that the application remains robust and reliable.\r\n\r\nIn terms of Documentation, I’ve provided clear instructions on setting up and using the chat application. This will help users to easily understand how to use the application.\r\n\r\nFinally, for the Submission, I’m preparing to submit the code, a detailed design document explaining the architecture, and a demo video showcasing the chat application in action. The design document will detail the client-server architecture, user authentication mechanism, message encryption scheme, key management techniques, scalability considerations, and error handling mechanisms."
                                }
                            );
                        }
                    }
                    var tSyllabusAssignments = context.Assignment.Where(a => a.Title == "Syllabus: The Dynamics of Change: A Cross-Disciplinary Exploration").ToList();
                    foreach (var s in tStudents)
                    {
                        var tClasses = context.Registration.Where(r => r.UserId == s.Id).ToList();
                        foreach (var a in tSyllabusAssignments)
                        {
                            if (tClasses.Exists(c => c.ClassId == a.ClassId))
                            {
                                context.Submission.Add(
                                    new Submission
                                    {
                                        AssignmentId = a.Id,
                                        Assignment = a,
                                        StudentId = s.Id,
                                        Student = s,
                                        TurnInTime = DateTime.Parse("04/" + ((int)(Double.Round(rand.NextDouble() * 3) + 10)).ToString() + "/2024 23:59:00"),
                                        SubmissionText = "In the realm of computer science, change is a constant, driving innovation and technological advancement. A pertinent example is the evolution of programming paradigms—from procedural to object-oriented programming, and now to functional programming. This shift reflects a change in how developers approach problem-solving, aiming for more efficient, scalable, and maintainable code. For instance, the adoption of functional programming concepts in languages like JavaScript and Python has changed the landscape of web development, leading to more robust and concurrent applications.",
                                        Points = ((int)(rand.NextDouble() * 20) + 30)
                                    }
                                );
                            }
                        }
                    }
                }
                #endregion

                context.SaveChanges();
            }
        }
    }
}
