using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    public Button backButton;
    public Button enableMapGenerationBtn;
    public Text enableMapGenerationText;

    private OptionManager optionManager;


    void Start()
    {
        optionManager = GameObject.Find(ObjectName.optionManager).GetComponent<OptionManager>();

        backButton.onClick.AddListener(Back);
        enableMapGenerationBtn.onClick.AddListener(swapGenerationMapOption);

        updateBtnText();
    }

    private void updateBtnText()
    {
        if (this.optionManager.shouldGenerateMap)
        {
            enableMapGenerationText.text = "Desactiver la generation de carte automatique";
        }
        else
        {
            enableMapGenerationText.text = "Activer la generation de la carte automatique";
        }
    }

    private void swapGenerationMapOption()
    {
        this.optionManager.shouldGenerateMap = !this.optionManager.shouldGenerateMap;
        updateBtnText();
    }

    private void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
