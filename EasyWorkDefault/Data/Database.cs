using EasyWorkDefault.Classes;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWorkDefault.Data
{
    public class Database
    {
        public static void CreateDatabase()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "easywork.db");

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
                        notification_of_work INTEGER NOT NULL,
                        FOREIGN KEY (notification_of_work) REFERENCES notification_of_work(notification_of_work_id)
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
                        notification_work_position TEXT NOT NULL,
                        work_position INTEGER NOT NULL,
                        job_level TEXT NOT NULL,
                        contract_type TEXT NOT NULL,
                        employment_dimensions TEXT NOT NULL,
                        type_of_work INTEGER NOT NULL,
                        salary_range_start INTEGER NOT NULL,
                        salary_range_end INTEGER NOT NULL,
                        working_days TEXT NOT NULL,
                        working_hours_start TEXT NOT NULL,
                        working_hours_end TEXT NOT NULL,
                        date_of_expiry_start TEXT NOT NULL,
                        date_of_expiry_end TEXT NOT NULL,
                        category INTEGER NOT NULL,
                        responsibilities TEXT NOT NULL,
                        candidate_requirements TEXT NOT NULL,
                        employer_offers TEXT NOT NULL,
                        about_the_company TEXT NOT NULL,
                        FOREIGN KEY (work_position) REFERENCES work_position(work_position_id)
                            ON UPDATE CASCADE
                            ON DELETE CASCADE,
                        FOREIGN KEY (type_of_work) REFERENCES type_of_work(type_of_work_id)
                            ON UPDATE CASCADE
                            ON DELETE CASCADE,
                        FOREIGN KEY (category) REFERENCES category(category_id)
                            ON UPDATE CASCADE
                            ON DELETE CASCADE
                    );

                    CREATE TABLE IF NOT EXISTS residence_place (
                        residence_place_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        place_name INTEGER NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS type_of_work (
                        type_of_work_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        type_of_work_type TEXT NOT NULL
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
                            ON DELETE CASCADE,
                        FOREIGN KEY (links) REFERENCES links(links_id)
                            ON UPDATE CASCADE
                            ON DELETE CASCADE);

                    INSERT INTO users (name, surname, email, password_hash, isAdmin, links)
                    VALUES ('admin', 'admin','admin@gmail.com', 'admin', 1, NULL); ";

                    createTableCommand.ExecuteNonQuery();
                }
            }
        }

        public static void SaveUserToDatabase(User newUser)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "easywork.db");

            using (var db = new SqliteConnection($"Filename={dbPath}"))
            {
                db.Open();

                string defaultImagePath = Path.Combine("Images", "default_image.jpg");

                var insertUserCommand = new SqliteCommand();
                insertUserCommand.Connection = db;

                insertUserCommand.CommandText = @"
            INSERT INTO users (name, surname, email, password_hash, isAdmin, profile_image_path)
            VALUES (@name, @surname, @email, @passwordHash, @isAdmin, @profileImagePath);";

                insertUserCommand.Parameters.AddWithValue("@name", newUser.Name);
                insertUserCommand.Parameters.AddWithValue("@surname", newUser.Surname);
                insertUserCommand.Parameters.AddWithValue("@email", newUser.Email);
                insertUserCommand.Parameters.AddWithValue("@passwordHash", newUser.PasswordHash);
                insertUserCommand.Parameters.AddWithValue("@isAdmin", newUser.IsAdmin);
                insertUserCommand.Parameters.AddWithValue("@profileImagePath", defaultImagePath);

                insertUserCommand.ExecuteNonQuery();
            }
        }


        public static void GetUserFromDatabase()
        {

        }
    }
}
