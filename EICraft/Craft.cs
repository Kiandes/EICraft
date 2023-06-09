namespace EICraft;
public class Craft
{
    private EiafxData _data;
    
    public Craft(EiafxData data)
    {
        _data = data;
    }

    public  Dictionary<string, (Ingredient, int, int)> GetCraft(int howMany, int tierNumber, string artifactFamilyId)
    {
        var artifactFamily = _data.artifactFamilies.Find(af => af.id == artifactFamilyId);
        var artifactTier = artifactFamily?.tiers.Find(t => t.tier_number == tierNumber);

        if (artifactTier?.recipe is not null)
        {
            var ingredients = new Dictionary<string, (Ingredient, int, int)>();
            artifactTier.recipe.ingredients.ForEach(i => IngredientRecursion(ingredients, i, howMany));
            return ingredients;
        }
        else
        {
            throw new ArgumentException("Tier number is incorrect!", nameof(tierNumber));
        }
    }
    
    private int HowManyArtifactsInInventory(string ingredientId, int ingredientTierNumber)
    {
        //TODO CHECK INVENTORY (also have to save previous request in case we need some artifacts more than one time)
        return 0;
    }
    
    private void IngredientRecursion(IDictionary<string, (Ingredient, int, int)> ingredients, Ingredient ingredient, int count)
    {
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
                ingredients.Add(ingredient.name, (ingredient, ingredient.count, 0));
            }
    
            return;
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
    
            return;
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
    
    
        var recipe = _data.artifactFamilies.Find(af => af.afx_id == ingredient.afx_id)?.tiers
            .Find(t => t.afx_level == ingredient.afx_level)?.recipe;
    
        if (recipe is null) Environment.Exit(1);
    
        recipe.ingredients.ForEach(i => IngredientRecursion(ingredients, i, difference));
    }

    public static void Print(Dictionary<string, (Ingredient, int, int)> ingredients)
    {
        Console.WriteLine($"{"Name",-50} {"Using",-30} {"Need More",-30}");
        foreach (var ingredient in ingredients)
        {
            Console.WriteLine(
                $"{ingredient.Value.Item1.name,-50} {ingredient.Value.Item2,-30} {ingredient.Value.Item3,-30}");
        }
    }
}