using Biblioteca2.Data;
using Biblioteca2.Models;
using Biblioteca2.Models.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var appSettingsString = builder.Configuration.GetSection("AppSettings");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<Biblioteca2Context>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

//- Acesso às secções AppSettings em appsettings.json configure strongly typed settings objects
builder.Services.Configure<AppSettings>(appSettingsString);     //--- jsp
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();                              //--- jsp

//--- Recolher datetime da dll da aplicação
builder.Services.AddSingleton<BuildInfo>();                     // Store build info in DI

//--- Adicionar serviços Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Usando Políticas de COR
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", // Uma política mais permissiva (cuidado em produção!)
        builder =>
        {
            builder.AllowAnyOrigin()    // Permite *qualquer* domínio
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
var app = builder.Build();

//--- Habilitar o middleware Swagger em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//--- Habilitar o middleware CORS
app.UseCors("AllowAllOrigins"); // Habilita o middleware CORS

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
