using WebSharper.AspNetCore;

namespace WebSharper.AspNetMvc.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddWebSharper().AddWebSharperContent();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseWebSharperRemoting();

            //app.MapStaticAssets();
            app.UseStaticFiles();
            app.MapRazorPages()
               .WithStaticAssets();


            app.Run();
        }
    }
}
