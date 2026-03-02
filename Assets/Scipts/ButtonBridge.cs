using UnityEngine;

public class ButtonBridge : MonoBehaviour
{
    // Tuto metodu nastavíš na tlačítko START v Menu
    public void StartGame()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.StartGame();
    }

    // Tuto metodu nastavíš na tlačítko KONEC v Menu
    public void QuitGame()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.QuitGame();
    }

    // Tuto metodu nastavíš na tlačítko MENU v Pauze
    public void ReturnToMenu()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.ReturnToMainMenu();
    }

    // Tuto metodu nastavíš na tlačítko RESUME v Pauze
    public void ResumeGame()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.ResumeGame();
    }
}