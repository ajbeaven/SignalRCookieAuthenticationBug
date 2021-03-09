using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Chat
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSignalR();

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder.WithOrigins("https://localhost:44349")
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials();
				});
			});

			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
					options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
					options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
				})
				.AddCookie(IdentityConstants.ApplicationScheme, options => ConfigureCookieAuthenticationOptions(options, IdentityConstants.ApplicationScheme))
				.AddCookie(IdentityConstants.ExternalScheme, options => ConfigureCookieAuthenticationOptions(options, IdentityConstants.ExternalScheme));

			services.AddAuthorization();
		}

		private void ConfigureCookieAuthenticationOptions(CookieAuthenticationOptions options, string scheme)
		{
			options.LoginPath = new PathString("/account/login");
			options.SlidingExpiration = false;
			options.ExpireTimeSpan = TimeSpan.FromDays(14);
			options.ReturnUrlParameter = "returnUrl";
			options.LogoutPath = new PathString("/account/logout");
			options.Cookie.HttpOnly = true;
			options.Cookie.SameSite = SameSiteMode.Strict;
			options.Cookie.Name = scheme + ".SDAUTH";
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<ChatHub>("/chathub");
			});
		}
	}
}
