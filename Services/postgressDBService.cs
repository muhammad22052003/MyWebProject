﻿using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;
using System.Text;
using WebProject.interfaces;
using WebProject.Models;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using NpgsqlTypes;

namespace WebProject.Services
{
    public class postgressDBService : IDBService
    {
        private string _connectionString {  get; set; }
        private NpgsqlConnection _connection{  get; set; }

        private NpgsqlConnection GetConnection()
        {
            return _connection;
        }

        private async Task OpenConnection()
        {
            Console.WriteLine(_connection.ConnectionString);

            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                await _connection.OpenAsync();
            }
        }

        private async Task CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                await _connection.CloseAsync();
            }
        }

        public postgressDBService(string connectionString)
        {
            _connectionString = connectionString;

            _connection = new NpgsqlConnection(connectionString);
        }

        async public Task AddData(string tableName, IModel data)
        {
            StringBuilder addDataQuery = new StringBuilder($"INSERT INTO {tableName}(");

            var properties = data.GetType().GetProperties().ToList();
            for (int i = 0; i < properties.Count; i++)
            {
                addDataQuery.Append(properties[i].Name.ToLower());
                if(i != properties.Count - 1)
                {
                    addDataQuery.Append(",");
                }
            }
            addDataQuery.Append(") VALUES(");
            for (int i = 0; i < properties.Count; i++)
            {
                addDataQuery.Append($"@value{i}");
                if (i != properties.Count - 1)
                {
                    addDataQuery.Append(",");
                }
            }

            addDataQuery.Append(")");

            NpgsqlCommand command = new NpgsqlCommand(addDataQuery.ToString(), _connection);

            addDataQuery.Append(")");
            for (int i = 0; i < properties.Count; i++)
            {
                Console.WriteLine($"@value {i} : " + properties[i].GetValue(data));

                command.Parameters.Add($"@value{i}", ConvertTypeToPostgress(properties[i].PropertyType)).
                                                    Value = properties[i].GetValue(data);
            }

            await OpenConnection();

            await command.ExecuteNonQueryAsync();

            await CloseConnection();
        }

        async public Task DeleteData(string tableName, IModel user)
        {
            //  Основной строка запроса
            StringBuilder getDataQuery = new StringBuilder($"DELETE FROM {tableName} WHERE id = \'{user.Id}\'");
            //  Команда запроса
            NpgsqlCommand command = new NpgsqlCommand(getDataQuery.ToString(), GetConnection());

            await OpenConnection();

            await command.ExecuteNonQueryAsync();

            await CloseConnection();
        }

        async public Task EditData(IModel model, string tableName)
        {
            if (model == null || model.Id == null) { return; }

            //  Основной строка запроса
            StringBuilder getDataQuery = new StringBuilder($"SELECT * FROM {tableName} WHERE id = \'{model.Id}\'");
            //  Команда запроса
            NpgsqlCommand command = new NpgsqlCommand(getDataQuery.ToString(), GetConnection());
            //  Адаптер данных
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            //  Таблица данных из БД
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            if (dataTable.Rows.Count <= 0) { return; }

            getDataQuery = new StringBuilder($"UPDATE {tableName} SET ");

            List<PropertyInfo> properties = model.GetType().GetProperties().ToList();

            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].Name.ToLower() == "id") { properties.RemoveAt(i); }

                getDataQuery.Append($"{properties[i].Name} = @paramValue{i}");
                if (i + 1 != properties.Count) { getDataQuery.Append(","); }
            }

            getDataQuery.Append($" WHERE id = \'{model.Id}\'");

            command = new NpgsqlCommand(getDataQuery.ToString(), GetConnection());

            for (int i = 0; i < properties.Count; i++)
            {
                command.Parameters.Add($"@paramValue{i}", ConvertTypeToPostgress(properties[i].PropertyType)).Value = model.GetType().GetProperty(properties[i].Name).GetValue(model);
            }

            await OpenConnection();

            await command.ExecuteNonQueryAsync();

            await CloseConnection();
        }

        async public Task<List<TModel>> GetData<TModel>(string tableName, string condition = null) where TModel : IModel
        {
            //  Основной строка запроса
            StringBuilder getDataQuery = new StringBuilder($"SELECT * FROM {tableName} ");
            //  Команда запроса
            NpgsqlCommand command = new NpgsqlCommand();
            //  Адаптер данных
            NpgsqlDataAdapter adapter;
            //  Таблица данных из БД
            DataTable dataTable = new DataTable();

            Type type = typeof(TModel);

            if(condition != null)
            {
                getDataQuery.Append($"WHERE {condition}");
            }

            command = new NpgsqlCommand(getDataQuery.ToString(), GetConnection());

            adapter = new NpgsqlDataAdapter(command);

            Console.WriteLine(_connection.ConnectionString);

            adapter.Fill(dataTable);

            List<IModel> models = new List<IModel>();

            var properties = type.GetProperties();

            int i = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                models.Add(Activator.CreateInstance(type) as IModel);

                foreach (var property in properties)
                {
                    if (row[property.Name] == DBNull.Value)
                    {
                        continue;
                    }

                    var value = row[property.Name.ToLower()];

                    type.GetProperty(property.Name).SetValue(models[i], value);
                }

                i++;
            }

            await CloseConnection();

            return models.Cast<TModel>().ToList();
        }

        /// <summary>
        /// Конвертатция тип на mysqldbtype
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private NpgsqlDbType ConvertTypeToPostgress(Type type)
        {
            if (type == typeof(short)) { return NpgsqlDbType.Smallint; }
            else if (type == typeof(int)) { return NpgsqlDbType.Integer; }
            else if (type == typeof(long)) { return NpgsqlDbType.Bigint; }
            else if (type == typeof(double)) { return NpgsqlDbType.Double; }
            else if (type == typeof(string)) { return NpgsqlDbType.Varchar; }
            else if (type == typeof(DateTime)) { return NpgsqlDbType.Timestamp; }
            else if (type == typeof(bool)) { return NpgsqlDbType.Boolean; }

            return NpgsqlDbType.Varchar;
        }
    }
}