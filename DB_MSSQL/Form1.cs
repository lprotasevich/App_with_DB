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

namespace DB_MSSQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void обПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Пример работы на WF C# и MS SQL (ADO.NET)", "Внимание!");
        }

        private void button_download_Click(object sender, EventArgs e)
        {
            // создаем соединение
            string conStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=TrainingJentyUsersDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();// открываем соединение (закрывать не нужно, т.к. используем using)

                // делаем запрос в БД, загружаем все данные таблицы UsersJenty в форму
                string query = "SELECT * FROM UsersJenty";
                SqlCommand cmd = new SqlCommand(query, connection);

                using (SqlDataReader reader = cmd.ExecuteReader())// считываем данные 
                {
                    // проверяем данные
                    if (reader.HasRows == false)
                    {
                        MessageBox.Show("Данные не найдены!", "Ошибка!");
                    }
                    else
                    {
                        MessageBox.Show("Данные успешно загружены!", "Ура!");
                        // записываем данные в форму
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader["id"], reader["Name_user"], reader["Surname_user"], reader["Tel"], reader["Department"], reader["Working_position"], reader["Work_experience"], reader["Registration_datetime"]);
                        }
                    }
                }
            }

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Выберите одну строку!", "Внимание!");
                return;
            }

            int index = dataGridView1.SelectedRows[0].Index;

            if (dataGridView1.Rows[index].Cells[1].Value == null || // Name_user
                dataGridView1.Rows[index].Cells[2].Value == null || // Surname_user
                dataGridView1.Rows[index].Cells[5].Value == null || // Working_position
                dataGridView1.Rows[index].Cells[6].Value == null)   // Work_experience
            {
                MessageBox.Show("Не все данные введены!\nОбязательные данные: Имя, Фамилия, Должность, Стаж.\nНеобязательные данные: Телефон, Отдел.\nПоля ID и Дата регистрации заполняются компьютером!", "Внимание!");
                return;
            }

            // считываем добавленные данные из формы
            string username = dataGridView1.Rows[index].Cells[1].Value.ToString();
            string usersurname = dataGridView1.Rows[index].Cells[2].Value.ToString();

            string usertel = "no";// необязательное поле (Телефон)
            string userdep = "no";// необязательное поле (Отдел)
            if (dataGridView1.Rows[index].Cells[3].Value != null)
                usertel = dataGridView1.Rows[index].Cells[3].Value.ToString();
            if (dataGridView1.Rows[index].Cells[4].Value != null)
                userdep = dataGridView1.Rows[index].Cells[4].Value.ToString();

            string userpos = dataGridView1.Rows[index].Cells[5].Value.ToString();
            string userexper = dataGridView1.Rows[index].Cells[6].Value.ToString();

            // создаем соединение
            string conStr = @"Data Source=PROTASEVICH\SQLEXPRESS;Initial Catalog=TrainingJentyUsersDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                // запрос к БД (добавление нового пользователя) 
                connection.Open();
                string query = "";
                if (usertel != "no" & userdep != "no")
                    query = "INSERT INTO UsersJenty (Name_user, Surname_user, Tel, Department, Working_position, Work_experience) VALUES ('" + username + "', '" + usersurname + "', " + usertel + ", '" + userdep + "', '" + userpos + "', " + userexper + ");";
                else if(usertel == "no" & userdep != "no")
                    query = "INSERT INTO UsersJenty (Name_user, Surname_user, Department, Working_position, Work_experience) VALUES ('" + username + "', '" + usersurname + "', '" + userdep + "', '" + userpos + "', " + userexper + ");";
                else if(usertel != "no" & userdep == "no")
                    query = "INSERT INTO UsersJenty (Name_user, Surname_user, Tel, Working_position, Work_experience) VALUES ('" + username + "', '" + usersurname + "', " + usertel + ", '" + userpos + "', " + userexper + ");";
                else if(usertel == "no" & userdep == "no")
                    query = "INSERT INTO UsersJenty (Name_user, Surname_user, Working_position, Work_experience) VALUES ('" + username + "', '" + usersurname + "', '" + userpos + "', " + userexper + ");";
                else
                {
                    MessageBox.Show("Нужно проверить поля Телефон и Отдел!", "Ошибка!");
                    connection.Close();// нужно ли?
                    return;
                }

                SqlCommand cmd = new SqlCommand(query, connection);

                // проверяем добавились ли данные в базу
                if (cmd.ExecuteNonQuery() != 1)
                    MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
                else
                    MessageBox.Show("Данные добавлены!", "Ура!");
            }
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Выберите одну строку!", "Внимание!");
                return;
            }

            int index = dataGridView1.SelectedRows[0].Index;

            if (dataGridView1.Rows[index].Cells[1].Value == null || // Name_user
                dataGridView1.Rows[index].Cells[2].Value == null || // Surname_user
                dataGridView1.Rows[index].Cells[5].Value == null || // Working_position
                dataGridView1.Rows[index].Cells[6].Value == null)   // Work_experience
            {
                MessageBox.Show("Не все данные введены!\nОбязательные данные: Имя, Фамилия, Должность, Стаж.\nНеобязательные данные: Телефон, Отдел.\nПоля ID и Дата регистрации заполняются компьютером!", "Внимание!");
                return;
            }

            // считываем добавленные данные из формы
            string userid = dataGridView1.Rows[index].Cells[0].Value.ToString();
            string username = dataGridView1.Rows[index].Cells[1].Value.ToString();
            string usersurname = dataGridView1.Rows[index].Cells[2].Value.ToString();

            string usertel = "no";// необязательное поле (Телефон)
            string userdep = "no";// необязательное поле (Отдел)
            if (dataGridView1.Rows[index].Cells[3].Value != null)
                usertel = dataGridView1.Rows[index].Cells[3].Value.ToString();
            if (dataGridView1.Rows[index].Cells[4].Value != null)
                userdep = dataGridView1.Rows[index].Cells[4].Value.ToString();

            string userpos = dataGridView1.Rows[index].Cells[5].Value.ToString();
            string userexper = dataGridView1.Rows[index].Cells[6].Value.ToString();

            // создаем соединение
            string conStr = @"Data Source=PROTASEVICH\SQLEXPRESS;Initial Catalog=TrainingJentyUsersDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                // запрос к БД (добавление нового пользователя) 
                connection.Open();
                string query = "";
                if (usertel != "no" & userdep != "no")
                    query = "UPDATE UsersJenty SET Name_user = '" + username + "', Surname_user = '" + usersurname + "', Tel = " + usertel + ", Department = '" + userdep + "', Working_position = '" + userpos + "', Work_experience = " + userexper + " WHERE id = " + userid;
                else if (usertel == "no" & userdep != "no")
                    query = "UPDATE UsersJenty SET Name_user = '" + username + "', Surname_user = '" + usersurname + "', Department = '" + userdep + "', Working_position = '" + userpos + "', Work_experience = " + userexper + " WHERE id = " + userid;
                else if (usertel != "no" & userdep == "no")
                    query = "UPDATE UsersJenty SET Name_user = '" + username + "', Surname_user = '" + usersurname + "', Tel = " + usertel + ", Working_position = '" + userpos + "', Work_experience = " + userexper + " WHERE id = " + userid;
                else if (usertel == "no" & userdep == "no")
                    query = "UPDATE UsersJenty SET Name_user = '" + username + "', Surname_user = '" + usersurname + "', Working_position = '" + userpos + "', Work_experience = " + userexper + " WHERE id = " + userid;
                else
                {
                    MessageBox.Show("Нужно проверить поля Телефон и Отдел!", "Ошибка!");
                    connection.Close();// нужно ли?
                    return;
                }

                SqlCommand cmd = new SqlCommand(query, connection);

                // проверяем добавились ли данные в базу
                if (cmd.ExecuteNonQuery() != 1)
                    MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
                else
                    MessageBox.Show("Данные обновлены!", "Ура!");
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Выберите одну строку!", "Внимание!");
                return;
            }

            int index = dataGridView1.SelectedRows[0].Index;

            if (dataGridView1.Rows[index].Cells[0].Value == null) //проверяем есть ли строка по id
            {
                MessageBox.Show("Такой строки не существует!", "Внимание!");
                return;
            }

            // считываем id строки которую хотим удалить
            string userid = dataGridView1.Rows[index].Cells[0].Value.ToString();

            // создаем соединение
            string conStr = @"Data Source=PROTASEVICH\SQLEXPRESS;Initial Catalog=TrainingJentyUsersDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                // запрос к БД (добавление нового пользователя) 
                connection.Open();
                string query = "DELETE FROM UsersJenty WHERE id = " + userid;

                SqlCommand cmd = new SqlCommand(query, connection);

                // проверяем добавились ли данные в базу
                if (cmd.ExecuteNonQuery() != 1)
                    MessageBox.Show("Ошибка выполнения запроса!", "Ошибка!");
                else
                {
                    MessageBox.Show("Данные удалены!", "Ура!");
                    dataGridView1.Rows.RemoveAt(index);
                }
                    
            }
        }

        private void button_clearscreen_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
