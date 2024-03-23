using SyskenTLib.GitSetting.Config;
using UnityEditor;

namespace SyskenTLib.GitSetting.Editor
{
    public class ConfigManager
    {



        public STGitConfig SearchConfig()
        {
            string[] guids = AssetDatabase.FindAssets("t:STGitConfig");
            if (guids.Length > 0)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<STGitConfig>(filePath);
            }


            return null;
        }
    }
}