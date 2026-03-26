using System;
using System.Configuration;

namespace QUANLYKHO.Data
{
    public class Configuration
    {
        private static Configuration _instance;
        private string _connectionString;

        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Configuration();
                }
                return _instance;
            }
        }

        private Configuration()
        {
            LoadConnectionString();
        }
        private void LoadConnectionString()
        {
            try
            {
                _connectionString = ConfigurationManager.ConnectionStrings["QLKhoDb"]?.ConnectionString;
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new ConfigurationErrorsException("Không tìm thấy connection string tên 'QLKhoDb' trong App.config");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi đọc chuỗi kết nối: " + ex.Message, ex);
            }
        }

        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException("Chuỗi kết nối chưa được tải.");
            return _connectionString;
        }
    }
}