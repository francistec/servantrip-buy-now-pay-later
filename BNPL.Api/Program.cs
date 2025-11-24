using BNPL.Application.Services;
using BNPL.Domain.Services;
using BNPL.Infrastructure.Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PaymentSchedulerService>(sp =>
    new PaymentSchedulerService(builder.Configuration["Stripe:SecretKey"]));
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<PaymentCalculator>();


var app = builder.Build();

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
