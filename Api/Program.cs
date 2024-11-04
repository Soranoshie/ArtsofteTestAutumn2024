using Logic.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var config = new Config(builder.Environment.IsDevelopment());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(config);
builder.Services.RegisterModules();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();