using System;
using System.Collections.Generic;
using FastFood.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB;

public partial class FastFoodEntities : DbContext
{
    public FastFoodEntities()
    {
    }

    public FastFoodEntities(DbContextOptions<FastFoodEntities> options)
        : base(options)
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("pk__baiviet__aedd56471e6bac84");

            entity.Property(e => e.ArticleId).HasDefaultValueSql("nextval('baiviet_mabaiviet_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsApproved).HasDefaultValue(false);

            entity.HasOne(d => d.Approver).WithMany(p => p.ArticleApprovers).HasConstraintName("fk__baiviet__nguoidu__24e777c3");

            entity.HasOne(d => d.Author).WithMany(p => p.ArticleAuthors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__baiviet__nguoita__23f3538a");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("pk__danhmuc__b3750887a72adb6e");

            entity.Property(e => e.CategoryId).HasDefaultValueSql("nextval('danhmuc_madanhmuc_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Categories).HasConstraintName("fk__danhmuc__nguoita__3f115e1a");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("pk__khachhan__88d2f0e54c649103");

            entity.Property(e => e.CustomerId).HasDefaultValueSql("nextval('khachhang_makhachhang_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<CustomerAccount>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("pk__khachhan__88d2f0e52583d8d1");

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Customer).WithOne(p => p.CustomerAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__khachhang__makha__48cfd27e");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("pk__nhanvien__77b2ca47a97cef09");

            entity.Property(e => e.EmployeeId).HasDefaultValueSql("nextval('nhanvien_manhanvien_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<EmployeeAccount>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("pk__nhanvien__77b2ca47bd3eb5fd");

            entity.Property(e => e.EmployeeId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Role).HasDefaultValue(false);
            entity.Property(e => e.TemporaryPassword).HasDefaultValue(false);

            entity.HasOne(d => d.Employee).WithOne(p => p.EmployeeAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__nhanviend__manha__5070f446");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cauhoi_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('cauhoi_macauhoi_seq'::regclass)");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("pk__nguyenli__c7519355d8c171d2");

            entity.Property(e => e.IngredientId).HasDefaultValueSql("nextval('nguyenlieu_manguyenlieu_seq'::regclass)");
            entity.Property(e => e.Inventory).HasDefaultValue(0);
            entity.Property(e => e.LimitReorder).HasDefaultValue(0);
        });

        modelBuilder.Entity<InventoryIn>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("pk__nhapkho__b0602950595fbc2c");

            entity.Property(e => e.InventoryId).HasDefaultValueSql("nextval('nhapkho_manhapkho_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Quantity).HasDefaultValue(0);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InventoryIns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__nhapkho__nguoinh__351ddf8c");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.InventoryIns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__nhapkho__manguye__3429bb53");
        });

        modelBuilder.Entity<LoggingEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("pk__lichsutr__c443222a34ced8ea");

            entity.Property(e => e.EventId).HasDefaultValueSql("nextval('lichsutruycap_malichsu_seq'::regclass)");
            entity.Property(e => e.AccessedTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserType).HasDefaultValue(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("pk__donhang__129584adb6026bda");

            entity.Property(e => e.OrderId).HasDefaultValueSql("nextval('donhang_madonhang_seq'::regclass)");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.OrderStatus).HasDefaultValue(0);
            entity.Property(e => e.ShippingFee).HasDefaultValue(0);
            entity.Property(e => e.TotalPay).HasDefaultValue(0);
            entity.Property(e => e.TotalPrice).HasDefaultValue(0);

            entity.HasOne(d => d.BuyerNavigation).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__donhang__nguoimu__55f4c372");

            entity.HasOne(d => d.OrderStatusNavigation).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__donhang__trangth__531856c7");

            entity.HasOne(d => d.Promo).WithMany(p => p.Orders).HasConstraintName("fk_makhuyenmai_makhuyenmai");

            entity.HasOne(d => d.SellerNavigation).WithMany(p => p.Orders).HasConstraintName("fk__donhang__nguoiba__55009f39");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("pk_madonhang_masanpham");

            entity.Property(e => e.Quantity).HasDefaultValue(0);
            entity.Property(e => e.TotalPrice).HasDefaultValue(0);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__chitietdo__madon__59fa5e80");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__chitietdo__masan__5aee82b9");
        });

        modelBuilder.Entity<OrdersStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__trangtha__aade4138b98a7a4e");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("pk__thanhtoa__d4b25844738881ed");

            entity.Property(e => e.PaymentId).HasDefaultValueSql("nextval('thanhtoan_mathanhtoan_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.PaymentStatus).HasDefaultValue(false);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__thanhtoan__madon__72c60c4a");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("pk__quyenhan__3eaf3ee6b849fe0f");

            entity.Property(e => e.PermissionId).HasDefaultValueSql("nextval('quyenhannhanvien_maquyenhan_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("pk__sanpham__fac7442d52ed7601");

            entity.Property(e => e.ProductId).HasDefaultValueSql("nextval('sanpham_masanpham_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Discount).HasDefaultValue(0);
            entity.Property(e => e.FinalPrice).HasComputedColumnSql("((original_price * (100 - discount)) / 100)", true);
            entity.Property(e => e.IsApprove).HasDefaultValue(false);
            entity.Property(e => e.OriginalPrice).HasDefaultValue(0);

            entity.HasOne(d => d.Approver).WithMany(p => p.ProductApprovers).HasConstraintName("fk__sanpham__nguoidu__22751f6c");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("fk__sanpham__madanhm__3a81b327");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations).HasConstraintName("fk__sanpham__nguoita__208cd6fa");
        });

        modelBuilder.Entity<ProductIngredient>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.IngredientId }).HasName("pk_manguyenlieu_masanpham");

            entity.Property(e => e.QuantityNeeded).HasDefaultValue(0);

            entity.HasOne(d => d.Ingredient).WithMany(p => p.ProductIngredients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__sanpham_n__mangu__1db06a4f");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductIngredients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__sanpham_n__masan__1cbc4616");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.ProductId }).HasName("pk_makhachhang_masanpham");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.StarRating).HasDefaultValue(3);

            entity.HasOne(d => d.Customer).WithMany(p => p.ProductReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__danhgiasa__makha__31b762fc");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk__danhgiasa__masan__32ab8735");
        });

        modelBuilder.Entity<Promo>(entity =>
        {
            entity.HasKey(e => e.PromoId).HasName("pk__makhuyen__3213e83fdffe0aae");

            entity.Property(e => e.PromoId).HasDefaultValueSql("nextval('makhuyenmai_id_seq'::regclass)");
            entity.Property(e => e.DiscountAmount).HasDefaultValue(0);
            entity.Property(e => e.StartTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Usage).HasDefaultValue(0);
        });

        modelBuilder.Entity<StoreInfo>(entity =>
        {
            entity.HasKey(e => e.StoreName).HasName("pk__thongtin__859441546ed8d011");
        });

        modelBuilder.Entity<Sysdiagram>(entity =>
        {
            entity.HasKey(e => e.DiagramId).HasName("pk__sysdiagr__c2b05b613ccd2ecd");
        });

        modelBuilder.Entity<WorkSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("pk__giolamvi__3213e83f3dd009f0");

            entity.Property(e => e.ScheduleId).HasDefaultValueSql("nextval('giolamvieccuahang_id_seq'::regclass)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
