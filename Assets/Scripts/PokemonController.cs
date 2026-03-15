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

    public TMP_Text LeaderBoardText;
    public TMP_Text PlayerLabel;
    public TMP_Text PokemonName;
    public TMP_InputField PlayerId;
    public TMP_InputField PokemonId;
    public Image PokemonImage;
    public Button PokemonBtn;

    public Button PlayerUpdateBtn;

    private string LeaderBoardPrefix;
    private string PlayerName;

    private Dictionary<string, int> leaderBoard = new Dictionary<string, int>();

    private void Start()
    {
        LeaderBoardPrefix = LeaderBoardText.text;
        PlayerName = PlayerId.placeholder.GetComponent<TMP_Text>().text;

        LoadPokemon(PokemonId.placeholder.GetComponent<TMP_Text>().text);

        PokemonId.onSubmit.AddListener((value) =>
        {
            LoadPokemon(value);
        });

        PokemonBtn.onClick.AddListener(() =>
        {
            LoadLeaderBoard();
        });

        PlayerUpdateBtn.onClick.AddListener(() =>
        {
            PlayerName = PlayerId.text;
            PlayerLabel.text = "Name: " + PlayerName;
        });
    }

    private void LoadPokemon(string id)
    {
        PokemonInfo pokemonInfo = GetPokemonById(id);
        PokemonName.text = pokemonInfo.name;
        StartCoroutine(LoadImage(pokemonInfo.sprites.front_default));
    }

    private PokemonInfo GetPokemonById(string id)
    {
        string url = $"https://pokeapi.co/api/v2/pokemon/{id}/";

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();

            return JsonUtility.FromJson<PokemonInfo>(jsonResponse);
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to fetch Pokemon #{id}");
            return null;
        }
    }

    private IEnumerator LoadImage(string imageUrl)
    {
        using (WWW www = new WWW(imageUrl))
        {
            yield return www;
            PokemonImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    private void LoadLeaderBoard()
    {
        if (leaderBoard.ContainsKey(PlayerName))
        {
            leaderBoard[PlayerName]++;
        }
        else
        {
            leaderBoard.Add(PlayerName, 1);
        }

        int itr = 0;
        LeaderBoardText.text = LeaderBoardPrefix;
        foreach (var entry in leaderBoard)
        {
            LeaderBoardText.text += $"\n{itr++}. {entry.Key}: {entry.Value}";
        }
    }
}
