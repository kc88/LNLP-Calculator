using UnityEngine;
using UnityEngine.UI;

public class GameManager :MonoBehaviour
{
    [SerializeField] private GameObject textBlock1;
    [SerializeField] private GameObject textBlock2;
    [SerializeField] private GameObject textBlock3;
    private Text textBlockText1;
    private Text textBlockText2;
    private Text textBlockText3;
    private string solution = "";

    private DataBase GameDateStates;
    private PlayerDataState PDS;

    private void Awake()
    {
        GameDateStates = new DataBase();
        GameDateStates.LoadGame();
        PDS = GameDateStates.playerDataState;
        textBlockText1 = textBlock1.GetComponentInChildren<Text>();
        textBlockText2 = textBlock2.GetComponentInChildren<Text>();
        textBlockText3 = textBlock3.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        DisplayText();
    }

    private void DisplayText()
    {
        textBlockText1.text = PDS.lnlpNum1.ToDisplay();
        textBlockText2.text = PDS.lnlpNum2.ToDisplay();
        textBlockText3.text = solution + PDS.lnlpNum3.ToDisplay();
    }

    public void SaveGameState()
    {
        GameDateStates.SaveGame();
    }

    public void DeleteGameState()
    {
        GameDateStates.DeleteGame();
    }

    #region Number1
    public void MultiplyGameValue1(float value)
    {
        PDS.lnlpNum1 *= value;
        DisplayText();
    }

    public void DivideGameValue1(float value)
    {
        PDS.lnlpNum1 /= value;
        DisplayText();
    }

    public void PowerGameValue1(float value)
    {
        PDS.lnlpNum1 = LNLP.Pow(PDS.lnlpNum1, value);
        DisplayText();
    }
    #endregion

    #region Number2
    public void MultiplyGameValue2(float value)
    {
        PDS.lnlpNum2 *= value;
        DisplayText();
    }

    public void DivideGameValue2(float value)
    {
        PDS.lnlpNum2 /= value;
        DisplayText();
    }

    public void PowerGameValue2(float value)
    {
        PDS.lnlpNum2 = LNLP.Pow(PDS.lnlpNum1, value);
        DisplayText();
    }
    #endregion

    #region Number3
    public void MultiplyTogether()
    {
        PDS.lnlpNum3 = PDS.lnlpNum1 * PDS.lnlpNum2;
        solution = PDS.lnlpNum1 + " * " + PDS.lnlpNum2 + " = ";
        DisplayText();
    }

    public void DivideTogether()
    {
        PDS.lnlpNum3 = PDS.lnlpNum1 / PDS.lnlpNum2;
        solution = PDS.lnlpNum1 + " / " + PDS.lnlpNum2 + " = ";
        DisplayText();
    }

    public void PowerTogether()
    {
        PDS.lnlpNum3 = LNLP.Pow( PDS.lnlpNum1, (float)PDS.lnlpNum2);
        solution = "LNL.Pow( " + PDS.lnlpNum1 + ", " + PDS.lnlpNum2 + ") = ";
        DisplayText();
    }

    public void AddTogether()
    {
        PDS.lnlpNum3 = PDS.lnlpNum1 + PDS.lnlpNum2;
        solution = PDS.lnlpNum1 + " + " + PDS.lnlpNum2 + " = ";
        DisplayText();
    }

    public void SubtractTogether()
    {
        PDS.lnlpNum3 = PDS.lnlpNum1 - PDS.lnlpNum2;
        solution = PDS.lnlpNum1 + " - " + PDS.lnlpNum2 + " = ";
        DisplayText();
    }

    public void ModulusTogether()
    {
        //PDS.lnlpNum3 = PDS.lnlpNum1 % PDS.lnlpNum2;
        solution = PDS.lnlpNum1 + " % " + PDS.lnlpNum2 + " = ";
        DisplayText();
    }
    #endregion
}
