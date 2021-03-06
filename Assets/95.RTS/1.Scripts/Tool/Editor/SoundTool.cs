using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

public class SoundTool : EditorWindow
{
    public const int widthMiddle = 200;
    public const int widthLarge = 300;
    public const int widthXLarge = 450;
    private int selection = 0;
    private Vector2 ScrollPoint1 = Vector2.zero;
    private Vector2 ScrollPoint2 = Vector2.zero;

    private AudioClip soundSource;
    private static SoundData soundData;

    [MenuItem("Tools/Sound Tool")]
    static void init()
    {
        soundData = ScriptableObject.CreateInstance<SoundData>();
        soundData.LoadData();

        SoundTool window = (SoundTool)EditorWindow.GetWindow<SoundTool>(false, "Sound Tool");
        window.Show();
     }

    private void OnGUI()
    {
        if(SoundTool.soundData == null)
        {
            return;
        }

        EditorGUILayout.BeginVertical();
        {   // Add, Copy, Remove button 영역

            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button("Add", GUILayout.Width(widthMiddle)))
                {
                    SoundTool.soundData.Addsound("NewSound");
                    this.selection = SoundTool.soundData.GetDataCount() - 1;
                    this.soundSource = null;
                    GUI.FocusControl("ID");
                }
                GUI.SetNextControlName("Copy");
                if(GUILayout.Button("Copy", GUILayout.Width(widthMiddle)))
                {
                    GUI.FocusControl("Copy");
                    SoundTool.soundData.CopyData(this.selection);
                    this.soundSource = null;
                    this.selection = SoundTool.soundData.GetDataCount() - 1;
                }
                //Debug.LogWarning("GetData :" + SoundTool.soundData.GetDataCount());
                if (SoundTool.soundData.GetDataCount() > 1)
                {
                    GUI.SetNextControlName("Remove");
                    if(GUILayout.Button("Remove", GUILayout.Width(widthMiddle)))
                    {
                        GUI.FocusControl("Remove");
                        this.soundSource = null;
                        SoundTool.soundData.RemoveData(this.selection);
                    }
                }

            }
            EditorGUILayout.EndHorizontal();

            // 가운데 UI 영역
            EditorGUILayout.BeginHorizontal();
            {
                // data list -> selection grid
                EditorGUILayout.BeginVertical(GUILayout.Width(widthLarge));
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.BeginVertical("box");
                    {
                        this.ScrollPoint1 = EditorGUILayout.BeginScrollView(this.ScrollPoint1);
                        {
                            if(SoundTool.soundData.GetDataCount() > 0)
                            {
                                int preSelection = this.selection;
                                this.selection = GUILayout.SelectionGrid(this.selection, SoundTool.soundData.GetNameList(true), 1);
                                //Debug.LogWarning("preSelection :" + preSelection);
                                //Debug.LogWarning("Selection :" + selection);
                                if (preSelection != selection)
                                {
                                    Debug.LogWarning("preSelection :" + preSelection);
                                    Debug.LogWarning("Selection :" + selection);
                                    this.soundSource = null;
                                }
                            }
                        }
                        EditorGUILayout.EndScrollView();
                        
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();

                // 상세 설정 수정하는 UI
                EditorGUILayout.BeginVertical();
                {
                    this.ScrollPoint2 = EditorGUILayout.BeginScrollView(this.ScrollPoint2);
                    {
                        if(SoundTool.soundData.GetDataCount() > 0)
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.Separator();
                                GUI.SetNextControlName("ID");
                                EditorGUILayout.LabelField("ID", this.selection.ToString(), GUILayout.Width(widthLarge));
                                SoundTool.soundData.names[selection] = EditorGUILayout.TextField("Name", SoundTool.soundData.names[selection], GUILayout.Width(widthXLarge));
                                SoundTool.soundData.soundClips[selection].PlayType = (SoundPlayType)EditorGUILayout.EnumPopup("PlayType :", SoundTool.soundData.soundClips[selection].PlayType, GUILayout.Width(widthLarge));
                                SoundTool.soundData.soundClips[selection].maxVolume = EditorGUILayout.FloatField("Volume :", SoundTool.soundData.soundClips[selection].maxVolume, GUILayout.Width(widthXLarge));
                                SoundTool.soundData.soundClips[selection].IsLoop = EditorGUILayout.Toggle("Loop Clip :", SoundTool.soundData.soundClips[selection].IsLoop, GUILayout.Width(widthLarge));

                                EditorGUILayout.Separator();
                                if(soundSource == null && SoundTool.soundData.soundClips[selection].ClipName != string.Empty)
                                {
                                    soundSource = SoundTool.soundData.soundClips[selection].GetClip();
                                }
                                soundSource = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", this.soundSource,
                                    typeof(AudioClip), false, GUILayout.Width(widthLarge));

                                Debug.LogWarning("SoundSource :" + soundSource);
                                if (soundSource != null)
                                {
                                    SoundTool.soundData.soundClips[selection].ClipPath = EditorHelper.GetPath(this.soundSource);
                                    SoundTool.soundData.soundClips[selection].ClipName = this.soundSource.name;
                                    SoundTool.soundData.soundClips[selection].Pitch = EditorGUILayout.Slider("Pitch", SoundTool.soundData.soundClips[selection].Pitch, -3.0f, 3.0f, GUILayout.Width(widthLarge));
                                    SoundTool.soundData.soundClips[selection].DopplerLevel = EditorGUILayout.Slider("Doppler Level", SoundTool.soundData.soundClips[selection].DopplerLevel, 0.0f, 5.0f, GUILayout.Width(widthLarge));
                                    SoundTool.soundData.soundClips[selection].audioRolloffMode = (AudioRolloffMode)EditorGUILayout.EnumPopup("RollOffMode", SoundTool.soundData.soundClips[selection].audioRolloffMode, GUILayout.Width(widthLarge));
                                    SoundTool.soundData.soundClips[selection].MinDistance = EditorGUILayout.FloatField("MinDistance", SoundTool.soundData.soundClips[selection].MinDistance, GUILayout.Width(widthLarge));
                                    SoundTool.soundData.soundClips[selection].MaxDistance = EditorGUILayout.FloatField("MaxDistance", SoundTool.soundData.soundClips[selection].MaxDistance, GUILayout.Width(widthLarge));
                                    SoundTool.soundData.soundClips[selection].SpatialBlend = EditorGUILayout.Slider("Spatial Blend", SoundTool.soundData.soundClips[selection].SpatialBlend, 0.0f, 1.0f, GUILayout.Width(widthLarge));
                                   

                                }
                                else
                                {
                                    SoundTool.soundData.soundClips[selection].ClipName = string.Empty;
                                    SoundTool.soundData.soundClips[selection].ClipPath = string.Empty;

                                }
                                EditorGUILayout.Separator();
                                if(GUILayout.Button("Add Loop", GUILayout.Width(widthLarge)))
                                {
                                    SoundTool.soundData.soundClips[selection].AddLoop();
                                }
                                for(int i = 0; i < SoundTool.soundData.soundClips[selection].CheckTime.Length;i++)
                                {
                                    EditorGUILayout.BeginVertical("box");
                                    {
                                        GUILayout.Label("Loop Step" + i, EditorStyles.boldLabel);
                                        if (GUILayout.Button("Remove", GUILayout.Width(widthLarge)))
                                        {
                                            SoundTool.soundData.soundClips[selection].RemoveLoop(i);
                                            return;
                                        }
                                        SoundTool.soundData.soundClips[selection].CheckTime[i] = EditorGUILayout.FloatField("CheckTime" + i.ToString(),
                                            SoundTool.soundData.soundClips[selection].CheckTime[i], GUILayout.Width(widthLarge));
                                        SoundTool.soundData.soundClips[selection].SetTime[i] = EditorGUILayout.FloatField("SetTime" + i.ToString(),
                                            SoundTool.soundData.soundClips[selection].SetTime[i], GUILayout.Width(widthLarge));

                                    }
                                    EditorGUILayout.EndVertical();

                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                    
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        // 하단 버튼 영역
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        {
            GUI.SetNextControlName("Reload");
            if(GUILayout.Button("Reload"))
            {
                GUI.FocusControl("Reload");
                soundData = ScriptableObject.CreateInstance<SoundData>();
                soundData.LoadData();
                selection = 0;
                this.soundSource = null;
            }
            GUI.SetNextControlName("Save");
            if(GUILayout.Button("Save"))
            {
                GUI.FocusControl("Save");
                SoundTool.soundData.SaveData();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }

            //Debug.LogWarning("SoundClip :" + SoundTool.soundData.soundClips.Length);

            if (SoundTool.soundData.soundClips.Length > 1)
            {
                GUI.SetNextControlName("Import");
                if(GUILayout.Button("Import"))
                {
                    GUI.FocusControl("Import");
                    CreateEnumStructure();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public void CreateEnumStructure()
    {
        string enumName = "SoundList";
        StringBuilder builder = new StringBuilder();
        builder.AppendLine();
        for( int i = 0; i < SoundTool.soundData.names.Length; i++)
        {
            if(SoundTool.soundData.names[i].ToLower().Contains("none") == false)
            {
                builder.AppendLine("\n" + SoundTool.soundData.names[i] + "=" + i.ToString() + ",");
            }
        }
        EditorHelper.CreateEnumStructure(enumName, builder);
        
    }

}
