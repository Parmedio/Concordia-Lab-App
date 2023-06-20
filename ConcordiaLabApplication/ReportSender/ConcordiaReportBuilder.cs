using BusinessLogic.DTOs.ReportDto;

using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;

using ReportSender.ReportDto;

using System.Globalization;

namespace ReportSender;

internal class ConcordiaReportBuilder
{
    internal static readonly CultureInfo DocumentLocale
         = new CultureInfo("en-US");
    internal const PageOrientation Orientation
        = PageOrientation.Portrait;
    internal static readonly Box Margins = new Box(29, 20, 29, 20);
    internal static readonly XUnit PageWidth =
        (PredefinedSizeBuilder.ToSize(PaperSize.A4).Width -
            (Margins.Left + Margins.Right));

    internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7f);
    internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9f);
    internal static readonly FontBuilder FNT9B_G =
        Fonts.Helvetica(9f).SetBold().SetColor(Color.Gray);
    internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
    internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11f);
    internal static readonly FontBuilder FNT11_B =
        Fonts.Helvetica(11f).SetBold();
    internal static readonly FontBuilder FNT20 = Fonts.Helvetica(20f);
    internal static readonly FontBuilder FNT20B =
        Fonts.Helvetica(20f).SetBold();

    private static readonly string[] ColumnNames = {
                "To Do", "In Progress", "Completed"
            };
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
        sectionBuilder.SetOrientation(Orientation)
            .SetMargins(Margins);
        sectionBuilder.AddHeaderToBothPages(60, buildHeader);
        ReportInformation(sectionBuilder.AddTable());
        BuildProgressInfo(sectionBuilder);

        IEnumerable<ScientistProductivity> projectedScientists = Scientists.Select(p => new ScientistProductivity() { Id = p.Id, Name = p.Name, Percentage = Math.Round(Convert.ToDouble(p.Experiments.Count(p => p.ColumnId == 3 || p.ColumnId == 6)) / Convert.ToDouble(p.Experiments.Count())) });
        var HighProductivityScientists = projectedScientists.Where(p => p.Percentage >= 60);
        var MediumProductivityScientists = projectedScientists.Where(p => p.Percentage < 60 && p.Percentage > 30);
        var LowProductivityScientists = projectedScientists.Where(p => p.Percentage <= 30);
        if (HighProductivityScientists.Any())
            ScientistsInfo(sectionBuilder.AddTable(), HighProductivityScientists, Color.Green);
        if (MediumProductivityScientists.Any())
            ScientistsInfo(sectionBuilder.AddTable(), MediumProductivityScientists, Color.Yellow);
        if (LowProductivityScientists.Any())
            ScientistsInfo(sectionBuilder.AddTable(), LowProductivityScientists, Color.Red);
        return documentBuilder;
    }

    private void ScientistsInfo(TableBuilder tableBuilder, IEnumerable<ScientistProductivity> scientistsInfo, Color color)
    {
        tableBuilder
            .SetRepeatHeaders(true)
            .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.Solid)
            .SetBorderColor(color)
            .AddColumnPercentToTable("Id", 10f)
            .AddColumnPercentToTable("Scientist", 65f)
            .AddColumnPercentToTable("%", 25)
            .SetHeaderRowStyleFont(FNT11_B)
            .SetHeaderRowStyleBackColor(color)
            .SetHeaderRowStyleHorizontalAlignment(HorizontalAlignment.Left);

        foreach (var scientist in scientistsInfo)
        {
            var rowBuilder = tableBuilder.AddRow().ApplyStyle(StyleBuilder.New()
                    .SetFont(FNT9)
                    .SetPaddingTop(2.8f)
                    .SetPaddingBottom(4.2f));
            rowBuilder.AddCell(scientist.Id.ToString());
            rowBuilder.AddCell(scientist.Name);
            rowBuilder.AddCell(scientist.Percentage.ToString());
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
        rowBuilder
            .ApplyStyle(
                StyleBuilder.New()
                    .SetFont(FNT9B_G)
                    .SetPaddingTop(4.8f)
                    .SetPaddingBottom(8.2f)
            );

        foreach (string headName in ColumnNames)
        {
            rowBuilder.AddCell(headName);
        }
        rowBuilder = tableBuilder.AddRow();
        rowBuilder
            .ApplyStyle(
                StyleBuilder.New()
                    .SetFont(FNT10)
                    .SetPaddingTop(3.5f)
                    .SetPaddingBottom(7.5f)
            );
        foreach (var index in Enumerable.Range(1, 3))
        {
            rowBuilder.AddCell(GetCountOfColumn(index));
        }
    }

    private string GetCountOfColumn(int columnId)
        => Experiments.Where(p => p.ColumnId == columnId || p.ColumnId == columnId + 3).Count().ToString();

}
