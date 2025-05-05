// Unity 譜面編輯器主要邏輯
// 功能：讀音樂、播放、鍵盤記錄、匯出 JSON 音譜
// 音符類型：1=單擊，2=長按起點，2.5=長按結尾，3=上滑
// 輸出格式：JSON，包含 bpm 與 notes 陣列

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class ScoreEditor : MonoBehaviour
{
    public AudioSource musicSource;
    public TextMeshProUGUI timeDisplay;
    public Button exportButton;
    public float bpm = 120f; // 可手動設定 bpm

    [System.Serializable]
    public class Note
    {
        public float time;  // 毫秒
        public int lane;
        public float type;  // 1, 2, 2.5, 3
    }

    [System.Serializable]
    public class NoteChart
    {
        public float bpm;
        public List<Note> notes;
    }

    private List<Note> noteList = new List<Note>();
    private float currentTime => musicSource.time * 1000f; // 轉成毫秒

    void Update()
    {
        timeDisplay.text = (currentTime / 1000f).ToString("F2") + "s";

        // 7 個鍵對應 lane 0~6，都是 type 1（單擊）
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddNote(0, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha2)) AddNote(1, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha3)) AddNote(2, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha4)) AddNote(3, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha5)) AddNote(4, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha6)) AddNote(5, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha7)) AddNote(6, 1f);

        // 進階音符類型（使用 Q/W/E）搭配目前 lane 設定
        if (Input.GetKeyDown(KeyCode.Q)) AddNote(currentLane, 2f);   // 長按開始
        if (Input.GetKeyDown(KeyCode.W)) AddNote(currentLane, 2.5f); // 長按結束
        if (Input.GetKeyDown(KeyCode.E)) AddNote(currentLane, 3f);   // 上滑

        // 使用左右鍵切換目前的 lane（限 Q/W/E 用）
        if (Input.GetKeyDown(KeyCode.LeftArrow)) currentLane = Mathf.Max(0, currentLane - 1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) currentLane = Mathf.Min(6, currentLane + 1);
    }

    void AddNote(int lane, float type)
    {
        Note n = new Note
        {
            time = currentTime,
            type = type,
            lane = lane
        };
        noteList.Add(n);
        Debug.Log($"[ADD] type {n.type} @ {n.time}ms, lane {n.lane}");
    }

    public void ExportChart()
    {
        NoteChart chart = new NoteChart { bpm = bpm, notes = noteList };
        string json = JsonUtility.ToJson(chart, true);
        string path = Path.Combine(Application.dataPath, "notechart.json");
        File.WriteAllText(path, json);
        Debug.Log("匯出 JSON 成功：" + path);
    }

    private int currentLane = 0;

    void Start()
    {
        exportButton.onClick.AddListener(ExportChart);
    }
}
