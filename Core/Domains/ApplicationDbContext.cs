namespace Remake_Kafka_Example_01.Core.Domains
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TableProduct_V2> TableProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseOracle("User Id=DBTEST1; Password=123456;Data Source=localhost:1521/mypdb;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("DBTEST1")
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<TableProduct_V2>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("TABLE_PRODUCT_V2");

                entity.Property(e => e.Id)
                    .IsUnicode(false)
                    .HasColumnType("VARCHAR2(200)")
                    .HasColumnName("ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
                entity.Property(e => e.Price)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("PRICE");
                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("QUANTITY");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
