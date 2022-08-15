
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Smart_farm.Model.DBContext db = new Smart_farm.Model.DBContext(app.Configuration);
db.Database.EnsureCreated();

try
{
    Smart_farm.Model.Arudino arduino = new Smart_farm.Model.Arudino("COM3") { dbContext = db };
    arduino.FetchWork();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

