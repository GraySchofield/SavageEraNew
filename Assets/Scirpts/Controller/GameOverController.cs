using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {
    public Text TotalDays;
    private AsyncOperation async;
    public Slider LoadingBar;
    public GameObject loadingScreen;
    public CanvasGroup GroupButtonResume;
    private bool isReloading = false;
    public Button RestartButton;
    public Button ResumeButton;
    // Use this for initialization

    public GameObject CongraduationText;
    public Animator DayTextAnimator;

    public GameObject RestartConfirmPanel;

    void Start () {
        int day_passed = (int)Game.Current.GameTime / Config.SecondsPerDay;
        TotalDays.text = Lang.Current["day_count"] + day_passed;
        if (Game.Current.CurrentGameMode == GameMode.Normal)
        {
            //normal mode
            GroupButtonResume.interactable = true;
            GroupButtonResume.alpha = 1;
            if(day_passed > Rank.Current.DyingDayCountNormal)
            {
                CongraduationText.SetActive(true);
                DayTextAnimator.SetTrigger("BreakRecord");
            }

        }
        else
        {
            //survival mode
            if (day_passed > Rank.Current.DyingDayCountSurvival)
            {
                CongraduationText.SetActive(true);
                DayTextAnimator.SetTrigger("BreakRecord");
            }

        }

        Rank.Current.UpdateDyingDayCount(day_passed, Game.Current.CurrentGameMode);

    }


    public void RestartWithConfirm()
    {
       if(Game.Current.CurrentGameMode == GameMode.Normal)
        {
            //show confirm dialog
            RestartConfirmPanel.SetActive(true);
        }
        else
        {
            //survival mode, just restart 
            RestartGame();
        }
    }

    public void CancelRestart()
    {
        RestartConfirmPanel.SetActive(false);
    }


    public void RestartGame()
    {
        if (!isReloading)
        {
            RestartButton.enabled = false;
            loadingScreen.SetActive(true);
            Game.ResetGame();
            Game.LoadGame(GameStateManager.current_mode);
            isReloading = true;
           // InvokeRepeating("ReStartLevelifLoaded", 0, 0.5f);
        }
    }

    public void ResumeGame()
    {
        if (!isReloading)
        {
            ResumeButton.enabled = false;
            loadingScreen.SetActive(true);
            Game.ClearGame();
            Game.LoadBackUpGame();
            isReloading = true;
           // InvokeRepeating("ReStartLevelifLoaded", 0, 0.5f);
        }
    }


    private bool isGameLoaded()
    {
        if (Game.Current != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void Update()
    {
        if (isGameLoaded() && isReloading)
        {
            isReloading = false;
            Game.Current.ReloadGameData();

            if (Game.Current.IsAtHome)
            {
                StartCoroutine(loadLevelInBG(1));
            }
            else
            {
                StartCoroutine(loadLevelInBG(2));
            }

        }
    }


    private void ReStartLevelifLoaded()
    {
        if (isGameLoaded())
        {          
                //reinject the events 
                Game.Current.ReloadGameData();

                if (Game.Current.IsAtHome)
                {
                    StartCoroutine(loadLevelInBG(1));
                }
                else
                {
                    StartCoroutine(loadLevelInBG(2));
                }
                CancelInvoke();   
        }
    }


    IEnumerator loadLevelInBG(int level)
    {
        async = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);

        while (!async.isDone)
        {
            LoadingBar.value = async.progress;
            yield return null;
        }
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
        if (GameStateManager.isAndroid)
        {
            loadAndroidLeaderBoard();
        }
        else
        {
            loadIOSLeaderBoard();
        }
        */
    }

}
