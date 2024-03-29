
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using SyskenTLib.GitSetting.Config;


namespace SyskenTLib.GitSetting.Editor
{
    public class GitLargeFileManager
    {


        #region 公開


        /// <summary>
        /// 拡張子指定して、設定内容をまとめる
        /// </summary>
        /// <returns></returns>
        public ResultConfigData SearchLFSLargeFileExt()
        {
            ConfigManager _configManager = new ConfigManager();
            STGitConfig config = _configManager.SearchConfig();

            string saveFileName = config.GetGitAttributeFileName;
            float limitLargeFileSizeMB = config.GetLargeFileSizeMB;
            string gitLFSParam = config.GetGitLfsAttributeParam1;//GITLFSの１行毎のパラメータ
            
            SearchResultData resultData= SearchFilePathLFSLargeFile(limitLargeFileSizeMB);

            if (resultData.selectDirectoryPath == "")
            {
                //何も選択しなかった場合
                return new ResultConfigData();
            }

            string selectRootDirectory = resultData.selectDirectoryPath;

            Dictionary<string, string> extDic = new Dictionary<string, string>();
            //
            // ファイル名を指定して、LFS対象にする
            //
            resultData.resultPathList.ForEach(filePath =>
            {
                string ext = Path.GetExtension(filePath);

                //新しい拡張子だった場合のみ追加
                extDic.TryAdd(ext, ext);
                
            });

            string addConfigTxt = "";
            //
            // 新しく追加したい設定
            //
            extDic.Keys.ToList().ForEach(key =>
            {
                addConfigTxt += " *"+ key + " " + gitLFSParam + "\n";
            });
            
            //
            // 既存設定ファイル読み込み
            //
            string saveFilePath = selectRootDirectory +"/"+ saveFileName;
            string oldConfigTxt = ReadOldConfig(saveFilePath);
            
            //新しい設定内容
            string totalConfigTxt = oldConfigTxt +"\n"+ addConfigTxt;
            List<string> totalConfigSplitTxtList = totalConfigTxt.Split("\n").ToList();//1行づつわける
            
            //設定の無駄な行を削除
            string newConfigTxt = DeleteUselessLineFromConfigTxt(totalConfigSplitTxtList);

            //古い設定と新しい設定が同じか
            bool isSameOldAndNew = false;
            if (oldConfigTxt == newConfigTxt)
            {
                isSameOldAndNew = true; 
            }
            
            
            //
            // 新しい設定を書き込み
            //
            if (string.IsNullOrEmpty(newConfigTxt)== false)
            {
                ResultConfigData resultConfigData = new ResultConfigData();
                resultConfigData.isSameOldAndNewConfig = isSameOldAndNew;
                resultConfigData.oldConfigContent = oldConfigTxt;
                resultConfigData.targetConfigPath = saveFilePath;
                resultConfigData.targetConfigContent = newConfigTxt;
                return resultConfigData;
            }
            
            return new ResultConfigData();
        }
        
         public ResultConfigData SearchLFSLargeFileName()
        {
            ConfigManager _configManager = new ConfigManager();
            STGitConfig config = _configManager.SearchConfig();

            string saveFileName = config.GetGitAttributeFileName;
            float limitLargeFileSizeMB = config.GetLargeFileSizeMB;
            string gitLFSParam = config.GetGitLfsAttributeParam1;//GITLFSの１行毎のパラメータ
            
            SearchResultData resultData= SearchFilePathLFSLargeFile(limitLargeFileSizeMB);

            if (resultData.selectDirectoryPath == "")
            {
                //何も選択しなかった場合
                return new ResultConfigData();
            }

            string selectRootDirectory = resultData.selectDirectoryPath;
            

            List<string> targetFileList = new List<string>();
            //
            // ファイル名を指定して、LFS対象にする
            //
            resultData.resultPathList.ForEach(filePath =>
            {
                string validFilePath = filePath.Replace(selectRootDirectory+"/", "");

                //新しいファイルだった場合のみ追加
                targetFileList.Add(validFilePath);
                
            });

            string addConfigTxt = "";
            //
            // 新しく追加したい設定
            //
            targetFileList.ForEach(filePath =>
            {
                addConfigTxt += "\""+ filePath + "\"  " + gitLFSParam + "\n";
            });
            
            //
            // 既存設定ファイル読み込み
            //
            string saveFilePath = selectRootDirectory +"/"+ saveFileName;
            string oldConfigTxt = ReadOldConfig(saveFilePath);
            
            //新しい設定内容
            string totalConfigTxt = oldConfigTxt +"\n"+ addConfigTxt;
            List<string> totalConfigSplitTxtList = totalConfigTxt.Split("\n").ToList();//1行づつわける
            
            //設定の無駄な行を削除
            string newConfigTxt = DeleteUselessLineFromConfigTxt(totalConfigSplitTxtList);
            
            //古い設定と新しい設定が同じか
            bool isSameOldAndNew = false;
            if (oldConfigTxt == newConfigTxt)
            {
                isSameOldAndNew = true; 
            }

            
            //
            // 新しい設定を書き込み
            //
            if (string.IsNullOrEmpty(newConfigTxt)== false)
            {
                
                ResultConfigData resultConfigData = new ResultConfigData();
                resultConfigData.isSameOldAndNewConfig = isSameOldAndNew;
                resultConfigData.oldConfigContent = oldConfigTxt;
                resultConfigData.targetConfigPath = saveFilePath;
                resultConfigData.targetConfigContent = newConfigTxt;
                return resultConfigData;
            }
            
            return new ResultConfigData();
        }


        public void WriteLFSConfig(ResultConfigData targetConfigData)
        {
            if (targetConfigData == null)
            {
                return;
            }
            
            
            if (targetConfigData.targetConfigPath.Length < 4)
            {
                Debug.Log("対象のファイルパスが短すぎます.");
                return;
            }
            
            if (targetConfigData.targetConfigContent.Length<4)
            {
                Debug.Log("設定内容が短すぎます.");
                return;
            }
            
            
            //ファイル書き込み
            File.WriteAllText(targetConfigData.targetConfigPath,targetConfigData.targetConfigContent);
        }
        
        

        #endregion
        
        
        
        /// <summary>
        /// 指定容量より大きいファイルのリストを返す
        /// </summary>
        /// <param name="limitLargeFileSizeMB"></param>
        /// <returns></returns>
        public SearchResultData SearchFilePathLFSLargeFile(float limitLargeFileSizeMB = 2.0f)
        {
            SearchResultData resultData = new SearchResultData();
            
            
            var selectDirpath = EditorUtility.OpenFolderPanel(
                "Search Root Directory"
                , Application.dataPath
                , string.Empty);
            

            if (string.IsNullOrEmpty(selectDirpath) == true)
            {
                //何も選ばなかったとき
                return resultData;
            }
            
            resultData.selectDirectoryPath = selectDirpath;

            string[] subFolders = System.IO.Directory.GetDirectories(
                selectDirpath, "*", System.IO.SearchOption.AllDirectories);

            List<string> targetPathList = new List<string>() { };
            targetPathList.Add(selectDirpath);//検索の親フォルダを対象にする
            targetPathList.AddRange(subFolders.ToList());
            
            targetPathList.ToList().ForEach(targetDirectoryPath =>
            {
                // Debug.Log(targetDirectoryPath);
                
                List<string> filePathList=  System.IO.Directory.GetFiles(
                    targetDirectoryPath, "*", System.IO.SearchOption.TopDirectoryOnly).ToList();

                filePathList.ForEach(filePath =>
                {
                    FileInfo fileinfo = new FileInfo(filePath);
                    float fileSizeMB = fileinfo.Length / 1024.0f / 1024.0f;

                    // Debug.Log(fileSizeMB+"  "+ filePath);
                    
                    if (limitLargeFileSizeMB < fileSizeMB)
                    {
                        //大きなファイルだった場合
                        resultData.resultPathList.Add(filePath);
                    }
                   
                });
               
               
            });

            return resultData;
        }

        private string ReadOldConfig(string saveFilePath)
        {
            string oldConfigTxt = "";
            if (File.Exists(saveFilePath) == true)
            {
                //ファイルがあった場合
                oldConfigTxt = File.ReadAllText(saveFilePath);
            }

            return oldConfigTxt;
        }

        /// <summary>
        /// 設定内容から無駄なものを削除する
        /// </summary>
        /// <param name="validConfigTxtDic"></param>
        /// <returns></returns>
        private string DeleteUselessLineFromConfigTxt(  List<string> totalConfigSplitTxtList )
        {
            
            //
            // 重複行の削除
            //
            Dictionary<string, string> validConfigTxtDic = new Dictionary<string, string>();
            totalConfigSplitTxtList.ForEach(oneConfigTxt =>
            {
                if (validConfigTxtDic.ContainsKey(oneConfigTxt) == false)
                {
                    //新規設定だった場合
                    validConfigTxtDic.Add(oneConfigTxt,oneConfigTxt);
                }
            });
            
            
            //
            // その他の無駄な行を削除
            //
            string newConfigTxt = "";
            validConfigTxtDic.Keys.ToList().ForEach(key =>
            {
                if (key == "\n")
                {
                    //意図しない設定行だった場合
                }
                else
                {
                    //正しい設定内容だった場合
                    newConfigTxt += key+"\n";   
                }
            });

            return newConfigTxt;
        }
        
        

    }
}