using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AndroidKeyboard : MonoBehaviour {
    private TouchScreenKeyboard keyboard;
    public bool password,email,social;
    

    private void Start() {
        //keyboard = TouchScreenKeyboard.Open("" , TouchScreenKeyboardType.Default , false , false , true);
        
        if (password)
        this.gameObject.GetComponent<InputField>().keyboardType = TouchScreenKeyboardType.Default;
        else if(email)
            this.gameObject.GetComponent<InputField>().inputType = InputField.InputType.Standard;
        else if(social)
            this.gameObject.GetComponent<InputField>().inputType = InputField.InputType.AutoCorrect;
    }
}
