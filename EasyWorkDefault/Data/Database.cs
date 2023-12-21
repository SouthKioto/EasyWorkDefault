using EasyWorkDefault.Classes;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

                            CREATE TABLE IF NOT EXISTS work_position (
                                work_position_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                work_position_name TEXT NOT NULL
                            );

                            INSERT INTO work_position (work_position_name) VALUES 
                                ('Praktykant'), ('Debiutant'), ('Adept'), ('Praktyk'), ('Ekspert');

                            CREATE TABLE IF NOT EXISTS job_level (
                                job_level_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                level_name TEXT NOT NULL
                            );

                            INSERT INTO job_level (level_name) VALUES 
                                ('praktykant/stażysta'), ('asystent'), ('młodszy specjalista (junior)'),
                                ('specjalista (mid)'), ('starszy specjalista (senior)'), ('ekspert'),
                                ('kierownik/koordynator'), ('menedżer'), ('dyrektor'), ('prezes');

                            CREATE TABLE IF NOT EXISTS contract_type (
                                contract_type_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                type_name TEXT NOT NULL
                            );
    
                            INSERT INTO contract_type (type_name) VALUES 
                                ('o pracę'), ('o dzieło'), ('zlecenie'), ('B2B'), ('zastępstwo'), ('staż/praktyka');

                            CREATE TABLE IF NOT EXISTS work_type (
                                work_type_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                type_name TEXT NOT NULL
                            );
                            
                            INSERT INTO work_type (type_name) VALUES 
                                ('stacjonarna'), ('hybrydowa'), ('zdalna');

                            CREATE TABLE IF NOT EXISTS employment_dimensions (
                                employment_dimensions_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                dimension_name TEXT NOT NULL
                            );

                            INSERT INTO employment_dimensions (dimension_name) VALUES 
                                ('część etatu'), ('cały etat'), ('tymczasowa');

                            CREATE TABLE IF NOT EXISTS notification_of_work (
                                notification_of_work_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                notification_title TEXT NOT NULL,
                                notification_descript TEXT NOT NULL,
                                work_position_id INTEGER NOT NULL,
                                job_level_id INTEGER NOT NULL,
                                contract_type_id INTEGER NOT NULL,
                                employment_dimensions_id INTEGER NOT NULL,
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
                                work_type_id INTEGER NOT NULL,
                                FOREIGN KEY (work_position_id) REFERENCES work_position(work_position_id)
                                    ON UPDATE CASCADE
                                    ON DELETE CASCADE,
                                FOREIGN KEY (job_level_id) REFERENCES job_level(job_level_id)
                                    ON UPDATE CASCADE
                                    ON DELETE CASCADE,
                                FOREIGN KEY (contract_type_id) REFERENCES contract_type(contract_type_id)
                                    ON UPDATE CASCADE
                                    ON DELETE CASCADE,
                                FOREIGN KEY (employment_dimensions_id) REFERENCES employment_dimensions(employment_dimensions_id)
                                    ON UPDATE CASCADE
                                    ON DELETE CASCADE,
                                FOREIGN KEY (category_id) REFERENCES category(category_id)
                                    ON UPDATE CASCADE
                                    ON DELETE CASCADE,
                                FOREIGN KEY (user_id) REFERENCES users(user_id)
                                    ON UPDATE CASCADE
                                    ON DELETE CASCADE,
                                FOREIGN KEY (work_type_id) REFERENCES work_type(work_type_id)
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
                                prof_image BLOB DEFAULT NULL,
                                residence_place TEXT DEFAULT NULL,
                                curr_position TEXT,
                                curr_position_description TEXT,
                                career_summary TEXT,
                                work_experience TEXT,
                                education TEXT,
                                language_skills TEXT,
                                skills TEXT,
                                courses TEXT,
                                password_hash TEXT NOT NULL,
                                links TEXT DEFAULT NULL,
                                isAdmin INTEGER NOT NULL DEFAULT 0
                            );

                            CREATE TABLE IF NOT EXISTS user_language_skills (
                                user_id INTEGER,
                                language_skill TEXT,
                                FOREIGN KEY (user_id) REFERENCES users(user_id)
                                    ON UPDATE CASCADE
                                    ON DELETE CASCADE
                            );

                            CREATE TABLE IF NOT EXISTS LikedAnnoucementUserAnnoucement (
                                user_id INTEGER,
                                notification_of_work_id INTEGER,
                                PRIMARY KEY (user_id, notification_of_work_id),
                                FOREIGN KEY (user_id) REFERENCES users(user_id)
                                    ON UPDATE CASCADE
                                    ON DELETE CASCADE,
                                FOREIGN KEY (notification_of_work_id) REFERENCES notification_of_work(notification_of_work_id)
                                    ON UPDATE CASCADE
                                    ON DELETE CASCADE
                            );

                            INSERT INTO users (name, surname, email, password_hash, isAdmin)
                            VALUES ('admin', 'admin', 'admin@gmail.com', 'admin', 1);";

                    createTableCommand.ExecuteNonQuery();
                }
            }
        }

        public static void AddToLike(int userId, int notificationOfWorkId)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var checkLikedCommand = new SqliteCommand($"SELECT COUNT(*) FROM LikedAnnoucementUserAnnoucement WHERE user_id = {userId} AND notification_of_work_id = {notificationOfWorkId}", db);
                int existingLikes = Convert.ToInt32(checkLikedCommand.ExecuteScalar());

                if (existingLikes == 0)
                {
                    var addToLikedCommand = new SqliteCommand($"INSERT INTO LikedAnnoucementUserAnnoucement (user_id, notification_of_work_id) VALUES ({userId}, {notificationOfWorkId})", db);
                    addToLikedCommand.ExecuteNonQuery();
                    MessageBox.Show("Ogłoszenie dodane do ulubionych.");
                }
                else
                {
                    MessageBox.Show("Ogłoszenie już jest w ulubionych.");
                }
            }
        }

        public static void RemoveFromLiked(int userId, int notificationOfWorkId)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var checkLikedCommand = new SqliteCommand($"SELECT COUNT(*) FROM LikedAnnoucementUserAnnoucement WHERE user_id = {userId} AND notification_of_work_id = {notificationOfWorkId}", db);
                int existingLikes = Convert.ToInt32(checkLikedCommand.ExecuteScalar());

                if (existingLikes > 0)
                {
                    var removeFromLikedCommand = new SqliteCommand($"DELETE FROM LikedAnnoucementUserAnnoucement WHERE user_id = {userId} AND notification_of_work_id = {notificationOfWorkId}", db);
                    removeFromLikedCommand.ExecuteNonQuery();
                    MessageBox.Show("Ogłoszenie usunięte z ulubionych.");
                }
                else
                {
                    MessageBox.Show("Ogłoszenie nie jest w ulubionych.");
                }
            }
        }

        public static List<LikedAnnouncement> GetLikedAnnouncementsForUser(int userId)
        {
            List<LikedAnnouncement> likedAnnouncements = new List<LikedAnnouncement>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectLikedAnnouncementsCommand = new SqliteCommand($@"
                    SELECT nw.notification_of_work_id, nw.notification_title, nw.date_of_expiry_end
                    FROM LikedAnnoucementUserAnnoucement lu
                    JOIN notification_of_work nw ON lu.notification_of_work_id = nw.notification_of_work_id
                    WHERE lu.user_id = {userId}", db);

                using (var reader = selectLikedAnnouncementsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int announcementId = reader.GetInt32(reader.GetOrdinal("notification_of_work_id"));
                        string announcementTitle = reader.GetString(reader.GetOrdinal("notification_title"));
                        DateTime announcementExpiry = reader.GetDateTime(reader.GetOrdinal("date_of_expiry_end"));

                        LikedAnnouncement likedAnnouncement = new LikedAnnouncement
                        {
                            NotificationOfWorkId = announcementId,
                            NotificationTitle = announcementTitle,
                            ExpiryDate = announcementExpiry,
                        };

                        likedAnnouncements.Add(likedAnnouncement);
                    }
                }
            }

            return likedAnnouncements;
        }


        public static AnnouncementDetails GetAnnouncementDetails(int notificationOfWorkId)
        {
            AnnouncementDetails announcementDetails = null;

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectAnnouncementCommand = new SqliteCommand();
                selectAnnouncementCommand.Connection = db;

                selectAnnouncementCommand.CommandText = @"
            SELECT *
            FROM notification_of_work
            WHERE notification_of_work_id = @notificationOfWorkId;";

                selectAnnouncementCommand.Parameters.AddWithValue("@notificationOfWorkId", notificationOfWorkId);

                using (var reader = selectAnnouncementCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Tworzymy obiekt AnnouncementDetails na podstawie danych z bazy danych
                        announcementDetails = new AnnouncementDetails
                        {
                            NotificationOfWorkId = reader.GetInt32(reader.GetOrdinal("notification_of_work_id")),
                            AnnouncementTitle = reader.GetString(reader.GetOrdinal("notification_title")),
                            ExpiryDate = reader.GetDateTime(reader.GetOrdinal("date_of_expiry_end")),
                            // Dodaj inne właściwości ogłoszenia, które chcesz pobrać
                            // ...
                        };
                    }
                }
            }

            return announcementDetails;
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

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectUsersCommand = new SqliteCommand();
                selectUsersCommand.Connection = db;

                selectUsersCommand.CommandText = @"
                        SELECT *
                        FROM users;";

                using (var reader = selectUsersCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int user_id = reader.GetInt32(reader.GetOrdinal("user_id"));
                        string user_name = reader.GetString(reader.GetOrdinal("name"));
                        string user_surname = reader.GetString(reader.GetOrdinal("surname"));


                        User user = new User
                        {
                            ID = user_id,
                            Name = user_name,
                            Surname = user_surname,
                        };

                        users.Add(user);
                    }
                }
            }

            return users;
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

        public static void AddAdvertToDatabase(NotificationoOfWork newAdvert, string selectedCategoryName, User currentUser)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                newAdvert.Category = GetOrCreateCategory(db, selectedCategoryName);
                newAdvert.User_Id = currentUser.ID;


                var insertAdvertCommand = new SqliteCommand();
                insertAdvertCommand.Connection = db;

                Console.WriteLine($"Próba wstawienia kategorii ID: {newAdvert.Category}");

                Console.WriteLine($"Próba wstawienia ID użytkownika: {newAdvert.User_Id}");

                insertAdvertCommand.CommandText = @"
                        INSERT INTO notification_of_work (
                            notification_title, notification_descript, work_position_id,
                            job_level_id, contract_type_id, employment_dimensions_id, 
                            salary_range_start, salary_range_end, working_days, 
                            working_hours_start, working_hours_end, date_of_expiry_start, 
                            date_of_expiry_end, category_id, responsibilities, 
                            user_id, work_type_id)
                        VALUES (
                            @title, @description, @position, @jobLevel, @contractType, @dimensions,
                            @salaryStart, @salaryEnd, @workDays, @workHoursStart, @workHoursEnd, 
                            @expiryDateStart, @expiryDateEnd, @categoryId, @responsibilities, 
                            @userId, @workTypeId);";

                insertAdvertCommand.Parameters.AddWithValue("@title", newAdvert.Notification_title);
                insertAdvertCommand.Parameters.AddWithValue("@description", newAdvert.notification_descript);
                insertAdvertCommand.Parameters.AddWithValue("@position", newAdvert.Notification_work_position);
                insertAdvertCommand.Parameters.AddWithValue("@jobLevel", newAdvert.Job_level);
                insertAdvertCommand.Parameters.AddWithValue("@workTypeId", newAdvert.WorkType);
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
                insertAdvertCommand.Parameters.AddWithValue("@userId", newAdvert.User_Id);
                insertAdvertCommand.Parameters.AddWithValue("@responsibilities", newAdvert.Responsibilities);

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
                SELECT n.*, 
                       wp.work_position_name AS work_position_name,
                       jl.level_name AS job_level_name,
                       ct.type_name AS contract_type_name,
                       ed.dimension_name AS employment_dimensions_name
                FROM notification_of_work n
                LEFT JOIN work_position wp ON n.work_position_id = wp.work_position_id
                LEFT JOIN job_level jl ON n.job_level_id = jl.job_level_id
                LEFT JOIN contract_type ct ON n.contract_type_id = ct.contract_type_id
                LEFT JOIN employment_dimensions ed ON n.employment_dimensions_id = ed.employment_dimensions_id;";

                using (var reader = selectAdvertisementsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int annouc_id = reader.GetInt32(reader.GetOrdinal("notification_of_work_id"));
                        string title = reader.GetString(reader.GetOrdinal("notification_title"));
                        string description = reader.GetString(reader.GetOrdinal("notification_descript"));
                        string work_posi_name = reader.GetString(reader.GetOrdinal("work_position_name"));
                        string job_lvl_name = reader.GetString(reader.GetOrdinal("job_level_name"));
                        string conctract_type_name = reader.GetString(reader.GetOrdinal("contract_type_name"));
                        string dimensions_name = reader.GetString(reader.GetOrdinal("employment_dimensions_name"));
                        decimal salary_start = reader.GetDecimal(reader.GetOrdinal("salary_range_start"));
                        decimal salary_end = reader.GetDecimal(reader.GetOrdinal("salary_range_end"));
                        string working_days = reader.GetString(reader.GetOrdinal("working_days"));
                        TimeSpan working_hours_start = reader.GetTimeSpan(reader.GetOrdinal("working_hours_start"));
                        TimeSpan working_hours_end = reader.GetTimeSpan(reader.GetOrdinal("working_hours_end"));
                        DateTime dateofexpiry_start = reader.GetDateTime(reader.GetOrdinal("date_of_expiry_start"));
                        DateTime dateofexpiry_end = reader.GetDateTime(reader.GetOrdinal("date_of_expiry_end"));
                        int user_id = reader.GetInt32(reader.GetOrdinal("user_id"));

                        NotificationoOfWork advertisement = new NotificationoOfWork
                        {
                            NotificationId = annouc_id,
                            Notification_title = title,
                            notification_descript = description,
                            Notification_work_position_name = work_posi_name,
                            Job_level_name = job_lvl_name,
                            Contract_type_name = conctract_type_name,
                            Employment_dimensions_name = dimensions_name,
                            Salary_range_start = salary_start,
                            Salary_range_end = salary_end,
                            Working_days = working_days,
                            Working_hours_start = working_hours_start,
                            Working_hours_end = working_hours_end,
                            Date_of_expiry_start = dateofexpiry_start,
                            Date_of_expiry_end = dateofexpiry_end,
                            User_Id = user_id,
                        };

                        advertisements.Add(advertisement);
                    }
                }
            }

            return advertisements;
        }

        public static List<JobLevel> GetJobLevelsFromDatabase()
        {
            List<JobLevel> jobLevels = new List<JobLevel>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectJobLevelsCommand = new SqliteCommand();
                selectJobLevelsCommand.Connection = db;

                selectJobLevelsCommand.CommandText = @"
            SELECT job_level_id, level_name
            FROM job_level;";

                using (var reader = selectJobLevelsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int jobLevelId = reader.GetInt32(reader.GetOrdinal("job_level_id"));
                        string levelName = reader.GetString(reader.GetOrdinal("level_name"));

                        JobLevel jobLevel = new JobLevel
                        {
                            JobLevel_Id = jobLevelId,
                            LevelName = levelName
                        };

                        jobLevels.Add(jobLevel);
                    }
                }
            }

            return jobLevels;
        }

        public static List<ContractType> GetContractTypesFromDatabase()
        {
            List<ContractType> contractTypes = new List<ContractType>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectContractTypesCommand = new SqliteCommand();
                selectContractTypesCommand.Connection = db;

                selectContractTypesCommand.CommandText = @"
            SELECT contract_type_id, type_name
            FROM contract_type;";

                using (var reader = selectContractTypesCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int contractTypeId = reader.GetInt32(reader.GetOrdinal("contract_type_id"));
                        string typeName = reader.GetString(reader.GetOrdinal("type_name"));

                        ContractType contractType = new ContractType
                        {
                            ContractType_Id = contractTypeId,
                            TypeName = typeName
                        };

                        contractTypes.Add(contractType);
                    }
                }
            }

            return contractTypes;
        }

        public static List<WorkType> GetWorkTypesFromDatabase()
        {
            List<WorkType> workTypes = new List<WorkType>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectWorkTypesCommand = new SqliteCommand();
                selectWorkTypesCommand.Connection = db;

                selectWorkTypesCommand.CommandText = @"
                                SELECT work_type_id, type_name
                                FROM work_type;";

                using (var reader = selectWorkTypesCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int workTypeId = reader.GetInt32(reader.GetOrdinal("work_type_id"));
                        string typeName = reader.GetString(reader.GetOrdinal("type_name"));

                        WorkType workType = new WorkType
                        {
                            WorkType_Id = workTypeId,
                            TypeName = typeName
                        };

                        workTypes.Add(workType);
                    }
                }
            }

            return workTypes;
        }

        public static List<EmploymentDimension> GetEmploymentDimensionsFromDatabase()
        {
            List<EmploymentDimension> employmentDimensions = new List<EmploymentDimension>();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var selectDimensionsCommand = new SqliteCommand();
                selectDimensionsCommand.Connection = db;

                selectDimensionsCommand.CommandText = @"
                        SELECT employment_dimensions_id, dimension_name
                        FROM employment_dimensions;";

                using (var reader = selectDimensionsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int dimensionId = reader.GetInt32(reader.GetOrdinal("employment_dimensions_id"));
                        string dimensionName = reader.GetString(reader.GetOrdinal("dimension_name"));

                        EmploymentDimension employmentDimension = new EmploymentDimension
                        {
                            EmploymentDimensions_Id = dimensionId,
                            DimensionName = dimensionName
                        };

                        employmentDimensions.Add(employmentDimension);
                    }
                }
            }

            return employmentDimensions;
        }

        private static int GetOrCreateCategory(SqliteConnection db, string categoryName)
        {
            var selectCategoryCommand = new SqliteCommand();
            selectCategoryCommand.Connection = db;
            selectCategoryCommand.CommandText = "SELECT category_id FROM category WHERE category_name = @categoryName";
            selectCategoryCommand.Parameters.AddWithValue("@categoryName", categoryName);

            var categoryId = selectCategoryCommand.ExecuteScalar();

            if (categoryId == null)
            {
                var insertCategoryCommand = new SqliteCommand();
                insertCategoryCommand.Connection = db;
                insertCategoryCommand.CommandText = "INSERT INTO category (category_name) VALUES (@categoryName); SELECT last_insert_rowid();";
                insertCategoryCommand.Parameters.AddWithValue("@categoryName", categoryName);

                categoryId = insertCategoryCommand.ExecuteScalar();
            }

            return Convert.ToInt32(categoryId);
        }

        public static void RemoveAnnoucementDataFromDatabase(int notificationId)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var deleteAnnoucementCommand = new SqliteCommand();
                deleteAnnoucementCommand.Connection = db;
                deleteAnnoucementCommand.CommandText = "DELETE FROM notification_of_work WHERE notification_of_work_id = @NotificationId";
                deleteAnnoucementCommand.Parameters.AddWithValue("@NotificationId", notificationId);
                deleteAnnoucementCommand.ExecuteNonQuery();
            }
        }

        public static void EditAdvertInDatabase(NotificationoOfWork modifiedAdvert)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazadanychpraca.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                var editAdvertCommand = new SqliteCommand();
                editAdvertCommand.Connection = db;

                editAdvertCommand.CommandText = @"
            UPDATE notification_of_work
            SET
                notification_title = @title,
                notification_descript = @description,
                work_position_id = @position,
                job_level_id = @jobLevel,
                contract_type_id = @contractType,
                employment_dimensions_id = @dimensions,
                salary_range_start = @salaryStart,
                salary_range_end = @salaryEnd,
                working_days = @workDays,
                working_hours_start = @workHoursStart,
                working_hours_end = @workHoursEnd,
                date_of_expiry_start = @expiryDateStart,
                date_of_expiry_end = @expiryDateEnd,
                category_id = @categoryId,
                work_type_id = @workTypeId
            WHERE notification_of_work_id = @advertId;";

                editAdvertCommand.Parameters.AddWithValue("@title", modifiedAdvert.Notification_title);
                editAdvertCommand.Parameters.AddWithValue("@description", modifiedAdvert.notification_descript);
                editAdvertCommand.Parameters.AddWithValue("@position", modifiedAdvert.Notification_work_position);
                editAdvertCommand.Parameters.AddWithValue("@jobLevel", modifiedAdvert.Job_level);
                editAdvertCommand.Parameters.AddWithValue("@contractType", modifiedAdvert.Contract_type);
                editAdvertCommand.Parameters.AddWithValue("@dimensions", modifiedAdvert.Employment_dimensions);
                editAdvertCommand.Parameters.AddWithValue("@salaryStart", modifiedAdvert.Salary_range_start);
                editAdvertCommand.Parameters.AddWithValue("@salaryEnd", modifiedAdvert.Salary_range_end);
                editAdvertCommand.Parameters.AddWithValue("@workDays", modifiedAdvert.Working_days);
                editAdvertCommand.Parameters.AddWithValue("@workHoursStart", modifiedAdvert.Working_hours_start.ToString(@"hh\:mm"));
                editAdvertCommand.Parameters.AddWithValue("@workHoursEnd", modifiedAdvert.Working_hours_end.ToString(@"hh\:mm"));
                editAdvertCommand.Parameters.AddWithValue("@expiryDateStart", modifiedAdvert.Date_of_expiry_start);
                editAdvertCommand.Parameters.AddWithValue("@expiryDateEnd", modifiedAdvert.Date_of_expiry_end);
                editAdvertCommand.Parameters.AddWithValue("@categoryId", modifiedAdvert.Category);
                editAdvertCommand.Parameters.AddWithValue("@workTypeId", modifiedAdvert.WorkType);
                editAdvertCommand.Parameters.AddWithValue("@advertId", modifiedAdvert.NotificationId);

                editAdvertCommand.ExecuteNonQuery();
            }
        }
    }
}
