using UnityEngine;
using UnityEngine.Audio;

public class ApplicationSetting : MonoBehaviour {
    [SerializeField]
    private AudioMixer audioMixer;
	void Start () {
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        // Sound settings
        audioMixer.SetFloat("masterVol" , PlayerPrefs.GetFloat("Settings.MasterVolum"));
        audioMixer.SetFloat("musicVol" , PlayerPrefs.GetFloat("Settings.MusicVolum"));
        audioMixer.SetFloat("soundVol" , PlayerPrefs.GetFloat("Settings.SoundVolum"));
        audioMixer.SetFloat("npcVol" , PlayerPrefs.GetFloat("Settings.NPCVolum"));
    }
}
