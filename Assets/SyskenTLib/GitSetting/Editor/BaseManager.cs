using System.Collections.Generic;
using System.IO;
using SyskenTLib.GitSetting.Config;
using UnityEngine;

namespace SyskenTLib.GitSetting.Editor
{
    public class BaseManager
    {

        public void CreateGitIgnore()
        {


            ConfigManager _configManager = new ConfigManager();
            STGitConfig config = _configManager.SearchConfig();

            string unityprojectDirPath = Path.GetDirectoryName(Application.dataPath);
            string filePath = unityprojectDirPath + "/" + config.GetIgnoreFileName;

            if (File.Exists(filePath) == false)
            {
                Debug.Log(filePath+" を作成しました。");
                Debug.Log(config.GetGitIgnoreContentTxt);
                File.WriteAllText(filePath, config.GetGitIgnoreContentTxt);
            }
            else
            {
                Debug.Log("すでにGitIgnoreファイルが設定されています");
            }
        }
        
        public void CreateGitLFSConfig()
        {


            ConfigManager _configManager = new ConfigManager();
            STGitConfig config = _configManager.SearchConfig();

            string unityprojectDirPath = Path.GetDirectoryName(Application.dataPath);
            string filePath = unityprojectDirPath + "/" + config.GetGitAttributeFileName;

            if (File.Exists(filePath) == false)
            {
                Debug.Log(filePath+" を作成しました。");
                Debug.Log(config.GetGitIgnoreContentTxt);
                File.WriteAllText(filePath, config.GetGitLFSAttributesContentTxt);
            }
            else
            {
                Debug.Log("すでにGitLFS設定ファイルが設定されています");
            }
        }

        public List<string> CreateGitKeepFiles(List<string> targetDirectoryPathList)
        {
            List<string> createdFilePathList = new List<string>();
            
            targetDirectoryPathList.ForEach(targetDirectoryPath =>
            {
                ConfigManager _configManager = new ConfigManager();
                STGitConfig config = _configManager.SearchConfig();
                
                string filePath = targetDirectoryPath + "/" + config.GetGitKeepFileName;

                if (File.Exists(filePath) == false)
                {
                    File.WriteAllText(filePath, "");
                    createdFilePathList.Add(filePath);
                }
                else
                {
                }
            });

            return createdFilePathList;
        }



    }
}