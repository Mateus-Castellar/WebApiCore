using AppCore.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(lbda => lbda.Id);

        builder.Property(lbda => lbda.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(lbda => lbda.Descricao)
            .IsRequired()
            .HasColumnType("varchar(1000)");

        builder.Property(lbda => lbda.Imagem)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.ToTable("Produtos");
    }
}
