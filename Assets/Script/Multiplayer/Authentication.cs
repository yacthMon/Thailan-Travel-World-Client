using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Facebook.Unity;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
public class Authentication : MonoBehaviour {
    public static Authentication Instance;
    public bool fbLogin;
    public string fbName;
    public Sprite fbDisplay;
    private string fbId;
    private string fbToken;
	void Start () {
        if (!Instance) {
            Instance = this as Authentication;
        }	
	}
    void Awake() {
        if (!FB.IsInitialized) {
            // Initialize the Facebook SDK
            FB.Init(InitCallback , OnHideUnity);
        } else {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback() {
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown) {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }


    public void LoginWithFacebook() {
        var perms = new List<string>() { "public_profile" , "email" , "user_friends" };
        FB.LogInWithReadPermissions(perms , AuthCallback);
        Popup.Instance.ShowLoading();
    }

    private void AuthCallback(ILoginResult result) {
        if (FB.IsLoggedIn) {            
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;                        
            Debug.Log("Facebook id : " + aToken.UserId);            
            Debug.Log("Facebook token : " + aToken.TokenString);
            Debug.Log("Facebook token : " + aToken.ExpirationTime);
            fbId = aToken.UserId;
            fbToken = aToken.TokenString;
            fbLogin = true; 
            FB.API("/me?fields=name" , HttpMethod.GET , GetFacebookName);
            FB.API("/me/picture?type=square&height=64&width=64" , HttpMethod.GET , GetFacebookDisplay);
            // Move below to CheckDataBeforeSending(); 
            //DGTRemote.GetInstance().RequestAuthenticationFacebook(aToken.UserId, aToken.TokenString);
            /*Debug.Log("Permissions");
             foreach (string perm in aToken.Permissions) {
                Debug.Log(perm);
            }*/
        } else {
            Popup.Instance.HideLoading();            
            Debug.Log("User cancelled login");
        }
    }

    public void GetFacebookName(IResult result) {
        if (result.Error == null) {
            this.fbName = result.ResultDictionary["name"].ToString();            
            CheckDataBeforeSending();
        } else {
            Popup.Instance.HideLoading();
            Popup.Instance.showPopup("Facebook Login" , "เกิดข้อผิดพลาดขณะดึงข้อมูล.");
            Debug.Log("Facebook : "+result.Error + " at first name");
        }
    }

    public void GetFacebookDisplay(IGraphResult result) {
        if (result.Error == null) {
            if (result.Texture != null) {                
                this.fbDisplay = Sprite.Create(result.Texture , new Rect(0 , 0 , 64 , 64) , new Vector2());                
                Debug.Log("Facebook get display success " + this.fbDisplay);
                CheckDataBeforeSending();
            }
        } else {
            Popup.Instance.HideLoading();
            Popup.Instance.showPopup("Facebook Login" , "เกิดข้อผิดพลาดขณะดึงข้อมูล.");
            Debug.Log("Facebook : " + result.Error + " at display profile");
        }
    }    
    
    public void CheckDataBeforeSending() {
        if(this.fbName!=null && this.fbDisplay!=null) {
            Popup.Instance.HideLoading();
            Debug.Log("Data facebook ready");
            DGTRemote.GetInstance().RequestAuthenticationFacebook(fbId , fbToken);
        }
            
    }
    public void RegisterFacebookData() {
        FB.API("/me?fields=email,gender" , HttpMethod.GET , GetFacebookData);
    }

    public void GetFacebookData(IResult result) {
        if (result.Error == null) {
            try {
                string email = result.ResultDictionary["email"].ToString();
                string gender = result.ResultDictionary["gender"].ToString();
                DGTRemote.Instance.RequestRegisterFacebookData(email,gender);
            } catch (Exception e) {
                Popup.Instance.showPopup("Facebook Login" , "Error during getting Facebook data\r\n"+e.ToString());
            }            
        } else {
            Popup.Instance.showPopup("Facebook Login" , "Error Facebook Register \r\nไม่สามารถสร้าง Account ด้วย Facebook ได้");
        }
    }

    public void LogoutFacebook() {
        Debug.Log("LogoutFacebook init=" + FB.IsInitialized + " login=" + FB.IsLoggedIn);
        if (!FB.IsInitialized) {
            FB.Init(InitCallbackLogout , OnInitHideUnity);
        } else if (FB.IsInitialized && FB.IsLoggedIn) {
            fbId = "";
            fbToken = "";
            FB.LogOut();            
        } else {
            Debug.Log("LogoutFacebook else case " + FB.IsInitialized + " " + FB.IsLoggedIn);
        }
    }

    private void OnInitHideUnity(bool isGameShown) {
        if (isGameShown)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    private void InitCallbackLogout() {
        if (FB.IsInitialized) {
            FB.LogOut();            
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

#region Normal Login

    public void DoNormalLogin() {
        string username = GameObject.Find("inputUsername").GetComponent<InputField>().text;
        string password = GameObject.Find("inputPassword").GetComponent<InputField>().text;
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)) {
            DGTRemote.Instance.RequestAuthentication(username , GetMd5Hash(password));
        } else {
            GameObject.Find("Login button").GetComponent<Button>().interactable = true;
            Popup.Instance.showPopup("การเข้าสู่ระบบ" , "กรุณากรอกข้อมูลให้ครบถ้วน");
        }
    }

    public void DoRegister() {
        string username = GameObject.Find("registerUsername").GetComponent<InputField>().text;
        string password = GameObject.Find("registerPassword").GetComponent<InputField>().text;
        string email = GameObject.Find("registerEmail").GetComponent<InputField>().text;
        string gender = GameObject.Find("registerGender").GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) &&
            !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(gender)) {
            if (string.Compare(password , GameObject.Find("registerPasswordRetype").GetComponent<InputField>().text) == 0) {
                if (IsValidEmail(email)) {
                    password = GetMd5Hash(password);
                    DGTRemote.Instance.RequestRegister(username , password , email , gender);
                } else {
                    Popup.Instance.showPopup("การสมัครบัญชี" , "Email ที่คุณกรอกไม่ถูกต้อง");
                }
            } else {
                Popup.Instance.showPopup("การสมัครบัญชี" , "รหัสที่กรอกไม่ตรงกัน");
            }
        } else {
            Popup.Instance.showPopup("การสมัครบัญชี" , "กรุณากรอกข้อมูลให้ครบ");
            GameObject.Find("btnRegister").GetComponent<Button>().interactable = true;
        }
    }
#endregion


    #region Tool
    public static string GetMd5Hash(string input) {
        MD5 md5Hash = MD5.Create();
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++) {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    bool IsValidEmail(string email) {
        return Regex.IsMatch(email , @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z" , RegexOptions.IgnoreCase);
    }

    #endregion
}
