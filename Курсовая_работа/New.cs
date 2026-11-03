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
   
    public partial class New : Form
    {
        private DataBase database = new DataBase();
        private List<Person> _curators = new List<Person>();
        private List<Person> _programmers = new List<Person>();

        public New()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            LoadCurators();
            LoadProgrammers();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                database.OpenConnection();

                // Получаем выбранного куратора
                var selectedCurator = comboBox3.SelectedItem as Person;
                if (selectedCurator == null)
                {
                    MessageBox.Show("Выберите куратора!");
                    return;
                }

                // Получаем выбранных программистов
                var selectedProgrammers = listBoxProgrammers.SelectedItems.Cast<Person>().ToList();
                if (!selectedProgrammers.Any())
                {
                    MessageBox.Show("Выберите хотя бы одного программиста!");
                    return;
                }

                // Начинаем транзакцию
                using (var transaction = database.getConnection().BeginTransaction())
                {
                    try
                    {
                        // 1. Сохраняем клиента
                        int clientId = SaveClient(transaction, textBox15.Text, textBox16.Text,
                                               textBox17.Text, textBox18.Text);

                        // 2. Сохраняем договор
                        int contractId = SaveContract(transaction, clientId, selectedCurator.Id,
                            decimal.Parse(textBox2.Text), dateTimePicker1.Value,
                            comboBox1.GetItemText(comboBox1.SelectedItem));

                        // 3. Сохраняем отчетность
                        decimal contractAmount = decimal.Parse(textBox2.Text);
                        SaveReport(transaction, contractId, dateTimePicker3.Value, textBox13.Text, contractAmount);

                        // 4. Сохраняем ТЗ
                        int specId = SaveTechnicalSpec(transaction, contractId, selectedCurator.Id,
                            dateTimePicker2.Value, textBox7.Text,
                            comboBox2.GetItemText(comboBox2.SelectedItem));

                        // 5. Сохраняем связи с программистами
                        SaveProgrammerLinks(transaction, specId, selectedProgrammers);

                        // 6. Сохраняем программный продукт (без указания ID)
                        SaveProduct(transaction, specId, textBox25.Text,
                            dateTimePicker4.Value, textBox27.Text,
                            comboBox4.GetItemText(comboBox4.SelectedItem));



                        transaction.Commit();
                        MessageBox.Show("Все данные успешно сохранены!");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка сохранения: {ex.Message}\n\nИзменения не сохранены.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                database.closeConnection();
            }
        }

        private void LoadCurators()
        {
            try
            {
                database.OpenConnection();
                string query = "SELECT ID_Куратора, ФИО, Должность FROM Куратор";
                SqlCommand cmd = new SqlCommand(query, database.getConnection());

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _curators.Add(new Person
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Position = reader.IsDBNull(2) ? "" : reader.GetString(2)
                        });
                    }
                }

                comboBox3.DataSource = _curators;
                comboBox3.DisplayMember = "FullName";
                comboBox3.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки кураторов: {ex.Message}");
            }
            finally
            {
                database.closeConnection();
            }
        }

        private void LoadProgrammers()
        {
            try
            {
                database.OpenConnection();
                string query = "SELECT ID_программиста, ФИО, Должность FROM Программист";
                SqlCommand cmd = new SqlCommand(query, database.getConnection());

                _programmers.Clear();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _programmers.Add(new Person
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Position = reader.IsDBNull(2) ? "" : reader.GetString(2)
                        });
                    }
                }

                listBoxProgrammers.DataSource = _programmers;
                listBoxProgrammers.DisplayMember = "FullName";
                listBoxProgrammers.ValueMember = "Id";
                listBoxProgrammers.SelectionMode = SelectionMode.MultiExtended;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки программистов: {ex.Message}");
            }
            finally
            {
                database.closeConnection();
            }
        }

        // Методы сохранения отдельных сущностей:

        private int SaveClient(SqlTransaction transaction, string name, string phone, string email, string address)
        {
            string query = @"INSERT INTO Клиент (ФИО, Телефон, email, адрес) 
                    VALUES (@name, @phone, @email, @address);
                    SELECT SCOPE_IDENTITY();";

            using (var cmd = new SqlCommand(query, database.getConnection(), transaction))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@address", address);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int SaveContract(SqlTransaction transaction, int clientId, int curatorId,
            decimal amount, DateTime signDate, string status)
        {
            string query = @"INSERT INTO Договор (ID_Клиента, ID_Куратора, сумма_договора, дата_подписания, статус) 
                    VALUES (@clientId, @curatorId, @amount, @signDate, @status);
                    SELECT SCOPE_IDENTITY();";

            using (var cmd = new SqlCommand(query, database.getConnection(), transaction))
            {
                cmd.Parameters.AddWithValue("@clientId", clientId);
                cmd.Parameters.AddWithValue("@curatorId", curatorId);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@signDate", signDate);
                cmd.Parameters.AddWithValue("@status", status);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void SaveReport(SqlTransaction transaction, int contractId, DateTime reportDate, string period, decimal contractAmount)
        {
            // Вставляем ВСЕ обязательные поля, кроме вычисляемых
            string query = @"INSERT INTO Отчётность 
                    (ID_Договора, сумма_договора, дата_подачи, период_отчётности) 
                    VALUES (@contractId, @contractAmount, @reportDate, @period)";

            using (var cmd = new SqlCommand(query, database.getConnection(), transaction))
            {
                cmd.Parameters.AddWithValue("@contractId", contractId);
                cmd.Parameters.AddWithValue("@contractAmount", contractAmount); // Берём из параметра
                cmd.Parameters.AddWithValue("@reportDate", reportDate);
                cmd.Parameters.AddWithValue("@period", period);
                cmd.ExecuteNonQuery();
            }

            // сумма_налогов вычислится автоматически по формуле (сумма_договора * 0.20)
        }

        private int SaveTechnicalSpec(SqlTransaction transaction, int contractId, int curatorId,
            DateTime createDate, string description, string status)
        {
            string query = @"INSERT INTO ТЗ (ID_Договора, ID_Куратора, дата_создания, описание_требований, статус) 
                    VALUES (@contractId, @curatorId, @createDate, @description, @status);
                    SELECT SCOPE_IDENTITY();";

            using (var cmd = new SqlCommand(query, database.getConnection(), transaction))
            {
                cmd.Parameters.AddWithValue("@contractId", contractId);
                cmd.Parameters.AddWithValue("@curatorId", curatorId);
                cmd.Parameters.AddWithValue("@createDate", createDate);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@status", status);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void SaveProgrammerLinks(SqlTransaction transaction, int specId, List<Person> programmers)
        {
            string query = @"INSERT INTO ТЗ_Программист (ID_ТЗ, ID_программиста) 
                    VALUES (@specId, @programmerId)";

            foreach (var programmer in programmers)
            {
                using (var cmd = new SqlCommand(query, database.getConnection(), transaction))
                {
                    cmd.Parameters.AddWithValue("@specId", specId);
                    cmd.Parameters.AddWithValue("@programmerId", programmer.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void SaveProduct(SqlTransaction transaction, int specId,
            string name, DateTime deliveryDate, string version, string status)
        {
            string query = @"INSERT INTO Программный_продукт (ID_ТЗ, название, дата_сдачи, Версия, статус) 
                    VALUES (@specId, @name, @deliveryDate, @version, @status);
                    SELECT SCOPE_IDENTITY();";

            using (var cmd = new SqlCommand(query, database.getConnection(), transaction))
            {
                cmd.Parameters.AddWithValue("@specId", specId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                cmd.Parameters.AddWithValue("@version", version);
                cmd.Parameters.AddWithValue("@status", status);

                cmd.ExecuteNonQuery();
            }
        }

        public class Person
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string Position { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

            public override string ToString()
            {
                return $"{FullName} ({Position})";
            }
        }

    }

}
