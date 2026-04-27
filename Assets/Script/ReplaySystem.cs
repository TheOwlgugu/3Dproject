using System.Collections.Generic;
using UnityEngine;

public class ReplaySystem : MonoBehaviour
{
    [Header("References")]
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private Player playerScript;

    [Header("Recording")]
    public bool isRecording = false;
    private List<ReplayFrame> recordingFrames = new List<ReplayFrame>();
    private float recordTimer = 0f;

    [Header("Playback")]
    public bool isPlayingBack = false;
    private int playbackIndex = 0;
    private float playbackTimer = 0f;

    [Header("Pause")]
    public bool isPaused = false;

    [System.Serializable]
    public struct ReplayFrame
    {
        public float timeStamp;
        public Vector3 position;
        public Quaternion rotation;

        public ReplayFrame(float time, Vector3 pos, Quaternion rot)
        {
            timeStamp = time;
            position = pos;
            rotation = rot;
        }
    }

    void Awake()
    {
        playerTransform = GetComponent<Transform>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerScript = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Debug.Log("R键被按下了（无条件检测）");
        // 快捷键 R : 开始/停止录制
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isPlayingBack && !isPaused)
            {
                if (!isRecording) StartRecording();
                else StopRecording();
            }
        }

        // 快捷键 V : 回放最后录制的动作
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!isRecording && !isPlayingBack && recordingFrames.Count > 0 && !isPaused)
            {
                StartPlayback();
            }
        }

        // 快捷键 P : 暂停/恢复
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        // 录制逻辑
        if (isRecording && !isPlayingBack && !isPaused)
        {
            RecordFrame();
        }

        // 回放逻辑
        if (isPlayingBack && !isPaused)
        {
            PlaybackUpdate();
        }

        // 暂停控制
        if (isPaused)
        {
            if (playerScript != null && playerScript.enabled)
                playerScript.enabled = false;
            if (playerRigidbody != null && !playerRigidbody.isKinematic)
                playerRigidbody.isKinematic = true;
        }
        else
        {
            if (playerScript != null && !playerScript.enabled && !isPlayingBack)
                playerScript.enabled = true;
            if (playerRigidbody != null && playerRigidbody.isKinematic && !isPlayingBack)
                playerRigidbody.isKinematic = false;
        }
    }

    void StartRecording()
    {
        isRecording = true;
        recordingFrames.Clear();
        recordTimer = 0f;
        Debug.Log("开始录制");
    }
        
    void StopRecording()
    {
        isRecording = false;
        Debug.Log($"录制结束，共 {recordingFrames.Count} 帧");
    }

    void RecordFrame()
    {
        ReplayFrame frame = new ReplayFrame(recordTimer, playerTransform.position, playerTransform.rotation);
        recordingFrames.Add(frame);
        recordTimer += Time.deltaTime;
    }

    void StartPlayback()
    {
        isPlayingBack = true;
        playbackIndex = 0;
        playbackTimer = 0f;

        if (playerScript != null) playerScript.enabled = false;
        if (playerRigidbody != null) playerRigidbody.isKinematic = true;

        if (recordingFrames.Count > 0)
        {
            playerTransform.position = recordingFrames[0].position;
            playerTransform.rotation = recordingFrames[0].rotation;
        }

        Debug.Log("开始回放");
    }

    void StopPlayback()
    {
        isPlayingBack = false;
        if (playerScript != null && !isPaused) playerScript.enabled = true;
        if (playerRigidbody != null && !isPaused) playerRigidbody.isKinematic = false;
        Debug.Log("回放结束");
    }

    void PlaybackUpdate()
    {
        if (playbackIndex >= recordingFrames.Count - 1)
        {
            StopPlayback();
            return;
        }

        playbackTimer += Time.deltaTime;

        ReplayFrame current = recordingFrames[playbackIndex];
        ReplayFrame next = recordingFrames[playbackIndex + 1];

        float t = (playbackTimer - current.timeStamp) / (next.timeStamp - current.timeStamp);
        if (t >= 1.0f)
        {
            playbackIndex++;
            t = 0f;
            if (playbackIndex < recordingFrames.Count - 1)
            {
                current = recordingFrames[playbackIndex];
                next = recordingFrames[playbackIndex + 1];
            }
        }

        Vector3 interpolatedPos = Vector3.Lerp(current.position, next.position, t);
        Quaternion interpolatedRot = Quaternion.Slerp(current.rotation, next.rotation, t);
        playerTransform.position = interpolatedPos;
        playerTransform.rotation = interpolatedRot;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log(isPaused ? "游戏暂停" : "游戏恢复");
    }
}