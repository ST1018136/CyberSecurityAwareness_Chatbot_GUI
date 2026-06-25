using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CyberSecurityAwareness_Chatbot_GUI
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=localhost;Database=cyberbot_db;Uid=root;Pwd=Crescendo@05;";

        public DatabaseHelper()
        {
            CreateTablesIfNotExist();
        }

        private void CreateTablesIfNotExist()
        {
           
            // Using // inside a SQL string causes a syntax error at runtime
            string createTasksTable = @"
                CREATE TABLE IF NOT EXISTS tasks (
                    id INT PRIMARY KEY AUTO_INCREMENT,
                    title VARCHAR(200) NOT NULL,
                    description TEXT,
                    reminder_date DATETIME,
                    is_completed BOOLEAN DEFAULT FALSE,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                )";

            string createLogTable = @"
                CREATE TABLE IF NOT EXISTS activity_log (
                    id INT PRIMARY KEY AUTO_INCREMENT,
                    action VARCHAR(255) NOT NULL,
                    details TEXT,
                    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                )";

            ExecuteNonQuery(createTasksTable);
            ExecuteNonQuery(createLogTable);
        }

        // executes the non query sql command
        public void ExecuteNonQuery(string query)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // executes a query and returns the data reader
        public MySqlDataReader ExecuteQuery(string query)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            return cmd.ExecuteReader();
        }

        // inserts new tasks into the database
        public int InsertTask(string title, string description, DateTime? reminderDate)
        {
            string query = @"INSERT INTO tasks (title, description, reminder_date) 
                            VALUES (@title, @description, @reminderDate)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@reminderDate",
                        reminderDate.HasValue ? reminderDate.Value : (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                    return (int)cmd.LastInsertedId;
                }
            }
        }

        public List<Task> GetTasks()
        {
            List<Task> tasks = new List<Task>();
            string query = "SELECT * FROM tasks ORDER BY created_at DESC";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                            Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                ? "" : reader.GetString("description"),
                            ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date"))
                                ? (DateTime?)null : reader.GetDateTime("reminder_date"),
                            IsCompleted = reader.GetBoolean("is_completed"),
                            CreatedAt = reader.GetDateTime("created_at")
                        });
                    }
                }
            }
            return tasks;
        }

        // deletes a task
        public void DeleteTask(int id)
        {
            string query = $"DELETE FROM tasks WHERE id = {id}";
            ExecuteNonQuery(query);
        }

        public void CompleteTask(int id)
        {
            string query = $"UPDATE tasks SET is_completed = TRUE WHERE id = {id}";
            ExecuteNonQuery(query);
        }

        public void LogActivity(string action, string details)
        {
            // FIX: Use parameterized query to avoid SQL injection and apostrophe crashes
            string query = "INSERT INTO activity_log (action, details) VALUES (@action, @details)";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@details", details);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // gets the most recent avtivity logs
        public List<ActivityLog> GetActivityLog(int limit = 10)
        {
            List<ActivityLog> logs = new List<ActivityLog>();
            string query = $"SELECT * FROM activity_log ORDER BY timestamp DESC LIMIT {limit}";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logs.Add(new ActivityLog
                        {
                            Id = reader.GetInt32("id"),
                            Action = reader.GetString("action"),
                            Details = reader.IsDBNull(reader.GetOrdinal("details"))
                                ? "" : reader.GetString("details"),
                            Timestamp = reader.GetDateTime("timestamp")
                        });
                    }
                }
            }
            return logs;
        }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // represents the activity log entry
    public class ActivityLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}