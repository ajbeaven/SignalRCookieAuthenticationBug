using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SignalRCookieAuthenticationBug.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
					options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
					options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
				})
				.AddCookie(IdentityConstants.ApplicationScheme, options => ConfigureCookieAuthenticationOptions(options, IdentityConstants.ApplicationScheme))
				.AddCookie(IdentityConstants.ExternalScheme, options => ConfigureCookieAuthenticationOptions(options, IdentityConstants.ExternalScheme));
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

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
