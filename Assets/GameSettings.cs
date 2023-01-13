using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettings:MonoBehaviour
{
    [SerializeField]int _maxPlayers = 8;
    [SerializeField]int _maxBotsDifficulty = 4;
    [SerializeField]int _minInARowToWin = 3;
    [SerializeField]int _maxInARowToWin = 5;
    [SerializeField]int _minFieldSize = 3;
    [SerializeField]int _maxFieldSize = 10;

    private int _playersCount = 2;
    private int _fieldSize = 3;
    private int _botsCount = 0;
    private int _inARowToWin = 3;
    private int _botsDifficulty = 1;

    [SerializeField]Slider _playersCountSlider;
    [SerializeField]Slider _botsCountSlider;
    [SerializeField]Slider _botsDifficultySlider;
    [SerializeField]Slider _inARowToWinSlider;
    [SerializeField]Slider _fieldSizeSlider;

    [SerializeField]TextMeshProUGUI _playersUI;
    [SerializeField]TextMeshProUGUI _fieldSizeUI;
    [SerializeField]TextMeshProUGUI _botsCountUI;
    [SerializeField]TextMeshProUGUI _inARowToWinUI;
    [SerializeField]TextMeshProUGUI _botDifficultyUI;

    [SerializeField]Game game;

    private void Awake() 
    {
        _fieldSizeSlider.minValue = _minFieldSize;
        _fieldSizeSlider.maxValue = _maxFieldSize;
        _botsCountSlider.minValue = 0;
        _botsCountSlider.maxValue = _playersCount;
        _inARowToWinSlider.minValue = _minInARowToWin;
        _botsDifficultySlider.maxValue = _maxBotsDifficulty;
        _botsDifficultySlider.minValue = 1;
        _botDifficultyUI.text = _botsDifficulty.ToString();
        UpdateFieldSize();
        ApplyGameSettings();
    }
    public void ApplyGameSettings()
    {
        game.AdjustGameSettings(_playersCount,_fieldSize,_inARowToWin,_botsCount,_botsDifficulty);
    }
    private void UpdatePlayersCount()
    {
        _playersCountSlider.minValue = 2;
        int maxPlayers = _maxPlayers-9+_fieldSize;
        if (maxPlayers<2)
            maxPlayers = 2;
        if (maxPlayers>_maxPlayers)
            maxPlayers = _maxPlayers;
        if (_playersCountSlider.value>maxPlayers)
            _playersCountSlider.value = maxPlayers;
        _playersCountSlider.maxValue = maxPlayers;
        _playersCount = (int)_playersCountSlider.value;
        _playersUI.text = _playersCount.ToString();
        if (maxPlayers==2)
            _playersCountSlider.direction = Slider.Direction.RightToLeft;
        else
            _playersCountSlider.direction = Slider.Direction.LeftToRight;
        UpdateBotsCount();
    }
    private void UpdateBotsCount()
    {
        _botsCountSlider.maxValue = _playersCount;
        _botsCount = (int)_botsCountSlider.value;
        if (_botsCount>_playersCount)
            _botsCount = _playersCount;
        _botsCountUI.text = _botsCount.ToString();
    }
    private void UpdateFieldSize()
    {
        _fieldSizeUI.text = _fieldSize.ToString();
        UpdatePlayersCount();
        UpdateInARowToWin();
    }
    private void UpdateInARowToWin()
    {
        int maxInARowToWin = _fieldSize;
        if (maxInARowToWin>_maxInARowToWin)
            maxInARowToWin = _maxInARowToWin;
        _inARowToWinSlider.maxValue = maxInARowToWin;
        _inARowToWin = (int)_inARowToWinSlider.value;
        _inARowToWinUI.text = _inARowToWin.ToString();
        if (maxInARowToWin==_minInARowToWin)
            _inARowToWinSlider.direction = Slider.Direction.RightToLeft;
        else
            _inARowToWinSlider.direction = Slider.Direction.LeftToRight;
    }
    public void onPlayersCountSliderChange(float value)
    {
        UpdatePlayersCount();
    }
    public void onFieldSizeChange(float value)
    {
        _fieldSize = (int)value;
        UpdateFieldSize();
    }
    public void onBotsCountChange(float value)
    {
        UpdateBotsCount();
    }
    public void onInARowToWinChange(float value)
    {
        UpdateInARowToWin();
    }
    public void onBotsDifficultyChange(float value)
    {
        _botsDifficulty = (int)value;
        _botDifficultyUI.text = _botsDifficulty.ToString();
    }

}
