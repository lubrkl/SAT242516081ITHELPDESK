using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ITHelpDesk.Models;

namespace ITHelpDesk.Services;

public interface IReportService
{
    byte[] GenerateTicketReport(IEnumerable<Ticket> tickets);
    byte[] GenerateEmployeeReport(IEnumerable<Employee> employees);
}

public class ReportService : IReportService
{
    public ReportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateTicketReport(IEnumerable<Ticket> tickets)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Header().Text("Destek Talepleri Raporu").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.ConstantColumn(40);
                        c.RelativeColumn(3);
                        c.RelativeColumn(2);
                        c.RelativeColumn(1);
                        c.RelativeColumn(1);
                    });

                    table.Header(h =>
                    {
                        h.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                        h.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Başlık").Bold();
                        h.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Talep Eden").Bold();
                        h.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Öncelik").Bold();
                        h.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Durum").Bold();
                    });

                    foreach (var t in tickets)
                    {
                        table.Cell().Padding(5).Text(t.TicketId.ToString());
                        table.Cell().Padding(5).Text(t.Title);
                        table.Cell().Padding(5).Text(t.RequesterName ?? "");
                        table.Cell().Padding(5).Text(t.PriorityName ?? "");
                        table.Cell().Padding(5).Text(t.StatusName ?? "");
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Sayfa ");
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    public byte[] GenerateEmployeeReport(IEnumerable<Employee> employees)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Header().Text("Çalışanlar Raporu").FontSize(20).Bold().AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.ConstantColumn(40);
                        c.RelativeColumn(2);
                        c.RelativeColumn(2);
                        c.RelativeColumn(2);
                    });

                    table.Header(h =>
                    {
                        h.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                        h.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Ad Soyad").Bold();
                        h.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Email").Bold();
                        h.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Departman").Bold();
                    });

                    foreach (var e in employees)
                    {
                        table.Cell().Padding(5).Text(e.EmployeeId.ToString());
                        table.Cell().Padding(5).Text(e.FullName);
                        table.Cell().Padding(5).Text(e.Email);
                        table.Cell().Padding(5).Text(e.DepartmentName ?? "");
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Sayfa ");
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();
    }
}