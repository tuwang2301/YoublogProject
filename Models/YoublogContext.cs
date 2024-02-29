using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace YoublogProject.Models;

public partial class YoublogContext : DbContext
{
    public YoublogContext()
    {
    }

    public YoublogContext(DbContextOptions<YoublogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<FriendRequest> FriendRequests { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Reaction> Reactions { get; set; }

    public virtual DbSet<Share> Shares { get; set; }

    public virtual DbSet<Story> Stories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-3130NPR;Initial Catalog=youblog;User ID=sa;Password=sa;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAAACE423C5");

            entity.HasIndex(e => e.ContentId, "UQ__Comments__2907A87FEC33014F").IsUnique();

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.ContentId).HasColumnName("ContentID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ContentNavigation).WithOne(p => p.Comment)
                .HasForeignKey<Comment>(d => d.ContentId)
                .HasConstraintName("FK__Comments__Conten__6166761E");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Comments__PostID__634EBE90");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Comments__UserID__625A9A57");
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__Contents__2907A87ECD52F2C5");

            entity.Property(e => e.ContentId).HasColumnName("ContentID");
            entity.Property(e => e.ContentUrl).HasColumnName("ContentURL");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.FriendshipId).HasName("PK__Friends__4D531A7443DDFEAC");

            entity.HasIndex(e => new { e.UserId1, e.UserId2 }, "UC_Friendship").IsUnique();

            entity.Property(e => e.FriendshipId).HasColumnName("FriendshipID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId1).HasColumnName("UserID1");
            entity.Property(e => e.UserId2).HasColumnName("UserID2");

            entity.HasOne(d => d.UserId1Navigation).WithMany(p => p.FriendUserId1Navigations)
                .HasForeignKey(d => d.UserId1)
                .HasConstraintName("FK__Friends__UserID1__671F4F74");

            entity.HasOne(d => d.UserId2Navigation).WithMany(p => p.FriendUserId2Navigations)
                .HasForeignKey(d => d.UserId2)
                .HasConstraintName("FK__Friends__UserID2__681373AD");
        });

        modelBuilder.Entity<FriendRequest>(entity =>
        {
            entity.HasKey(e => e.FriendRequestId).HasName("PK__FriendRe__0CCD2A59A4AA788D");

            entity.ToTable("FriendRequest");

            entity.HasIndex(e => new { e.FromUserId, e.ToUserId }, "UC_FriendRequest").IsUnique();

            entity.Property(e => e.FriendRequestId).HasColumnName("FriendRequestID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.FromUser).WithMany(p => p.FriendRequestFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .HasConstraintName("FK__FriendReq__FromU__6BE40491");

            entity.HasOne(d => d.ToUser).WithMany(p => p.FriendRequestToUsers)
                .HasForeignKey(d => d.ToUserId)
                .HasConstraintName("FK__FriendReq__ToUse__6CD828CA");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Posts__AA12603834465A9C");

            entity.HasIndex(e => e.ContentId, "UQ__Posts__2907A87FF6110B00").IsUnique();

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.ContentId).HasColumnName("ContentID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PrivacyMode).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Content).WithOne(p => p.Post)
                .HasForeignKey<Post>(d => d.ContentId)
                .HasConstraintName("FK__Posts__ContentID__540C7B00");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Posts__UserID__55009F39");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(e => e.ReactionId).HasName("PK__Reaction__46DDF9D402EDCF3C");

            entity.HasIndex(e => new { e.UserId, e.PostId }, "UC_UserPost").IsUnique();

            entity.Property(e => e.ReactionId).HasColumnName("ReactionID");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.Type).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Post).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Reactions__PostI__5D95E53A");

            entity.HasOne(d => d.User).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reactions__UserI__5CA1C101");
        });

        modelBuilder.Entity<Share>(entity =>
        {
            entity.HasKey(e => e.ShareId).HasName("PK__Shares__D32A3F8E1987B8DD");

            entity.Property(e => e.ShareId).HasColumnName("ShareID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Post).WithMany(p => p.Shares)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Shares__PostID__6FB49575");

            entity.HasOne(d => d.User).WithMany(p => p.Shares)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Shares__UserID__70A8B9AE");
        });

        modelBuilder.Entity<Story>(entity =>
        {
            entity.HasKey(e => e.StoryId).HasName("PK__Stories__3E82C0281C7ECCEB");

            entity.Property(e => e.StoryId).HasColumnName("StoryID");
            entity.Property(e => e.ContentId).HasColumnName("ContentID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.PrivacyMode).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Content).WithMany(p => p.Stories)
                .HasForeignKey(d => d.ContentId)
                .HasConstraintName("FK__Stories__Content__58D1301D");

            entity.HasOne(d => d.User).WithMany(p => p.Stories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Stories__UserID__57DD0BE4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACF717FE72");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
