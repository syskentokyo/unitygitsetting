using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SyskenTLib.GitSetting.Editor
{
    
    public class MainWindow : EditorWindow
    {
            private string _currentlogOutTxt = "";
            
            private Vector2 _mainScroll;
            private Vector2 _scroll2;

            private BaseManager _baseManager = new BaseManager();
            private GitEmptyDirectoryManager _gitEmptyDirectoryManager = new GitEmptyDirectoryManager();
            private GitLargeFileManager _gitLargeFileManager = new GitLargeFileManager();

            private ResultConfigData _currentLFSExtConfigData = null;
            private ResultConfigData _currentLFSNameConfigData = null;
            
        [MenuItem("SyskenTLib/GitSetting/Main", priority = 1)]
        private static void ShowWindow()
        {
            var window = GetWindow<MainWindow>();
            window.titleContent = new UnityEngine.GUIContent("GitSetting");
            window.Show();
        }


         private void OnGUI()
        { 
                
                _mainScroll = EditorGUILayout.BeginScrollView(_mainScroll,GUILayout.Height(position.height));
            
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.LabelField("Git Setting");
            
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Base");

            EditorGUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Create GitIgnore"
                    ,GUILayout.MinWidth( 100 ),GUILayout.MinHeight( 30 ),
                    GUILayout.MaxWidth( 300 ),GUILayout.MaxHeight( 30 ) ))
            {
                    _baseManager.CreateGitIgnore();;
            }
            EditorGUILayout.Space(30);
            if (GUILayout.Button("Create Git Attributes"
                    ,GUILayout.MinWidth( 100 ),GUILayout.MinHeight( 30 ),
                    GUILayout.MaxWidth( 300 ),GUILayout.MaxHeight( 30 ) ))
            {
                    _baseManager.CreateGitLFSConfig();;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Empty Directory");
            if (GUILayout.Button("Search Empty Directory"
                    ,GUILayout.MinWidth( 100 ),GUILayout.MinHeight( 30 ),
                    GUILayout.MaxWidth( 300 ),GUILayout.MaxHeight( 30 ) ))
            {
                    
                    AssetDatabase.Refresh();
                    
                    
                    List<string> targetDirectoryPathList = _gitEmptyDirectoryManager.SearchEmptyDirectory();
                    _currentlogOutTxt = "";
                    targetDirectoryPathList.ForEach(directoryPath =>
                    {
                            _currentlogOutTxt += directoryPath + "\n";
                    });
                    
                    AssetDatabase.SaveAssets();;
            }
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Create GitKeep To Empty Directory"
                    ,GUILayout.MinWidth( 100 ),GUILayout.MinHeight( 30 ),
                    GUILayout.MaxWidth( 300 ),GUILayout.MaxHeight( 30 ) ))
            {
                    AssetDatabase.Refresh();
                    
                    List<string> targetDirectoryPathList = _gitEmptyDirectoryManager.SearchEmptyDirectory();
                    List<string>  createdFilePath = _baseManager.CreateGitKeepFiles(targetDirectoryPathList);
                    
                    
                    
                    
                    _currentlogOutTxt = "GitKeepファイルを作成しました\n\n";
                    createdFilePath.ForEach(filePath =>
                    {
                            _currentlogOutTxt += filePath + "\n";
                    });
                    AssetDatabase.SaveAssets();;
            }
            
            
            EditorGUILayout.Space(4);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal("Box");
            
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.LabelField("Git LFS");
            EditorGUILayout.LabelField("Common File Extension");
            EditorGUILayout.LabelField("EX: *.png");
            if (GUILayout.Button("1:Search"
                    ,GUILayout.MinWidth( 100 ),GUILayout.MinHeight( 30 ),
                    GUILayout.MaxWidth( 300 ),GUILayout.MaxHeight( 30 ) ))
            {
                    AssetDatabase.Refresh();
                    
                    

                    _currentLFSExtConfigData = _gitLargeFileManager.SearchLFSLargeFileExt();
                    string sameOldAndNewMessage = "";
                    if (_currentLFSExtConfigData.isSameOldAndNewConfig == true)
                    {
                            //同じ設定だった場合
                            sameOldAndNewMessage = "\n\n設定に変更がありません。\n\n";
                    }
                    
                    
                    _currentlogOutTxt = "検索のみの結果\n\n";
                    _currentlogOutTxt += sameOldAndNewMessage;
                    
                    _currentlogOutTxt += "GitLFS設定します。\n";
                    _currentlogOutTxt += _currentLFSExtConfigData.targetConfigPath+"\n";
                    _currentlogOutTxt += "\n\n";
                    _currentlogOutTxt += "New Config:\n";
                    _currentlogOutTxt += _currentLFSExtConfigData.targetConfigContent+"\n";
                    
                    _currentlogOutTxt += "Old Config:\n";
                    _currentlogOutTxt += _currentLFSExtConfigData.oldConfigContent+"\n";
                    
                    //スクロールリセット
                    _scroll2 = Vector2.zero;;
                    
                    AssetDatabase.SaveAssets();;
            }
            if (GUILayout.Button("2:Write"
                        ,GUILayout.MinWidth( 100 ),GUILayout.MinHeight( 30 ),
                        GUILayout.MaxWidth( 300 ),GUILayout.MaxHeight( 30 ) ))
            {
                    AssetDatabase.Refresh();

                    if (_currentLFSExtConfigData != null)
                    {
                            
                            _gitLargeFileManager.WriteLFSConfig(_currentLFSExtConfigData);
                            string sameOldAndNewMessage = "";
                            if (_currentLFSExtConfigData.isSameOldAndNewConfig == true)
                            {
                                    //同じ設定だった場合
                                    sameOldAndNewMessage = "\n\n設定に変更がありません。\n\n";
                            }
                            
                            
                            _currentlogOutTxt = "";
                            _currentlogOutTxt += sameOldAndNewMessage;
                            
                            _currentlogOutTxt += "GitLF設定します。\n";
                            _currentlogOutTxt += _currentLFSExtConfigData.targetConfigPath + "\n";
                            _currentlogOutTxt += "\n\n";
                            _currentlogOutTxt += "New Config:\n";
                            _currentlogOutTxt += _currentLFSExtConfigData.targetConfigContent + "\n";

                            _currentlogOutTxt += "Old Config:\n";
                            _currentlogOutTxt += _currentLFSExtConfigData.oldConfigContent + "\n";
                            _currentLFSExtConfigData = null;
                            
                                                
                            //スクロールリセット
                            _scroll2 = Vector2.zero;;

                    }

                    AssetDatabase.SaveAssets();;
            }
            EditorGUILayout.Space(30);
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Individual File Extension");
            EditorGUILayout.LabelField("EX: target1.png");
 if (GUILayout.Button("1:Search"
                    ,GUILayout.MinWidth( 100 ),GUILayout.MinHeight( 30 ),
                    GUILayout.MaxWidth( 300 ),GUILayout.MaxHeight( 30 ) ))
            {
                    AssetDatabase.Refresh();
                    

                    _currentLFSNameConfigData = _gitLargeFileManager.SearchLFSLargeFileName();
                    string sameOldAndNewMessage = "";
                    if (_currentLFSNameConfigData.isSameOldAndNewConfig == true)
                    {
                            //同じ設定だった場合
                            sameOldAndNewMessage = "\n\n設定に変更がありません。\n\n";
                    }
                    
                    
                    _currentlogOutTxt = "検索のみの結果\n\n";
                    _currentlogOutTxt += sameOldAndNewMessage;
                    
                    _currentlogOutTxt += "GitLF設定します。\n";
                    _currentlogOutTxt += _currentLFSNameConfigData.targetConfigPath+"\n";
                    _currentlogOutTxt += "\n\n";
                    _currentlogOutTxt += "New Config:\n";
                    _currentlogOutTxt += _currentLFSNameConfigData.targetConfigContent+"\n";
                    
                    _currentlogOutTxt += "Old Config:\n";
                    _currentlogOutTxt += _currentLFSNameConfigData.oldConfigContent+"\n";
                    
                                        
                    //スクロールリセット
                    _scroll2 = Vector2.zero;;

                    
                    AssetDatabase.SaveAssets();;
            }
            if (GUILayout.Button("2:Write"
                        ,GUILayout.MinWidth( 100 ),GUILayout.MinHeight( 30 ),
                        GUILayout.MaxWidth( 300 ),GUILayout.MaxHeight( 30 ) ))
            {
                    AssetDatabase.Refresh();

                    if (_currentLFSNameConfigData != null)
                    {

                            _gitLargeFileManager.WriteLFSConfig(_currentLFSNameConfigData);
                            string sameOldAndNewMessage = "";
                            if (_currentLFSNameConfigData.isSameOldAndNewConfig == true)
                            {
                                    //同じ設定だった場合
                                    sameOldAndNewMessage = "\n\n設定に変更がありません。\n\n";
                            }

                            
                            _currentlogOutTxt = "";
                            _currentlogOutTxt += sameOldAndNewMessage;
                            
                            _currentlogOutTxt += "GitLF設定します。\n";
                            _currentlogOutTxt += _currentLFSNameConfigData.targetConfigPath + "\n";
                            _currentlogOutTxt += "\n\n";
                            _currentlogOutTxt += "New Config:\n";
                            _currentlogOutTxt += _currentLFSNameConfigData.targetConfigContent + "\n";

                            _currentlogOutTxt += "Old Config:\n";
                            _currentlogOutTxt += _currentLFSNameConfigData.oldConfigContent + "\n";
                            _currentLFSNameConfigData = null;
                            
                                                
                            //スクロールリセット
                            _scroll2 = Vector2.zero;;

                    }

                    AssetDatabase.SaveAssets();;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            
            _scroll2 = EditorGUILayout.BeginScrollView(_scroll2,GUILayout.Height(400));
            EditorGUILayout.TextArea(_currentlogOutTxt, GUILayout.Height(10000));
            EditorGUILayout.EndScrollView();
            
            
            
            EditorGUILayout.Space(30);
          
            
            
            EditorGUILayout.EndScrollView();
        }

        
        
        
    }
}
