using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PDFHelper
{
    public class PDFHelper
    {
        private string _pdfPath;
        public PdfReader _pdfReader;

        public PDFHelper(string pdfPath)
        {
            _pdfPath = pdfPath;
            _pdfReader = new PdfReader(_pdfPath);
        }

        /// <summary>
        /// Get all the field names of the PDF
        /// </summary>
        /// <returns></returns>
        public List<string> GetFieldNames()
        {
            List<string> items = new List<string>();
            try
            {
                foreach (KeyValuePair<string, AcroFields.Item> field in _pdfReader.AcroFields.Fields)
                    items.Add(field.Key.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return items;
        }

        /// <summary>
        /// Fillout form fields in PDF and save pdf to the disk
        /// </summary>
        /// <param name="items"></param>
        /// <param name="outputPdfPath"></param>
        public void FillFields(Dictionary<string, string> items, string outputPdfPath)
        {
            try
            {
                PdfStamper pdfStamper = new PdfStamper(_pdfReader, new FileStream(outputPdfPath, FileMode.Create));
                AcroFields pdfFormFields = pdfStamper.AcroFields;

                foreach (KeyValuePair<string, string> item in items)
                {
                    pdfFormFields.SetField(item.Key, item.Value);
                }
                pdfStamper.FormFlattening = true;  // set to 'true' to prevent editing
                pdfStamper.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string CombineMultiplePDFs(string[] filePaths, string outputPdfPath)
        {
            try
            {
                Document document = new Document();
                PdfCopy writer = new PdfCopy(document, new FileStream(outputPdfPath, FileMode.Create));

                if (writer != null)
                {
                    document.Open();

                    foreach (string fileName in filePaths)
                    {
                        PdfReader reader = new PdfReader(fileName);
                        reader.ConsolidateNamedDestinations();

                        for (int j = 1; j <= reader.NumberOfPages; j++)
                        {
                            PdfImportedPage page = writer.GetImportedPage(reader, j);
                            writer.AddPage(page);
                        }

                        PRAcroForm form = reader.AcroForm;
                        if (form != null)
                        {
                            writer.AddDocument(reader);
                        }

                        reader.Close();
                    }

                    writer.Close();
                    document.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return outputPdfPath;
        }

        public void ExtractPage(string outputPdfPath, int pageNumber, string password = "")
        {
            try
            {
                PdfReader reader = _pdfReader;
                Document document = new Document(reader.GetPageSizeWithRotation(pageNumber));
                Stream os = new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create);
                PdfCopy pdfCopyProvider = new PdfCopy(document, os);

                document.Open();
                PdfImportedPage importedPage = pdfCopyProvider.GetImportedPage(reader, pageNumber);
                pdfCopyProvider.AddPage(importedPage);
                document.Close();

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExtractPages(string outputPdfPath, int startPage, int endPage)
        {
            try
            {
                PdfReader reader = _pdfReader;
                Document sourceDocument = new Document(reader.GetPageSizeWithRotation(startPage));
                PdfCopy pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

                sourceDocument.Open();
                for (int i = startPage; i <= endPage; i++)
                {
                    PdfImportedPage importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }
                sourceDocument.Close();

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExtractPages(string outputPdfPath, int[] extractThesePages)
        {
            try
            {
                PdfReader reader = _pdfReader;
                Document sourceDocument = new Document(reader.GetPageSizeWithRotation(extractThesePages[0]));
                PdfCopy pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

                sourceDocument.Open();
                foreach (int pageNumber in extractThesePages)
                {
                    PdfImportedPage importedPage = pdfCopyProvider.GetImportedPage(reader, pageNumber);
                    pdfCopyProvider.AddPage(importedPage);
                }
                sourceDocument.Close();

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ExtractText(int pageNumber)
        {
            PdfReader reader = _pdfReader;
            string text = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, pageNumber, new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy());
            return text;
        }

        public static bool CreateBlankPDF(string pdfPath)
        {
            bool created = false;
            try
            {
                Document document = new Document(PageSize.LETTER, 20, 15, 25, 25);
                PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));
                document.Open();
                document.Add(new Paragraph());
                document.Close();
                created = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return created;
        }

        public static bool CreatePDF(string text, string outputPdfPath)
        {
            bool created = false;
            try
            {
                Document document = new Document(PageSize.LETTER, 20, 15, 25, 25);
                PdfWriter.GetInstance(document, new FileStream(outputPdfPath, FileMode.Create));
                document.Open();
                document.Add(new Paragraph(text));
                document.Close();
                created = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return created;
        }

        public int CountPages()
        {
            return _pdfReader.NumberOfPages;
        }

        public void DeleteFirstPage(string outputPdfPath)
        {
            ExtractPages(outputPdfPath, 2, _pdfReader.NumberOfPages);
        }

        public void DeleteLastPage(string outputPdfPath)
        {
            ExtractPages(outputPdfPath, 1, _pdfReader.NumberOfPages - 1);
        }

        public byte[] ConvertToBytes()
        {
            return ConvertToBytes(_pdfPath);
        }

        public static byte[] ConvertToBytes(string fullPDFPath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenRead(fullPDFPath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        public MemoryStream CreatePDFInMemory(string text)
        {
            Document document = new Document();
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                document.Add(new Paragraph(text));
                writer.CloseStream = false;
                document.Close();
                //Get the pointer to the beginning of the stream. 
                memoryStream.Position = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return memoryStream;
        }


        public MemoryStream ConvertPDFToMemory()
        {
            MemoryStream ms = null;

            try
            {
                PdfReader reader = _pdfReader;
                using (ms = new MemoryStream())
                {
                    PdfStamper stamper = new PdfStamper(reader, ms);
                    stamper.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ms;
        }

        public MemoryStream ConvertPDFToMemory(Dictionary<string, string> items)
        {
            MemoryStream ms = null;

            try
            {
                PdfReader reader = _pdfReader;
                using (ms = new MemoryStream())
                {
                    PdfStamper stamper = new PdfStamper(reader, ms);
                    AcroFields pdfFormFields = stamper.AcroFields;
                    foreach (KeyValuePair<string, string> item in items)
                    {
                        pdfFormFields.SetField(item.Key, item.Value);
                    }
                    stamper.FormFlattening = true;
                    stamper.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ms;
        }

        public void Download()
        {
            byte[] pdf = ConvertToBytes(_pdfPath);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=file.pdf");
            HttpContext.Current.Response.OutputStream.Write(pdf, 0, pdf.Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.OutputStream.Close();
            HttpContext.Current.Response.End();
        }

        public static void DownloadAsPDF(MemoryStream ms)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=file.pdf");
            HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.OutputStream.Close();
            HttpContext.Current.Response.End();
            ms.Close();
        }
    }
}