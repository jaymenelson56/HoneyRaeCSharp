using HoneyRaesAPI.Models;

List<Customer> customers = new List<Customer> {};
List<Employee> employees = new List<Employee> {};
List<ServiceTicket> serviceTickets = new List<ServiceTicket> {};

Customer customer1 = new Customer
{
    Id = 1,
    Name = "Batman",
    Address = " 1244 Park Row, Gotham City ,NJ"
};
Customer customer2 = new Customer
{
    Id = 2,
    Name = "Robin",
    Address = " 1244 Park Row, Gotham City ,NJ"
};
Customer customer3 = new Customer
{
    Id = 3,
    Name = "Joker",
    Address = " 566 Bolland Park, Gotham City ,NJ"
};
customers.AddRange(new[] { customer1, customer2, customer3});

Employee employee1 = new Employee
{
    Id = 1,
    Name = "Jim Gordon",
    Specialty = "Phones"
};
Employee employee2 = new Employee
{
    Id = 2,
    Name = "Oswald Cobblepot",
    Specialty = "Game Consoles"
};

employees.AddRange(new[] { employee1, employee2 });

ServiceTicket ticket1 = new ServiceTicket
{
    Id = 1,
    CustomerId = 1,
    EmployeeId = 1,
    Description = "Pinball is stuck in the phone",
    Emergency = true,
    DateCompleted = DateTime.Now.AddDays(-5),
};
ServiceTicket ticket2 = new ServiceTicket
{
    Id = 2,
    CustomerId = 2,
    EmployeeId = 2,
    Description = "Pinball is stuck in Xbox",
    Emergency = false,
    DateCompleted = DateTime.Now.AddDays(-3),
};
ServiceTicket ticket3 = new ServiceTicket
{
    Id = 3,
    CustomerId = 3,
    Description = "Pinball is stuck in pinball machine",
    Emergency = false,
};
ServiceTicket ticket4 = new ServiceTicket
{
    Id = 4,
    CustomerId = 1,
    Description = "Pinball is stuck to another pinball",
    Emergency = false,
};
ServiceTicket ticket5 = new ServiceTicket
{
    Id = 5,
    CustomerId = 2,
    EmployeeId = 1,
    Description = "Pinball is stuck in stomach",
    Emergency = true,
    DateCompleted = DateTime.Now.AddDays(-2),
};
serviceTickets.AddRange(new[] { ticket1, ticket2, ticket3, ticket4, ticket5 });

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/hello", () => 
{
    return "hello";
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
