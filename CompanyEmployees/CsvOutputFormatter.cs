using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DTO;

namespace CompanyEmployees;

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
        if (typeof(CompanyDto).IsAssignableFrom(type) ||
            typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type) ||
            typeof(EmployeeDto).IsAssignableFrom(type) ||
            typeof(IEnumerable<EmployeeDto>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }

        return false;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        switch (context.Object)
        {
            case CompanyDto companyDto:
                FormatCsv(buffer, companyDto);
                break;
            case IEnumerable<CompanyDto> companies:
                foreach (var company in companies)
                {
                    FormatCsv(buffer, company);
                }
                break;
            case EmployeeDto employee:
                FormatCsv(buffer, employee);
                break;
            case IEnumerable<EmployeeDto> employees:
                foreach (var employee in employees)
                {
                    FormatCsv(buffer, employee);
                }
                break;
            default:
                throw new ArgumentException("Unknown argument", nameof(context.Object));
        }
        
        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, CompanyDto company) =>
        buffer.AppendLine($"{company.Id}, \"{company.Name}\", \"{company.FullAddress}\"");
    
    private static void FormatCsv(StringBuilder buffer, EmployeeDto employee) =>
        buffer.AppendLine($"{employee.Id}, \"{employee.Name}\", \"{employee.Age}\", \"{employee.Position}\"");
}