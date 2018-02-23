using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation : MonoBehaviour {
    [SerializeField]
    public string[] dialogue;
    [SerializeField]
    public AudioClip[] voice;

    public int step { get; set; }
    public bool fade;
    public Conversation() {
        dialogue = null;
        step = -1;
    }
    public Conversation(string[] d) {
        this.dialogue = d;
        this.voice = null;
        step = -1;
    }
    public Conversation(string[] d, AudioClip[] a) {
        this.dialogue = d;
        this.voice = a;
        step = -1;
    }
    public string nextDialogue(){
        return dialogue[++step];
    }

    public bool isConversationEnd()    {
        return dialogue.Length-1 == step;
    }

    public void resetStep()    {
        step = -1;
    }
    public bool next() {
        if (!isConversationEnd()) {// ถ้า Conversation ยังไม่หมด ให้ เพิ่ม step และ return true
            step++;
            return true;
        }
        //Conversation หมดแล้ว
        if (fade) {// เช็คว่า Conversation นี้ ต้อง fade ไหม ? 
            FadeScreen fs = FadeScreen.Instance;
            fs.fadeOut(3);// สั่ง fadeOut และให้ fadeIn หลังจากผ่านไป 3 วิ
        }
        
        return false;
    }
    public string getDialogue() {
        return dialogue[step];
    }
    public string getDialogue(int step)    {
        return dialogue[step];
    }
    public AudioClip getClip() {
        return voice!=null?voice[step]:null;
    }
    public AudioClip getClip(int step) {
        return voice != null ? voice[step] : null;
    }

}

