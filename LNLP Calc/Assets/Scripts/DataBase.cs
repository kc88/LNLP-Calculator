using UnityEngine;

public enum DataStates
{
    PlayerDataState = 0,
    Total
};

public class DataBase
{
    // Saved Data States
    public PlayerDataState playerDataState;

    // Time Between last log in
    private float secondsBetweenLogin = 0;
    private string secondsGoneKey = "secondsGoneKey";

    public void LoadGame()
    {
        CreateNewClasses();

        string json = PlayerPrefs.GetString( SavedFileName(DataStates.PlayerDataState) );
        JsonUtility.FromJsonOverwrite(json, playerDataState);

        // Calculate the time offline
        secondsBetweenLogin = SecondsSince2015() - PlayerPrefs.GetFloat(secondsGoneKey, SecondsSince2015());
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString( SavedFileName(DataStates.PlayerDataState), JsonUtility.ToJson(playerDataState) );

        // Save the seconds since 2015
        PlayerPrefs.SetFloat(secondsGoneKey, SecondsSince2015());
    }

    public void DeleteGame()
    {
        PlayerPrefs.DeleteAll();
        CreateNewClasses();
    }

    public float GetTotalSecondsOffline()
    {
        return secondsBetweenLogin;
    }

    private void CreateNewClasses()
    {
        playerDataState = new PlayerDataState();
    }

    private string SavedFileName(DataStates DS)
    {
        string fileNum = "001";
        string s;
        switch (DS)
        {
            case DataStates.PlayerDataState:       s = "PlayerDataState"; break;
            default: s = "Undefined"; break;
        }
        return (s + fileNum);

    }

    private float SecondsSince2015()
    {
        System.DateTime epochStart = new System.DateTime(2015, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (float)(System.DateTime.UtcNow - epochStart).TotalSeconds;
    }

}