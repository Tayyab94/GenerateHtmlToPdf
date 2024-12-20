using GenerateHTML_ToPdf.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharp;
using PdfSharp.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace GenerateHTML_ToPdf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFController : ControllerBase
    {
        [HttpGet("FeesStructure")]
        public async Task<ActionResult> GeneratePdf(PdfRequest req)
        {
            var data = new PdfSharpCore.Pdf.PdfDocument();
            string htmlContent = "<div style = 'margin: 20px auto; heigth:1000px; max-width: 600px; padding: 20px; border: 1px solid #ccc; background-color: #FFFFFF; font-family: Arial, sans-serif;' >";
            htmlContent += "<div style = 'margin-bottom: 20px; text-align: center;'>";
            htmlContent += "<img src = 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcROnYPD5QO8ZJvPQt8ClnJNPXduCeX89dSOxA&usqp=CAU' alt = 'School Logo' style = 'max-width: 100px; margin-bottom: 10px;' >";
            htmlContent += "</div>";
            htmlContent += "<p style = 'margin: 0;' >Jobin School Management</p>";
            htmlContent += "<p style = 'margin: 0;' > 123 School Street, Sample Street</p>";
            htmlContent += "<p style = 'margin: 0;' > Phone: 123 - 456 - 7890 </p>";
            htmlContent += "<p style = 'margin: 0;' > Tamilnadu </p>";
            htmlContent += "<div style = 'text-align: center; margin-bottom: 20px;'>";
            htmlContent += "<h1> Fees Structure </h1>";
            htmlContent += "</div>";
            htmlContent += "<h3> StudentDetails:</h3>";
            htmlContent += "<p> Name:" + req.Name + "</p>";
            htmlContent += "<p> STD:" + req.Std + "</p>";
            htmlContent += "<table style = 'width: 100%; border-collapse: collapse;'>";
            htmlContent += "<thead>";
            htmlContent += "<tr>";
            htmlContent += "<th style = 'padding: 8px; text-align: left; border-bottom: 1px solid #ddd;' > Fee Description </th>";
            htmlContent += "<th style = 'padding: 8px; text-align: left; border-bottom: 1px solid #ddd;' > Amount(INR) </th>";
            htmlContent += "</tr><hr/>";
            htmlContent += "</thead>";
            htmlContent += "<tbody>";

            decimal totalAmount = 0;

            if(req.fees is not null && req.fees.Count>0)
            {
                req.fees.ForEach(x =>
                {
                    htmlContent += "<tr>";
                    htmlContent += "<td style = 'padding: 8px; text-align: left; border-bottom: 1px solid #ddd;' >" + x.FeesDescription + " </td>";
                    htmlContent += "<td style = 'padding: 8px; text-align: left; border-bottom: 1px solid #ddd;' >Rs " + x.Amount + "/- </td>";
                    htmlContent += "</tr>";

                    if(decimal.TryParse(x.Amount, out decimal feeAmount))
                    {
                        totalAmount += feeAmount;
                    }
                });

                htmlContent += "</tbody>";
                htmlContent += "<tfoot>";
                htmlContent += "<tr>";
                htmlContent += "<td style = 'padding: 8px; text-align: right; font-weight: bold;'> Total:</td>";
                htmlContent += "<td style = 'padding: 8px; text-align: left; border-top: 1px solid #ddd;' >Rs" + totalAmount + "/- </td>";
                htmlContent += "</tr>";
                htmlContent += "</tfoot>";
            }

            htmlContent += "</table>";
            htmlContent += "</div>";

            double pageWidth = 80 * 2.83465; // 80mm to points
            double pageHeight = 300 * 2.83465; // Example: 300mm height (adjust as needed)
            var pageSize = new PdfSharpCore.Drawing.XSize(pageWidth, pageHeight);
            PdfGenerator.AddPdfPages(data, htmlContent,PdfSharpCore.PageSize.B5
                );
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                data.Save(ms);
                response = ms.ToArray();
            }
            string fileName = "FeesStructure" + req.Name + ".pdf";
            return File(response, "application/pdf", fileName);
        }
    }
}
