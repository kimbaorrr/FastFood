using FastFood.Repositories;
using FastFood.Repositories.Interfaces;
using FastFood.Services;
using FastFood.Services.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Logging.AddConsole();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Repositories
builder.Services.AddSingleton<IArticleRepository, ArticleRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<ICustomerAccountRepository, CustomerAccountRepository>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IEmployeeAccountRepository, EmployeeAccountRepository>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddSingleton<IIngredientRepository, IngredientRepository>();
builder.Services.AddSingleton<IInventoryInRepository, InventoryInRepository>();
builder.Services.AddSingleton<ILoggingEventRepository, LoggingEventRepository>();
builder.Services.AddSingleton<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IOrdersStatusRepository, OrdersStatusRepository>();
builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();
builder.Services.AddSingleton<IPermissionRepository, PermissionRepository>();
builder.Services.AddSingleton<IProductIngredientRepository,  ProductIngredientRepository>();
builder.Services.AddSingleton<IProductRepository,  ProductRepository>();
builder.Services.AddSingleton<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddSingleton<IPromoRepository, PromoRepository>();
builder.Services.AddSingleton<IStoreInfoRepository,  StoreInfoRepository>();
builder.Services.AddSingleton<IWorkScheduleRepository, WorkScheduleRepository>();

// Services
builder.Services.AddSingleton<IArticleService, ArticleService>();
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<IFileUploadService, FileUploadService>();
builder.Services.AddSingleton<IIngredientService, IngredientService>();
builder.Services.AddSingleton<IInventoryInService, InventoryInService>();
builder.Services.AddSingleton<ILoggingEventService, LoggingEventService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<IProductReviewService, ProductReviewService>();
builder.Services.AddSingleton<IProductService, ProductService>();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseRouting();

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
