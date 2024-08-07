﻿using Npgsql;

namespace telegram_bot_suka_blay
{
    public class DataBase
    {
        static NpgsqlConnection con;
        NpgsqlCommand cmd;
        public DataBase()
        {
            con = new NpgsqlConnection(
                 connectionString: "Server=localhost;Port=5432;Username=postgres;Password=root;Database=users");
            con.Open();
            cmd = new NpgsqlCommand();
            cmd.Connection = con;
        }
        ~DataBase()
        {
            con.Close();
        }

        public async void InsertUser(User us)
        {
            var user = await GetUser(us.Id);
            if (user.Id == 0)
            {
                cmd.CommandText = $"INSERT INTO users (id, age, comradeid) VALUES ({us.Id}, {us.Age}, {us.ComradeId})";
                cmd.ExecuteNonQuery();
            }
            else
            {
                Console.WriteLine($"User {us.Id} already exist!");
            }
        }

        public async void DeleteUser(User us)
        {
            cmd.CommandText = $"DELETE FROM users WHERE id = {us.Id}";
            cmd.ExecuteNonQuery();
        }
        
        public async void UpdateUser(User us)
        {
            cmd.CommandText = $"UPDATE users SET age = {us.Age}, comradeid = {us.ComradeId} WHERE id = {us.Id}";
            cmd.ExecuteNonQuery();
        }
        

        public async Task<User> GetUser(long id)
        {
            User result;

            cmd.CommandText = $"SELECT * FROM users WHERE id='{id}'";
            NpgsqlDataReader reader = cmd.ExecuteReader();

            if (await reader.ReadAsync())
            {
                result = new User(Convert.ToInt64(reader["id"]), (int)reader["age"], 
                    Convert.ToInt64(reader["comradeid"]), (string)(reader["state"]));
            }
            else
            {
                result = new User();
            }
            reader.Close();
            return result;
        }
    };
}
