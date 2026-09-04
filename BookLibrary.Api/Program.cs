using BookLibrary.Api.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureDatabase();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddServices();

var app = builder.Build();

app.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseCors();

app.MapHealthChecksEndpoint();
app.MapControllers();

app.Run();
