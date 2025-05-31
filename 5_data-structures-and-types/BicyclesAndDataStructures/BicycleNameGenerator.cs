namespace BicyclesAndDataStructures;

public class BicycleNameGenerator
{
    private readonly Random _rand = new Random();
    private readonly string[] _ownerNames =
    [
        "John's",
        "Jane's",
        "Joe's",
        "Jennie's",
        "Steve's",
        "Lily's",
        "Michael's",
        "Sarah's",
        "David's",
        "Emma's",
        "Peter's",
        "Sophie's",
        "Thomas's",
        "Maria's",
        "Robert's",
        "Anna's",
        "Nobody's"
    ];

    private readonly string[] _adjectives =
    [
        "Amazing",
        "Incredible",
        "Depressing",
        "Shiny",
        "Decaying",
        "Deteriorating",
        "Eye-catching",
        "Polished",
        "Majestic",
        "Elegant",
        "Ugly",
        "Eye-straining",
        "Tiny",
        "Spectacular",
        "Existing",
    ];

    private readonly string[] _vehicleDescriptor =
    [
        "Bike",
        "Bicycle",
        "\"Bicycle\"",
        "Two-wheeled Vehicle",
        "Unicycle",
    ];

    private HashSet<string> _generatedNames = [];

    public string GenerateRandomName()
    {
        string generatedName;
        do
        {
            var owner = _ownerNames[_rand.Next(_ownerNames.Length)];
            var adjective = _adjectives[_rand.Next(_adjectives.Length)];
            var vehicleDescriptor = _vehicleDescriptor[_rand.Next(_vehicleDescriptor.Length)];
            generatedName = $"{owner} {adjective} {vehicleDescriptor}";
        } while (_generatedNames.Contains(generatedName));

        return generatedName;
    }
}