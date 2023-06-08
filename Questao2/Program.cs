using System;
using System.Net;
using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        string team1 = "Paris Saint-Germain";
        int year1 = 2013;
        int totalGoals1 = GetTotalScoredGoals(team1, year1);

        Console.WriteLine("Time: " + team1);
        Console.WriteLine("Ano: " + year1);
        Console.WriteLine("Total de gols: " + totalGoals1);

        string team2 = "Chelsea";
        int year2 = 2014;
        int totalGoals2 = GetTotalScoredGoals(team2, year2);

        Console.WriteLine("\nTime: " + team2);
        Console.WriteLine("Ano: " + year2);
        Console.WriteLine("Total de gols: " + totalGoals2);
    }

    public static int GetTotalScoredGoals(string team, int year)
    {
        string url = $"https://jsonmock.hackerrank.com/api/football_matches?team1={team}&year={year}";
        WebClient client = new WebClient();
        string json = client.DownloadString(url);
        dynamic data = JsonConvert.DeserializeObject(json);
        int totalGoals = 0;

        foreach (var match in data.data)
        {
            int team1Goals = Convert.ToInt32(match.team1goals);
            totalGoals += team1Goals;
        }

        return totalGoals;
    }
}
