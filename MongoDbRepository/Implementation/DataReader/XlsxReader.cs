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
using Repository.Models.Admin.Auto;

namespace MongoDbRepository.Implementation.DataReader
{
    public class XlsxReader : IReader
    {

        public List<Auto> ReadFile(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            var result = excelReader.AsDataSet().Tables[0];
            List<Auto> rows = (from DataRow row in result.Rows
                               select new Auto
                               {
                                   DealerId = Convert.ToString(row["dealer_id"]),
                                   Make = Convert.ToString(row["make"]),
                                   Model = Convert.ToString(row["model"]),
                                   Trim = Convert.ToString(row["sub_model"]),
                                   Year = Convert.ToInt16(string.IsNullOrEmpty(Convert.ToString(row["year"])) ? "0" : Convert.ToString(row["year"])),
                                   Vin = Convert.ToString(row["vin"]),
                                   Category = Convert.ToString(row["category"]),
                                   Mileage = Convert.ToDouble(string.IsNullOrEmpty(Convert.ToString(row["mileage"])) ? "0" : Convert.ToString(row["mileage"])),
                                   Price = Convert.ToDouble(string.IsNullOrEmpty(Convert.ToString(row["price"])) ? "0" : Convert.ToString(row["price"])),
                                   Condition = Convert.ToString(row["condition"]),
                                   InteriorColor = Convert.ToString(row["interior_color"]),
                                   ExteriorColor = Convert.ToString(row["exterior_color"]),
                                   Description = Convert.ToString(row["description"]),
                                   DealershipPhone = Convert.ToString(row["dealership_phone"]),
                                   DealershipCity = Convert.ToString(row["dealership_city"]),
                                   DealershipState = Convert.ToString(row["dealership_state"]),
                                   DealershipZip = Convert.ToString(row["dealership_zip"]),
                                   PhotosUrl = (string.IsNullOrEmpty(Convert.ToString(row["photo_urls"])) ? null : Convert.ToString(row["photo_urls"]).Split(',').ToList()),
                                   StockNumber = Convert.ToString(row["stock_number"]),
                                   Transmission = Convert.ToString(row["transmission"]),
                                   FranchiseID = Convert.ToString(row["Dealer/franchise ID"]),
                                   DealerName = Convert.ToString(row["Dealer Name"]),
                                   DealerAddress = Convert.ToString(row["Dealer Address"]),
                                   DealerCity = Convert.ToString(row["Dealer City"]),
                                   DealerState = Convert.ToString(row["Dealer State"]),
                                   DealerZip = Convert.ToString(row["Dealer ZIP"]),
                                   DealerPhone = Convert.ToString(row["Dealer phone"]),
                                   DealerEmail = Convert.ToString(row["Dealer email"]),
                                   LeadEmailAddress = Convert.ToString(row["lead email address"]),
                                   CCEmailAddress = Convert.ToString(row["cc email address"]),
                                   LeadType = Convert.ToString(row["Lead Type"])
                               }).ToList();

            excelReader.Close();

            return rows;
        }


        public List<Auto> ReadFile(byte[] byteArray)
        {
            throw new NotImplementedException();
        }
    }
}
