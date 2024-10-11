using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentApp.API.Configuration;
using StudentApp.Application;
using StudentApp.Application.CQRS.StudentCommandQuery.Command;
using StudentApp.Core.Context;
using StudentApp.Infrastructure;
using StudentApp.Infrastructure.Models;
using static StudentApp.API.proto.checkPermissionService;

var builder = WebApplication.CreateBuilder(args);

#region Add Db Context

builder.Services.AddDbContext<StudentAppContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("StudentAppConnectionString")));

#endregion

#region Add MediatR
builder.Services.AddMediatR(typeof(CreateStudentCommand));

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
#endregion

#region Add Options

//fill configs from appsetting.json
builder.Services.AddOptions();
builder.Services.Configure<Configs>(builder.Configuration.GetSection("Configs"));
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddMiniProfiler(options => options.RouteBasePath = "/profiler").AddEntityFramework();
builder.Services.AddSwagger();
builder.Services.AddJWT();
builder.Services.AddGrpcClient<checkPermissionServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["GRPC_SERVER_ADDRESS"]);
});

#region DI

builder.Services.AddInfrastructureDI();
builder.Services.AddApplicationService();

#endregion

#region register AutoMapper

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfig());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

#endregion



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiniProfiler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
