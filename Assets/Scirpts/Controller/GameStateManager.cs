using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine.SocialPlatforms;

public class GameStateManager : MonoBehaviour {
	private float timeToCheck = 0f;
    private AsyncOperation async;
    public Slider LoadingBar;
    public static GameMode current_mode;
    public GameObject loadingScreen;
    private bool isLoading = false;
    public GameObject LanguagePanel;
    public GameObject ChooseLanguageButton;
  //  public static bool isAndroid = true;

    
    void Awake(){
#if UNITY_IPHONE
        ChooseLanguageButton.SetActive(true);
#endif
    }


    void Start()
	{
      
        //load the static factories from the beginning
        Config.DataPath = Application.persistentDataPath;
        LoadXmlFactories(); 
       // Debug.Log(Application.persistentDataPath);
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
#elif UNITY_IPHONE
		UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
		if (!Social.localUser.authenticated) {
			Social.localUser.Authenticate ((bool success) => {
			});
		}
    }

	void Update()
	{
		if( isLoading)
		{
			if (this.isGameLoaded())
            {//Roload XML
                isLoading = false;
                //timeToCheck += 999f;
                if (Config.DoRelod)
                {
                    Game.Current.ReloadGameData();
                }
                Game.Current.CurrentGameMode = current_mode; //set the mode

                //For Debug use
                if (Config.IsDebugMode)
                {
                    GameFactory.GenerateTestData();
                }

                if (Game.Current.IsAtHome){
                    StartCoroutine(loadLevelInBG(1));
				}else{
                    StartCoroutine(loadLevelInBG(2));
                }
                //Game.Current.SaveGame ();
            }
			//timeToCheck += 0.5f; // check every 0.5 second
		}
		
	}


    IEnumerator loadLevelInBG(int level)
    {
        async = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);

        while (!async.isDone)
        {
            LoadingBar.value = async.progress;
          //  Debug.LogError(async.progress);
            yield return null;
        }
    }


    private bool isGameLoaded()
	{
		if(Game.Current != null && Shop.Current != null
            && Rank.Current != null && Achievement.Current != null)
		{
			Debug.Log("Fully loaded Event length : " + Game.Current.EventEngine.EventCount);
			return true;
		}
		else
		{
			return false;
		}
	}


    public void LoadGameInSurvivalMode()
    {
        loadingScreen.SetActive(true);
        isLoading = true;
        Game.LoadGame(GameMode.Survial);
        current_mode = GameMode.Survial;
        Shop.LoadShop();
        Rank.LoadRank();
        Achievement.LoadAchievement();
    }


    public void LoadGameInNormalMode()
    {
        loadingScreen.SetActive(true);
        isLoading = true;
        Game.LoadGame(GameMode.Normal);
        current_mode = GameMode.Normal;
        Shop.LoadShop();
        Rank.LoadRank();
        Achievement.LoadAchievement();
    }

    private void LoadXmlFactories()
    {
        Lang.ReloadXML();
        ItemFactory.Reload();
        BuildingFactory.Reload();
        ActionFactory.Reload();
        ConsequenceFactory.Reload();
        Cookbook.ReloadXML();
        StoryFactory.Reload();
    }

    public void OpenLanguageSelection()
    {
        LanguagePanel.SetActive(true);
    }


    public void SetLanguage(int lang)
    {
        PlayerPrefs.SetInt("Lang", lang);
        PlayerPrefs.Save();
        Lang.ReloadXML();
        LanguagePanel.SetActive(false);
        SceneManager.LoadScene(0);
    }


    


    private void loadAndroidLeaderBoard()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                // handle success or failure
                if (success)
                {

                    //post all leader board scores
                    if (Rank.Current.DayCountNormal > 0)
                    {
                        Social.ReportScore(Rank.Current.DayCountNormal, "CgkIop-T-Y4IEAIQAg", (bool s1) =>
                        {
                            // handle success or failure         
                        });
                    }


                    if (Rank.Current.DayCountSurvival > 0)
                    {
                        Social.ReportScore(Rank.Current.DayCountSurvival, "CgkIop-T-Y4IEAIQAQ", (bool s1) =>
                        {
                            // handle success or failure         
                        });
                    }

                    if (Rank.Current.EvilCountNormal > 0)
                    {
                        Social.ReportScore(Rank.Current.EvilCountNormal, "CgkIop-T-Y4IEAIQAw", (bool s1) =>
                        {
                            // handle success or failure         
                        });
                    }

                    if (Rank.Current.EvilCountSurvival > 0)
                    {
                        Social.ReportScore(Rank.Current.EvilCountSurvival, "CgkIop-T-Y4IEAIQBA", (bool s1) =>
                        {
                            // handle success or failure         
                        });
                    }

                    Social.ShowLeaderboardUI();

                }
            });
        }
        else
        {
            //already authenticated
            //post all leader board scores
            Social.ReportScore(Rank.Current.DayCountNormal, "CgkIop-T-Y4IEAIQAg", (bool s1) =>
            {
                // handle success or failure         
            });

            Social.ReportScore(Rank.Current.DayCountSurvival, "CgkIop-T-Y4IEAIQAQ", (bool s1) =>
            {
                // handle success or failure         
            });

            Social.ReportScore(Rank.Current.EvilCountNormal, "CgkIop-T-Y4IEAIQAw", (bool s1) =>
            {
                // handle success or failure         
            });

            Social.ReportScore(Rank.Current.EvilCountSurvival, "CgkIop-T-Y4IEAIQBA", (bool s1) =>
            {
                // handle success or failure         
            });

            Social.ShowLeaderboardUI();
        }
    }

    private void loadIOSLeaderBoard()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                // handle success or failure
                if (success)
                {

                    //post all leader board scores
                    if (Rank.Current.DayCountNormal > 0)
                    {
                        Social.ReportScore(Rank.Current.DayCountNormal, "com.thegsstorm.wildhunter.normalmode.longest", (bool s1) =>
                        {
                            // handle success or failure         
                        });
                    }


                    if (Rank.Current.DayCountSurvival > 0)
                    {
                        Social.ReportScore(Rank.Current.DayCountSurvival, "com.thegsstorm.wildhunter.survivalmode.longest", (bool s1) =>
                        {
                            // handle success or failure         
                        });
                    }

                    if (Rank.Current.EvilCountNormal > 0)
                    {
                        Social.ReportScore(Rank.Current.EvilCountNormal, "com.thegsstorm.wildhunter.normalmode.evilcloistermostlevel", (bool s1) =>
                        {
                            // handle success or failure         
                        });
                    }

                    if (Rank.Current.EvilCountSurvival > 0)
                    {
                        Social.ReportScore(Rank.Current.EvilCountSurvival, "com.thegsstorm.wildhunter.survivalmode.evilcloistermostlevel", (bool s1) =>
                        {
                            // handle success or failure         
                        });
                    }

                    Social.ShowLeaderboardUI();

                }
            });
        }
        else
        {
            //already authenticated
            //post all leader board scores
            Social.ReportScore(Rank.Current.DayCountNormal, "com.thegsstorm.wildhunter.normalmode.longest", (bool s1) =>
            {
                // handle success or failure         
            });

            Social.ReportScore(Rank.Current.DayCountSurvival, "com.thegsstorm.wildhunter.survivalmode.longest", (bool s1) =>
            {
                // handle success or failure         
            });

            Social.ReportScore(Rank.Current.EvilCountNormal, "com.thegsstorm.wildhunter.normalmode.evilcloistermostlevel", (bool s1) =>
            {
                // handle success or failure         
            });

            Social.ReportScore(Rank.Current.EvilCountSurvival, "com.thegsstorm.wildhunter.survivalmode.evilcloistermostlevel", (bool s1) =>
            {
                // handle success or failure         
            });

            Social.ShowLeaderboardUI();
        }
    }

    public void OpenRankScreen()
    {
        Rank.LoadRank();
#if UNITY_ANDROID
        loadAndroidLeaderBoard();
#elif UNITY_IPHONE
        loadIOSLeaderBoard();
#endif
        /*
        if (isAndroid)
        {
            loadAndroidLeaderBoard();
        }
        else
        {
            loadIOSLeaderBoard();
        }
        */
    }


    public void OpenFacebookPage()
    {
        StartCoroutine(OpenFacebook());
    }


    IEnumerator OpenFacebook()
    {
#if UNITY_ANDROID
        Application.OpenURL("fb://page/830235143757310");
#elif UNITY_IPHONE
        Application.OpenURL("fb://profile/830235143757310");
#endif
        yield return new WaitForSeconds(1);
        if (leftApp)
        {
            leftApp = false;
        }
        else
        {
            Application.OpenURL("https://www.facebook.com/GSStorm-830235143757310/?ref=hl");
        }
    }

    bool leftApp = false;

    void OnApplicationPause()
    {
        leftApp = true;
    }


    public void OpenRatingPage()
    {

#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.gsstorm.survival");
#elif UNITY_IPHONE
		Application.OpenURL("itms-apps://itunes.apple.com/app/id1084944146");
#endif

        /*
        if (isAndroid)
        {
            Application.OpenURL("market://details?id=com.gsstorm.survival");
        }
        else
        {
            Application.OpenURL("itms-apps:itunes.apple.com/app/id1032617047");
        }
        */
    }

}
