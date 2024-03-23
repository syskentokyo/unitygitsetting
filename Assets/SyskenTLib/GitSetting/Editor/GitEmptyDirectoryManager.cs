using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace SyskenTLib.GitSetting.Editor
{
    public class GitEmptyDirectoryManager
    {
        
        public List<string> SearchEmptyDirectory()
        {
            List<string> targetDirectoryPathList = new List<string>();
            
            var selectDirpath = EditorUtility.OpenFolderPanel(
                "Search Root Directory"
                , Application.dataPath
                , string.Empty);

            if (string.IsNullOrEmpty(selectDirpath) == true)
            {
                //何も選ばなかったとき
                return new List<string>();
            }

            string[] subFolders = System.IO.Directory.GetDirectories(
                selectDirpath, "*", System.IO.SearchOption.AllDirectories);
            
            subFolders.ToList().ForEach(subDirectoryPath =>
            {
                if (System.IO.Directory.GetFiles(
                        subDirectoryPath, "*", System.IO.SearchOption.TopDirectoryOnly).Length == 0)
                {
                    targetDirectoryPathList.Add(subDirectoryPath);
                }
            });
            

            return targetDirectoryPathList;
        }
        
        
        
    }
}