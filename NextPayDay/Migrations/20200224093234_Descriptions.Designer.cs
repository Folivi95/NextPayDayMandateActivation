﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NextPayDay.Data;

namespace NextPayDay.Migrations
{
    [DbContext(typeof(OTPDbContext))]
    [Migration("20200224093234_Descriptions")]
    partial class Descriptions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NextPayDay.Model.OTPActivation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CardNumber");

                    b.Property<string>("DescriptionOne");

                    b.Property<string>("DescriptionTwo");

                    b.Property<string>("MandateId");

                    b.Property<string>("OTP");

                    b.Property<string>("RemitaTransRef");

                    b.Property<string>("RequestId");

                    b.Property<string>("StatusMessage");

                    b.HasKey("ID");

                    b.ToTable("OTPActivations");
                });
#pragma warning restore 612, 618
        }
    }
}