using System.Text.RegularExpressions;
using EICraft;
using Newtonsoft.Json;

// Reading the JSON
var data = JsonConvert.DeserializeObject<EiafxData>(File.ReadAllText(@"eiafx-data.json"));

if(data is null) Environment.Exit(1);

// Instantiating a Craft object
var craft = new Craft(data);

var regexInput = new Regex(@"\s");

var ongoing = true;
while (ongoing)
{
    Console.Write("Craft [e.g. 1 T4 puzzle-cube]: ");
    var userInput = Console.ReadLine();
    if (userInput is not null)
    {
        var command = regexInput.Split(userInput.Trim().ToLower());
        // Checking if input is correct
        if (command.Length == 3 && int.TryParse(command[0], out var howMany) && int.TryParse(command[1][1..], out var tierNumber) &&
            data.artifactFamilies.Exists(af => af.id.Equals(command[2])))
        {
            try
            {
                var ingredients = craft.GetCraft(howMany, tierNumber, command[2]);
                Craft.Print(ingredients);
            }
            catch (ArgumentException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        } 
        else if (command[0] == "q")
        {
            ongoing = false;
        }
    }
}