using EasyWorkDefault.Classes;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;

namespace EasyWorkDefault.Data
{
    public class Database
    {
        public static void CreateDatabase()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            if (!File.Exists(dbPath))
            {
                using (var db = new SqliteConnection($"Filename={dbPath}"))
                {
                    db.Open();
                    var createTableCommand = new SqliteCommand();
                    createTableCommand.Connection = db;

                    createTableCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS category (
                    category_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    category_name TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS company_details (
                    company_details_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    company_name TEXT NOT NULL,
                    company_address TEXT NOT NULL,
                    company_location TEXT NOT NULL,
                    notification_of_work_id INTEGER NOT NULL,
                    FOREIGN KEY (notification_of_work_id) REFERENCES notification_of_work(notification_of_work_id)
                        ON UPDATE CASCADE
                        ON DELETE CASCADE
                );

                CREATE TABLE IF NOT EXISTS links (
                    links_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Github TEXT NOT NULL,
                    Microsoft TEXT NOT NULL,
                    NHentai TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS notification_of_work (
                    notification_of_work_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    notification_title TEXT NOT NULL,
                    notification_descript TEXT NOT NULL,
                    work_position TEXT NOT NULL,
                    job_level TEXT NOT NULL,
                    contract_type TEXT NOT NULL,
                    employment_dimensions TEXT NOT NULL,
                    salary_range_start DECIMAL(11,0) NOT NULL,
                    salary_range_end DECIMAL(11,0) NOT NULL,
                    working_days TEXT NOT NULL,
                    working_hours_start TIME NOT NULL,
                    working_hours_end TIME NOT NULL,
                    date_of_expiry_start DATE NOT NULL,
                    date_of_expiry_end DATE NOT NULL,
                    category_id INTEGER NOT NULL,
                    responsibilities TEXT NOT NULL,
                    candidate_requirements TEXT NOT NULL DEFAULT '',
                    employer_offers TEXT NOT NULL DEFAULT '',
                    about_the_company TEXT NOT NULL DEFAULT '',
                    user_id INTEGER NOT NULL,
                    FOREIGN KEY (category_id) REFERENCES category(category_id)
                        ON UPDATE CASCADE
                        ON DELETE CASCADE,
                    FOREIGN KEY (user_id) REFERENCES users(user_id)
                        ON UPDATE CASCADE
                        ON DELETE CASCADE
                );

                CREATE TABLE IF NOT EXISTS residence_place (
                    residence_place_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    place_name INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS users (
                    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    surname TEXT NOT NULL,
                    birth_date DATE,
                    email TEXT NOT NULL,
                    tel_number INTEGER,
                    prof_image BLOB NOT NULL DEFAULT '',
                    residence_place INTEGER,
                    curr_position TEXT,
                    curr_position_description TEXT,
                    career_summary TEXT,
                    work_experience TEXT,
                    education TEXT,
                    language_skills TEXT,
                    skills TEXT,
                    courses TEXT,
                    password_hash TEXT NOT NULL,
                    links INTEGER,
                    isAdmin INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (residence_place) REFERENCES residence_place(residence_place_id)
                        ON UPDATE CASCADE
                        ON DELETE SET NULL,
                    FOREIGN KEY (links) REFERENCES links(links_id)
                        ON UPDATE CASCADE
                        ON DELETE SET NULL
                );

                CREATE TABLE IF NOT EXISTS user_language_skills (
                    user_id INTEGER,
                    language_skill TEXT,
                    FOREIGN KEY (user_id) REFERENCES users(user_id)
                        ON UPDATE CASCADE
                        ON DELETE CASCADE
                );

                INSERT INTO users (name, surname, email, password_hash, isAdmin)
                VALUES ('admin', 'admin', 'admin@gmail.com', 'admin', 1);";

                    createTableCommand.ExecuteNonQuery();
                }
            }
        }

        public static User GetUserFromDatabase(string userEmail)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectUserCommand = new SqliteCommand();
                selectUserCommand.Connection = db;

                selectUserCommand.CommandText = @"
            SELECT user_id, name, surname, email, password_hash, isAdmin, prof_image
            FROM users
            WHERE email = @userEmail;";

                selectUserCommand.Parameters.AddWithValue("@userEmail", userEmail);

                using (var reader = selectUserCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32(reader.GetOrdinal("user_id"));
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        string surname = reader.GetString(reader.GetOrdinal("surname"));
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        string passwordHash = reader.GetString(reader.GetOrdinal("password_hash"));
                        bool isAdmin = reader.GetBoolean(reader.GetOrdinal("isAdmin"));

                        User retrievedUser = new User
                        {
                            ID = userId,
                            Name = name,
                            Surname = surname,
                            Email = email,
                            PasswordHash = passwordHash,
                            IsAdmin = isAdmin,
                        };

                        return retrievedUser;
                    }
                }
            }
            return null;
        }

        public static void SaveUserToDatabase(User newUser)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var insertUserCommand = new SqliteCommand();
                insertUserCommand.Connection = db;

                insertUserCommand.CommandText = @"
                    INSERT INTO users (name, surname, email, password_hash, isAdmin)
                    VALUES (@name, @surname, @email, @passwordHash, @isAdmin);";

                insertUserCommand.Parameters.AddWithValue("@name", newUser.Name);
                insertUserCommand.Parameters.AddWithValue("@surname", newUser.Surname);
                insertUserCommand.Parameters.AddWithValue("@email", newUser.Email);
                insertUserCommand.Parameters.AddWithValue("@passwordHash", newUser.PasswordHash);
                insertUserCommand.Parameters.AddWithValue("@isAdmin", newUser.IsAdmin);

                insertUserCommand.ExecuteNonQuery();
            }
        }

        public static void UpdateUserData(User updatedUser)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var updateUserCommand = new SqliteCommand();
                updateUserCommand.Connection = db;

                updateUserCommand.CommandText = @"
                            UPDATE users
                            SET name = @name,
                                surname = @surname,
                                birth_date = @birthDate,
                                tel_number = @telNumber,
                                residence_place = @residencePlace,
                                curr_position = @currPosition,
                                curr_position_description = @currPositionDescription,
                                career_summary = @careerSummary,
                                work_experience = @workExperience,
                                education = @education,
                                skills = @skills,
                                courses = @courses
                            WHERE email = @userEmail;";

                updateUserCommand.Parameters.AddWithValue("@name", updatedUser.Name);
                updateUserCommand.Parameters.AddWithValue("@surname", updatedUser.Surname);
                updateUserCommand.Parameters.AddWithValue("@birthDate", updatedUser.BirthDate);
                updateUserCommand.Parameters.AddWithValue("@telNumber", updatedUser.TelNumber);
                updateUserCommand.Parameters.AddWithValue("@residencePlace", updatedUser.ResidencePlace);
                updateUserCommand.Parameters.AddWithValue("@currPosition", updatedUser.CurrPosition);
                updateUserCommand.Parameters.AddWithValue("@currPositionDescription", updatedUser.CurrPositionDescription);
                updateUserCommand.Parameters.AddWithValue("@careerSummary", updatedUser.CareerSummary);
                updateUserCommand.Parameters.AddWithValue("@workExperience", updatedUser.WorkExperience);
                updateUserCommand.Parameters.AddWithValue("@education", updatedUser.Education);
                updateUserCommand.Parameters.AddWithValue("@skills", updatedUser.Skills);
                updateUserCommand.Parameters.AddWithValue("@courses", updatedUser.Courses);
                updateUserCommand.Parameters.AddWithValue("@userEmail", updatedUser.Email);

                updateUserCommand.ExecuteNonQuery();
            }

            UpdateUserLanguageSkills(updatedUser.Email, updatedUser.LanguageSkills);
        }

        public static void UpdateUserLanguageSkills(string userEmail, List<LanguageSkill> languageSkills)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var deleteLanguageSkillsCommand = new SqliteCommand();
                deleteLanguageSkillsCommand.Connection = db;
                deleteLanguageSkillsCommand.CommandText = @"
                    DELETE FROM user_language_skills
                    WHERE user_id = (SELECT user_id FROM users WHERE email = @userEmail);";
                deleteLanguageSkillsCommand.Parameters.AddWithValue("@userEmail", userEmail);
                deleteLanguageSkillsCommand.ExecuteNonQuery();


                var insertLanguageSkillsCommand = new SqliteCommand();
                insertLanguageSkillsCommand.Connection = db;
                insertLanguageSkillsCommand.CommandText = @"
                    INSERT INTO user_language_skills (user_id, language_skill)
                    VALUES ((SELECT user_id FROM users WHERE email = @userEmail), @languageSkill);";
                insertLanguageSkillsCommand.Parameters.AddWithValue("@userEmail", userEmail);

                foreach (var languageSkill in languageSkills)
                {
                    insertLanguageSkillsCommand.Parameters.AddWithValue("@languageSkill", languageSkill.ToString());
                    insertLanguageSkillsCommand.ExecuteNonQuery();
                    insertLanguageSkillsCommand.Parameters.RemoveAt("@languageSkill");
                }
            }
        }

        public static List<Category> GetCategoriesFromDatabase()
        {
            List<Category> categories = new List<Category>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCategoriesCommand = new SqliteCommand();
                selectCategoriesCommand.Connection = db;

                selectCategoriesCommand.CommandText = @"
                SELECT category_id, category_name
                FROM category;";

                using (var reader = selectCategoriesCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int categoryId = reader.GetInt32(reader.GetOrdinal("category_id"));
                        string categoryName = reader.GetString(reader.GetOrdinal("category_name"));

                        Category category = new Category
                        {
                            Category_Id = categoryId,
                            CategoryName = categoryName
                        };

                        categories.Add(category);
                    }
                }
            }

            return categories;
        }

        public static int AddCategoryToDatabase(string categoryName)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var insertCategoryCommand = new SqliteCommand();
                insertCategoryCommand.Connection = db;

                insertCategoryCommand.CommandText = @"
                INSERT INTO category (category_name)
                VALUES (@categoryName); SELECT last_insert_rowid();";

                insertCategoryCommand.Parameters.AddWithValue("@categoryName", categoryName);
                return Convert.ToInt32(insertCategoryCommand.ExecuteScalar());
            }
        }

        public static Category GetCategoryByName(string categoryName)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectCategoryCommand = new SqliteCommand();
                selectCategoryCommand.Connection = db;

                selectCategoryCommand.CommandText = @"
            SELECT category_id, category_name
            FROM category
            WHERE category_name = @categoryName;";

                selectCategoryCommand.Parameters.AddWithValue("@categoryName", categoryName);

                using (var reader = selectCategoryCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int categoryId = reader.GetInt32(reader.GetOrdinal("category_id"));
                        string categoryDbName = reader.GetString(reader.GetOrdinal("category_name"));

                        Category category = new Category
                        {
                            Category_Id = categoryId,
                            CategoryName = categoryDbName
                        };

                        return category;
                    }
                }
            }

            return null;
        }

        public static List<WorkPosition> GetWorkPositionsFromDatabase()
        {
            List<WorkPosition> workPositions = new List<WorkPosition>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectWorkPositionsCommand = new SqliteCommand();
                selectWorkPositionsCommand.Connection = db;

                selectWorkPositionsCommand.CommandText = @"
                    SELECT work_position_id, work_position_name
                    FROM work_position;";

                using (var reader = selectWorkPositionsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int workPositionId = reader.GetInt32(reader.GetOrdinal("work_position_id"));
                        string workPositionName = reader.GetString(reader.GetOrdinal("work_position_name"));

                        WorkPosition workPosition = new WorkPosition
                        {
                            WorkPosition_Id = workPositionId,
                            WorkPosition_Name = workPositionName
                        };

                        workPositions.Add(workPosition);
                    }
                }
            }

            return workPositions;
        }

        public static void AddAdvertToDatabase(NotificationoOfWork newAdvert, string selectedCategoryName)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                newAdvert.Category = AddCategoryToDatabase(selectedCategoryName);

                var insertAdvertCommand = new SqliteCommand();
                insertAdvertCommand.Connection = db;

                Console.WriteLine($"Próba wstawienia kategorii ID: {newAdvert.Category}");

                Console.WriteLine($"Próba wstawienia ID użytkownika: {newAdvert.User_Id}");

                insertAdvertCommand.CommandText = @"
            INSERT INTO notification_of_work (notification_title, notification_descript, work_position,
                job_level, contract_type, employment_dimensions, salary_range_start, salary_range_end,
                working_days, working_hours_start, working_hours_end, date_of_expiry_start, date_of_expiry_end,
                category_id, responsibilities, user_id)
            VALUES (@title, @description, @position, @jobLevel, @contractType, @dimensions,
                @salaryStart, @salaryEnd, @workDays, @workHoursStart, @workHoursEnd, @expiryDateStart, @expiryDateEnd,
                @categoryId, @responsibilities, @userId);";

                insertAdvertCommand.Parameters.AddWithValue("@title", newAdvert.Notification_title);
                insertAdvertCommand.Parameters.AddWithValue("@description", newAdvert.notification_descript);
                insertAdvertCommand.Parameters.AddWithValue("@position", newAdvert.Notification_work_position);
                insertAdvertCommand.Parameters.AddWithValue("@jobLevel", newAdvert.Job_level);
                insertAdvertCommand.Parameters.AddWithValue("@contractType", newAdvert.Contract_type);
                insertAdvertCommand.Parameters.AddWithValue("@dimensions", newAdvert.Employment_dimensions);
                insertAdvertCommand.Parameters.AddWithValue("@salaryStart", newAdvert.Salary_range_start);
                insertAdvertCommand.Parameters.AddWithValue("@salaryEnd", newAdvert.Salary_range_end);
                insertAdvertCommand.Parameters.AddWithValue("@workDays", newAdvert.Working_days);
                insertAdvertCommand.Parameters.AddWithValue("@workHoursStart", newAdvert.Working_hours_start.ToString(@"hh\:mm"));
                insertAdvertCommand.Parameters.AddWithValue("@workHoursEnd", newAdvert.Working_hours_end.ToString(@"hh\:mm"));
                insertAdvertCommand.Parameters.AddWithValue("@expiryDateStart", newAdvert.Date_of_expiry_start);
                insertAdvertCommand.Parameters.AddWithValue("@expiryDateEnd", newAdvert.Date_of_expiry_end);
                insertAdvertCommand.Parameters.AddWithValue("@categoryId", newAdvert.Category);
                insertAdvertCommand.Parameters.AddWithValue("@responsibilities", newAdvert.Responsibilities);
                insertAdvertCommand.Parameters.AddWithValue("@userId", newAdvert.User_Id);
                insertAdvertCommand.ExecuteNonQuery();
            }
        }

        public static List<NotificationoOfWork> GetAllAdvertisements()
        {
            List<NotificationoOfWork> advertisements = new List<NotificationoOfWork>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectAdvertisementsCommand = new SqliteCommand();
                selectAdvertisementsCommand.Connection = db;

                selectAdvertisementsCommand.CommandText = @"
                        SELECT *
                        FROM notification_of_work;";

                using (var reader = selectAdvertisementsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string title = reader.GetString(reader.GetOrdinal("notification_title"));
                        string description = reader.GetString(reader.GetOrdinal("notification_descript"));

                        NotificationoOfWork advertisement = new NotificationoOfWork
                        {
                            Notification_title = title,
                            notification_descript = description,
                        };

                        advertisements.Add(advertisement);
                    }
                }
            }

            return advertisements;
        }
    }
}
