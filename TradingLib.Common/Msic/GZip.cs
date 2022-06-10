using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.IO;
using ZLib;

namespace TradingLib.Common
{
    public class ZlibNet
    {
        public static byte[] Compress(byte[] inData)
        {
            //压缩
            //bytes = new Byte[fs.Length];
            //fs.Read(bytes, 0, bytes.Length);

            //fs.Close();
            //fs.Dispose();

            //output = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);


            //ZOutputStream zOut = new ZOutputStream(output, zlibConst.Z_DEFAULT_COMPRESSION);
            //zOut.Write(bytes, 0, bytes.Length);
            //zOut.finish();
            //zOut.Close();

            byte[] outData = null;
            Stream outStream = new MemoryStream();
            ZOutputStream outZStream = new ZOutputStream(outStream, zlibConst.Z_DEFAULT_COMPRESSION);
            //Stream inStream = new MemoryStream(inData);
            try
            {
                outZStream.Write(inData, 0, inData.Length);
                outZStream.finish();
                outZStream.Flush();
                //CopyStream(inStream, outZStream);
                outStream.Seek(0, SeekOrigin.Begin);        // 将outStream设置到头，以便从头读取数据
                int outLength = (int)outStream.Length;
                outData = new byte[outLength];
                int k = outStream.Read(outData, 0, outLength);        // outStream此时包括了解压缩后的数据，将其放入到字节数组desBuffer中
            }
            finally
            {
                outZStream.Close();
                outStream.Close();
                //inStream.Close();
            }
            return outData;

        }

        public static byte[] Decompress(byte[] inData)
        {
            byte[] outData = null;
            Stream outStream = new MemoryStream();
            ZOutputStream outZStream = new ZOutputStream(outStream);
            Stream inStream = new MemoryStream(inData, 0, inData.Length);
            try
            {
                CopyStream(inStream, outZStream);
                outStream.Seek(0, SeekOrigin.Begin);        // 将outStream设置到头，以便从头读取数据
                int outLength = (int)outStream.Length;
                outData = new byte[outLength];
                int k = outStream.Read(outData, 0, outLength);        // outStream此时包括了解压缩后的数据，将其放入到字节数组desBuffer中
            }
            finally
            {
                outZStream.Close();
                outStream.Close();
                inStream.Close();
            }
            return outData;
        }

        private static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            int i = (int)input.Length;
            byte[] buffer = new byte[i];
            int len = 0;
            try
            {
                while ((len = input.Read(buffer, 0, i)) > 0)
                {
                    output.Write(buffer, 0, len);           // output如果为ZOutputStream，则此处进行实际的压缩 或 解压缩
                }
                output.Flush();
            }
            catch (Exception ex)
            {


            }
        }

    }
    public class GZip
    {

        public static string Compress(string text)
        {
            // convert text to bytes
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            // get a stream
            MemoryStream ms = new MemoryStream();
            // get ready to zip up our stream
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                // compress the data into our buffer
                zip.Write(buffer, 0, buffer.Length);
            }
            // reset our position in compressed stream to the start
            ms.Position = 0;
            // get the compressed data
            byte[] compressed = ms.ToArray();
            ms.Read(compressed, 0, compressed.Length);
            // prepare final data with header that indicates length
            byte[] gzBuffer = new byte[compressed.Length + 4];
            //copy compressed data 4 bytes from start of final header
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            // copy header to first 4 bytes
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            // convert back to string and return
            return Convert.ToBase64String(gzBuffer);
        }

        public static string Uncompress(string compressedText)
        {
            // get string as bytes
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            // prepare stream to do uncompression
            MemoryStream ms = new MemoryStream();
            // get the length of compressed data
            int msgLength = BitConverter.ToInt32(gzBuffer, 0);
            // uncompress everything besides the header
            ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
            // prepare final buffer for just uncompressed data
            byte[] buffer = new byte[msgLength];
            // reset our position in stream since we're starting over
            ms.Position = 0;
            // unzip the data through stream
            GZipStream zip = new GZipStream(ms, CompressionMode.Decompress);
            // do the unzip
            zip.Read(buffer, 0, buffer.Length);
            // convert back to string and return
            return Encoding.UTF8.GetString(buffer);
        }

        public static void CompressFile(string path)
        {

            FileStream sourceFile = File.OpenRead(path);

            FileStream destinationFile = File.Create(path + ".gz");



            byte[] buffer = new byte[sourceFile.Length];

            sourceFile.Read(buffer, 0, buffer.Length);



            using (GZipStream output = new GZipStream(destinationFile,

                CompressionMode.Compress))
            {

                Console.WriteLine("Compressing {0} to {1}.", sourceFile.Name,

                    destinationFile.Name, false);



                output.Write(buffer, 0, buffer.Length);

            }



            // Close the files.

            sourceFile.Close();

            destinationFile.Close();

        }

        public static void UncompressFile(string path)
        {

            FileStream sourceFile = File.OpenRead(path);

            FileStream destinationFile = File.Create(path + ".txt");
            // Because the uncompressed size of the file is unknown, 

            // we are using an arbitrary buffer size.

            byte[] buffer = new byte[4096];
            int n;
            using (GZipStream input = new GZipStream(sourceFile, CompressionMode.Decompress, false))
            {
                n = input.Read(buffer, 0, buffer.Length);

                destinationFile.Write(buffer, 0, n);

            }
            // Close the files.

            sourceFile.Close();

            destinationFile.Close();

        }

    }
}
