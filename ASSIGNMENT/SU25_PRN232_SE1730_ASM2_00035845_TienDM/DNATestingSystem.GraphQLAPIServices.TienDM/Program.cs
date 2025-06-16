using System.Text.Json.Serialization;
using DNATestingSystem.Services.TienDM;
using DNATestingSystem.Repository.TienDM;
using DNATestingSystem.Repository.TienDM.DBContext;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Types;
using DNATestingSystem.GraphQLAPIServices.TienDM.GraphQLs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Entity Framework DbContext
builder.Services.AddDbContext<SE18_PRN232_SE1730_G3_DNATestingSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repository Pattern - UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Service Layer Pattern - ServiceProviders
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();

// Add individual services (optional if using ServiceProviders)
builder.Services.AddScoped<SystemUserAccountService>();
builder.Services.AddScoped<AppointmentsTienDmService>();
builder.Services.AddScoped<AppointmentStatusesTienDmService>();
builder.Services.AddScoped<ServicesNhanVtService>();

// Add GraphQL Server with Query and Mutation types
builder.Services.AddGraphQLServer()
    .AddQueryType<DNATestingSystem.GraphQLAPIServices.TienDM.GraphQLs.Query>()
    .AddMutationType<DNATestingSystem.GraphQLAPIServices.TienDM.GraphQLs.Mutations>()
    .BindRuntimeType<DateTime, DateTimeType>();

//builder.Services.AddGraphQLServer().BindRuntimeType<DateTime, DateTimeType>();

// Add Controllers with JSON options for reference handling
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});

// Add CORS policy
builder.Services.AddCors();

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

// Add CORS middleware
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

// Configure routing and endpoints
app.UseRouting().UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.MapControllers();

app.Run();
