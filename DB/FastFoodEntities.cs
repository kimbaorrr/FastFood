using System;
using System.Collections.Generic;
using FastFood.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB;

public partial class FastFoodEntities : DbContext
{
    public FastFoodEntities(DbContextOptions<FastFoodEntities> options) : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAccount> CustomerAccounts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAccount> EmployeeAccounts { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<InventoryIn> InventoryIns { get; set; }

    public virtual DbSet<LoggingEvent> LoggingEvents { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrdersStatus> OrdersStatuses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductIngredient> ProductIngredients { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<Promo> Promos { get; set; }

    public virtual DbSet<StoreInfo> StoreInfos { get; set; }

    public virtual DbSet<Sysdiagram> Sysdiagrams { get; set; }

    public virtual DbSet<WorkSchedule> WorkSchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("pk__baiviet__aedd56471e6bac84");

            entity.ToTable("articles");

            entity.HasIndex(e => e.Title, "uq__baiviet__371689aaa357a80b").IsUnique();

            entity.Property(e => e.ArticleId)
                .HasDefaultValueSql("nextval('baiviet_mabaiviet_seq'::regclass)")
                .HasColumnName("article_id");
            entity.Property(e => e.ApproveAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("approve_at");
            entity.Property(e => e.ApproverId).HasColumnName("approver_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CoverImage).HasColumnName("cover_image");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false)
                .HasColumnName("is_approved");
            entity.Property(e => e.Summary)
                .HasMaxLength(255)
                .HasColumnName("summary");
            entity.Property(e => e.Tags)
                .HasMaxLength(255)
                .HasColumnName("tags");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Approver).WithMany(p => p.ArticleApprovers)
                .HasForeignKey(d => d.ApproverId)
                .HasConstraintName("fk__baiviet__nguoidu__24e777c3");

            entity.HasOne(d => d.Author).WithMany(p => p.ArticleAuthors)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__baiviet__nguoita__23f3538a");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("pk__danhmuc__b3750887a72adb6e");

            entity.ToTable("categories");

            entity.HasIndex(e => e.CategoryName, "uq__danhmuc__650cae4e957f6e3a").IsUnique();

            entity.Property(e => e.CategoryId)
                .HasDefaultValueSql("nextval('danhmuc_madanhmuc_seq'::regclass)")
                .HasColumnName("category_id");
            entity.Property(e => e.BackgroundImage).HasColumnName("background_image");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.ThumbnailImage).HasColumnName("thumbnail_image");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Categories)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk__danhmuc__nguoita__3f115e1a");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("pk__khachhan__88d2f0e54c649103");

            entity.ToTable("customers");

            entity.HasIndex(e => e.Email, "uq__khachhan__a9d10534018c4d92").IsUnique();

            entity.Property(e => e.CustomerId)
                .HasDefaultValueSql("nextval('khachhang_makhachhang_seq'::regclass)")
                .HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Bod).HasColumnName("bod");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(15)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.ThumbnailImage).HasColumnName("thumbnail_image");
        });

        modelBuilder.Entity<CustomerAccount>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("pk__khachhan__88d2f0e52583d8d1");

            entity.ToTable("customer_accounts");

            entity.HasIndex(e => e.UserName, "uq__khachhan__55f68fc01bca21a4").IsUnique();

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Customer).WithOne(p => p.CustomerAccount)
                .HasForeignKey<CustomerAccount>(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__khachhang__makha__48cfd27e");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("pk__nhanvien__77b2ca47a97cef09");

            entity.ToTable("employees");

            entity.HasIndex(e => e.Email, "uq_email").IsUnique();

            entity.Property(e => e.EmployeeId)
                .HasDefaultValueSql("nextval('nhanvien_manhanvien_seq'::regclass)")
                .HasColumnName("employee_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(15)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.ThumbnailImage).HasColumnName("thumbnail_image");
        });

        modelBuilder.Entity<EmployeeAccount>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("pk__nhanvien__77b2ca47bd3eb5fd");

            entity.ToTable("employee_accounts");

            entity.HasIndex(e => e.UserName, "uq__nhanvien__55f68fc0955a1e2e").IsUnique();

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("employee_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Permission).HasColumnName("permission");
            entity.Property(e => e.Role)
                .HasDefaultValue(false)
                .HasColumnName("role");
            entity.Property(e => e.TemporaryPassword)
                .HasDefaultValue(false)
                .HasColumnName("temporary_password");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Employee).WithOne(p => p.EmployeeAccount)
                .HasForeignKey<EmployeeAccount>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__nhanviend__manha__5070f446");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cauhoi_pkey");

            entity.ToTable("feedbacks");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('cauhoi_macauhoi_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .HasColumnName("customer_name");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("pk__nguyenli__c7519355d8c171d2");

            entity.ToTable("ingredients");

            entity.HasIndex(e => e.IngredientName, "uq__nguyenli__0b228abdb3fb41d7").IsUnique();

            entity.Property(e => e.IngredientId)
                .HasDefaultValueSql("nextval('nguyenlieu_manguyenlieu_seq'::regclass)")
                .HasColumnName("ingredient_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.IngredientName)
                .HasMaxLength(50)
                .HasColumnName("ingredient_name");
            entity.Property(e => e.Inventory)
                .HasDefaultValue(0)
                .HasColumnName("inventory");
            entity.Property(e => e.LimitReorder)
                .HasDefaultValue(0)
                .HasColumnName("limit_reorder");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<InventoryIn>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("pk__nhapkho__b0602950595fbc2c");

            entity.ToTable("inventory_in");

            entity.Property(e => e.InventoryId)
                .HasDefaultValueSql("nextval('nhapkho_manhapkho_seq'::regclass)")
                .HasColumnName("inventory_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryIns)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__nhapkho__nguoinh__351ddf8c");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.InventoryIns)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__nhapkho__manguye__3429bb53");
        });

        modelBuilder.Entity<LoggingEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("pk__lichsutr__c443222a34ced8ea");

            entity.ToTable("logging_events");

            entity.Property(e => e.EventId)
                .HasDefaultValueSql("nextval('lichsutruycap_malichsu_seq'::regclass)")
                .HasColumnName("event_id");
            entity.Property(e => e.AccessedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("accessed_time");
            entity.Property(e => e.BrowserAgent)
                .HasMaxLength(100)
                .HasColumnName("browser_agent");
            entity.Property(e => e.Device)
                .HasMaxLength(100)
                .HasColumnName("device");
            entity.Property(e => e.EventDetail)
                .HasMaxLength(255)
                .HasColumnName("event_detail");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnName("ip_address");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");
            entity.Property(e => e.UserType)
                .HasDefaultValue(false)
                .HasColumnName("user_type");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("pk__donhang__129584adb6026bda");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .HasDefaultValueSql("nextval('donhang_madonhang_seq'::regclass)")
                .HasColumnName("product_id");
            entity.Property(e => e.ActualDeliveryTime)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("actual_delivery_time");
            entity.Property(e => e.Buyer).HasColumnName("buyer");
            entity.Property(e => e.EstimatedDeliveryTime).HasColumnName("estimated_delivery_time");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasDefaultValue(0)
                .HasColumnName("order_status");
            entity.Property(e => e.PromoId).HasColumnName("promo_id");
            entity.Property(e => e.Seller).HasColumnName("seller");
            entity.Property(e => e.ShipperName)
                .HasMaxLength(100)
                .HasColumnName("shipper_name");
            entity.Property(e => e.ShippingFee)
                .HasDefaultValue(0)
                .HasColumnName("shipping_fee");
            entity.Property(e => e.ShippingId)
                .HasMaxLength(50)
                .HasColumnName("shipping_id");
            entity.Property(e => e.ShippingMethod)
                .HasMaxLength(100)
                .HasColumnName("shipping_method");
            entity.Property(e => e.ShippingUnit)
                .HasMaxLength(100)
                .HasColumnName("shipping_unit");
            entity.Property(e => e.TotalPay)
                .HasDefaultValue(0)
                .HasColumnName("total_pay");
            entity.Property(e => e.TotalPrice)
                .HasDefaultValue(0)
                .HasColumnName("total_price");

            entity.HasOne(d => d.BuyerNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Buyer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__donhang__nguoimu__55f4c372");

            entity.HasOne(d => d.OrderStatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__donhang__trangth__531856c7");

            entity.HasOne(d => d.PromoCodeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromoId)
                .HasConstraintName("fk_makhuyenmai_makhuyenmai");

            entity.HasOne(d => d.SellerNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Seller)
                .HasConstraintName("fk__donhang__nguoiba__55009f39");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("pk_madonhang_masanpham");

            entity.ToTable("order_details");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.TotalPrice)
                .HasDefaultValue(0)
                .HasColumnName("total_price");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__chitietdo__madon__59fa5e80");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__chitietdo__masan__5aee82b9");
        });

        modelBuilder.Entity<OrdersStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__trangtha__aade4138b98a7a4e");

            entity.ToTable("orders_status");

            entity.HasIndex(e => e.StatusName, "uq__trangtha__9489ef66db060174").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Progress).HasColumnName("progress");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("pk__thanhtoa__d4b25844738881ed");

            entity.ToTable("payments");

            entity.Property(e => e.PaymentId)
                .HasDefaultValueSql("nextval('thanhtoan_mathanhtoan_seq'::regclass)")
                .HasColumnName("payment_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentStatus)
                .HasDefaultValue(false)
                .HasColumnName("payment_status");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__thanhtoan__madon__72c60c4a");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("pk__quyenhan__3eaf3ee6b849fe0f");

            entity.ToTable("permissions");

            entity.HasIndex(e => e.Description, "uq__quyenhan__24b0ca9edb8a3f03").IsUnique();

            entity.Property(e => e.PermissionId)
                .HasDefaultValueSql("nextval('quyenhannhanvien_maquyenhan_seq'::regclass)")
                .HasColumnName("permission_id");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("pk__sanpham__fac7442d52ed7601");

            entity.ToTable("products");

            entity.Property(e => e.ProductId)
                .HasDefaultValueSql("nextval('sanpham_masanpham_seq'::regclass)")
                .HasColumnName("product_id");
            entity.Property(e => e.ApprovedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("approved_at");
            entity.Property(e => e.ApproverId).HasColumnName("approver_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Discount)
                .HasDefaultValue(0)
                .HasColumnName("discount");
            entity.Property(e => e.FinalPrice)
                .HasComputedColumnSql("((original_price * (100 - discount)) / 100)", true)
                .HasColumnName("final_price");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.IsApprove)
                .HasDefaultValue(false)
                .HasColumnName("is_approve");
            entity.Property(e => e.OriginalPrice)
                .HasDefaultValue(0)
                .HasColumnName("original_price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("product_name");
            entity.Property(e => e.Summary)
                .HasMaxLength(100)
                .HasColumnName("summary");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Approver).WithMany(p => p.ProductApprovers)
                .HasForeignKey(d => d.ApproverId)
                .HasConstraintName("fk__sanpham__nguoidu__22751f6c");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk__sanpham__madanhm__3a81b327");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk__sanpham__nguoita__208cd6fa");
        });

        modelBuilder.Entity<ProductIngredient>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.IngredientId }).HasName("pk_manguyenlieu_masanpham");

            entity.ToTable("product_ingredients");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.QuantityNeeded)
                .HasDefaultValue(0)
                .HasColumnName("quantity_needed");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .HasColumnName("unit");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.ProductIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__sanpham_n__mangu__1db06a4f");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductIngredients)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__sanpham_n__masan__1cbc4616");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.ProductId }).HasName("pk_makhachhang_masanpham");

            entity.ToTable("product_reviews");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ReviewContent)
                .HasMaxLength(255)
                .HasColumnName("review_content");
            entity.Property(e => e.StarRating)
                .HasDefaultValue(3)
                .HasColumnName("star_rating");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__danhgiasa__makha__31b762fc");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__danhgiasa__masan__32ab8735");
        });

        modelBuilder.Entity<Promo>(entity =>
        {
            entity.HasKey(e => e.PromoId).HasName("pk__makhuyen__3213e83fdffe0aae");

            entity.ToTable("promos");

            entity.Property(e => e.PromoId)
                .HasDefaultValueSql("nextval('makhuyenmai_id_seq'::regclass)")
                .HasColumnName("promo_id");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0)
                .HasColumnName("discount_amount");
            entity.Property(e => e.EndTime)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("end_time");
            entity.Property(e => e.PromoCode)
                .HasMaxLength(20)
                .HasColumnName("promo_code");
            entity.Property(e => e.StartTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("start_time");
            entity.Property(e => e.Usage)
                .HasDefaultValue(0)
                .HasColumnName("usage");
        });

        modelBuilder.Entity<StoreInfo>(entity =>
        {
            entity.HasKey(e => e.StoreName).HasName("pk__thongtin__859441546ed8d011");

            entity.ToTable("store_info");

            entity.Property(e => e.StoreName)
                .HasMaxLength(50)
                .HasColumnName("store_name");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FacebookUrl)
                .HasMaxLength(100)
                .HasColumnName("facebook_url");
            entity.Property(e => e.Hotline)
                .HasMaxLength(12)
                .HasColumnName("hotline");
            entity.Property(e => e.InstagramUrl)
                .HasMaxLength(100)
                .HasColumnName("instagram_url");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.Slogan)
                .HasMaxLength(100)
                .HasColumnName("slogan");
            entity.Property(e => e.XUrl)
                .HasMaxLength(100)
                .HasColumnName("x_url");
            entity.Property(e => e.YoutubeUrl)
                .HasMaxLength(100)
                .HasColumnName("youtube_url");
        });

        modelBuilder.Entity<Sysdiagram>(entity =>
        {
            entity.HasKey(e => e.DiagramId).HasName("pk__sysdiagr__c2b05b613ccd2ecd");

            entity.ToTable("sysdiagrams");

            entity.HasIndex(e => new { e.PrincipalId, e.Name }, "uk_principal_name").IsUnique();

            entity.Property(e => e.DiagramId).HasColumnName("diagram_id");
            entity.Property(e => e.Definition).HasColumnName("definition");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.PrincipalId).HasColumnName("principal_id");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<WorkSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("pk__giolamvi__3213e83f3dd009f0");

            entity.ToTable("work_schedule");

            entity.HasIndex(e => e.WorkDate, "uq__giolamvi__3272f2bc0b5c074d").IsUnique();

            entity.Property(e => e.ScheduleId)
                .HasDefaultValueSql("nextval('giolamvieccuahang_id_seq'::regclass)")
                .HasColumnName("schedule_id");
            entity.Property(e => e.WorkDate)
                .HasMaxLength(20)
                .HasColumnName("work_date");
            entity.Property(e => e.WorkTime)
                .HasMaxLength(50)
                .HasColumnName("work_time");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
