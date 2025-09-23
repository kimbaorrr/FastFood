using FastFood.DB;
using FastFood.Repositories;
using FastFood.Repositories.Interfaces;
using FastFood.Services;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using VNPAY.NET;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{

    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});

builder.Services.AddAuthentication()
    .AddCookie("CustomerScheme")
    .AddCookie("EmployeeScheme");

builder.Services.AddAuthorization();


builder.Services.AddMemoryCache();
builder.Logging.AddConsole();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(7);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// DbContext
builder.Services.AddDbContext<FastFoodEntities>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings:Postgres"]);
    options.UseLazyLoadingProxies();
});

// Repositories
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerAccountRepository, CustomerAccountRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeAccountRepository, EmployeeAccountRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IInventoryInRepository, InventoryInRepository>();
builder.Services.AddScoped<ILoggingEventRepository, LoggingEventRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrdersStatusRepository, OrdersStatusRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IProductIngredientRepository,  ProductIngredientRepository>();
builder.Services.AddScoped<IProductRepository,  ProductRepository>();
builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddScoped<IPromoRepository, PromoRepository>();
builder.Services.AddScoped<IStoreInfoRepository,  StoreInfoRepository>();
builder.Services.AddScoped<IWorkScheduleRepository, WorkScheduleRepository>();

// Services
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IInventoryInService, InventoryInService>();
builder.Services.AddScoped<ILoggingEventService, LoggingEventService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductReviewService, ProductReviewService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IVnpay, Vnpay>();


WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "admin/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

await app.RunAsync();
