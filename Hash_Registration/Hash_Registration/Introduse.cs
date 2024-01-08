using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BCrypt.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static Hash_Registration.Introduse;

namespace Hash_Registration
{
    public partial class Introduse : Form
    {
        string globalSalt;
        bool ShowPassword = false;
        bool Registration = false;
        public class DataModel
        {
            public List<USER> Users { get; set; }
        }
        public class USER
        {
            public int _ID { get; set; }
            public string _LOGIN { get; set; }
            public string _PASSWORD { get; set; }
            public char _ACCESS { get; set; }
            public string _DATE { get; set; }
        }
        // Создание глобальной соли при запуске формы
        private void InitializeSaltManager()
        {
            try
            {
                DateTime today = DateTime.Now.Date;
                string currentDate = today.ToString("MM.yyyy");
                // Создаем экземпляр SaltManager и получаем соль для текущего месяца
                globalSalt = SaltManager.GetMonthlySalt(currentDate);
            }
            catch (Exception ex)
            {
                // Обработка ошибок при получении соли
                Console.WriteLine($"Error initializing SaltManager: {ex.Message}");
            }
        }
        //Стэк функций для солей 
        public class SaltManager
    {
        // словарь файла 
        private static Dictionary<string, List<string>> monthlySalts = LoadMonthlySaltsFromJson();

        //Создание словаря файла 
        private static Dictionary<string, List<string>> LoadMonthlySaltsFromJson()
        {
            try
            {
                string jsonFilePath = "monthlysalts.json";

                if (File.Exists(jsonFilePath))
                {
                    string jsonData = File.ReadAllText(jsonFilePath);
                    return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading monthly salts from JSON: {ex.Message}");
            }

            return new Dictionary<string, List<string>>();
        }


        public static string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
            //получение соли 
            public static string GetMonthlySalt(string currentDate)
            {
                if (monthlySalts.ContainsKey(currentDate))
                {
                    if (IsMonthlySaltInJson(currentDate))
                    {
                        return GetSaltFromJsonForMonth(currentDate);
                    }
                    else
                    {
                        // В базе есть запись, но нет в JSON, добавим в JSON
                        SaveMonthlySaltToDatabase(currentDate, GenerateNewMonthlySalt());
                        return GetSaltFromJsonForMonth(currentDate);
                    }
                }
                else
                {
                    // В базе нет записи, создадим новую и добавим в JSON
                    SaveMonthlySaltToDatabase(currentDate, GenerateNewMonthlySalt());
                    return GetSaltFromJsonForMonth(currentDate);
                }
            }
            //генерация новой соли 
            private static string GenerateNewMonthlySalt()
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                return salt.ToString();
            }
            //проверка наличия соли на этот месяц 
            private static bool IsMonthlySaltInJson(string month)
            {
                return monthlySalts.ContainsKey(month);
            }
            //получение соли из словаря 
            private static string GetSaltFromJsonForMonth(string month)
            {
                try
                {
                    return monthlySalts[month][0]; // Assuming there is only one salt per month
                }
                catch (Exception ex)
                {
                    throw new Exception($"Salt not found for date {month}: {ex.Message}");
                }
            }
            // Схранение новой соли в базу 
            private static void SaveMonthlySaltToDatabase(string month, string saltValue)
            {
                try
                {
                    if (monthlySalts.ContainsKey(month))
                    {
                        // Если есть, добавляем новую соль к существующим
                        monthlySalts[month].Add(saltValue);
                    }
                    else
                    {
                        // Если нет, создаем новую запись для месяца
                        monthlySalts[month] = new List<string> { saltValue };
                    }
                    string jsonFilePath = "monthlysalts.json";
                    string jsonData = JsonSerializer.Serialize(monthlySalts);
                    File.WriteAllText(jsonFilePath, jsonData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving monthly salts to JSON: {ex.Message}");
                }
            }

    }


    //КЛАССЫ БАЗЫ ДЛЯ ХЭША
    public class DataModel_Hash
        {
            public List<USER_Hash> Hashes { get; set; }
        }
        public class USER_Hash
        {
            public DateTime Month { get; set; }
            public string SaltValue { get; set; }
        }
        public Introduse()
        {
            InitializeComponent();
            InitializeSaltManager();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (ShowPassword == true)
            {
                textBox2.PasswordChar = '*';
                ShowPassword = false;
            }
            else { ShowPassword = true; textBox2.PasswordChar = '\0'; }
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            if (ShowPassword == true)
            {
                label5.ForeColor = Color.Blue;

            }
            else
            {
                label5.ForeColor = Color.Gray;
            }
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            if (ShowPassword == true)
            {
                label5.ForeColor = Color.Red;

            }
            else
            {
                label5.ForeColor = Color.Black;
            }
        }
        private void label5_MouseDown(object sender, MouseEventArgs e)
        {
            label5.ForeColor = Color.Black;
        }

        private void label5_MouseUp(object sender, MouseEventArgs e)
        {
            if (ShowPassword == true)
            {
                label5.ForeColor = Color.Blue;
            }
            else
            {
                label5.ForeColor = Color.Black;
            }
        }

        async private void label4_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            int R = 0;
            int G = 0;
            int B = 0;
            int R_panel1 = 0;
            int G_panel1 = 0;
            int B_panel1 = 0;

            R = this.BackColor.R;
            G = this.BackColor.G;
            B = this.BackColor.B;
            R_panel1 = panel1.BackColor.R;
            G_panel1 = panel1.BackColor.G;
            B_panel1 = panel1.BackColor.B;
            if (Registration == false) {
                for (int i = 0; i < 25; i++, B+=2, B_panel1 += 2, await Task.Delay(1))
                {
                    if (R > 255) { R = 0; } else if (R < 0) { R = 255; }
                    if (G > 255) { G = 0; } else if (G < 0) { G = 255; }
                    if (B > 255) { B = 0; } else if (B < 0) { B = 255; }
                    panel1.BackColor = Color.FromArgb(R_panel1, G_panel1, B_panel1);
                    this.BackColor = Color.FromArgb(R, G, B);
                }
                Registration = true;
            }
            button3.Visible = true;
        }
        async private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            int R = 0;
            int G = 0;
            int B = 0;
            int R_panel1 = 0;
            int G_panel1 = 0;
            int B_panel1 = 0;

            R = this.BackColor.R;
            G = this.BackColor.G;
            B = this.BackColor.B;
            R_panel1 = panel1.BackColor.R;
            G_panel1 = panel1.BackColor.G;
            B_panel1 = panel1.BackColor.B;

            if (Registration == true)
            {
                for (int i = 0; i < 25; i++, B-=2, B_panel1-=2, await Task.Delay(1))
                {
                    if (R > 255) { R = 0; } else if (R < 0) { R = 255; }
                    if (G > 255) { G = 0; } else if (G < 0) { G = 255; }
                    if (B > 255) { B = 0; } else if (B < 0) { B = 255; }
                    panel1.BackColor = Color.FromArgb(R_panel1, G_panel1, B_panel1);
                    this.BackColor = Color.FromArgb(R, G, B);
                }
                Registration = false;
            }
            label4.Visible = true;
            //MessageBox.Show(Convert.ToString(B));
        }


        //КНОПКА ДЛЯ ВЗАИМОДЕЙСТВИЯ С БАЗОЙ
        private void button1_Click(object sender, EventArgs e)
        {   //имя файла
            string Users_base = "tbiUser.json";
            //чтение файла
            string jsonString_r = File.ReadAllText(Users_base);



            //проверка на состояние формы (регистрация/авторизация)
            if (Registration == false)
            {
                //структура десериализации из tbiUser
                var Db_write = JsonSerializer.Deserialize<DataModel>(jsonString_r);
                // проверка полей на содержание
                if (textBox1.Text != "" & textBox2.Text != "")
                {
                    var TextBase = GetUserByLogin(Db_write.Users, textBox1.Text);
                    if (TextBase != null)
                    {
                        // проверка на подлинность логина
                        if (textBox1.Text == GetUserByLogin(Db_write.Users, textBox1.Text)._LOGIN)
                        {
                            string hash = SaltManager.HashPassword(textBox2.Text, SaltManager.GetMonthlySalt(GetUserDate(Db_write.Users, textBox1.Text)));

                            USER userByPassword = GetUserByPassword(Db_write.Users, hash);

                            if (userByPassword != null)
                            {
                                // Обновление существующего пользователя
                                userByPassword._PASSWORD = SaltManager.HashPassword(textBox2.Text, globalSalt);
                                userByPassword._DATE = DateTime.Now.ToString("MM.yyyy");

                                // Обновление пользователя в базе данных
                                UpdateUserInDatabase(Db_write.Users, userByPassword);

                                // Вход выполнен успешно
                                Main M = new Main();
                                MessageBox.Show("Granted");
                                this.Hide();
                                M.Show();
                            }
                            else
                            {
                                MessageBox.Show("Denyed");
                            }
                        }
                        else { MessageBox.Show("Account is not exist"); }
                        //MessageBox.Show(Convert.ToString(login));
                    }
                    else { MessageBox.Show("Account is not exist"); }
                }
                else { textBox1.Text = ""; textBox2.Text = ""; }
            } else{
                //структура десериализации из tbiUser
                var Db_write = JsonSerializer.Deserialize<DataModel>(jsonString_r);
                //получение логинов базы либо null зависит от того есть ли результат поиска
                var TextBase = GetUserByLogin(Db_write.Users, textBox1.Text);
                // проверка полей на содержание
                if (textBox1.Text != "" & textBox2.Text != "")
                {
                    //Проверка если результат базы пуст то добавление в базу нового пользователя
                    if (TextBase == null)
                    {
                        //структура базы
                        Db_write.Users.Add(new USER
                        {
                            _ID = Db_write.Users.Count + 1,
                            _LOGIN = textBox1.Text,
                            _PASSWORD = SaltManager.HashPassword(textBox2.Text, globalSalt),
                            _ACCESS = 'A',
                            _DATE = DateTime.Now.ToString("MM.yyyy")
                    });
                        //структура сериализации в tbiUser
                        var jsonString = JsonSerializer.Serialize(Db_write);
                        //запись в файл
                        File.WriteAllText(Users_base, jsonString);
                        MessageBox.Show("Now its your account");
                        //переход на форму
                        Main M = new Main();
                        this.Hide();
                        M.Show();
                    }
                    else{ MessageBox.Show("Account with the same login is already exist"); }
                }
                else { textBox1.Text = ""; textBox2.Text = ""; }
            }
            //var user = GetUserById(Db_read.Users, 1); 
        }










        /*
         -------------------------------------------------------------------------------

         Db_write_hashes.Hashes.Add(new USER_Hash
            {
                Month = [здесь дата];
                SaltValue = [здесь соли];
            });
         --------------------------------------------------------------------------------
             //имя файла
         string fileName = "tbiUser.json";
             //чтение файла
         string jsonString_r = File.ReadAllText(fileName);
         --------------------------------------------------------------------------------
             //структура сериализации
         var jsonString = JsonSerializer.Serialize(Db_write_hashes);
             //запись в файл
         File.WriteAllText(fileName, jsonString);
         --------------------------------------------------------------------------------
             //структура десериализации
         var Db_write = JsonSerializer.Deserialize<DataModel_Hash>(jsonString_r);
         --------------------------------------------------------------------------------
         взаимодействие с одним из методов получения данных(чтобыне было технической ошибки в коде проверяй этим 'if (TextBase == null)') 
         var TextBase = GetUserByLogin(Db_write.Users, textBox1.Text);

            Вроде все

            Если знаешь как сделать так чтобы при сохранении в базу данные не были в одну строку то зделай
         оно не мешает, в конце строки просто Enter нажми
         */




        private void Introduse_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       
        static USER GetUserById(List<USER> users, int userId)
        {
            return users.Find(user => user._ID == userId);
        }
        static USER GetUserByLogin(List<USER> users, string userLogin)
        {
            return users.Find(user => user._LOGIN == userLogin);
        }
        static USER GetUserByPassword(List<USER> users, string userpass)
        {
            return users.Find(user => user._PASSWORD == userpass);
        }
        public  string GetUserDate(List<USER> users, string userLogin)
        {
            if (users != null)
            {
                USER user_l = users.Find(user => user._LOGIN == userLogin);
                if (user_l != null)
                {

                    return user_l._DATE;
                }
                else
                {
                    throw new ArgumentException("User not found with the given login.");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(users), "User list is null.");
            }
        }

        // Функция для обновления пользователя в базе данных
        static void UpdateUserInDatabase(List<USER> users, USER updatedUser)
        {
            // Находим индекс пользователя в списке
            int index = users.FindIndex(user => user._ID == updatedUser._ID);

            if (index != -1)
            {
                // Обновляем пользователя в списке
                users[index] = updatedUser;
                MessageBox.Show(Convert.ToString(index));
                // Сохраняем обновленные данные в файл (если необходимо)
                SaveUsersToJsonFile(users, "tbiUser.json");
            }
        }

        // Функция для сохранения пользователей в JSON-файл
        static void SaveUsersToJsonFile(List<USER> users, string filePath)
        {
            try
            {
                // Сериализация списка пользователей в JSON
                var jsonString = JsonSerializer.Serialize(new DataModel { Users = users });

                // Запись JSON-строки в файл
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users to JSON: {ex.Message}");
            }
        }
        private void Introduse_Load(object sender, EventArgs e)
        {

        }
    }
}
