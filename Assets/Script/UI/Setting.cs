using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider masterVolSlider, musicVolSlider, soundVolSlider;
	void Start() {
        float saveMasterVol = PlayerPrefs.GetFloat("Settings.MasterVolum"),
            saveMusicVol = PlayerPrefs.GetFloat("Settings.MusicVolum"),
            saveSoundVol = PlayerPrefs.GetFloat("Settings.SoundVolum"),
            saveNPCVol = PlayerPrefs.GetFloat("Settings.NPCVolum");
        masterVolSlider.value = saveMasterVol;        
        musicVolSlider.value = saveMusicVol;        
        soundVolSlider.value = saveSoundVol;
        // volum setting move to ApplicationSetting.cs
        //am.SetFloat("masterVol" , saveMasterVol);
        //am.SetFloat("musicVol" , saveMasterVol);
        //am.SetFloat("soundVol" , saveSoundVol);
        //am.SetFloat("npcVol" , saveNPCVol);
    }

    public void changeMasterVol(float vol) {
        audioMixer.SetFloat("masterVol" , vol);
        PlayerPrefs.SetFloat("Settings.MasterVolum" , vol);
    }
	
    public void changeMusicVol(float vol) {
        audioMixer.SetFloat("musicVol" , vol);
        PlayerPrefs.SetFloat("Settings.MusicVolum" , vol);
    }

    public void changeSoundVol(float vol) {
        audioMixer.SetFloat("soundVol" , vol);
        audioMixer.SetFloat("npcVol" , vol);
        PlayerPrefs.SetFloat("Settings.SoundVolum" , vol);
        PlayerPrefs.SetFloat("Settings.NPCVolum" , vol);
    }

    public void Logout() {
        DGTRemote.GetInstance().Logout();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
    }
}
