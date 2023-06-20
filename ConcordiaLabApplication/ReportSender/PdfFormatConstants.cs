using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;

using System.Globalization;

namespace ReportSender;

internal static class PdfFormatConstants
{

    internal static readonly CultureInfo DocumentLocale = new CultureInfo("en-US");
    internal const PageOrientation Orientation = PageOrientation.Portrait;
    internal static readonly Box Margins = new Box(29, 20, 29, 20);
    internal static readonly XUnit PageWidth =
        (PredefinedSizeBuilder.ToSize(PaperSize.A4).Width - (Margins.Left + Margins.Right));
    internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7f);
    internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9f);
    internal static readonly FontBuilder FNT9B_G = Fonts.Helvetica(9f).SetBold().SetColor(Color.Gray);
    internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
    internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11f);
    internal static readonly FontBuilder FNT11_B = Fonts.Helvetica(11f).SetBold();
    internal static readonly FontBuilder FNT20 = Fonts.Helvetica(20f);
    internal static readonly FontBuilder FNT20B = Fonts.Helvetica(20f).SetBold();
    internal static readonly Color highProductivityColor = Color.FromRgba(0.753, 1, 0.773, 0.39f);
    internal static readonly Color mediumProductivityColor = Color.FromRgba(1, 0.816, 0.62, 0.49);
    internal static readonly Color lowProductivityColor = Color.FromRgba(1, 0.663, 0.663, 0.663);
    internal static readonly string[] ColumnNames = {
                "To Do", "In Progress", "Completed"
            };

    internal static StyleBuilder ProgressBarRowStyle = StyleBuilder.New()
                .SetPaddingBottom(1f)
                .SetPaddingTop(1f)
                .SetBorderBottom(XUnit.FromPercent(0.3f), Stroke.Solid, Color.Black);

    internal static StyleBuilder ScientistProgressRowStyle = StyleBuilder.New()
                    .SetFont(FNT9)
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .SetPaddingTop(2.8f)
                    .SetMarginTop(2f)
                    .SetMarginBottom(2f)
                    .SetBorderBottom(XUnit.FromPercent(0.3f), Stroke.Solid, Color.Black)
                    .SetPaddingBottom(4.2f);

    internal static StyleBuilder InfoTableHeaderStyle = StyleBuilder.New()
                      .SetFont(FNT9B_G)
                      .SetPaddingTop(4.8f)
                      .SetHorizontalAlignment(HorizontalAlignment.Center)
                      .SetPaddingBottom(8.2f);

    internal static StyleBuilder InfoTableContentStyle = StyleBuilder.New()
                    .SetFont(FNT10)
                    .SetHorizontalAlignment(HorizontalAlignment.Center)
                    .SetPaddingTop(3.5f)
                    .SetPaddingBottom(7.5f);
}
