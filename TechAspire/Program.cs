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
	options.AddPolicy("MyPolicy", policy =>
		policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())); // Điều chỉnh cho môi trường sản xuất

// Cấu hình DbContext với SQL Server
builder.Services.AddDbContext<DataContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("TechAspire"));
});

// Cấu hình Identity
builder.Services.AddIdentity<AppUserModel, IdentityRole>()
	.AddEntityFrameworkStores<DataContext>()
	.AddDefaultTokenProviders();

// Cấu hình Email Sender
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Cấu hình Identity Options
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
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};

		// Xử lý khi token hết hạn
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
