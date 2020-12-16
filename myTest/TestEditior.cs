using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class TestEditior : EditorWindow
{
    [MenuItem("Managers/AudioManagers")]
    static void CreateWindow()
    {
        //Rect rect = new Rect(100, 200, 300, 400);
        //TestEditior testEditor = EditorWindow.GetWindowWithRect(typeof(TestEditior), rect) as TestEditior;
        TestEditior testEditor = EditorWindow.GetWindow<TestEditior>("音效管理");
        testEditor.Show();
    }
    private string savePath ;
    private void Awake()
    {
        savePath = Application.dataPath + "\\Editor\\myTest\\audio.txt";
        LoadAudioList();
    }

    private string audioName;
    private string audioPath;
    private Dictionary<string, string> audioDic = new Dictionary<string, string>();
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("名字");
        GUILayout.Label("路径");
        GUILayout.Label("操作");
        GUILayout.EndHorizontal();

        foreach (string key in audioDic.Keys)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(key);
            string value;
            audioDic.TryGetValue(key, out value);
            GUILayout.Label(value);
            if (GUILayout.Button("删除"))
            {
                audioDic.Remove(key);
                SaveAudioList();
                return;
            }
            GUILayout.EndHorizontal();
        }



        audioName = EditorGUILayout.TextField("名字：", audioName);
        audioPath = EditorGUILayout.TextField("路径：", audioPath);

        if (GUILayout.Button("添加音效"))
        {
            object go = Resources.Load(audioPath);
            if (go == null)
            {
                Debug.Log("音效不存在于路径于" + audioPath + "加载不成功");
                audioPath = "";
            }
            else
            {
                if (audioDic.ContainsKey(audioName))
                {
                    Debug.Log("音效已经存在" + audioName + ", 请修改");
                }
                else
                {
                    audioDic.Add(audioName, audioPath);
                    SaveAudioList();
                }
            }
        }

    }

    private void OnInspectorUpdate()
    {
        LoadAudioList();
    }


    
    private void SaveAudioList()
    {
        StringBuilder str = new StringBuilder();
        foreach (string key in audioDic.Keys)
        {
            string value;
            audioDic.TryGetValue(key, out value);
            str.Append(key + "," + value + "\n");
        }

        File.WriteAllText(savePath, str.ToString());
    }

    private void LoadAudioList()
    {
        audioDic = new Dictionary<string, string>();
        if (File.Exists(savePath) == false) return;
       
        string[] lines = File.ReadAllLines(savePath);
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            string[] keyvalue = line.Split(',');
            audioDic.Add(keyvalue[0], keyvalue[1]);
        }
    }
}
