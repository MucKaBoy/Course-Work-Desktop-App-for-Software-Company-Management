using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Курсовая_работа
{
    public partial class AddPersonForm : Form
    {
        public enum PersonType { Programmer, Curator }
        private readonly PersonType _type;
        private readonly DataBase _database = new DataBase();

        public AddPersonForm(PersonType type)
        {
            InitializeComponent();
            _type = type;
            UpdateLabels();
            this.Text = _type == PersonType.Programmer ? "Добавить программиста" : "Добавить куратора";
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("ФИО обязательно для заполнения!");
                return;
            }

            _database.OpenConnection();

            try
            {
                string tableName = _type == PersonType.Programmer ? "Программист" : "Куратор";
                string query = $@"INSERT INTO {tableName} (ФИО, Должность, телефон, email) 
                                VALUES (@FullName, @Position, @Phone, @Email);
                                SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, _database.getConnection()))
                {
                    command.Parameters.AddWithValue("@FullName", txtFullName.Text);
                    command.Parameters.AddWithValue("@Position", string.IsNullOrEmpty(txtPosition.Text) ? (object)DBNull.Value : txtPosition.Text);
                    command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text);
                    command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text);

                    int newId = Convert.ToInt32(command.ExecuteScalar());
                    MessageBox.Show($"Данные сохранены успешно! ID: {newId}");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка базы данных: {ex.Message}\nНомер ошибки: {ex.Number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                _database.closeConnection();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО для поиска!");
                return;
            }

            // Определяем правильные имена столбцов
            string idColumnName = _type == PersonType.Curator ? "ID_Куратора" : "ID_программиста";
            string tableName = _type == PersonType.Curator ? "Куратор" : "Программист";

            // Запрос для поиска ID
            string query = $"SELECT {idColumnName} FROM {tableName} WHERE ФИО = @name";

            _database.OpenConnection();
            try
            {
                var cmd = new SqlCommand(query, _database.getConnection());
                cmd.Parameters.AddWithValue("@name", txtFullName.Text);

                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    MessageBox.Show("Сотрудник не найден!");
                    return;
                }

                if (MessageBox.Show("Удалить этого сотрудника?", "Подтверждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Запрос для удаления
                    query = $"DELETE FROM {tableName} WHERE {idColumnName} = @id";
                    cmd.CommandText = query;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", result);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Сотрудник успешно удален!");
                        // Очищаем поля после удаления
                        txtFullName.Text = "";
                        txtPosition.Text = "";
                        txtPhone.Text = "";
                        txtEmail.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить сотрудника!");
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Ошибка внешнего ключа
                {
                    MessageBox.Show("Нельзя удалить - есть связанные договоры/ТЗ!");
                }
                else
                {
                    MessageBox.Show($"Ошибка базы данных: {ex.Message}");
                }
            }
            finally
            {
                _database.closeConnection();
            }
        }
        private void UpdateLabels()
        {
            string personTypeText = _type == PersonType.Curator ? "куратора" : "программиста";

            // Обновляем все Label
            labelFullName.Text = $"ФИО {personTypeText}:";
            labelPosition.Text = $"Должность {personTypeText}:";
            labelPhone.Text = $"Телефон {personTypeText}:";
            labelEmail.Text = $"Email {personTypeText}:";

            // Также обновляем текст формы
            this.Text = $"Добавить {personTypeText}";
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public AddPersonForm.PersonType Type { get; set; }
    }

}
