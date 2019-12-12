using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 中文检测
/// </summary>
public static class ChineseChecker
{
    static Regex P_regex = new Regex("[\u4E00-\u9FA5]");
    private static string[] CS_Symbos = new string[16]
    {
        @"#region", @"LogModule.", @"[Header",@"[EnumLabel", @"[ContextMenu","#endregion",
        @"throw new", @"[TimelineTrackAttribute",@"[TrackGroupAttribute", @"[Message", @"[Inspector",
        @"[RegionType",@"Debug.Log", @"Debug.LogError", @"Debug.LogWarning", @"Debug.LogFormat"
    };

    private static string CS_Comment_Symbol = @"//";


    private static string[] Lua_Symbos = new string[7]
    {
         @"error(",@"log(",@"warn(",@"redLog(",@"orangeLog(",@"print(",@"purpleLog("
    };
    private static string Lua_Comment_Symbol = @"--";

    static string[] CS_Split_Symbol = new string[1] { CS_Comment_Symbol };
    static string[] Lua_Split_Symbol = new string[1] { Lua_Comment_Symbol };
    static Encoding Lua_Encoding = new UTF8Encoding(false);

    /// <summary>
    /// 检查CS 忽略符号
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool ChackIgnoreCSSymbo(string str)
    {
        for (int i = 0, imax = CS_Symbos.Length; i < imax; i++)
        {
            if (str.Contains(CS_Symbos[i]))
                return false;
        }
        return true;
    }
    /// <summary>
    /// 检查Lua 忽略符号
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static bool ChackIgnoreLuaSymbo(string str)
    {
        for (int i = 0, imax = Lua_Symbos.Length; i < imax; i++)
        {
            if (str.Contains(Lua_Symbos[i]))
                return false;
        }
        return true;
    }

    /// <summary>
    /// 匹配包含中文字符
    /// </summary>
    /// <param name="str"></param>
    public static bool MatchChinese(string str)
    {
        return P_regex.IsMatch(str);
    }

    public static StringBuilder CheckCSFile(string path)
    {
        if (!File.Exists(path)) return null;
        StringBuilder sb = new StringBuilder();
        using (var reader = new StreamReader(path, Encoding.Default))
        {
            int line = 1;
            var str = reader.ReadLine();
            string tmp;
            int multiCommentCnt = 0;//多行注释个数
            bool igoner = false;
            while (str != null)
            {
                if (igoner) igoner = false;
                if (str.Contains(@"/*"))
                {
                    multiCommentCnt++;
                }
                if (str.Contains(@"*/"))
                {
                    multiCommentCnt--;
                    igoner = true;
                }
                if (!igoner && multiCommentCnt <= 0 && MatchChinese(str))
                {
                    if (ChackIgnoreCSSymbo(str))
                    {
                        if (str.Contains(CS_Comment_Symbol))
                        {
                            //Debug.LogError($"{path}第{line}行存在//");
                            var idx = str.IndexOf(CS_Comment_Symbol, 0);
                            //Debug.Log(idx);
                            if (idx > 0)
                            {
                                var strs = str.Split(CS_Split_Symbol, StringSplitOptions.None);
                                if (strs.Length > 0 && MatchChinese(strs[0]))
                                {
                                    //tmp = $"{path}第{line}行:{str}";
                                    tmp = string.Format("{0}第{1}行: {2}", path, line, str);
                                    Debug.LogError(tmp);
                                    sb.AppendLine(tmp);
                                }
                            }
                        }
                        else
                        {
                            //tmp = $"{path}第{line}行{str}";
                            tmp = string.Format("{0}第{1}行: {2}", path, line, str);
                            Debug.LogError(tmp);
                            sb.AppendLine(tmp);
                        }
                    }
                }

                line++;
                str = reader.ReadLine();
            }
            reader.Close();
        }
        if (sb.Length == 0) return null;
        return sb;
    }

    public static StringBuilder CheckLuaFile(string path)
    {
        if (!File.Exists(path)) return null;
        var sb = new StringBuilder();
        using (var reader = new StreamReader(path, Lua_Encoding))
        {
            int line = 1;
            var str = reader.ReadLine();
            string tmp;
            int multiCommentCnt = 0;
            bool ignore = false;
            while (str != null)
            {
                ignore = false;
                if (str.Contains(@"--[[")) multiCommentCnt++;
                if (multiCommentCnt > 0 && (str.Contains(@"]]") || str.Contains(@"]]--")))
                {
                    multiCommentCnt--;
                    ignore = true;
                }

                if (!ignore && multiCommentCnt <= 0 && MatchChinese(str))
                {
                    if (ChackIgnoreLuaSymbo(str))
                    {
                        if (str.Contains(Lua_Comment_Symbol))
                        {
                            var idx = str.IndexOf(Lua_Comment_Symbol, 0);
                            //Debug.Log(idx);
                            if (idx > 0)
                            {
                                var strs = str.Split(Lua_Split_Symbol, StringSplitOptions.None);
                                if (strs.Length > 0 && MatchChinese(strs[0]))
                                {
                                    tmp = string.Format("{0}第{1}行: {2}", path, line, str);
                                    //tmp = $"{path}第{line}行:{str}";
                                    //Debug.LogError(tmp);
                                    sb.AppendLine(tmp);
                                }
                            }
                        }
                        else
                        {
                            //tmp = $"{path}第{line}行:{str}";
                            tmp = string.Format("{0}第{1}行: {2}", path, line, str);
                            //Debug.LogError(tmp);
                            sb.AppendLine(tmp);
                        }
                    }
                }

                line++;
                str = reader.ReadLine();
            }
            reader.Close();
        }
        if (sb.Length == 0) return null;
        return sb;
    }
}
