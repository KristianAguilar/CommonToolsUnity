using SaveFiles;
using TMPro;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointsTxt;
    [SerializeField] private TMP_InputField _addPointsIF
        ;

    // can use a custom folder or let null
    private string _customFolderPath = "customFolder";
    private string _pointsFileName = "pointsData.json";

    private int _currentPoints;

    private void Start()
    {
        Load();
    }

    public void Load()
    {
        DataPoints data = new DataPoints();
        if (SaveFilesService.TryToLoadDataFile(_pointsFileName, out data, _customFolderPath))
        {
            _currentPoints = data.points;
            _pointsTxt.text = "Points: " + data.points.ToString();
        }
        else
        {
            _currentPoints = 0;
            _pointsTxt.text = "Points: 0";
        }
    }

    public void Save()
    {
        DataPoints dataPoints = new DataPoints()
        {
            points = _currentPoints
        };
        SaveFilesService.SaveDataFile(_pointsFileName, dataPoints, _customFolderPath);
    }

    public void AddPoints()
    {
        if (string.IsNullOrEmpty(_addPointsIF.text))
            return;

        int addPoints = int.Parse(_addPointsIF.text);
        _currentPoints += addPoints;
        _pointsTxt.text = "Points: " + _currentPoints.ToString();
        _addPointsIF.text = string.Empty;
    }
}

public class DataPoints
{
    public int points;
}
