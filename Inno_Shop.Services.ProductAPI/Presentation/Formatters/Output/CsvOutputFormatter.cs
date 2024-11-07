using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace Inno_Shop.Services.ProductAPI.Presentation.Formatters.Output;


public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (typeof(ProductDto).IsAssignableFrom(type) ||
        typeof(IEnumerable<ProductDto>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }
        return false;
    }

    public override async Task WriteResponseBodyAsync(
        OutputFormatterWriteContext context,
        Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();
        if (context.Object is IEnumerable<ProductDto> products)
        {
            foreach (var product in products)
            {
                FormatCsv(buffer, product);
            }
        }
        else if (context.Object is ProductDto product)
        {
            FormatCsv(buffer, product);
        }

        await response.WriteAsync(buffer.ToString());
    }
    private static void FormatCsv(StringBuilder buffer, ProductDto product)
    {
        buffer.AppendLine($"{product.ProductID},\"{product.Name},\"{product.Price}\",\"{product.CategoryName}\",\"{product.Availability}\",\"{product.CreationDate}\",\"{product.Description}\",\"{product.ImageUrl}\"");
    }
}
