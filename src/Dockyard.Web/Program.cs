using Dockyard.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// These blocks means invalid config halts the app from starting
builder.Services
    .AddOptions<OidcSettings>()
    .Bind(builder.Configuration.GetSection(OidcSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
 
builder.Services
    .AddOptions<DockyardOptions>()
    .Bind(builder.Configuration.GetSection(DockyardOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
