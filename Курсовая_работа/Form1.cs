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
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }
    public partial class Form1 : Form
    {
        DataBase database = new DataBase();

        int selectedRow;

        public Form1()
        {
            FormLogin Login = new FormLogin();
            Login.Show();
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("ID_Договора", "Договор");
            dataGridView1.Columns.Add("ФИО_клиента", "Клиент");
            dataGridView1.Columns.Add("ФИО_куратора", "Куратор");
            dataGridView1.Columns.Add("сумма_договора", "Сумма");
            dataGridView1.Columns.Add("дата_подписания", "Дата подписания");
            dataGridView1.Columns.Add("статус", "Статус");
            dataGridView1.Columns.Add("описание_требований", "Требования");
            dataGridView1.Columns.Add("статус_тз", "Статус задания");
            dataGridView1.Columns.Add("число_программистов", "Количество программистов");
            dataGridView1.Columns.Add("название", "Название проекта");
            dataGridView1.Columns.Add("IsNew", String.Empty);


        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetDecimal(3), record.GetDateTime(4), record.GetString(5), record.GetString(6), record.GetString(7), record.GetInt32(8), record.GetString(9), RowState.ModifiedNew);
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"SELECT  d.ID_Договора, c.ФИО AS ФИО_клиента, kr.ФИО AS ФИО_куратора, d.сумма_договора, d.дата_подписания, d.статус AS статус_договора, tz.описание_требований, tz.статус AS статус_тз, (SELECT COUNT(*)  FROM ТЗ_Программист tp  WHERE tp.ID_ТЗ = tz.ID_ТЗ) AS число_программистов,pp.название FROM Договор d JOIN ТЗ tz ON d.ID_Договора = tz.ID_Договора JOIN Отчётность o ON d.ID_Договора = o.ID_Договора JOIN Клиент c ON d.ID_Клиента = c.ID_Клиента JOIN Куратор kr ON d.ID_Куратора = kr.ID_Куратора LEFT JOIN Программный_продукт pp ON tz.ID_ТЗ = pp.ID_ТЗ; ";

            SqlCommand command = new SqlCommand(queryString, database.getConnection());

            database.OpenConnection();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
            CreateColumns2();
            RefreshDataGrid2(dataGridView2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            New newform = new New();
            newform.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[selectedRow];
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox6.Text = row.Cells[5].Value.ToString();
                textBox7.Text = row.Cells[6].Value.ToString();
                textBox8.Text = row.Cells[7].Value.ToString();
                textBox9.Text = row.Cells[8].Value.ToString();
                textBox10.Text = row.Cells[9].Value.ToString();
                textBox11.Text = row.Cells[10].Value.ToString();
                textBox12.Text = row.Cells[11].Value.ToString();
                textBox13.Text = row.Cells[12].Value.ToString();
                textBox14.Text = row.Cells[13].Value.ToString();
                textBox15.Text = row.Cells[14].Value.ToString();
                textBox16.Text = row.Cells[15].Value.ToString();
                textBox17.Text = row.Cells[16].Value.ToString();
                textBox18.Text = row.Cells[17].Value.ToString();
                textBox19.Text = row.Cells[18].Value.ToString();
                textBox20.Text = row.Cells[19].Value.ToString();
                textBox21.Text = row.Cells[20].Value.ToString();
                textBox22.Text = row.Cells[21].Value.ToString();
                textBox23.Text = row.Cells[22].Value.ToString();
                textBox24.Text = row.Cells[23].Value.ToString();
                textBox25.Text = row.Cells[24].Value.ToString();
                textBox26.Text = row.Cells[25].Value.ToString();
                textBox27.Text = row.Cells[26].Value.ToString();
                textBox28.Text = row.Cells[27].Value.ToString();

            }
        }
        private void CreateColumns2()
        {

            dataGridView2.Columns.Add("ID_Договора", "№ Договора");
            dataGridView2.Columns.Add("сумма_договора", "Сумма договора");
            dataGridView2.Columns.Add("дата_подписания", "Дата подписания");
            dataGridView2.Columns.Add("статус_договора", "Статус договора");
            dataGridView2.Columns.Add("ID_ТЗ", "№ ТЗ");
            dataGridView2.Columns.Add("дата_создания", "Дата создания ТЗ");
            dataGridView2.Columns.Add("описание_требований", "Требования");
            dataGridView2.Columns.Add("статус_тз", "Статус ТЗ");
            dataGridView2.Columns.Add("количество_программистов", "Программистов");
            dataGridView2.Columns.Add("ID_отчётности", "№ Отчётности");
            dataGridView2.Columns.Add("сумма_налогов", "Налоги");
            dataGridView2.Columns.Add("дата_подачи", "Дата подачи отчёта");
            dataGridView2.Columns.Add("период_отчётности", "Отчётный период");
            dataGridView2.Columns.Add("ID_Клиента", "№ Клиента");
            dataGridView2.Columns.Add("ФИО_клиента", "Клиент");
            dataGridView2.Columns.Add("Телефон_клиента", "Телефон клиента");
            dataGridView2.Columns.Add("email_клиента", "Email клиента");
            dataGridView2.Columns.Add("адрес", "Адрес клиента");
            dataGridView2.Columns.Add("ID_Куратора", "№ Куратора");
            dataGridView2.Columns.Add("ФИО_куратора", "Куратор");
            dataGridView2.Columns.Add("Должность", "Должность куратора");
            dataGridView2.Columns.Add("телефон_куратора", "Телефон куратора");
            dataGridView2.Columns.Add("email_куратора", "Email куратора");
            dataGridView2.Columns.Add("ID_Продукта", "№ Продукта");
            dataGridView2.Columns.Add("название", "Название продукта");
            dataGridView2.Columns.Add("дата_сдачи", "Дата сдачи");
            dataGridView2.Columns.Add("Версия", "Версия продукта");
            dataGridView2.Columns.Add("статус_продукта", "Статус продукта");
            dataGridView2.Columns.Add("IsNew", string.Empty);


            dataGridView2.Columns["сумма_договора"].DefaultCellStyle.Format = "N2";
            dataGridView2.Columns["сумма_налогов"].DefaultCellStyle.Format = "N2";
            dataGridView2.Columns["дата_подписания"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dataGridView2.Columns["дата_создания"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dataGridView2.Columns["дата_подачи"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dataGridView2.Columns["дата_сдачи"].DefaultCellStyle.Format = "dd.MM.yyyy";


        }
        private void ReadSingleRow2(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetDecimal(1), record.GetDateTime(2), record.GetString(3), record.GetInt32(4), record.GetDateTime(5), record.GetString(6), record.GetString(7), record.GetInt32(8), record.GetInt32(9), record.GetDecimal(10), record.GetDateTime(11), record.GetString(12), record.GetInt32(13), record.GetString(14), record.GetString(15), record.GetString(16), record.GetString(17), record.GetInt32(18), record.GetString(19), record.GetString(20), record.GetString(21), record.GetString(22), record.GetInt32(23), record.GetString(24), record.GetDateTime(25), record.GetString(26), record.GetString(27), RowState.ModifiedNew);
        }
        private void RefreshDataGrid2(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = 
            @"SELECT d.ID_Договора, d.сумма_договора, d.дата_подписания,
            d.статус AS статус_договора, tz.ID_ТЗ, tz.дата_создания,
            tz.описание_требований, tz.статус AS статус_тз, (SELECT COUNT(*)
            FROM ТЗ_Программист tp  WHERE tp.ID_ТЗ = tz.ID_ТЗ)
            AS количество_программистов, o.ID_отчётности,
            o.сумма_налогов, o.дата_подачи, o.период_отчётности,
            c.ID_Клиента, c.ФИО AS ФИО_клиента, c.Телефон AS Телефон_клиента,
            c.email AS email_клиента, c.адрес, kr.ID_Куратора, kr.ФИО AS ФИО_куратора,
            kr.Должность, kr.телефон AS телефон_куратора, kr.email AS email_куратора,
            pp.ID_Продукта, pp.название, pp.дата_сдачи, pp.Версия, pp.статус AS статус_продукта 
            FROM Договор d JOIN ТЗ tz ON d.ID_Договора = tz.ID_Договора JOIN Отчётность o ON d.ID_Договора = o.ID_Договора 
            JOIN Клиент c ON d.ID_Клиента = c.ID_Клиента JOIN Куратор kr ON d.ID_Куратора = kr.ID_Куратора 
            LEFT JOIN Программный_продукт pp ON tz.ID_ТЗ = pp.ID_ТЗ;";

            SqlCommand command = new SqlCommand(queryString, database.getConnection());

            database.OpenConnection();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow2(dgw, reader);
            }
            reader.Close();

        }

        private void AddCurator_Click(object sender, EventArgs e)
        {
            using (var form = new AddPersonForm(AddPersonForm.PersonType.Curator))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Можно обновить список кураторов в главной форме
                    MessageBox.Show("Новый куратор добавлен!");
                }
            }
        }

        private void AddProgrammer_Click(object sender, EventArgs e)
        {
            using (var form = new AddPersonForm(AddPersonForm.PersonType.Programmer))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Можно обновить список программистов в главной форме
                    MessageBox.Show("Новый программист добавлен!");
                }
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите договор для удаления!");
                return;
            }

            // Получаем ID выбранного договора
            int contractId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID_Договора"].Value);

            // Подтверждение удаления
            if (MessageBox.Show("Вы уверены, что хотите удалить этот договор и все связанные данные?",
                "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            try
            {
                // Создаем транзакцию для удаления всех связанных данных
                database.OpenConnection();
                using (SqlTransaction transaction = database.getConnection().BeginTransaction())
                {
                    try
                    {
                        // 1. Удаляем программные продукты (если есть)
                        string deleteProductQuery = @"
                    DELETE FROM Программный_продукт 
                    WHERE ID_ТЗ IN (SELECT ID_ТЗ FROM ТЗ WHERE ID_Договора = @ContractId)";
                        SqlCommand cmdProduct = new SqlCommand(deleteProductQuery, database.getConnection(), transaction);
                        cmdProduct.Parameters.AddWithValue("@ContractId", contractId);
                        cmdProduct.ExecuteNonQuery();

                        // 2. Удаляем связи программистов с ТЗ
                        string deleteProgrammerLinksQuery = @"
                    DELETE FROM ТЗ_Программист 
                    WHERE ID_ТЗ IN (SELECT ID_ТЗ FROM ТЗ WHERE ID_Договора = @ContractId)";
                        SqlCommand cmdLinks = new SqlCommand(deleteProgrammerLinksQuery, database.getConnection(), transaction);
                        cmdLinks.Parameters.AddWithValue("@ContractId", contractId);
                        cmdLinks.ExecuteNonQuery();

                        // 3. Удаляем ТЗ
                        string deleteSpecQuery = "DELETE FROM ТЗ WHERE ID_Договора = @ContractId";
                        SqlCommand cmdSpec = new SqlCommand(deleteSpecQuery, database.getConnection(), transaction);
                        cmdSpec.Parameters.AddWithValue("@ContractId", contractId);
                        cmdSpec.ExecuteNonQuery();

                        // 4. Удаляем отчетность
                        string deleteReportQuery = "DELETE FROM Отчётность WHERE ID_Договора = @ContractId";
                        SqlCommand cmdReport = new SqlCommand(deleteReportQuery, database.getConnection(), transaction);
                        cmdReport.Parameters.AddWithValue("@ContractId", contractId);
                        cmdReport.ExecuteNonQuery();

                        // 5. Удаляем сам договор
                        string deleteContractQuery = "DELETE FROM Договор WHERE ID_Договора = @ContractId";
                        SqlCommand cmdContract = new SqlCommand(deleteContractQuery, database.getConnection(), transaction);
                        cmdContract.Parameters.AddWithValue("@ContractId", contractId);
                        cmdContract.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show("Договор и все связанные данные успешно удалены!");

                        // Обновляем DataGridView
                        RefreshDataGrid(dataGridView1);
                        RefreshDataGrid2(dataGridView2);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}\nИзменения отменены.");
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

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = @"
        SELECT 
            d.ID_Договора, 
            c.ФИО AS ФИО_клиента, 
            kr.ФИО AS ФИО_куратора, 
            d.сумма_договора, 
            d.дата_подписания, 
            d.статус AS статус_договора, 
            tz.описание_требований, 
            tz.статус AS статус_тз, 
            (SELECT COUNT(*) FROM ТЗ_Программист tp WHERE tp.ID_ТЗ = tz.ID_ТЗ) AS число_программистов,
            pp.название 
        FROM 
            Договор d 
            JOIN ТЗ tz ON d.ID_Договора = tz.ID_Договора 
            JOIN Отчётность o ON d.ID_Договора = o.ID_Договора 
            JOIN Клиент c ON d.ID_Клиента = c.ID_Клиента 
            JOIN Куратор kr ON d.ID_Куратора = kr.ID_Куратора 
            LEFT JOIN Программный_продукт pp ON tz.ID_ТЗ = pp.ID_ТЗ
            WHERE
            CONCAT(
                CAST(d.ID_Договора AS NVARCHAR),
                COALESCE(c.ФИО, ''),
                COALESCE(kr.ФИО, ''),
                CAST(d.сумма_договора AS NVARCHAR),
                CONVERT(NVARCHAR, d.дата_подписания, 120),
                COALESCE(d.статус, ''),
                COALESCE(tz.описание_требований, ''),
                COALESCE(tz.статус, ''),
                COALESCE(pp.название, '')
            ) LIKE '%" + txt_search.Text + "%'";
            ;

            SqlCommand com = new SqlCommand(searchString, database.getConnection());
            database.OpenConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }
            read.Close();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void txt_edit_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите договор для редактирования!");
                return;
            }

            // Получаем ID выбранного договора
            int contractId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID_Договора"].Value);

            try
            {
                database.OpenConnection();
                using (SqlTransaction transaction = database.getConnection().BeginTransaction())
                {
                    try
                    {
                        // 1. Обновляем данные договора
                        UpdateContract(transaction, contractId);

                        // 2. Обновляем ТЗ
                        UpdateTechnicalSpec(transaction, contractId);

                        // 3. Обновляем отчётность
                        UpdateReport(transaction, contractId);

                        // 4. Обновляем программный продукт (если есть)
                        UpdateProduct(transaction, contractId);

                        transaction.Commit();
                        MessageBox.Show("Данные успешно обновлены!");

                        // Обновляем DataGridView
                        RefreshDataGrid(dataGridView1);
                        RefreshDataGrid2(dataGridView2);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка обновления: {ex.Message}\nИзменения отменены.");
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
        private void UpdateContract(SqlTransaction transaction, int contractId)
        {
            string query = @"
        UPDATE Договор 
        SET 
            сумма_договора = @amount,
            дата_подписания = @signDate,
            статус = @status
        WHERE ID_Договора = @contractId";

            SqlParameter[] parameters = {
        new SqlParameter("@amount", decimal.Parse(textBox2.Text)),
        new SqlParameter("@signDate", DateTime.Parse(textBox3.Text)),
        new SqlParameter("@status", textBox4.Text),
        new SqlParameter("@contractId", contractId)
    };

            database.ExecuteUpdateQuery(query, parameters, transaction);
        }

        private void UpdateTechnicalSpec(SqlTransaction transaction, int contractId)
        {
            string query = @"
        UPDATE ТЗ 
        SET 
            описание_требований = @description,
            статус = @status
        WHERE ID_Договора = @contractId";

            SqlParameter[] parameters = {
        new SqlParameter("@description", textBox7.Text),
        new SqlParameter("@status", textBox8.Text),
        new SqlParameter("@contractId", contractId)
    };

            database.ExecuteUpdateQuery(query, parameters, transaction);
        }

        private void UpdateReport(SqlTransaction transaction, int contractId)
        {
            string query = @"
        UPDATE Отчётность 
        SET 
            дата_подачи = @reportDate,
            период_отчётности = @period
        WHERE ID_Договора = @contractId";

            SqlParameter[] parameters = {
        new SqlParameter("@reportDate", DateTime.Parse(textBox12.Text)),
        new SqlParameter("@period", textBox13.Text),
        new SqlParameter("@contractId", contractId)
    };

            database.ExecuteUpdateQuery(query, parameters, transaction);
        }

        private void UpdateProduct(SqlTransaction transaction, int contractId)
        {
            // Проверяем, есть ли данные о продукте
            if (!string.IsNullOrEmpty(textBox24.Text))
            {
                string query = @"
            UPDATE Программный_продукт 
            SET 
                название = @name,
                дата_сдачи = @deliveryDate,
                Версия = @version,
                статус = @status
            WHERE ID_Продукта = @productId";

                SqlParameter[] parameters = {
            new SqlParameter("@name", textBox25.Text),
            new SqlParameter("@deliveryDate", DateTime.Parse(textBox26.Text)),
            new SqlParameter("@version", textBox27.Text),
            new SqlParameter("@status", textBox28.Text),
            new SqlParameter("@productId", int.Parse(textBox24.Text))
        };

                database.ExecuteUpdateQuery(query, parameters, transaction);
            }
        }
        private bool ValidateData()
        {
            // Проверка суммы договора
            if (!decimal.TryParse(textBox11.Text, out _))
            {
                MessageBox.Show("Введите корректную сумму договора (например: 15000.50)");
                textBox11.Focus();
                return false;
            }

            // Проверка даты подписания
            if (!DateTime.TryParse(textBox3.Text, out _))
            {
                MessageBox.Show("Введите корректную дату подписания (например: 2023-01-15)");
                textBox3.Focus();
                return false;
            }

            // Проверка ID продукта (если заполнено)
            if (!string.IsNullOrEmpty(textBox24.Text) && !int.TryParse(textBox24.Text, out _))
            {
                MessageBox.Show("Введите корректный ID продукта (целое число)");
                textBox24.Focus();
                return false;
            }

            return true;
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAllData();
                MessageBox.Show("Данные успешно обновлены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}");
            }
        }
        private void RefreshAllData()
        {
            // Очищаем текстовые поля
            ClearTextBoxes();

            // Обновляем обе таблицы
            RefreshDataGrid(dataGridView1);
            RefreshDataGrid2(dataGridView2);

            // Сбрасываем выделение
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
        }
        private void ClearTextBoxes()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
            }
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            FormRegister frm_sign = new FormRegister();
            frm_sign.Show();
            this.Close();
        }
    }




    }
