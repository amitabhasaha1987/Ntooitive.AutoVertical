using Repository.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class UtilityClass
    {
        public static string CreateQueryString(string minPrice, string maxPrice, string Mileage, string FromYear, string ToYear, string UsedNewType,
                                               List<Make> Makes, List<Model> Models, string VehicleType, string ExteriorColor,
                                               string InteriorColor, string make, string model)
        {
            try
            {
                string queryString = null;
                if (minPrice != "0" && maxPrice != "0")
                {
                    queryString = "'Price': {'$gte': " + minPrice + ",'$lte': " + maxPrice + "}";
                }
                else if (minPrice != "0" && maxPrice == "0")
                {
                    queryString = "'Price': {'$gte': " + minPrice + "}";
                }
                else if (minPrice == "0" && maxPrice != "0")
                {
                    queryString = "'Price': {'$gt':0,'$lte': " + maxPrice + "}";
                }


                if (!string.IsNullOrEmpty(Mileage))
                {
                    if (Mileage != "0")
                    {
                        queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Mileage': {'$lte': " + Mileage + "}" : "'Mileage': {'$lte': " + Mileage + "}";
                    }
                }


                if (!string.IsNullOrEmpty(FromYear) && !string.IsNullOrEmpty(ToYear))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Year': {'$gte': " + FromYear + ",'$lte': " + ToYear + "}" : "'Year': {'$gte': " + FromYear + ",'$lte': " + ToYear + "}";
                }
                else if (!string.IsNullOrEmpty(FromYear) && string.IsNullOrEmpty(ToYear))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Year': {'$gte': " + FromYear + "}" : "'Year': {'$gte': " + FromYear + "}";
                }
                else if (string.IsNullOrEmpty(FromYear) && !string.IsNullOrEmpty(ToYear))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Year': {'$lte': " + maxPrice + "}" : "'Year': {'$lte': " + maxPrice + "}";
                }


                if (!string.IsNullOrEmpty(UsedNewType))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Condition': '" + UsedNewType + "'" : "'Condition':'" + UsedNewType + "'";
                }


                if (Makes != null)
                {
                    string make1 = null;
                    foreach (var item in Makes)
                    {
                        make1 = string.IsNullOrEmpty(make1) ? "'" + item.MakersName + "'" : make1 + "," + "'" + item.MakersName + "'";
                    }
                    make1 = string.IsNullOrEmpty(make1) ? null : "Make : {$in :[" + make1 + "]}";
                    queryString = string.IsNullOrEmpty(make1) ? queryString : (!string.IsNullOrEmpty(queryString)) ? queryString + "," + make1 : make1;
                }


                if (Models != null)
                {
                    string model1 = null;
                    foreach (var item in Models)
                    {
                        model1 = item.ModelsName == "0" ? null : (string.IsNullOrEmpty(model1) ? "'" + item.ModelsName + "'" : model1 + "," + "'" + item.ModelsName + "'");
                    }
                    model1 = string.IsNullOrEmpty(model1) ? null : "Model : {$in :[" + model1 + "]}";
                    queryString = string.IsNullOrEmpty(model1) ? queryString : (!string.IsNullOrEmpty(queryString)) ? queryString + "," + model1 : model1;
                }


                if (!string.IsNullOrEmpty(VehicleType) && VehicleType != "0")
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'VehicleStyle':{$in :['" + VehicleType + "']}" : "'VehicleStyle':{$in :['" + VehicleType + "']}";
                }


                if (ExteriorColor != "0" && !string.IsNullOrEmpty(ExteriorColor))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "ExteriorColor : {$in :['" + ExteriorColor + "']}" : "ExteriorColor : {$in :['" + ExteriorColor + "']}";
                }


                if (InteriorColor != "0" && !string.IsNullOrEmpty(InteriorColor))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "InteriorColor : {$in :['" + InteriorColor + "']}" : "InteriorColor : {$in :['" + InteriorColor + "']}";
                }


                if (!string.IsNullOrEmpty(make))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Make':'" + make + "'" : "'Make':'" + make + "'";
                }

                if (!string.IsNullOrEmpty(model) && model != "0")
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Model':'" + model + "'" : "'Model':'" + model + "'";
                }

                if (!string.IsNullOrEmpty(queryString))
                {
                    if (!queryString.Contains("Price"))
                    {
                        queryString = queryString + "," + "'Price': {'$gt': 0}";
                    }

                }
                else
                {
                    queryString = "'Price': {'$gt': 0}";
                }


                return queryString;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string CreateQueryStringForDealer(string minPrice, string maxPrice, string Mileage, string FromYear, string ToYear, string UsedNewType, List<UserSrch> Dealers,
                                              List<Make> Makes, List<Model> Models, string VehicleType, string ExteriorColor,
                                              string InteriorColor, string dealersrch, string make, string model)
        {
            try
            {
                string queryString = null;
                if (minPrice != "0" && maxPrice != "0")
                {
                    queryString = "'Price': {'$gte': " + minPrice + ",'$lte': " + maxPrice + "}";
                }
                else if (minPrice != "0" && maxPrice == "0")
                {
                    queryString = "'Price': {'$gte': " + minPrice + "}";
                }
                else if (minPrice == "0" && maxPrice != "0")
                {
                    queryString = "'Price': {'$gt':0,'$lte': " + maxPrice + "}";
                }


                if (!string.IsNullOrEmpty(Mileage))
                {
                    if (Mileage != "0")
                    {
                        queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Mileage': {'$lte': " + Mileage + "}" : "'Mileage': {'$lte': " + Mileage + "}";
                    }
                }


                if (!string.IsNullOrEmpty(FromYear) && !string.IsNullOrEmpty(ToYear))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Year': {'$gte': " + FromYear + ",'$lte': " + ToYear + "}" : "'Year': {'$gte': " + FromYear + ",'$lte': " + ToYear + "}";
                }
                else if (!string.IsNullOrEmpty(FromYear) && string.IsNullOrEmpty(ToYear))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Year': {'$gte': " + FromYear + "}" : "'Year': {'$gte': " + FromYear + "}";
                }
                else if (string.IsNullOrEmpty(FromYear) && !string.IsNullOrEmpty(ToYear))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Year': {'$lte': " + maxPrice + "}" : "'Year': {'$lte': " + maxPrice + "}";
                }


                if (!string.IsNullOrEmpty(UsedNewType))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Condition': '" + UsedNewType + "'" : "'Condition':'" + UsedNewType + "'";
                }


                if (Dealers != null)
                {
                    string dealer1 = null;
                    foreach (var item in Dealers)
                    {

                        dealer1 = string.IsNullOrEmpty(dealer1) ? "'" + item.DealerName.Replace("'", "\\'") + "'" : dealer1 + "," + "'" + item.DealerName.Replace("'", "\\'") + "'";
                    }
                    dealer1 = string.IsNullOrEmpty(dealer1) ? null : "DealerName : {$in :[" + dealer1 + "]}";
                    queryString = string.IsNullOrEmpty(dealer1) ? queryString : (!string.IsNullOrEmpty(queryString)) ? queryString + "," + dealer1 : dealer1;
                }

                if (Makes != null)
                {
                    string make1 = null;
                    foreach (var item in Makes)
                    {
                        make1 = string.IsNullOrEmpty(make1) ? "'" + item.MakersName + "'" : make1 + "," + "'" + item.MakersName + "'";
                    }
                    make1 = string.IsNullOrEmpty(make1) ? null : "Make : {$in :[" + make1 + "]}";
                    queryString = string.IsNullOrEmpty(make1) ? queryString : (!string.IsNullOrEmpty(queryString)) ? queryString + "," + make1 : make1;
                }


                if (Models != null)
                {
                    string model1 = null;
                    foreach (var item in Models)
                    {
                        model1 = item.ModelsName == "0" ? null : (string.IsNullOrEmpty(model1) ? "'" + item.ModelsName + "'" : model1 + "," + "'" + item.ModelsName + "'");
                    }
                    model1 = string.IsNullOrEmpty(model1) ? null : "Model : {$in :[" + model1 + "]}";
                    queryString = string.IsNullOrEmpty(model1) ? queryString : (!string.IsNullOrEmpty(queryString)) ? queryString + "," + model1 : model1;
                }


                if (!string.IsNullOrEmpty(VehicleType) && VehicleType != "0")
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'VehicleStyle':{$in :['" + VehicleType + "']}" : "'VehicleStyle':{$in :['" + VehicleType + "']}";
                }


                if (ExteriorColor != "0" && !string.IsNullOrEmpty(ExteriorColor))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "ExteriorColor : {$in :['" + ExteriorColor + "']}" : "ExteriorColor : {$in :['" + ExteriorColor + "']}";
                }


                if (InteriorColor != "0" && !string.IsNullOrEmpty(InteriorColor))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "InteriorColor : {$in :['" + InteriorColor + "']}" : "InteriorColor : {$in :['" + InteriorColor + "']}";
                }


                if (!string.IsNullOrEmpty(make))
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Make':'" + make + "'" : "'Make':'" + make + "'";
                }

                if (!string.IsNullOrEmpty(model) && model != "0")
                {
                    queryString = (!string.IsNullOrEmpty(queryString)) ? queryString + "," + "'Model':'" + model + "'" : "'Model':'" + model + "'";
                }
                if (!string.IsNullOrEmpty(queryString))
                {
                    if (!queryString.Contains("Price"))
                    {
                        queryString = queryString + "," + "'Price': {'$gt': 0}";
                    }

                }
                else
                {
                    queryString = "'Price': {'$gt': 0}";
                }
                return queryString;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetString(string make)
        {
            try
            {
                string finalMake = null;
                if (!string.IsNullOrEmpty(make))
                {
                    if (!make.Contains(','))
                    {
                        finalMake = "['" + make + "']";
                    }
                    else
                    {
                        string[] a = make.Split(',');
                        for (int i = 0; i < a.Length; i++)
                        {
                            finalMake = string.IsNullOrEmpty(finalMake) ? "'" + a[i] + "'" : finalMake + ",'" + a[i] + "'";
                        }
                        finalMake = "[" + finalMake + "]";
                    }
                }
                return string.IsNullOrEmpty(finalMake) ? null : "$in:" + finalMake;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetUniqueKey()
        {
            const int maxSize = 8;
            var a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var chars = a.ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            const int size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void WriteLogException(Exception ex, string str)
        {
            System.IO.TextWriter sw = null;
            var logpath = ConfigurationManager.AppSettings["logpath"];
            // string logpath = "E:\\Auto\\AutoVerticals\\Ntooitive.AutoVertical\\DataImportConsole\\DumpData";
            sw = System.IO.File.AppendText(logpath + "\\" + "schedulelog.txt");
            sw.WriteLine(ex.InnerException + "........" + ex.Message + "................." + str);
            sw.Flush();
            sw.Close();
            sw = null;
        }
        public static void WriteLog(string str)
        {
            System.IO.TextWriter sw = null;
            var logpath = ConfigurationManager.AppSettings["logpath"];
            // string logpath = "E:\\Auto\\AutoVerticals\\Ntooitive.AutoVertical\\DataImportConsole\\DumpData";
            sw = System.IO.File.AppendText(logpath + "\\" + "schedulelog.txt");
            sw.WriteLine(str);
            sw.Flush();
            sw.Close();
            sw = null;
        }
    }
}
