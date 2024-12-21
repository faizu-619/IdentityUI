﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SSRD.IdentityUI.Core.Infrastructure.Data;

#nullable disable

namespace SSRD.IdentityUI.EntityFrameworkCore.MySql.Migrations
{
    [DbContext(typeof(IdentityDbContext))]
    [Migration("20241220225256_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SSRD.Audit.Data.AuditEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("ActionType")
                        .HasColumnType("int");

                    b.Property<string>("AppVersion")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GroupIdentifier")
                        .HasColumnType("longtext");

                    b.Property<string>("Host")
                        .HasColumnType("longtext");

                    b.Property<string>("Metadata")
                        .HasColumnType("longtext");

                    b.Property<string>("ObjectIdentifier")
                        .HasColumnType("longtext");

                    b.Property<string>("ObjectMetadata")
                        .HasColumnType("longtext");

                    b.Property<string>("ObjectType")
                        .HasColumnType("longtext");

                    b.Property<string>("RemoteIp")
                        .HasColumnType("longtext");

                    b.Property<string>("ResourceName")
                        .HasColumnType("longtext");

                    b.Property<string>("SubjectIdentifier")
                        .HasColumnType("longtext");

                    b.Property<string>("SubjectMetadata")
                        .HasColumnType("longtext");

                    b.Property<int>("SubjectType")
                        .HasColumnType("int");

                    b.Property<string>("TraceIdentifier")
                        .HasColumnType("longtext");

                    b.Property<string>("UserAgent")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Audit");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.EmailEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Group.GroupAttributeEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("Key");

                    b.ToTable("GroupAttributes");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Group.GroupEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Group.GroupUserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("FirstName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("LastName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<int>("TwoFactor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleClaimEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.UserClaimEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.UserLoginEntity", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.UserRoleEntity", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.UserTokenEntity", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.InviteEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("ExpiresAt")
                        .HasColumnType("datetime");

                    b.Property<string>("GroupId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("GroupRoleId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("GroupRoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("Invite");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.PermissionEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.PermissionRoleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("PermissionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("PermissionRole");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.RoleAssignmentEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("CanAssigneRoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("CanAssigneRoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleAssignments");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.SessionEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<int?>("EndType")
                        .HasColumnType("int");

                    b.Property<string>("Ip")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("LastAccess")
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.User.UserAttributeEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("Key");

                    b.HasIndex("UserId");

                    b.ToTable("UserAttributes");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.UserImageEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<byte[]>("BlobImage")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTimeOffset?>("_CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset?>("_ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserImage");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Group.GroupAttributeEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Group.GroupEntity", "Group")
                        .WithMany("GroupAttributes")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Group.GroupUserEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Group.GroupEntity", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleClaimEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.UserClaimEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.UserLoginEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.UserRoleEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.UserTokenEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.InviteEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Group.GroupEntity", "Group")
                        .WithMany("Invites")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", "GroupRole")
                        .WithMany()
                        .HasForeignKey("GroupRoleId");

                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.Navigation("Group");

                    b.Navigation("GroupRole");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.PermissionRoleEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.PermissionEntity", "Permission")
                        .WithMany("Roles")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.RoleAssignmentEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", "CanAssigneRole")
                        .WithMany("CanBeAssignedBy")
                        .HasForeignKey("CanAssigneRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", "Role")
                        .WithMany("CanAssigne")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CanAssigneRole");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.SessionEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.User.UserAttributeEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", "User")
                        .WithMany("Attributes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.UserImageEntity", b =>
                {
                    b.HasOne("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", "User")
                        .WithOne("UserImage")
                        .HasForeignKey("SSRD.IdentityUI.Core.Data.Entities.UserImageEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Group.GroupEntity", b =>
                {
                    b.Navigation("GroupAttributes");

                    b.Navigation("Invites");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.AppUserEntity", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("Claims");

                    b.Navigation("Groups");

                    b.Navigation("Logins");

                    b.Navigation("Sessions");

                    b.Navigation("Tokens");

                    b.Navigation("UserImage");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.Identity.RoleEntity", b =>
                {
                    b.Navigation("CanAssigne");

                    b.Navigation("CanBeAssignedBy");

                    b.Navigation("Permissions");

                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("SSRD.IdentityUI.Core.Data.Entities.PermissionEntity", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}