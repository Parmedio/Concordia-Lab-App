using BusinessLogic.DTOs.ReportDto;

using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;

using ReportSender.ReportDto;

using static ReportSender.PdfFormatConstants;
namespace ReportSender;

internal class ConcordiaReportBuilder
{
    private static int ticketNumber = 0;
    private static DateOnly currentDate;

    public IEnumerable<ExperimentForReportDto> Experiments { get; internal set; }
    public IEnumerable<ScientistForReportDto> Scientists { get; internal set; }

    public ConcordiaReportBuilder(IEnumerable<ExperimentForReportDto> experiments, IEnumerable<ScientistForReportDto> scientists, int reportNumber)
    {
        Experiments = experiments;
        Scientists = scientists;
        var currentDateTime = DateTime.UtcNow;
        currentDate = new DateOnly(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day);
        ticketNumber = reportNumber;
    }

    internal DocumentBuilder Build()
    {
        DocumentBuilder documentBuilder = DocumentBuilder.New();

        var sectionBuilder = documentBuilder.AddSection();
        sectionBuilder
            .SetOrientation(Orientation)
            .SetMargins(Margins);
        sectionBuilder
            .AddHeaderToBothPages(60, buildHeader);
        ReportInformation(sectionBuilder.AddTable());
        BuildProgressInfo(sectionBuilder);
        IEnumerable<ScientistProductivity> projectedScientists = GetProjectedScientists();

        var HighProductivityScientists = projectedScientists.Where(p => p.Percentage >= 60);
        var MediumProductivityScientists = projectedScientists.Where(p => p.Percentage < 60 && p.Percentage > 30);
        var LowProductivityScientists = projectedScientists.Where(p => p.Percentage <= 30);

        if (HighProductivityScientists.Any())
            ScientistsInfo(sectionBuilder.AddTable(), HighProductivityScientists, highProductivityColor);
        if (MediumProductivityScientists.Any())
            ScientistsInfo(sectionBuilder.AddTable(), MediumProductivityScientists, mediumProductivityColor);
        if (LowProductivityScientists.Any())
            ScientistsInfo(sectionBuilder.AddTable(), LowProductivityScientists, lowProductivityColor);

        return documentBuilder;
    }

    private IEnumerable<ScientistProductivity> GetProjectedScientists()
        => Scientists.Select(p => new ScientistProductivity() { Id = p.Id, Name = p.Name, Percentage = Math.Round((Convert.ToDouble(p.Experiments.Count(p => p.ColumnId == 3 || p.ColumnId == 6)) / (p.Experiments.Count() == 0 ? 1d : Convert.ToDouble(p.Experiments.Count()))) * 100d) }).OrderByDescending(p => p.Percentage);

    private void ScientistsInfo(TableBuilder tableBuilder, IEnumerable<ScientistProductivity> scientistsInfo, Color color)
    {
        tableBuilder
            .SetWidth(XUnit.FromPercent(100f))
            .SetMarginTop(15f)
            .SetRepeatHeaders(true)
            .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.Solid)
            .SetBorderColor(color)
            .AddColumnPercentToTable("Id", 10f)
            .AddColumnPercentToTable("Scientist", 55f)
            .AddColumnPercentToTable("%", 15f)
            .AddColumnPercentToTable(" ", 2f)
            .AddColumnPercentToTable(" ", 2f)
            .AddColumnPercentToTable(" ", 2f)
            .AddColumnPercentToTable(" ", 2f)
            .AddColumnPercentToTable(" ", 2f)
            .AddColumnPercentToTable(" ", 2f)
            .AddColumnPercentToTable(" ", 2f)
            .AddColumnPercentToTable(" ", 2f)
            .AddColumnPercentToTable(" ", 2f)
            .AddColumnPercentToTable(" ", 2f)
            .SetHeaderRowStyleFont(FNT11_B)
            .SetHeaderRowStyleBackColor(color)
            .SetHeaderRowStyleHorizontalAlignment(HorizontalAlignment.Left);

        foreach (var scientist in scientistsInfo)
        {

            var separatorRowBuilder = tableBuilder.AddRow().ApplyStyle(ProgressBarRowStyle);
            for (int i = 1; i <= 13; i++)
            {
                separatorRowBuilder.AddCell("");
            }
            var rowBuilder = tableBuilder.AddRow().ApplyStyle(ScientistProgressRowStyle);

            rowBuilder.AddCell(scientist.Id.ToString());
            rowBuilder.AddCell(scientist.Name);
            rowBuilder.AddCell(scientist.Percentage.ToString());

            int decimalPercentage = (int)scientist.Percentage / 10 + 1 >= 10 ? 10 : (int)scientist.Percentage / 10 + 1;

            for (int counter = 1; counter <= 10; counter++)
            {
                if (counter <= decimalPercentage)
                    rowBuilder.AddCell("").SetBackColor(color);
                else
                    rowBuilder.AddCell("");
            }
        }
    }

    private void buildHeader(RepeatingAreaBuilder builder)
    {
        var tableBuilder = builder.AddTable();
        tableBuilder.SetWidth(XUnit.FromPercent(100f))
            .SetBorder(Stroke.None)
            .AddColumnPercentToTable("", 100f);

        var rowBuilder = tableBuilder.AddRow();
        var cellBuilder = rowBuilder.AddCell()
               .SetVerticalAlignment(VerticalAlignment.Center)
               .SetHorizontalAlignment(HorizontalAlignment.Center)
               .SetPadding(0, 0, 0, 0);
        cellBuilder
            .AddParagraph("Concordia Research Center").SetFont(FNT20B);
        cellBuilder
            .AddParagraph("Status Report").SetFont(FNT20);
    }

    private void ReportInformation(TableBuilder tableBuilder)
    {
        tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetContentRowStyleFont(FNT10)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25);

        var rowBuilder = tableBuilder.AddRow();
        var cellBuilder = rowBuilder.AddCell("Report Number: ");
        cellBuilder
            .SetPadding(0, 3.5f, 0, 8.5f)
            .SetBorderWidth(0, 0, 0, 0.5f)
            .SetBorderStroke(
                Stroke.None, Stroke.None, Stroke.None, Stroke.Solid);
        cellBuilder = rowBuilder.AddCell(ticketNumber.ToString());
        cellBuilder
            .SetHorizontalAlignment(HorizontalAlignment.Right)
            .SetPadding(0, 3.5f, 0, 8.5f)
            .SetBorderWidth(0, 0, 10, 0.5f)
            .SetBorderColor(
                Color.Black, Color.Black, Color.White, Color.Black)
            .SetBorderStroke(
                Stroke.None, Stroke.None, Stroke.Solid, Stroke.Solid);
        cellBuilder = rowBuilder.AddCell("Issued: ");
        cellBuilder
            .SetPadding(0, 3.5f, 0, 8.5f)
            .SetBorderWidth(10, 0, 0, 0.5f)
            .SetBorderColor(
                Color.White, Color.Black, Color.Black, Color.Black)
            .SetBorderStroke(
                Stroke.Solid, Stroke.None, Stroke.None, Stroke.Solid);
        cellBuilder = rowBuilder.AddCell(currentDate.ToString("yyyy MM dd"));
        cellBuilder
            .SetHorizontalAlignment(HorizontalAlignment.Right)
            .SetPadding(0, 3.5f, 0, 8.5f)
            .SetBorderWidth(0, 0, 0, 0.5f)
            .SetBorderStroke(
                Stroke.None, Stroke.None, Stroke.None, Stroke.Solid);
    }

    private void BuildProgressInfo(SectionBuilder sectionBuilder)
    {
        sectionBuilder.AddParagraph("Progress")
            .SetFont(FNT11_B).SetMarginTop(22);
        sectionBuilder.AddLine(PageWidth, 2f, Stroke.Solid);
        FillProgressInfoTable(sectionBuilder.AddTable());
        sectionBuilder.AddLine(PageWidth, 1.5f, Stroke.Solid);
    }

    private void FillProgressInfoTable(TableBuilder tableBuilder)
    {
        tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 33)
                .AddColumnPercentToTable("", 34)
                .AddColumnPercentToTable("", 33);

        var rowBuilder = tableBuilder.AddRow();
        rowBuilder.ApplyStyle(InfoTableHeaderStyle);

        foreach (string headName in ColumnNames)
        {
            rowBuilder.AddCell(headName);
        }

        rowBuilder = tableBuilder.AddRow();
        rowBuilder.ApplyStyle(InfoTableContentStyle);

        foreach (var index in Enumerable.Range(1, 3))
        {
            rowBuilder.AddCell(GetCountOfColumn(index));
        }
    }

    private string GetCountOfColumn(int columnId)
        => Experiments.Where(p => p.ColumnId == columnId || p.ColumnId == columnId + 3).Count().ToString();

}
