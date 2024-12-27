
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechAspire.Hepler;
using TechAspire.Models;

namespace TechAspire.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class CouponCotroller : Controller
	{
		private readonly DataContext _dataContext;
		public CouponCotroller(DataContext context)
		{
			_dataContext = context;
		}
	
		




	}
}
