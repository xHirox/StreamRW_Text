# StreamRW_Text
System.IO.StreamReader/StreamWriterを使用した、基本的なテキストファイルの読み出し・書き出し用のクラスライブラリです。

------------------------
・使用する場合はソリューションに参照設定を追加して利用してください。
------------------------
・使用方法
・streamRW_Text.ReadText：読み出し
  string[] = streamRW_Text.ReadText(string filepath)でテキストファイルの文字配列を取得
・streamRW_Text.WriteText：書き込み
  streamRW_Text.WriteText(string filepath, string[] writearray, bool overwriteflag)で書き出し
------------------------
