using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using SWD_projekt.Core;

namespace SWD_projekt.Database
{
    public static class DBConnection
    {
        private const string DATABASE_FILE_EXTENSION = ".s3db";
        private const string DEFAULT_DATABASE = "database.s3db";
        private const string DATABASE_CREATION_FILE = "databaseCreationQuery.sql";
        private const string DATABASE_CONNECTION_STRING = "Data Source={0}; Foreign Keys=True";
        private static SQLiteConnection connection;
        private static string databaseFileName;

        public static void Init()
        {
            Init(DEFAULT_DATABASE);
        }

        public static void Init(string fileName)
        {
            if (null == connection)
            {
                if (!fileName.EndsWith(DATABASE_FILE_EXTENSION))
                    fileName += DATABASE_FILE_EXTENSION;
                databaseFileName = fileName;
                connection = new SQLiteConnection(String.Format(DATABASE_CONNECTION_STRING, databaseFileName));
            }
        }

        public static void OpenConnection()
        {
            connection.Open();
        }

        public static void CloseConnection()
        {
            connection.Close();
        }

        public static void CreateDatabase(bool deleteOld)
        {
            if (deleteOld)
                RemoveDatabase();
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    using (StreamReader reader = new StreamReader(DATABASE_CREATION_FILE))
                        command.CommandText = reader.ReadToEnd();
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        public static bool DatabaseExists()
        {
            return File.Exists(databaseFileName) && 0 != new FileInfo(databaseFileName).Length;
        }

        private static void RemoveDatabase()
        {
            if (File.Exists(databaseFileName) && 0 != new FileInfo(databaseFileName).Length)
                File.Delete(databaseFileName);
        }

        public static List<Cryteria> GetAllCryteria()
        {
            List<Cryteria> output = new List<Cryteria>();
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Cryteria";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            output.Add(new Cryteria(id, name));
                        }
                    }
                }
                transaction.Commit();
            }
            return output;
        }

        public static List<University> GetAllUniversities()
        {
            List<University> output = new List<University>();
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Universities";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            output.Add(new University(id, name));
                        }
                    }
                }
                transaction.Commit();
            }
            return output;
        }

        public static University GetUniversity(int id)
        {
            string universityName = "";
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Universities";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            universityName = reader.GetString(1);
                        }
                    }
                }
                transaction.Commit();
            }
            return new University(id, universityName);
        }

        public static string GetCryteriaValueForUniversity(int universityID, int cryteriaID)
        {
            string output = "";
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = String.Format("SELECT cryteriaValue FROM CryteriaValues WHERE universityID='{0}' AND cryteriaID='{1}'", universityID, cryteriaID);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            output = reader.GetString(0);
                        }
                    }
                }
                transaction.Commit();
            }
            return output;
        }
    }
}