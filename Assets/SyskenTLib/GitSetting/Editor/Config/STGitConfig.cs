using UnityEngine;

namespace SyskenTLib.GitSetting.Config
{

    public class STGitConfig : ScriptableObject
    {


        [Header("Base")]
        [SerializeField] private string _gitIgnoreFileName = ".gitignore";

        public string GetIgnoreFileName => _gitIgnoreFileName;
        
        
        [SerializeField] private string _gitkeepFileName = ".gitkeep";
        public string GetGitKeepFileName => _gitkeepFileName;
        
        
        
        [SerializeField] private string _gitAttributeFileName = ".gitattributes";
        public string GetGitAttributeFileName => _gitAttributeFileName;


        [SerializeField] private string _gitlfsAttributeParam1 = " filter=lfs diff=lfs merge=lfs -text";
        public string GetGitLfsAttributeParam1 => _gitlfsAttributeParam1; 

        [SerializeField] private float _largeFileSizeMB = 2.0f;
        public float GetLargeFileSizeMB => _largeFileSizeMB;
        
        [Header("GitIgnore")] [SerializeField,TextArea(10,200)] private string _gitIgnoreTxt;
        public string GetGitIgnoreContentTxt => _gitIgnoreTxt;
        
        
        [Header("GitLFS")] [SerializeField,TextArea(10,200)] private string _gitattributesTxt;
        public string GetGitLFSAttributesContentTxt => _gitattributesTxt;
    }
}