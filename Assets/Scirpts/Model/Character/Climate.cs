using UnityEngine;
using System.Collections;

public enum Season{
	Spring,
	Summer,
	Autumn,
	Winter
}

public enum Weather{
	Sunny,
	Rain,
	Snow,
	Fog,
}

[System.Serializable]
public class Climate {
	public Season theSeason {
		get;
		set;
	}

	public Weather WeatherToday {
		get;
		set;
	}
	
	//between 0 - 40
	//the current temprature
	public float Tempature {
		get;
		set;
	}

	//the base temp of the day
	public float BaseTempature {
		get;
		set;
	}

	//between 0 - 100
	//the current humidity
	public float Humidity {
		get;
		set;
	}

	//the base humidity of the day
	public float BaseHumidity {
		get;
		set;
	}


	public Climate(){
        // the initial climate of the game
        if (Config.IsDebugMode)
        {
            this.theSeason = Season.Autumn;
            this.WeatherToday = Weather.Fog;
            this.BaseTempature = 26f;
            this.BaseHumidity = 50f;
            this.Tempature = 22f;
            this.Humidity = 60f;
        }
        else
        {

            this.theSeason = Season.Autumn;
            this.WeatherToday = Weather.Sunny;
            this.BaseTempature = 26f;
            this.BaseHumidity = 50f;
            this.Tempature = 22f;
            this.Humidity = 60f;
        }
    }

	//generate daily based climate based on day passed
	public void GenerateClimate(){
		int day_count =	(int)Game.Current.GameTime / Config.SecondsPerDay;
		int season_count = ((int)Mathf.Floor(day_count / Config.SeasonLength)) % 4; // 0, 1, 2,3
		float percent_in_season = (day_count % Config.SeasonLength) / (float)Config.SeasonLength;
        switch (season_count)
        {
            case 0:
                //autumn
                this.theSeason = Season.Autumn;
                this.BaseHumidity = Random.Range(40f, 80f);
                if (percent_in_season <= 0.3)
                {
                    //start of season
                    this.BaseTempature = Random.Range(20f, 28f);

                }
                else if (percent_in_season <= 0.8)
                {
                    //middle of season
                    this.BaseTempature = Random.Range(15f, 22f);

                }
                else
                {
                    //end of season
                    this.BaseTempature = Random.Range(5f, 10f);
                }


                if (Random.value <= 0.05)
                {
                    this.WeatherToday = Weather.Fog;
                    this.BaseHumidity += 5;
                }
                else if (Random.value <= 0.1)
                {
                    this.WeatherToday = Weather.Rain;
                    this.BaseHumidity += 10;
                }
                else
                {
                    this.WeatherToday = Weather.Sunny;
                }
                break;
            case 1:
                //winter
                this.theSeason = Season.Winter;
                this.BaseHumidity = Random.Range(10f, 50f);
                if (percent_in_season <= 0.3)
                {
                    //start of season
                    this.BaseTempature = Random.Range(-5f, 5f);

                }
                else if (percent_in_season <= 0.8)
                {
                    //middle of season
                    this.BaseTempature = Random.Range(-15f, 0f);

                }
                else
                {
                    //end of season
                    this.BaseTempature = Random.Range(-5f, 10f);
                }

                if (this.BaseTempature <= 5 && Random.value <= 0.7)
                {


                    this.WeatherToday = Weather.Snow;
                }
                else
                {
                    this.WeatherToday = Weather.Sunny;
                }
                break;
            case 2:
                //spring
                this.theSeason = Season.Spring;
                this.BaseHumidity = Random.Range(60f, 100f);
                if (percent_in_season <= 0.3)
                {
                    //start of season
                    this.BaseTempature = Random.Range(5f, 10f);

                }
                else if (percent_in_season <= 0.8)
                {
                    //middle of season1
                    this.BaseTempature = Random.Range(10f, 18f);

                }
                else
                {
                    //end of season
                    this.BaseTempature = Random.Range(15f, 25f);
                }


                float c = Random.value;
                if (c <= 0.55)
                {
                    this.WeatherToday = Weather.Rain;
                    this.BaseHumidity += 10;
                }
                else if (c <= 0.6)
                {
                    this.WeatherToday = Weather.Fog;
                    this.BaseHumidity += 5;
                }
                else
                {
                    this.WeatherToday = Weather.Sunny;
                }

                break;
            case 3:
                //summer
                this.theSeason = Season.Summer;
                this.BaseHumidity = Random.Range(0f, 40f);
                if (percent_in_season <= 0.3)
                {
                    //start of season
                    this.BaseTempature = Random.Range(20, 33f);

                }
                else if (percent_in_season <= 0.8)
                {
                    //middle of season1
                    this.BaseTempature = Random.Range(30f, 42f);
                }
                else
                {
                    //end of season
                    this.BaseTempature = Random.Range(25f, 35f);
                }

                this.WeatherToday = Weather.Sunny;

                break;

        }

	}

	//randomize weather every 5 seconds
	public void GenerateCurrentWeather(){
		if (!Game.Current.Hero.isItemEquipped (ItemType.COLD_MACHINE)
			&& !Game.Current.Hero.isItemEquipped (ItemType.WARM_MACHINE)
		    && !Game.Current.Hero.isItemEquipped (ItemType.CAMP_FIRE)
		   ) {

			float amout = 0.06f *  (this.BaseTempature - this.Tempature);
			//Debug.Log("Amount : " + amout);

			if(Mathf.Abs(amout) > 0.2f){
				if(this.BaseTempature - this.Tempature > 0){
					amout = 0.2f;
				}else{
					amout = -0.2f;
				}
			}

			if(Mathf.Abs(amout) < 0.1f){
				if(this.BaseTempature - this.Tempature > 0){
					amout = 0.1f;
				}else{
					amout = -0.1f;
				}
			}

			//Debug.Log ("Base : " + this.BaseTempature);
			//Debug.Log("Temp : " + this.Tempature);
			//Debug.Log("Diff : " + (this.BaseTempature - this.Tempature));
			//Debug.Log ("moving temparateu" + amout);

			this.Tempature += amout; 
		} else {
			if(Game.Current.Hero.isItemEquipped (ItemType.COLD_MACHINE)){
                if (this.Tempature >= 3)
                {
                    this.Tempature -= 0.3f;
                }
			}
			else if (Game.Current.Hero.isItemEquipped (ItemType.WARM_MACHINE)){
                if (this.Tempature <= 33)
                {
                    this.Tempature += 0.3f;
                }
			}
			else if (Game.Current.Hero.isItemEquipped (ItemType.CAMP_FIRE)){
                if (this.Tempature <= 33)
                {
                    this.Tempature += 0.01f;
                }
			}
		}


		if (!Game.Current.Hero.isItemEquipped (ItemType.DRY_MACHINE)
		    && !Game.Current.Hero.isItemEquipped (ItemType.HUMID_MACHINE)
		    ) {

			float amout = 0.06f *  (this.BaseHumidity - this.Humidity);
			//Debug.Log("Amount : " + amout);
			
			if(Mathf.Abs(amout) > 0.2f){
				if(this.BaseHumidity - this.Humidity > 0){
					amout = 0.2f;
				}else{
					amout = -0.2f;
				}
			}
			
			if(Mathf.Abs(amout) < 0.1f){
				if(this.BaseHumidity - this.Humidity > 0){
					amout = 0.1f;
				}else{
					amout = -0.1f;
				}
			}

			this.Humidity += amout; 
		} else {
			if(Game.Current.Hero.isItemEquipped (ItemType.DRY_MACHINE)){
                if (this.Humidity >= 20)
                {
                    this.Humidity -= 0.3f;
                }
			}
			else if (Game.Current.Hero.isItemEquipped (ItemType.HUMID_MACHINE)){
                if (this.Humidity <= 80)
                {
                    this.Humidity += 0.3f;
                }
			}
		}

		if(this.Humidity < 0){
			this.Humidity = 0;
		}


	}


}
