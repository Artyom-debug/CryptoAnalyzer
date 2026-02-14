using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add layer dependencies
builder.AddApplicationServices();
builder.AddInfrastructureServices();

//add jwt authentication and authorization
builder.Services.AddAuthentication()
    .AddJwtBearer();
builder.Services.AddAuthorization();

//configure api
builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
