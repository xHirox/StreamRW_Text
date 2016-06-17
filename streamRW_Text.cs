using System;
using System.IO;

namespace StreamRW_Text
{
    /// <summary>
    /// StreamReader/StreamWriterでテキストファイルの読み出し・書き込みをする
    /// streamRW_Text.ReadText：読み出し
    /// string[] = streamRW_Text.ReadText(string filepath)でテキストファイルの文字配列を取得
    /// streamRW_Text.WriteText：書き込み
    /// streamRW_Text.WriteText(string filepath, string[] writearray, bool overwriteflag)で書き出し
    /// </summary>
    /// <remarks>
    /// values：書き込み用文字列
    /// ReadTextで読み出し、WriteTextで書き込み
    /// ※読み出し・書き込みの際は対象のファイルパスを事前に指定する必要がある
    /// </remarks>
    public class streamRW_Text
    {
        private string[] values;

        public streamRW_Text()
        {
            values = new string[0];
        }

        public streamRW_Text(string[] keys)
        {
            values = new string[keys.Length];
            values = keys;
        }

        public string[] getValues()
        {
            return values;
        }

        public void setValues(string key)
        {
            values.SetValue(key, values.Length + 1);
        }

        public void setValues(string key, bool flag)
        {
            if (flag)
            {
                if (values.Length == 0)
                {
                    values.SetValue(key, 0);
                }
                else
                {
                    values.SetValue(key, values.Length + 1);
                }
            }
            else
            {
                values.SetValue(key, values.Length + 1);
            }
        }

        public void setvalues(string[] keys)
        {
            Array.Resize(ref values, keys.Length);

            for (int i = 0; i < keys.Length; i++)
            {
                if (i == 0)
                {
                    values[(values.Length + i) + 1] = keys[i];
                }
                else
                {
                    values[values.Length + i] = keys[i];
                }
            }
        }
        
        public void setValues(string[] keys, bool flag)
        {
            Array.Resize(ref values, keys.Length);

            if (flag)
            {
                if (values.Length == 0)
                {
                    values = keys;
                }
                else
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (i == 0)
                        {
                            values[(values.Length + i) + 1] = keys[i];
                        }
                        else
                        {
                            values[values.Length + i] = keys[i];
                        }
                    }
                }
            }
            else
            {
                values = keys;
            }
        }

        /// <summary>
        /// 指定のファイルのテキストを読み出す
        /// </summary>
        /// <param name="path">読み出し対象ファイルのファイルパス(string型)</param>
        /// <returns>読み出した文字列の配列を返す(string[]型)</returns>
        public string[] ReadText(string path)
        {
            FileTextReader reader = new FileTextReader();

            if (path != null || !path.Equals(string.Empty))
            {
                reader.setFilePath(path);
            }

            reader.FileTextRead();

            return reader.getReadValue();

        }

        /// <summary>
        /// 指定のファイルへテキストを書き込む
        /// </summary>
        /// <param name="path">書き込み対象ファイルのパス(string型)</param>
        /// <param name="keys">書き込む文字列の配列(string[]型)</param>
        /// <param name="flag">true：ファイルに既存のテキストを上書きせず、末尾から追記する
        /// false：ファイル既存の文字列を無視して書き込む
        /// (StreamWriterの引数2を指定)</param>
        public void WriteText(string path, string[] keys, bool flag)
        {
            FileTextWriter writer = new FileTextWriter();

            // filepathがある場合はセット
            if (path != null || !path.Equals(string.Empty))
            {
                writer.setWritePath(path);
            }

            // 書き込み用の配列がある場合はセット
            if (keys != null)
            {
                writer.setWriteValues(keys);
            }
            // keysが指定されておらず、valuesが存在する場合はセット
            else if (keys == null && (values != null || values.Length != 0))
            {
                writer.setWriteValues(values);
            }

            // flagが指定されている場合はセット
            if (!flag)
            {
                writer.setOptionFlag(flag);
            }

            writer.FileTextWrite();
        }
    }

    /// <summary>
    /// StreamReaderでのテキスト読み出し
    /// </summary>
    /// <remarks>
    /// filepath：読み出し対象のファイルのパス
    /// Rvalues：読み出した文字列の配列
    /// errmsg：読み出し時にエラーを捕捉した場合のエラーメッセージ
    /// ※読み出し前にファイルパスを指定する必要がある
    /// </remarks>
    class FileTextReader
    {
        private string filepath;
        private string[] Rvalues;
        private string errmsg;

        public FileTextReader()
        {
            filepath = string.Empty;
            Rvalues = new string[0];
            errmsg = string.Empty;
        }

        public FileTextReader(string path)
        {
            filepath = path;
            Rvalues = new string[0];
            errmsg = string.Empty;
        }

        public string[] getReadValue()
        {
            return Rvalues;
        }

        public string getErrMsg()
        {
            return errmsg;
        }

        public void setFilePath(string path)
        {
            filepath = path;
        }

        private void setErrMsg(string msg)
        {
            errmsg = msg;
        }

        /// <summary>
        /// 対象ファイルからテキスト読み出し
        /// </summary>
        internal void FileTextRead()
        {
            if (filepath == null || filepath.Equals(string.Empty))
            {
                return;
            }

            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    string line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Array.Resize(ref Rvalues, Rvalues.Length + 1);
                        Rvalues[i] = line;
                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                setErrMsg(e.Message);
                Console.WriteLine(getErrMsg());
            }
        }
    }


    /// <summary>
    /// StreamWriterによるファイルへのテキスト書き込み
    /// </summary>
    /// <remarks>
    /// filepath：書き込み対象のファイルのパス
    /// Wvalues：書き込むテキストの配列
    /// optionflag：既存のデータに追記するか置き換えるかのフラグ
    /// errmsg：書き込み時に捕捉したエラーメッセージ
    /// ※書き込み前に対象のファイルのパスを指定する必要がある
    /// </remarks>
    class FileTextWriter
    {
        private string filepath;
        private string[] Wvalues;
        private bool optionflag;
        private string errmsg;

        public FileTextWriter()
        {
            filepath = string.Empty;
            Wvalues = new string[0];
            optionflag = true;
            errmsg = string.Empty;
        }

        public FileTextWriter(string path)
        {
            filepath = path;
            Wvalues = new string[0];
            optionflag = true;
            errmsg = string.Empty;
        }

        public FileTextWriter(string path, string[] keys)
        {
            filepath = path;
            Wvalues = keys;
            optionflag = true;
            errmsg = string.Empty;
        }

        public FileTextWriter(string path, string[] keys, bool flag)
        {
            filepath = path;
            Wvalues = keys;
            optionflag = flag;
            errmsg = string.Empty;
        }

        public string[] getWriteValues()
        {
            return Wvalues;
        }

        public string getWriteFilePath()
        {
            return filepath;
        }

        public bool IsOptionFlag()
        {
            return optionflag;
        }

        public string getErrMsg()
        {
            return errmsg;
        }

        public void setWritePath(string path)
        {
            filepath = path;
        }

        public void setWriteValues(string[] keys)
        {
            Array.Resize(ref Wvalues, keys.Length);
            Wvalues = keys;
        }

        public void setOptionFlag(bool flag)
        {
            optionflag = flag;
        }

        private void setErrMsg(string msg)
        {
            errmsg = msg;
        }

        public void setWriteValues(string[] keys, bool flag = true)
        {
            setOptionFlag(flag);

            Array.Resize(ref Wvalues, keys.Length);

            if (!optionflag)
            {
                Wvalues = keys;
            }
            else
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (i == 0)
                    {
                        Wvalues[(Wvalues.Length + i) + 1] = keys[i];
                    }
                    else
                    {
                        Wvalues[(Wvalues.Length + i)] = keys[i];
                    }
                }
            }
        }

        /// <summary>
        /// 対象ファイルへテキスト書き込み
        /// </summary>
        internal void FileTextWrite()
        {
            if (filepath == null || filepath.Equals(string.Empty))
            {
                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(filepath, optionflag))
                {
                    int i = 0;

                    while (i < Wvalues.Length)
                    {
                        if (i == 0)
                        {
                            sw.WriteLine("\r\n" + Wvalues[i]);
                        }
                        else
                        {
                            sw.WriteLine(Wvalues[i]);
                        }
                        
                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                setErrMsg(e.Message);
                Console.WriteLine(getErrMsg());
            }
        }
    }
}
