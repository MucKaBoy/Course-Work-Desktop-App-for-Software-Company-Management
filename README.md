# Course Work — Desktop App for Software Company Management
Курсовая работа: десктопное приложение на C# (WinForms) для управления 
договорами, техническими заданиями и отчётностью компании-разработчика ПО. 
Работает с базой данных SQL Server.

## Скриншоты

<img width="1191" height="655" alt="image" src="https://github.com/user-attachments/assets/ecd9aba7-1ac7-49fe-a94e-dc41e6e9e1a5" />

<img width="1026" height="408" alt="image" src="https://github.com/user-attachments/assets/25a0bfe2-7e73-450e-af37-d377dfd9cb8d" />

<img width="364" height="228" alt="image" src="https://github.com/user-attachments/assets/9ae13c84-573f-4bcc-bead-78bd0771a103" />

<img width="234" height="205" alt="image" src="https://github.com/user-attachments/assets/60c707dd-62c4-4cee-8a70-499f66392529" />

## Функционал

- Авторизация и регистрация пользователей
- Просмотр договоров с клиентами, кураторами, ТЗ и отчётностью (JOIN 6 таблиц)
- Добавление, редактирование, удаление договоров (с каскадным удалением связанных данных)
- Поиск по всем полям через `CONCAT` + `LIKE`
- Валидация ввода (сумма, даты, ID)
- Транзакции для атомарного изменения данных

## Стек

- **C# / .NET Framework** — WinForms
- **SQL Server** — база данных
- **ADO.NET** — `SqlConnection`, `SqlCommand`, `SqlDataReader`, `SqlTransaction`

## Структура БД

Основные таблицы: `Договор`, `ТЗ`, `Отчётность`, `Клиент`, `Куратор`, 
`Программный_продукт`, `ТЗ_Программист`, `register` (пользователи).

Файлы базы данных (`Program.mdf` и `Program_log.ldf`) находятся в папке `database/`.

## Как запустить (для разработчика)

1. Установить **SQL Server** (Express или Developer Edition) и **Visual Studio**.
2. Скопировать файлы `Program.mdf` и `Program_log.ldf` на свой компьютер.
3. В SQL Server Management Studio (SSMS) нажать правой кнопкой на `Базы данных` -> `Присоединить` (Attach) и выбрать `Program.mdf`.
4. Открыть проект в Visual Studio.
5. В файле `DataBase.cs` изменить строку подключения (Connection String) под свой компьютер:
   ``` SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-UBR7CD6\SQLEXPRESS;Initial Catalog=Program;Integrated Security=True");```
6. Собрать и запустить проект.
## Что бы я улучшил сейчас
Проект писался как курсовая, и я вижу несколько мест, которые сделал бы иначе:

Безопасность: сейчас пароли хранятся в открытом виде, а в формах логина и регистрации используется конкатенация строк (уязвимость к SQL-инъекциям). В продакшене использовал бы хеширование (BCrypt) и параметризованные запросы везде.

UI: 28 текстбоксов textBox1…textBox28 заменил бы на binding к модели или DataGridView с редактированием.

Архитектура: разделил бы проект на слои (UI / бизнес-логика / доступ к данным). Сейчас запросы пишутся прямо в обработчиках кнопок формы.

Обработка данных: ReadSingleRow жёстко завязан на порядок колонок — использовал бы имена колонок или ORM (Entity Framework).

## Чему научился
Проектирование реляционной БД (нормализация, внешние ключи, связи many-to-many)

Сложные SQL-запросы: JOIN 6 таблиц, подзапросы, CONCAT + LIKE для поиска

Транзакции в ADO.NET: BeginTransaction, Commit, Rollback

Параметризованные запросы (SqlParameter) — защита от SQL-инъекций

Работа с WinForms: DataGridView, формы, валидация
