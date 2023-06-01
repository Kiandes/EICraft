using EICraft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Reading the JSON
using var file = File.OpenText(@"eiafx-data.json");
using var reader = new JsonTextReader(file);
var serializer = new JsonSerializer();

var eiafx = JToken.ReadFrom(reader);
var eifaxDto = serializer.Deserialize<Root>(new JTokenReader(eiafx));

// Capturing user inputs
Console.Write("Tier [1-4]: T");
var tierNumberInput = Console.ReadLine();

Console.Write("Artifact ID [puzzle-cube, ship-in-a-bottle, ...]: ");
var artifactIDInput = Console.ReadLine();

if (eifaxDto is not null && tierNumberInput is not null && artifactIDInput is not null)
{
    var tierNumber = int.Parse(tierNumberInput);

    // Identifying the artifact family
    var artifactFamily = eifaxDto?.artifact_families.Find(af => af.id == artifactIDInput);

    // Identifying the artifact tier
    var artifactTier = artifactFamily?.tiers.Find(t => t.tier_number == tierNumber);

    if (artifactTier?.recipe is not null)
    {
        // Calling the recursion algorithm
        var ingredients = GetIngredients(eifaxDto.artifact_families, artifactTier.recipe);
        
        // Printing the results
        Console.WriteLine($"{"Name",-50} {"Using",-30} {"Need More",-30}");
        foreach (var ingredient in ingredients)
        {
            Console.WriteLine($"{ingredient.Value.Item1.name,-50} {ingredient.Value.Item2,-30} {ingredient.Value.Item3,-30}");
        }
    }
}

// Checks how many from one artifact do player have in inventory
static int HowManyArtifactsInInventory(string id, int tierNumber)
{
    //TODO CHECK INVENTORY
    return 0;
}

// Starts the recursion
static Dictionary<string, (Ingredient, int, int)> GetIngredients(List<ArtifactFamily> artifactFamilies, Recipe recipe)
{
    var ingredients = new Dictionary<string, (Ingredient, int, int)>();
    foreach (var ingredient in recipe.ingredients)
    {
        var subDictionary = IngredientRecursion(artifactFamilies, ingredient, 1);
        foreach (var sd in subDictionary)
        {
            if (ingredients.ContainsKey(sd.Key))
            {
                var tuple = ingredients[sd.Key];
                ingredients[sd.Key] = (tuple.Item1, tuple.Item2 + sd.Value.Item2, tuple.Item3 + sd.Value.Item3);
            }
            else
            {
                ingredients.Add(sd.Key, (sd.Value.Item1, sd.Value.Item2, sd.Value.Item3));
            }
        }
    }

    return ingredients;
}

// Recursion algorithm
static Dictionary<string, (Ingredient, int, int)> IngredientRecursion(List<ArtifactFamily> artifactFamilies, Ingredient ingredient, int count)
{
    var ingredients = new Dictionary<string, (Ingredient, int, int)>();

    var hmaii = HowManyArtifactsInInventory(ingredient.id, ingredient.tier_number);
    var difference = count * ingredient.count - hmaii;
    
    if (difference <= 0)
    {
        if (ingredients.ContainsKey(ingredient.name))
        {
            var tuple = ingredients[ingredient.name];
            ingredients[ingredient.name] = (tuple.Item1, tuple.Item2 + ingredient.count, tuple.Item3 + 0);
        }
        else
        {
            ingredients.Add(ingredient.name,(ingredient, ingredient.count, 0));
        }

        return ingredients;
    }

    if (ingredient.afx_level == 0)
    {
        if (ingredients.ContainsKey(ingredient.name))
        {
            var tuple = ingredients[ingredient.name];
            ingredients[ingredient.name] = (tuple.Item1, tuple.Item2 + hmaii, tuple.Item3 + difference);
        }
        else
        {
            ingredients.Add(ingredient.name, (ingredient, hmaii, difference));
        }

        return ingredients;
    }

    if (hmaii != 0)
    {
        if (ingredients.ContainsKey(ingredient.name))
        {
            var tuple = ingredients[ingredient.name];
            ingredients[ingredient.name] = (tuple.Item1, tuple.Item2 + hmaii, tuple.Item3 + 0);
        }
        else
        {
            ingredients.Add(ingredient.name, (ingredient, hmaii, 0));
        }
    }


    var recipe = artifactFamilies.Find(af => af.afx_id == ingredient.afx_id)?.tiers
        .Find(t => t.afx_level == ingredient.afx_level)?.recipe;

    if (recipe is null) throw new Exception();

    foreach (var subIngredient in recipe.ingredients)
    {
        var subDictionary = IngredientRecursion(artifactFamilies, subIngredient, difference);
        foreach (var sd in subDictionary)
        {
            if (ingredients.ContainsKey(sd.Key))
            {
                var tuple = ingredients[sd.Key];
                ingredients[sd.Key] = (tuple.Item1, tuple.Item2 + sd.Value.Item2, tuple.Item3 + sd.Value.Item3);
            }
            else
            {
                ingredients.Add(sd.Key, (sd.Value.Item1, sd.Value.Item2, sd.Value.Item3));
            }
        }
    }

    return ingredients;
}