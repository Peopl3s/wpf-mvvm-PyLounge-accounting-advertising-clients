using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.Common;

namespace WPFRJCode.Repositories
{
    /// <summary>
    /// Представляет базовый репозиторий для работы с БД
    /// </summary>
    public abstract class RepositoryBase
    {
        // Fields
        protected enum DataProvider{ OleDb, SqlServer, Odbc, None }
        private readonly string _connectionString;
        private readonly string _providerName;

        // Properties
        public string ProviderName => _providerName;
        public string ConnectionString => _connectionString;

        public RepositoryBase()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _providerName = ConfigurationManager.ConnectionStrings["DefaultConnection"].ProviderName;
        }

        /// <summary>
        /// Возвращает объект Соединения с БД DbConnection в зависимости от выбранного провайдера БД DataProvider
        /// В случае ошибки установки соединения возвращает null
        /// </summary>
        public DbConnection GetConnection()
        {
            DbConnection? connection = null;
            DataProvider dataProvider = GetProviderType(ProviderName);
            switch (dataProvider)
            {
                case DataProvider.SqlServer:
                    connection = new SqlConnection(_connectionString);
                    break;
                case DataProvider.OleDb:
                    connection = new OleDbConnection(_connectionString);
                    break;
                case DataProvider.Odbc:
                    connection = new OdbcConnection(_connectionString);
                    break;
            }
            return connection;
        }

        /// <summary>
        /// Возвращает тип провайдера БД по его названию providerName
        /// </summary>
        protected DataProvider GetProviderType(string providerName)
        {
            string dataProviderStnng = providerName;
            DataProvider dataProvider = DataProvider.None;
            if (Enum.IsDefined(typeof(DataProvider), dataProviderStnng))
            {
                dataProvider = (DataProvider)Enum.Parse(typeof(DataProvider), dataProviderStnng);
            }
            return dataProvider;
        }
    }
}
