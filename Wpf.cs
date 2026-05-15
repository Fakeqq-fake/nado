<Window x:Class="UserManagement.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Управление пользователями" Height="450" Width="800">
    <Grid Margin="10">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <!-- Панель поиска -->
        <StackPanel Grid.Row="0" Orientation="Horizontal" Margin="0,0,0,10">
            <TextBox x:Name="SearchTextBox" Width="200" Height="25"
                    PlaceholderText="Поиск по имени/логину..."
                    TextChanged="SearchTextBox_TextChanged"/>
            <Button Content="Найти" Width="80" Height="25" Margin="10,0,0,0"
                   Click="SearchButton_Click"/>
        </StackPanel>

        <!-- Таблица пользователей -->
        <DataGrid x:Name="UsersDataGrid" Grid.Row="1" AutoGenerateColumns="False"
                IsReadOnly="True" CanUserAddRows="False">
            <DataGrid.Columns>
                <DataGridTextColumn Header="ID" Binding="{Binding Id}" Width="50"/>
                <DataGridTextColumn Header="Имя" Binding="{Binding Name}" Width="150"/>
                <DataGridTextColumn Header="Логин" Binding="{Binding Login}" Width="120"/>
                <DataGridTextColumn Header="Email" Binding="{Binding Email}" Width="200"/>
            </DataGrid.Columns>
        </DataGrid>

        <!-- Панель действий -->
        <StackPanel Grid.Row="2" Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,10,0,0">
            <Button x:Name="AddButton" Content="Добавить" Width="100" Height="30" Margin="0,0,10,0"
                Click="AddButton_Click"/>
            <Button x:Name="EditButton" Content="Редактировать" Width="100" Height="30" Margin="0,0,10,0"
                Click="EditButton_Click"/>
            <Button x:Name="DeleteButton" Content="Удалить" Width="100" Height="30"
                Click="DeleteButton_Click"/>
        </StackPanel>
    </Grid>
</Window>


namespace UserManagement
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // В реальной системе — хэш using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UserManagement
{
    public partial class MainWindow : Window
    {
        private List<User> users;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            // Инициализация тестовых данных
            users = new List<User>
            {
                new User { Id = 1, Name = "Иван Иванов", Login = "ivan", Email = "ivan@example.com" },
                new User { Id = 2, Name = "Мария Петрова", Login = "maria", Email = "maria@example.com" }
            };
            UsersDataGrid.ItemsSource = users;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearchFilter();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ApplySearchFilter();
        }

        private void ApplySearchFilter()
        {
            string searchText = SearchTextBox.Text.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                UsersDataGrid.ItemsSource = users;
                return;
            }

            var filteredUsers = users.FindAll(u =>
                u.Name.ToLower().Contains(searchText) ||
                u.Login.ToLower().Contains(searchText));
            UsersDataGrid.ItemsSource = filteredUsers;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new UserEditWindow();
            if (addWindow.ShowDialog() == true)
            {
                users.Add(addWindow.User);
                ApplySearchFilter(); // Обновляем таблицу с учётом фильтра
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                var editWindow = new UserEditWindow(selectedUser);
                if (editWindow.ShowDialog() == true)
                {
                    int index = users.IndexOf(selectedUser);
                    users[index] = editWindow.User;
                    ApplySearchFilter(); // Обновляем таблицу с учётом фильтра
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User userToDelete)
            {
                if (MessageBox.Show($"Удалить пользователя {userToDelete.Name}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    users.Remove(userToDelete);
                    ApplySearchFilter(); // Обновляем таблицу с учётом фильтра
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.");
            }
        }
    }
}

        <Window x:Class="UserManagement.UserEditWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="{Binding WindowTitle}" Height="250" Width="350">
    <Grid Margin="15">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <TextBlock Grid.Row="0" Grid.Column="0" Text="Имя:" VerticalAlignment="Center"/>
        <TextBox Grid.Row="0" Grid.Column="1" x:Name="NameTextBox" Margin="5,0"/>

        <TextBlock Grid.Row="1" Grid.Column="0" Text="Логин:" VerticalAlignment="Center"/>
        <TextBox Grid.Row="1" Grid.Column="1" x:Name="LoginTextBox" Margin="5,0"/>

        <TextBlock Grid.Row="2" Grid.Column="0" Text="Email:" VerticalAlignment="Center"/>
        <TextBox Grid.Row="2" Grid.Column="1" x:Name="EmailTextBox" Margin="5,0"/>

        <TextBlock Grid.Row="3" Grid.Column="0" Text="Пароль:" VerticalAlignment="Center"/>
        <PasswordBox Grid.Row="3" Grid.Column="1" x:Name="PasswordBox" Margin="5,0"/>

        <StackPanel Grid.Row="4" Grid.ColumnSpan="2" Orientation="Horizontal"
                HorizontalAlignment="Right" Margin="0,20,0,0">
            <Button Content="Сохранить" Width="80" Click="SaveButton_Click" IsDefault="True"/>
            <Button Content="Отмена" Width="80" Margin="10,0,0,0" Click="CancelButton_Click" IsCancel="True"/>
        </StackPanel>
    </Grid>
</Window>



         using System.Windows;

namespace UserManagement
{
    public partial class UserEditWindow : Window
    {
        public User User { get; private set; }
        public string WindowTitle { get; set; }

        // Конструктор для добавления нового пользователя
        public UserEditWindow()
        {
            InitializeComponent();
            User = new User();
            WindowTitle = "Добавить пользователя";
            DataContext = this;
        }

        // Конструктор для редактирования существующего пользователя
        public UserEditWindow(User user)
        {
            InitializeComponent();
            User = user;
            WindowTitle = "Редактировать пользователя";
            DataContext = this;

            // Заполняем поля текущими данными
            NameTextBox.Text = user.Name;
            LoginTextBox.Text = user.Login;
            EmailTextBox.Text = user.Email;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем обязательные поля
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                MessageBox.Show("Заполните обязательные поля: Имя и Логин.");
                return;
            }

            // Сохраняем данные в объект
            User.Name = NameTextBox.Text.Trim();
            User.Login = LoginTextBox.Text.Trim();
            User.Email = EmailTextBox.Text.Trim();

            // Если введён новый пароль — сохраняем его (в реальной системе — хэш)
            if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                User.Password = PasswordBox.Password;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
 

      

