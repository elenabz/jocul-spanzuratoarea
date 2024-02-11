using JoculSpanzuratoarea.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

builder.Services.AddDbContext<ApplicationDbContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );

// Add services to the container.
builder.Services.AddRazorPages(); // specific to Razor Pages: AddRazorPages()
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

// add distributed memory cache session = store objects in memory 
// session = dictionary, key value pairs
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseBrowserLink();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

// specific to Razor Pages: MapRazorPages() + the routing maps to what is in Pages folder
// the Controller and View folders from .net MVC are replaced with Pages folder
// PageModel class replace the Controller from MVC 
app.MapRazorPages();
app.Run();



/*
 * dex online for words 
 * db for words 
 * legaturile intre tabele : in modele si in ApplicationDbContext
 * generate word
 * remove / (
 * diacritice
 * description
 * get definition of word from dex
 * git repository 
 * 
 * fix get word from db each time a letter is selected 
 * change session with db data
 * move game on home page 
 * images for game 
 * styling
 * 
 */

// @page "/index" pentru a seta ruta pentru aceasta pagina 

// MVC = add a View(page)
// Razor Pages = add a Razor Page and a Page Model 

// @page = Razor Page 
// Razor routing = /page_under_Pages_folder || /folder_under_Pages_folder/page
// if Razor can't find the page to redirect will not display page not found or an error 

/* tag helpers
 * MVC: asp-controller + asp-action = Razor PAges: asp-page="page" || "folder/page"(complete route ) (asp-route-params for MVC + RzPg)
 * 
 * [BindProperty]  for a property in PageModel will bind to the values that are send to post action of the page 
 *  (|| [BindPropertis] -> for all properties of the model)
 * 
 * OnGet() can have params
 * 
 
 

session: 
    session variables 
    public enum SessionKeyEnum
    {
        SessionKeyWord,
        SessionKeyMaskedWord,
        SessionKeyFailCount,
        SessionKeyGuessedFullWord
    }

-- remove session variable
HttpContext.Session.Remove(SessionKeyEnum.SessionKeyWord.ToString());

-- get and set session variables 
HttpContext.Session.SetString(SessionKeyEnum.SessionKeyWord.ToString(), word);
HttpContext.Session.GetInt32(SessionKeyEnum.SessionKeyFailCount.ToString())
 
 */