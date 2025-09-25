using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance;

    public string webcientId = "AIzaSyBIri6hR3UEoRI91NmeUc3TYD52phb6g8U";
    private FirebaseAuth Auth;

    private async void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        var deps = await Firebase.FirebaseApp.CheckDependenciesAsync();
        if(deps != DependencyStatus.Available)
        {
            Debug.LogError("Could no resolve dependencies" + deps);
            return;
        }

        Auth = FirebaseAuth.DefaultInstance;
        Debug.Log("Firebase Initialized");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignInWithGoogle()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration()
        {
            WebClientId = webcientId,
            RequestIdToken = true,
            RequestEmail = true,
            UseGameSignIn = false
        };

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(OnGoogleSignFinished);
    }

    private void OnGoogleSignFinished(System.Threading.Tasks.Task<GoogleSignInUser> userTask)
    {
        if (userTask.IsCanceled)
        {
            Debug.LogError("Google signin is cancled");
            return;
        }

        if (userTask.IsFaulted)
        {
            Debug.LogError("Google signin failed" + userTask.Exception.ToString());
            return;
        }

        Debug.Log("Google Signin sucess");
    }
}
