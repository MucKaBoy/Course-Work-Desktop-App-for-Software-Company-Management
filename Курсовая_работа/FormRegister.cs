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
    public partial class FormRegister : Form
    {
        DataBase database = new DataBase();

        public FormRegister()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            var login = textBoxLogin.Text;
            var password = textBoxPassword.Text;
            var check = checkuser();
            if (check == false)
            {
                string querystring = $"insert into register(login_user, password_user) values('{login}','{password}')";

                SqlCommand command = new SqlCommand(querystring, database.getConnection());
                database.OpenConnection();
            
            if(command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormLogin frm_login = new FormLogin();
                this.Hide();
                frm_login.ShowDialog();
            }
            else
            {
                MessageBox.Show("Ошибка создания аккаунта", "Ошибка регистрации!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
            database.closeConnection();
            }
            
        }
        private Boolean checkuser()
        {
            var loginUser = textBoxLogin.Text;
            var passUser = textBoxPassword.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select * from register where login_user = '{loginUser}' and password_user = '{passUser}'";
            SqlCommand command = new SqlCommand(querystring, database.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такой аккаунт уже есть!", "Ошибка регистрации!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else return false;
        }
    }
}
