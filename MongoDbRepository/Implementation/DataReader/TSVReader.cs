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
    public class TSVReader : IReader
    {


        public List<Auto> ReadFile(string filePath)
        {
            throw new NotImplementedException();
        }


        public List<Auto> ReadFile(byte[] byteArray)
        {
            Stream stream = new MemoryStream(byteArray);
            List<Auto> rows = new List<Auto>();

            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                int _lineNumber = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (_lineNumber != 0)
                    {

                        line = line.Replace("\"", string.Empty).Trim();

                        string[] a = line.Split(new char[] { (char)9 });


                        string str1 = a[0];

                        Auto row = new Auto();
                        if (a.Count() == 21)
                        {
                            row = new Auto()
                            {
                                DealerName = Convert.ToString(a[0]),
                                Make = Convert.ToString(a[1]),
                                Model = Convert.ToString(a[2]),
                                Trim = Convert.ToString(a[3]),
                                Year = string.IsNullOrEmpty(Convert.ToString(a[4])) ? 0 : Convert.ToInt16(a[4]),
                                Vin = Convert.ToString(a[5]),
                                //Category = Convert.ToString(a[6]),
                                Mileage = string.IsNullOrEmpty(Convert.ToString(a[7])) ? 0.0 : Convert.ToDouble(a[7]),
                                Price = string.IsNullOrEmpty(Convert.ToString(a[8])) ? 0.0 : Convert.ToDouble(a[8]),
                                Condition = Convert.ToString(a[9]),
                                InteriorColor = Convert.ToString(a[10]),
                                ExteriorColor = Convert.ToString(a[11]),
                                Description = Convert.ToString(a[12]),
                                DealerAddress = Convert.ToString(a[13]),
                                DealerCity = Convert.ToString(a[14]),
                                DealerState = Convert.ToString(a[15]),
                                DealerZip = Convert.ToString(a[16]),
                                DealerEmail = Convert.ToString(a[17]),
                                DealerPhone = Convert.ToString(a[18]),
                                PhotosUrl = (string.IsNullOrEmpty(Convert.ToString(a[19])) ? null : Convert.ToString(a[19]).Split(',').ToList()),
                                StockNumber = Convert.ToString(a[20]),
                                Transmission = null,
                                DealershipCity= Convert.ToString(a[14]),
                                DealershipPhone = Convert.ToString(a[18]),
                                DealershipState = Convert.ToString(a[15]),
                                DealershipZip = Convert.ToString(a[16])

                            };
                        }
                        else
                        {
                            row = new Auto()
                            {
                                DealerName = Convert.ToString(a[0]),
                                Make = Convert.ToString(a[1]),
                                Model = Convert.ToString(a[2]),
                                Trim = Convert.ToString(a[3]),
                                Year = string.IsNullOrEmpty(Convert.ToString(a[4])) ? 0 : Convert.ToInt16(a[4]),
                                Vin = Convert.ToString(a[5]),
                                //Category = Convert.ToString(a[6]),
                                Mileage = string.IsNullOrEmpty(Convert.ToString(a[7])) ? 0.0 : Convert.ToDouble(a[7]),// Convert.ToDouble(a[7]),
                                Price = string.IsNullOrEmpty(Convert.ToString(a[8])) ? 0.0 : Convert.ToDouble(a[8]),
                                Condition = Convert.ToString(a[9]),
                                InteriorColor = Convert.ToString(a[10]),
                                ExteriorColor = Convert.ToString(a[11]),
                                Description = Convert.ToString(a[12]),
                                DealerAddress = Convert.ToString(a[13]),
                                DealerCity = Convert.ToString(a[14]),
                                DealerState = Convert.ToString(a[15]),
                                DealerZip = Convert.ToString(a[16]),
                                DealerEmail = Convert.ToString(a[17]),
                                DealerPhone = Convert.ToString(a[18]),
                                PhotosUrl = (string.IsNullOrEmpty(Convert.ToString(a[19])) ? null : Convert.ToString(a[19]).Split(',').ToList()),
                                StockNumber = Convert.ToString(a[20]),
                                Transmission = Convert.ToString(a[21]),
                                DealershipCity = Convert.ToString(a[14]),
                                DealershipPhone = Convert.ToString(a[18]),
                                DealershipState = Convert.ToString(a[15]),
                                DealershipZip = Convert.ToString(a[16])

                            };
                        }
                        rows.Add(row);


                    }
                    _lineNumber = _lineNumber + 1;
                }
            }

            return rows;
        }
    }
}
