using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance; // Singleton
    public string PlayerName; // Pelaajan nimi
    public string HighScoreName; // Enn‰tystuloksen tekij‰
    public int HighScore; // Enn‰tystulos
    public TextMeshProUGUI highman;
    public TextMeshProUGUI scoreman;

    public InputField PlayerNameInputField; // Ved‰ InputField Unity-editorista t‰h‰n

    private void Awake()
    {
        // Singleton-logiikka
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData(); // Ladataan tallennettu data sovelluksen k‰ynnistyess‰
    }
    private void Start()
    {
        if (highman != null)
        {
            highman.text = HighScoreName; // Aseta korkean pistem‰‰r‰n tekij‰n nimi
        }
        if (scoreman != null)
        {
            scoreman.text = HighScore.ToString(); // Aseta korkein pistem‰‰r‰
        }
    }

    public void SaveNameAndStartGame()
    {
        // Hae teksti InputFieldist‰
        if (PlayerNameInputField != null)
        {
            PlayerName = PlayerNameInputField.text;
        }

        if (!string.IsNullOrEmpty(PlayerName))
        {
            Debug.Log("Pelaajan nimi tallennettu: " + PlayerName);
            SceneManager.LoadScene("main"); // Lataa p‰‰kohtaus
        }
        else
        {
            Debug.LogWarning("Pelaajan nimi on tyhj‰!");
        }
    }

    public void SaveData()
    {
        SaveFileData data = new SaveFileData
        {
            HighScore = HighScore,
            HighScoreName = HighScoreName
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        Debug.Log("Data tallennettu: " + json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveFileData data = JsonUtility.FromJson<SaveFileData>(json);

            HighScore = data.HighScore;
            HighScoreName = data.HighScoreName;

            Debug.Log("Data ladattu: " + json);
        }
        else
        {
            Debug.Log("Tallennustiedostoa ei lˆytynyt.");
        }
    }
    public void ClearSaveData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save data cleared!");
        }
        else
        {
            Debug.LogWarning("No save file to delete.");
        }

        // Tyhjennet‰‰n myˆs muistin arvot
        HighScore = 0;
        HighScoreName = "";
    }

}

// Sis‰kk‰inen luokka, joka m‰‰rittelee tallennettavan datan rakenteen
[System.Serializable]
class SaveFileData
{
    public int HighScore;
    public string HighScoreName;
}
