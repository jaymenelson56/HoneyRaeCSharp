using HoneyRaesAPI.Models;
using HoneyRaesAPI.Models.DTOs;

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

app.MapGet("/servicetickets", () =>
{
    return serviceTickets.Select(t => new ServiceTicketDTO
    {
        Id = t.Id,
        CustomerId = t.CustomerId,
        EmployeeId = t.EmployeeId,
        Description = t.Description,
        Emergency = t.Emergency,
        DateCompleted = t.DateCompleted
    });
});
   
app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }

    Employee employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);

    Customer customer = customers.FirstOrDefault(c => c.Id == serviceTicket.EmployeeId);
  
    return Results.Ok(new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = customer == null ? null : new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        EmployeeId = serviceTicket.EmployeeId,
        Employee = employee == null ? null : new EmployeeDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Specialty = employee.Specialty

        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency,
        DateCompleted = serviceTicket.DateCompleted
    });
});

app.MapGet("/employees", () =>
{
    return employees.Select(s => new EmployeeDTO
    {
        Id = s.Id,
        Name = s.Name,
        Specialty = s.Specialty
    });
});

app.MapGet("/employees/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(em => em.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }

    List<ServiceTicket> tickets = serviceTickets.Where(st => st.EmployeeId == id).ToList();

    return Results.Ok(new EmployeeDTO
    {
        Id = employee.Id,
        Name = employee.Name,
        Specialty = employee.Specialty,
        ServiceTickets = tickets.Select(t => new ServiceTicketDTO
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            EmployeeId = t.EmployeeId,
            Description = t.Description,
            Emergency = t.Emergency,
            DateCompleted = t.DateCompleted
        }).ToList()
    });
});

app.MapGet("/customers", () =>
{
    return customers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address
    });
});

app.MapGet("/customers/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(cu => cu.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }

    List<ServiceTicket> tickets = serviceTickets.Where(st => st.CustomerId == id).ToList();

    return Results.Ok(new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address,
        ServiceTickets = tickets.Select(t => new ServiceTicketDTO
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            EmployeeId = t.EmployeeId,
            Description = t.Description,
            Emergency = t.Emergency,
            DateCompleted = t.DateCompleted
        }).ToList()
    });
});

app.MapPost("/servicetickets", (ServiceTicket serviceTicket) => 
{
    Customer customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);
    if (customer == null)
    {
        return Results.BadRequest();
    }
    serviceTicket.Id = serviceTickets.Max(st => st.Id) + 1;
    serviceTickets.Add(serviceTicket);
    return Results.Created($"/servicetickets/{serviceTicket.Id}", new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency
    });
});

app.MapDelete("/servicetickets/{id}", (int id) => 
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);

    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    serviceTickets.Remove(serviceTicket);

    return Results.NoContent();
});

app.MapPut("/servicetickets/{id}", (int id, ServiceTicket serviceTicket) =>
{
    ServiceTicket ticketToUpdate = serviceTickets.FirstOrDefault(st => st.Id == id);

    if (ticketToUpdate == null)
    {
        return Results.NotFound();
    }
    if (id != serviceTicket.Id)
    {
        return Results.BadRequest();
    }
    ticketToUpdate.CustomerId = serviceTicket.CustomerId;
    ticketToUpdate.EmployeeId = serviceTicket.EmployeeId;
    ticketToUpdate.Description = serviceTicket.Description;
    ticketToUpdate.Emergency = serviceTicket.Emergency;
    ticketToUpdate.DateCompleted = serviceTicket.DateCompleted;

    return Results.NoContent();
});
app.MapPost("/servicetickets/{id}/complete", (int id) =>
{
    ServiceTicket ticketToComplete = serviceTickets.FirstOrDefault(st => st.Id == id);
    
    ticketToComplete.DateCompleted = DateTime.Today;
});

app.Run();

