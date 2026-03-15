using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.IO;
using UnityEngine.UI;

public class PokemonController : MonoBehaviour
{
    [Serializable]
    public class PokemonSprite
    {
        public string front_default;
        public string front_shiny;
    }
    
    [Serializable]
    public class PokemonInfo
    {
        public int id;
        public string name;
        public PokemonSprite sprites;
    }

    public TMP_Text PokemonName;
    public TMP_InputField PokemonId;
    public Image PokemonImage;
    public Button PokemonBtn;

    private void Start()
    {
        PokemonInfo weatherInfo = GetPokemonById();

        Debug.Log($"Name: {weatherInfo.name}, Id: {weatherInfo.id}, Image: {weatherInfo.sprites.front_default}");

        //CityText.text = $"City: {weatherInfo.name}";
        //WeatherText.text = $"{weatherInfo.weather[0].main} and {weatherInfo.weather[0].description}";
        //TemperatureText.text = $"Temperature: {weatherInfo.main.temp - 273.15f}";

        //currentTimeDelay = updateTimeDelay;
    }

    private PokemonInfo GetPokemonById()
    {
        string id = PokemonId.text;
        string url = $"https://pokeapi.co/api/v2/pokemon/{id}/";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();

        return JsonUtility.FromJson<PokemonInfo>(jsonResponse);
    }
}
