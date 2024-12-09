using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TechAspire.Repository;
using System.Text;
using TechAspire.Models;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình các dịch vụ
builder.Services.AddControllers();
builder.Services.AddSingleton<CloudinaryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình CORS
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Cấu hình DbContext với SQL Server
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TechAspire"));
});
// giúp ích rất ng


builder.Services.AddIdentity<AppUserModel, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, EmailSender>();


builder.Services.Configure<IdentityOptions>(options =>
{
 
    options.Password.RequireDigit = true; // Yêu cầu ít nhất một chữ số
    options.Password.RequireLowercase = true; // Yêu cầu ít nhất một chữ cái thường
    options.Password.RequireUppercase = true; // Yêu cầu ít nhất một chữ cái hoa
    options.Password.RequireNonAlphanumeric = false; // Không yêu cầu ký tự đặc biệt
    options.Password.RequiredLength = 8; // Độ dài mật khẩu tối thiểu
    options.Password.RequiredUniqueChars = 6; // Số ký tự khác nhau tối thiểu
});

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        // Xử lý lỗi khi xác thực token không thành công
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Cấu hình Swagger (chỉ hiển thị trong môi trường phát triển)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Các middleware
app.UseHttpsRedirection();
app.UseAuthentication();  // Đảm bảo thêm dòng này để sử dụng xác thực
app.UseAuthorization();   // Đảm bảo thêm dòng này để sử dụng phân quyền

// Cấu hình API endpoints
app.MapControllers();

app.Run();
