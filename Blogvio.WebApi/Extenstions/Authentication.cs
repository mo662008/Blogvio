﻿using Blogvio.WebApi.Data;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Security;
using Blogvio.WebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blogvio.WebApi.Extenstions
{
	public static class Authentication
	{
		public static void AddJWT(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.Configure<JWT>(configuration.GetSection("JWT"));
			services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidIssuer = configuration["JWT:Issuer"],
						ValidAudience = configuration["JWT:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
					};
				});
			services.AddScoped<IIdentityService, IdentityService>();
		}
	}
}
