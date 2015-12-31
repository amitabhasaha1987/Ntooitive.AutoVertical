using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Repository.Interfaces;
using System.Data.OleDb;
using Excel;
using Repository.Models;
using System.Net;
using System.ComponentModel;
using System.IO.Compression;


namespace MongoDbRepository.Implementation.TextFileCoverter
{
    public class TextToExcelCoverter : ITextToExcelConverter
    {
        public void ConvertTxtToExcel(string textPath, string excelPath)
        {
            Microsoft.Office.Interop.Excel.Application m_objExcel = null;
            Microsoft.Office.Interop.Excel.Workbooks m_objBooks = null;
            Microsoft.Office.Interop.Excel._Workbook m_objBook = null;
            Microsoft.Office.Interop.Excel.Sheets m_objSheets = null;
            Microsoft.Office.Interop.Excel._Worksheet m_objSheet = null;
            Microsoft.Office.Interop.Excel.Range m_objRange = null;
            Microsoft.Office.Interop.Excel.Font m_objFont = null;
            Microsoft.Office.Interop.Excel.QueryTables m_objQryTables = null;
            Microsoft.Office.Interop.Excel._QueryTable m_objQryTable = null;

            // Frequenty-used variable for optional arguments.
            object m_objOpt = System.Reflection.Missing.Value;

            // Paths used by the sample code for accessing and storing data.
            //object m_strSampleFolder = "E:\\Auto\\AutoVerticals\\Ntooitive.AutoVertical\\";

            m_objExcel = new Microsoft.Office.Interop.Excel.Application();
            m_objBooks = (Microsoft.Office.Interop.Excel.Workbooks)m_objExcel.Workbooks;
            //m_objBooks.OpenText(m_strSampleFolder + "sdutfeed_20151031.txt", Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, 1,
            //  Microsoft.Office.Interop.Excel.XlTextParsingType.xlDelimited, Microsoft.Office.Interop.Excel.XlTextQualifier.xlTextQualifierDoubleQuote,
            //  false, true, false, false, false, false, m_objOpt, m_objOpt,
            //  m_objOpt, m_objOpt, m_objOpt);
            m_objBooks.OpenText(textPath, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, 1,
             Microsoft.Office.Interop.Excel.XlTextParsingType.xlDelimited, Microsoft.Office.Interop.Excel.XlTextQualifier.xlTextQualifierDoubleQuote,
             false, true, false, false, false, false, m_objOpt, m_objOpt,
             m_objOpt, m_objOpt, m_objOpt);

            m_objBook = m_objExcel.ActiveWorkbook;

            // Save the text file in the typical workbook format and quit Excel.
            //m_objBook.SaveAs(m_strSampleFolder + "sdutfeed_20151031.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
            //m_objOpt, m_objOpt, m_objOpt, m_objOpt, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, m_objOpt, m_objOpt,
            //m_objOpt, m_objOpt);
            //m_objBook.Close(false, m_objOpt, m_objOpt);
            //m_objExcel.Quit();
            m_objBook.SaveAs(excelPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
           m_objOpt, m_objOpt, m_objOpt, m_objOpt, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, m_objOpt, m_objOpt,
           m_objOpt, m_objOpt);
            m_objBook.Close(false, m_objOpt, m_objOpt);
            m_objExcel.Quit();
        }
    }
}
