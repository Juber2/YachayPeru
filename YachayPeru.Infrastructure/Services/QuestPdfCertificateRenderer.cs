using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using YachayPeru.Application.Abstractions.Services;

namespace YachayPeru.Infrastructure.Services
{
    public class QuestPdfCertificateRenderer : ICertificatePdfRenderer
    {
        public byte[] Render(CertificatePdfData data)
        {
            var qrBytes = data.IncludeQrCode && !string.IsNullOrWhiteSpace(data.QrContent)
                ? GenerateQrPng(data.QrContent)
                : null;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(data.Orientation == "vertical" ? PageSizes.A4 : PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontFamily(MapFont(data.FontFamily)));

                    page.Content()
                        .Border(MapBorderWidth(data.BorderWidth))
                        .BorderColor(data.PrimaryColor)
                        .Padding(24)
                        .Column(col =>
                        {
                            if (data.ShowLogo && data.LogoBytes is not null)
                                col.Item().AlignCenter().Height(60).Image(data.LogoBytes).FitHeight();

                            col.Item().PaddingTop(10).AlignCenter().Text(data.MainTitle).FontSize(28).Bold().FontColor(data.PrimaryColor);

                            if (!string.IsNullOrWhiteSpace(data.Subtitle))
                                col.Item().AlignCenter().Text(data.Subtitle).FontSize(16).FontColor(data.SecondaryColor);

                            if (!string.IsNullOrWhiteSpace(data.BodyText))
                                col.Item().PaddingTop(20).AlignCenter().Text(data.BodyText).FontSize(12);

                            col.Item().PaddingTop(14).AlignCenter().Text(data.StudentName).FontSize(20).Bold().FontColor(data.AccentColor);
                            col.Item().AlignCenter().Text($"{data.RetoTitle} — {data.RegionTitle}").FontSize(14);

                            col.Item().PaddingTop(16).Column(infoCol =>
                            {
                                if (data.IncludeCompletionDate)
                                    infoCol.Item().Text($"Fecha de finalización: {data.CompletionDate:dd/MM/yyyy}");
                                if (data.IncludeScore)
                                    infoCol.Item().Text($"Puntaje: {data.Score:0.##}");
                                if (data.IncludeCertificateId)
                                    infoCol.Item().Text($"N.° de certificado: {data.Prefix}{data.CertificateId}");
                                if (data.IncludeInstructor && !string.IsNullOrWhiteSpace(data.SignerName))
                                    infoCol.Item().Text($"Instructor: {data.SignerName}");
                            });

                            if (!string.IsNullOrWhiteSpace(data.FooterText))
                                col.Item().PaddingTop(20).AlignCenter().Text(data.FooterText).FontSize(10);

                            col.Item().PaddingTop(24).Row(row =>
                            {
                                row.RelativeItem().Column(signCol =>
                                {
                                    if (data.SignatureBytes is not null)
                                        signCol.Item().Height(40).Image(data.SignatureBytes).FitHeight();

                                    if (!string.IsNullOrWhiteSpace(data.SignerName))
                                    {
                                        signCol.Item().Text(data.SignerName).Bold();
                                        if (!string.IsNullOrWhiteSpace(data.SignerTitle))
                                            signCol.Item().Text(data.SignerTitle).FontSize(9);
                                    }
                                });

                                if (data.ShowSeal && data.SealBytes is not null)
                                    row.RelativeItem().AlignRight().Height(60).Image(data.SealBytes).FitHeight();

                                if (qrBytes is not null)
                                    row.RelativeItem().AlignRight().Height(60).Image(qrBytes).FitHeight();
                            });
                        });
                });
            });

            return document.GeneratePdf();
        }

        private static byte[] GenerateQrPng(string content)
        {
            using var generator = new QRCodeGenerator();
            using var qrData = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var pngQr = new PngByteQRCode(qrData);
            return pngQr.GetGraphic(10);
        }

        private static string MapFont(string fontFamily) => fontFamily switch
        {
            "serif" => "Times New Roman",
            "script" => "Comic Sans MS",
            _ => "Arial"
        };

        private static float MapBorderWidth(string borderWidth) => borderWidth switch
        {
            "fino" => 1f,
            "grueso" => 4f,
            _ => 2f
        };
    }
}
