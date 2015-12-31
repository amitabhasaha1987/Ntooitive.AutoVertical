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

namespace MongoDbRepository.Implementation.FileDownloader
{
    public class FileDownloader : IDownloader
    {
        public string DownloadFile(string remoteFtpPath, string localDestinationPath)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(remoteFtpPath);
            ftpRequest.Credentials = new NetworkCredential("sandiegouniontribune", "EfVvqD7ZVV");
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            ftpRequest.KeepAlive = false;
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            StreamReader sr = new StreamReader(ftpResponse.GetResponseStream());
            string line = sr.ReadLine();
            string fName = string.Empty;

            string day = DateTime.Now.Day.ToString().PadLeft(2, '0');
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();

            string yearStrngVal = year + month + day;
            //string yearStrngVal = "20151214";
            string filename = "sdutfeed";
            while (!string.IsNullOrEmpty(line))
            {
                if (line.Contains(yearStrngVal) && line.Contains(filename))
                {
                    fName = filename + "_" + yearStrngVal + ".txt.gz";
                    break;
                }
                else
                {
                    line = sr.ReadLine();
                }
            }
            ftpResponse.Close();
            sr.Close();

            if (!string.IsNullOrEmpty(fName))
            {
                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential("sandiegouniontribune", "EfVvqD7ZVV");
                    //"_" + lstTimeStamp.Max().ToString().PadLeft(4, '0') + 
                    byte[] fileData = request.DownloadData(remoteFtpPath + fName);

                    using (FileStream file = File.Create(localDestinationPath + fName))
                    {
                        file.Write(fileData, 0, fileData.Length);
                        file.Close();
                    }
                }
            }
            return fName;
        }

        //public void DownloadFile(string remoteFtpPath, string localDestinationPath)
        //{
        //    using (WebClient request = new WebClient())
        //    {
        //        //var ftphost = "ftp.vast.com/";
        //        //var ftpfilepath = "/outbound/sdutfeed_20151031.txt.gz";
        //        //string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

        //        request.Credentials = new NetworkCredential("sandiegouniontribune", "EfVvqD7ZVV");
        //        byte[] fileData = request.DownloadData(remoteFtpPath);

        //        using (FileStream file = File.Create(localDestinationPath))
        //        {
        //            file.Write(fileData, 0, fileData.Length);
        //            file.Close();
        //        }
        //    }

        //}

        public string DownloadFileForClassifiedFeed(string remoteFtpPath, string localDestinationPath)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(remoteFtpPath);
            ftpRequest.Credentials = new NetworkCredential("ntootive", "ADITFILES1");
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            ftpRequest.KeepAlive = false;
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            StreamReader sr = new StreamReader(ftpResponse.GetResponseStream());
            string line = sr.ReadLine();

            string day = DateTime.Now.Day.ToString().PadLeft(2, '0');
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();

            //string yearStrngVal = month + day + year;
            string yearStrngVal = "12152015";
            List<int> lstTimeStamp = new List<int>();
            string filename = "motorslinersdut";
            while (!string.IsNullOrEmpty(line))
            {
                if (line.Contains(yearStrngVal) && line.Contains(filename))
                {
                    string a = line;
                    line = sr.ReadLine();
                    string[] fileName = a.Split(' ');
                    int count = fileName.Count() - 1;
                    string[] var = fileName[count].Split('_');
                    string myTime = System.Text.RegularExpressions.Regex.Replace(var[1], @"\D", "");
                    lstTimeStamp.Add(Convert.ToInt16(myTime));
                }
                else
                {
                    line = sr.ReadLine();
                }
            }
            ftpResponse.Close();
            sr.Close();
            string fName = string.Empty;
            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("ntootive", "ADITFILES1");
                fName = filename + yearStrngVal + "_" + lstTimeStamp.Max().ToString().PadLeft(4, '0') + ".xml";
                byte[] fileData = request.DownloadData(remoteFtpPath + fName);

                using (FileStream file = File.Create(localDestinationPath + fName))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
            }
            return fName;
        }
    }
}
