using System;
using System.Diagnostics;
using System.IO;
using CsvHelper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using org.mariuszgromada.math.mxparser;

namespace C2CPrint
{
    internal class Program
    {

        private static readonly BaseFont BaseFont = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        static void Main(string[] args)
        {
            FormulaValidator.ValidateContractFormula();

            if(args.Length == 0) return;

            var fileId = args.Length == 0 ? "bee1c2bf-512b-4a06-a6b9-27b22e1069c6" : args[0];

            if (!File.Exists($"{fileId}.csv")) return;


            var watch = new Stopwatch();

            watch.Start();

            try
            {

                using (var fs = new FileStream($"{fileId}.pdf", FileMode.Create))
                {
                    var document = new Document(PageSize.A4, 25, 25, 30, 1);
                    var writer = PdfWriter.GetInstance(document, fs);

                    // Add meta information to the document
                    document.AddAuthor("C2C Cheque Printing");
                    document.AddCreator("C2C Cheque Printing using iTestSharp");
                    document.AddKeywords("Payout");
                    document.AddSubject("Payout for Month");
                    document.AddTitle("Payouts");

                    // Open the document to enable you to write to the document
                    document.Open();

                    // Makes it possible to add text to a specific place in the document using 
                    // a X & Y placement syntax.
                    var cb = writer.DirectContent;

                    using (var textReader = File.OpenText($"{fileId}.csv"))
                    {
                        var csv = new CsvReader(textReader);
                        while (csv.Read())
                        {
                            var checkNum = csv.GetField<string>(0);
                            var amount = csv.GetField<string>(1);
                            var date = csv.GetField<string>(2);
                            var payFrom = csv.GetField<string>(3);
                            var payTo = csv.GetField<string>(4);
                            var payAccountName = csv.GetField<string>(5);
                            var addressLine1 = csv.GetField<string>(6);
                            var addressLine2 = csv.GetField<string>(7);
                            var city = csv.GetField<string>(8);
                            var state = csv.GetField<string>(9);
                            var zipCode = csv.GetField<string>(10);

                            // Add a logo to the invoice
                            var png = Image.GetInstance(Directory.GetCurrentDirectory() + "\\logo.png");
                            png.ScaleAbsolute(60, 60);
                            png.SetAbsolutePosition(100, 250);
                            cb.AddImage(png);

                            // First we must activate writing
                            cb.BeginText();

                            //Logo Text
                            WriteText(cb, "Financial Marketing", 160, 290, BaseFont, 10);
                            WriteText(cb, "Concepts, Inc.", 160, 270, BaseFont, 10);
                            WriteText(cb, "1102 North A1A Suite 202 · Ponte Vedra, FL 32081", 100, 240, BaseFont, 10);

                            //Cheque area
                            WriteText(cb, date, 100, 740, BaseFont, 10);
                            WriteText(cb, payAccountName, 100, 720, BaseFont, 10);
                            WriteText(cb, amount, 100, 700, BaseFont, 10);

                            //Footer
                            WriteText(cb, payAccountName, 100, 180, BaseFont, 10);
                            WriteText(cb, addressLine1, 100, 160, BaseFont, 10);
                            WriteText(cb, addressLine2, 100, 140, BaseFont, 10);
                            WriteText(cb, $"{city}, {state} - {zipCode}", 100, 120, BaseFont, 10);

                            // We need to end the writing before we change the page
                            cb.EndText();

                            // Make the page break
                            document.NewPage();
                        }
                    }

                    // Close the document, the writer and the filestream!
                    document.Close();
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"pdf created in {elapsedMs} ms");
            Console.ReadLine();
        }

        // This is the method writing text to the content byte
        private static void WriteText(PdfContentByte cb, string text, int xPos, int yPos, BaseFont font, int size)
        {
            cb.SetFontAndSize(font, size);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text, xPos, yPos, 0);
        }
    }
}
