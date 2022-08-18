using AppCore.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings
{
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.HasKey(lbda => lbda.Id);

            builder.Property(lbda => lbda.Logradouro)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(lbda => lbda.Numero)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(lbda => lbda.Complemento)
                .HasColumnType("varchar(250)");

            builder.Property(lbda => lbda.Cep)
                .IsRequired()
                .HasColumnType("varchar(8)");

            builder.Property(lbda => lbda.Bairro)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(lbda => lbda.Cidade)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(lbda => lbda.Estado)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.ToTable("Enderecos");
        }
    }
}
