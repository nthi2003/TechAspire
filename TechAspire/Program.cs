using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TechAspire.Repository;
using System.Text;
using TechAspire.Models;

var builder = WebApplication.CreateBuilder(args);

// Đọc cấu hình từ file appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Cấu hình các dịch vụ
builder.Services.AddControllers();
builder.Services.AddSingleton<CloudinaryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình CORS
builder.Services.AddCors(options =>
    options.AddPolicy("MyPolicy", policy =>
        policy.WithOrigins("https://localhost:7217")  // Cho phép chỉ nguồn gốc này
              .AllowAnyHeader()  // Cho phép mọi header
              .AllowAnyMethod()  // Cho phép mọi phương thức HTTP
              .AllowCredentials()  // Cho phép sử dụng credentials như cookies, HTTP authentication
    ));

// Cấu hình DbContext cho Entity Framework
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TechAspire"));
});

// Cấu hình Identity (AppUserModel, IdentityRole)
builder.Services.AddIdentity<AppUserModel, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// Cấu hình Email Sender Service
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Cấu hình các tùy chọn của Identity (Password Policy)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 6;
});

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["JwtSettings:SecretKey"] // Đọc khóa bí mật từ cấu hình
            )),
            ValidateIssuer = false, 
            ValidateAudience = false 
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                    context.Response.StatusCode = 401; // Trả về mã lỗi 401 (Unauthorized)
                }
                else
                {
                    // Xử lý các lỗi khác
                    Console.Error.WriteLine("Authentication failed: " + context.Exception.Message);
                }
                return Task.CompletedTask;
            }
        };
    });

// Cấu hình Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllowEditRole", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireRole("Admin");
    });
});

var app = builder.Build();

// Use Swagger and SwaggerUI in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyPolicy");
// Các middleware
app.UseHttpsRedirection();

// Cấu hình Authentication và Authorization middleware
app.UseAuthentication();  // Đảm bảo thêm dòng này để sử dụng xác thực
app.UseAuthorization();   // Đảm bảo thêm dòng này để sử dụng phân quyền

// Cấu hình API endpoints
app.MapControllers();

// Run the app
app.Run();
