// Unity �Э��s�边�D�n�޿�
// �\��GŪ���֡B����B��L�O���B�ץX JSON ����
// ���������G1=�����A2=�����_�I�A2.5=���������A3=�W��
// ��X�榡�GJSON�A�]�t bpm �P notes �}�C

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
    public float bpm = 120f; // �i��ʳ]�w bpm

    [System.Serializable]
    public class Note
    {
        public float time;  // �@��
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
    private float currentTime => musicSource.time * 1000f; // �ন�@��

    void Update()
    {
        timeDisplay.text = (currentTime / 1000f).ToString("F2") + "s";

        // 7 ������� lane 0~6�A���O type 1�]�����^
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddNote(0, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha2)) AddNote(1, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha3)) AddNote(2, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha4)) AddNote(3, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha5)) AddNote(4, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha6)) AddNote(5, 1f);
        if (Input.GetKeyDown(KeyCode.Alpha7)) AddNote(6, 1f);

        // �i�����������]�ϥ� Q/W/E�^�f�t�ثe lane �]�w
        if (Input.GetKeyDown(KeyCode.Q)) AddNote(currentLane, 2f);   // �����}�l
        if (Input.GetKeyDown(KeyCode.W)) AddNote(currentLane, 2.5f); // ��������
        if (Input.GetKeyDown(KeyCode.E)) AddNote(currentLane, 3f);   // �W��

        // �ϥΥ��k������ثe�� lane�]�� Q/W/E �Ρ^
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
        Debug.Log("�ץX JSON ���\�G" + path);
    }

    private int currentLane = 0;

    void Start()
    {
        exportButton.onClick.AddListener(ExportChart);
    }
}
