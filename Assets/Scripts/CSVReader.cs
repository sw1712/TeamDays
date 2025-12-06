using UnityEngine;

public class CSVReader : MonoBehaviour
{
    void Start()
    {
        TextAsset csvData = Resources.Load<TextAsset>("data");   // 확장자 X
        string[,] parsedData = ParseCSV(csvData.text);

        // 예시 출력
        Debug.Log(parsedData[0, 0]);
        Debug.Log(parsedData[1, 2]);
    }

    string[,] ParseCSV(string csvText)
    {
        string[] lines = csvText.Split('\n');
        int rows = lines.Length;
        int cols = lines[0].Split(',').Length;

        string[,] result = new string[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string[] values = lines[i].Split(',');
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = values[j].Trim();
            }
        }

        return result;
    }
}
