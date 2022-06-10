using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// write tradelink tick files
    /// </summary>
    public class TikWriter : BinaryWriter
    {
        string _symbol = string.Empty;
        string _file = string.Empty;
        string _path = string.Empty;

        public string FolderPath { get { return _path; } set { _path = value; } }
        int _date = 0;
        /// <summary>
        /// real symbol represented by tick file
        /// </summary>
        public string Symbol { get { return _symbol; } }
        /// <summary>
        /// path of this file
        /// </summary>
        public string Filepath { get { return _file; } }
        /// <summary>
        /// date represented by data
        /// </summary>
        public int Date { get { return _date; } }

        /// <summary>
        /// ticks written
        /// </summary>
        public int Count = 0;
        /// <summary>
        /// creates a tikwriter with no header, header is created from first tik
        /// </summary>
        public TikWriter()
        {

        }

        /// <summary>
        /// create a tikwriter for specific symbol on specific date
        /// auto-creates header
        /// </summary>
        /// <param name="realsymbol"></param>
        /// <param name="date"></param>
        //public TikWriter(string realsymbol) : this(Util.TLTickDir, realsymbol) { }
        /// <summary>
        /// create tikwriter with specific location, symbol and date.
        /// auto-creates header
        /// </summary>
        /// <param name="path"></param>
        /// <param name="realsymbol"></param>
        /// <param name="date"></param>
        public TikWriter(string path, string symbol,int date)
        {
            // store important stuff
            _symbol = symbol;
            _path = path;
            _date = date;

            // get filename from path and symbol
            _file = GetTickFileName(path, symbol, date);//Path.Combine(new string[] { path, "{0}-{1}{2}".Put(_symbol, _date, TikConst.DOT_EXT) });

            if (File.Exists(_file))
            {
                OutStream = new FileStream(_file, FileMode.Open, FileAccess.Write, FileShare.Read);
                //�Ѿ����ڵ��ļ� ���õ�ǰpositionΪĩβ �������ļ�׷������
                OutStream.Position = OutStream.Length;
            }
            else
            {
                OutStream = new FileStream(_file, FileMode.Create, FileAccess.Write, FileShare.Read);
            }
        }

        /// <summary>
        /// ���ĳ����Լ��Tick�ļ�����Ŀ¼
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string GetTickPath(string baseDir, string exchange,string symbol)
        {
            string path = Path.Combine(new string[] { baseDir, exchange, symbol });
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string GetTickFileName(string path, string symbol, int date)
        {
            return Path.Combine(new string[] { path, "{0}-{1}{2}".Put(symbol, date, TikConst.DOT_EXT) });
        }



        /// <summary>
        /// close a tickfile
        /// </summary>
        public override void Close()
        {
            base.Close();
        }

        /// <summary>
        /// ���ĳ����Լ������tick�ļ���
        /// </summary>
        /// <param name="path"></param>
        /// <param name="realysymbol"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTickFiles(string path, string symbol)
        {
            string[] files = Directory.GetFiles(path);
            return files.Where(fn => { string name = Path.GetFileName(fn); return name.StartsWith(symbol) && name.EndsWith(TikConst.DOT_EXT); });
        }


        /// <summary>
        /// �ж�ĳ���ļ��������Ƿ���Tick�ļ�
        /// </summary>
        /// <param name="path"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static bool HaveAnyTickFiles(string path, string symbol)
        {
            return GetTickFiles(path, symbol).Any();
        }

        /// <summary>
        /// ���ĳ����Լ���µ�Tick�ļ�����
        /// </summary>
        /// <param name="path"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static int GetEndTickDate(string path, string symbol)
        {
            IEnumerable<string> tickfiles = GetTickFiles(path, symbol);
            IEnumerable<int> datelist = tickfiles.Select(fn => GetTickFileDate(fn));

            return datelist.Max();

        }

        public static int GetStartTickDate(string path, string symbol)
        {
            IEnumerable<string> tickfiles = GetTickFiles(path, symbol);
            IEnumerable<int> datelist = tickfiles.Select(fn => GetTickFileDate(fn));

            return datelist.Min();
        }

        static int GetTickFileDate(string fn)
        {
            string name = Path.GetFileNameWithoutExtension(fn);
            string[] rec = name.Split('-');
            return int.Parse(rec[1]);
        }

        
       
        /// <summary>
        /// write a tick to file
        /// </summary>
        /// <param name="k"></param>
        public void NewTick(Tick k)
        {
            Write(Encoding.UTF8.GetBytes(TickImpl.Serialize(k)+"\n"));
            // write to disk
            Flush();
            // count it
            Count++;

        }
    }
}
