using Apangelia.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы в контейнер зависимостей
builder.AddConfiguration();

var app = builder.Build();

// Настраиваем конвейер обработки HTTP-запросов
app.UseConfiguration();

app.Run();