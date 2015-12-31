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

namespace MongoDbRepository.Implementation.FileUnzipper
{
    public class FileUnzipper : IUnzipper
    {
        public byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                int size = gzip.Count();
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        Console.Write(".");
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    var byteData = memory.ToArray();

                   
                    return byteData;
                }
            }
        }
    }
}
