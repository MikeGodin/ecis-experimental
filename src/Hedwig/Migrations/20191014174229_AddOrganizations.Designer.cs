// <auto-generated />
using System;
using Hedwig.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace hedwig.Migrations
{
    [DbContext(typeof(HedwigContext))]
    [Migration("20191014174229_AddOrganizations")]
    partial class AddOrganizations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hedwig.Models.Child", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("AmericanIndianOrAlaskaNative");

                    b.Property<bool?>("Asian");

                    b.Property<DateTime>("Birthdate");

                    b.Property<bool?>("BlackOrAfricanAmerican");

                    b.Property<int?>("FamilyId");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<int>("Gender");

                    b.Property<bool?>("HispanicOrLatinxEthnicity");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<string>("MiddleName")
                        .HasMaxLength(35);

                    b.Property<bool?>("NativeHawaiianOrPacificIslander");

                    b.Property<string>("Suffix")
                        .HasMaxLength(10);

                    b.Property<bool?>("White");

                    b.HasKey("Id");

                    b.HasIndex("FamilyId");

                    b.ToTable("Child");
                });

            modelBuilder.Entity("Hedwig.Models.Enrollment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("ChildId");

                    b.Property<DateTime>("Entry");

                    b.Property<DateTime?>("Exit");

                    b.Property<int>("SiteId");

                    b.HasKey("Id");

                    b.HasIndex("ChildId");

                    b.HasIndex("SiteId");

                    b.ToTable("Enrollment");
                });

            modelBuilder.Entity("Hedwig.Models.Family", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CaseNumber");

                    b.HasKey("Id");

                    b.ToTable("Family");
                });

            modelBuilder.Entity("Hedwig.Models.FamilyDetermination", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Determined");

                    b.Property<int>("FamilyId");

                    b.Property<decimal>("Income")
                        .HasColumnType("decimal(14,2)");

                    b.Property<int>("NumberOfPeople");

                    b.HasKey("Id");

                    b.HasIndex("FamilyId");

                    b.ToTable("FamilyDetermination");
                });

            modelBuilder.Entity("Hedwig.Models.Funding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EnrollmentId");

                    b.Property<DateTime>("Entry");

                    b.Property<DateTime?>("Exit");

                    b.Property<int>("Source");

                    b.HasKey("Id");

                    b.HasIndex("EnrollmentId");

                    b.ToTable("Funding");
                });

            modelBuilder.Entity("Hedwig.Models.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("Hedwig.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Permissions");

                    b.HasDiscriminator<string>("Type").HasValue("Permission");
                });

            modelBuilder.Entity("Hedwig.Models.Site", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int?>("OrganizationId");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Site");
                });

            modelBuilder.Entity("Hedwig.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<string>("MiddleName")
                        .HasMaxLength(35);

                    b.Property<string>("Suffix")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Hedwig.Models.OrganizationPermission", b =>
                {
                    b.HasBaseType("Hedwig.Models.Permission");

                    b.Property<int>("OrganizationId");

                    b.HasIndex("OrganizationId");

                    b.HasDiscriminator().HasValue("Organization");
                });

            modelBuilder.Entity("Hedwig.Models.SitePermission", b =>
                {
                    b.HasBaseType("Hedwig.Models.Permission");

                    b.Property<int>("SiteId");

                    b.HasIndex("SiteId");

                    b.HasDiscriminator().HasValue("Site");
                });

            modelBuilder.Entity("Hedwig.Models.Child", b =>
                {
                    b.HasOne("Hedwig.Models.Family", "Family")
                        .WithMany("Children")
                        .HasForeignKey("FamilyId");
                });

            modelBuilder.Entity("Hedwig.Models.Enrollment", b =>
                {
                    b.HasOne("Hedwig.Models.Child", "Child")
                        .WithMany("Enrollments")
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Hedwig.Models.Site", "Site")
                        .WithMany("Enrollments")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Hedwig.Models.FamilyDetermination", b =>
                {
                    b.HasOne("Hedwig.Models.Family", "Family")
                        .WithMany("Determinations")
                        .HasForeignKey("FamilyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Hedwig.Models.Funding", b =>
                {
                    b.HasOne("Hedwig.Models.Enrollment", "Enrollment")
                        .WithMany("Fundings")
                        .HasForeignKey("EnrollmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Hedwig.Models.Permission", b =>
                {
                    b.HasOne("Hedwig.Models.User", "User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Hedwig.Models.Site", b =>
                {
                    b.HasOne("Hedwig.Models.Organization", "Organization")
                        .WithMany("Sites")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("Hedwig.Models.OrganizationPermission", b =>
                {
                    b.HasOne("Hedwig.Models.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Hedwig.Models.SitePermission", b =>
                {
                    b.HasOne("Hedwig.Models.Site", "Site")
                        .WithMany()
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
