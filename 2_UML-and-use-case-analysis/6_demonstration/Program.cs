// this is just a simple demonstration to show how they could work with some mock data

var pathfinding = new PathfindingModule();
var obstacleDetector = new ObstacleDetection();
var garbageDetector = new GarbageDetection();

// Obstacle-detection -> Pathfinding
string obstacle = obstacleDetector.DetectObstacle();
pathfinding.RecalculateRoute(obstacle);

// Garbage-detection -> Pathfinding
string garbage = garbageDetector.FindGarbage();
pathfinding.SetDestination(garbage);


public class PathfindingModule
{
    public void RecalculateRoute(string obstacleInfo)
    {
        Console.WriteLine($"Recalculating path due to: {obstacleInfo}");
    }

    public void SetDestination(string garbageLocation)
    {
        Console.WriteLine($"New destination set: {garbageLocation}");
    }
}

public class ObstacleDetection
{
    public string DetectObstacle()
    {
        return "Table detected at (5,3)"; // mock data
    }
}

public class GarbageDetection
{
    public string FindGarbage()
    {
        return "Garbage found at (2,7)"; // mock data
    }
}