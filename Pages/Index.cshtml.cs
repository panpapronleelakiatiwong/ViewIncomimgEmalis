using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Pages.Rangsit
{
    public class IndexModel : PageModel
    {
        public List<EmailInfo> Emails { get; set; } = new List<EmailInfo>();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            try
            {
                string connectionString = "ใส่แอดเดรสdatabase";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string sql = "SELECT [from], [date], [subject] FROM emails";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var email = new EmailInfo
                            {
                                From = reader.GetString(0),
                                Date = reader.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss"),
                                Subject = reader.GetString(2)
                            };
                            Emails.Add(email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"เกิดข้อผิดพลาดขณะดึงข้อมูลจากฐานข้อมูล: {ex.Message}";
            }
        }
    }

    public class EmailInfo
    {
        public string From { get; set; }
        public string Date { get; set; }
        public string Subject { get; set; }
    }
}
