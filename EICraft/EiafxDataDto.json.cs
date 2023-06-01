using Newtonsoft.Json;

namespace EICraft;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ArtifactFamily
    {
        public string id { get; set; }
        public int afx_id { get; set; }
        public string name { get; set; }
        public int afx_type { get; set; }
        public string type { get; set; }
        public int sort_key { get; set; }
        public List<int> child_afx_ids { get; set; }
        public string effect { get; set; }
        public string effect_target { get; set; }
        public List<Tier> tiers { get; set; }
    }

    public class CraftingPrice
    {
        public double @base { get; set; }
        public double low { get; set; }
        public int domain { get; set; }
        public double curve { get; set; }
        public int initial { get; set; }
        public int minimum { get; set; }
    }

    public class Effect
    {
        public int afx_rarity { get; set; }
        public string rarity { get; set; }
        public string effect { get; set; }
        public string effect_target { get; set; }
        public string effect_size { get; set; }
        public double effect_delta { get; set; }
        public string family_effect { get; set; }
        public int? slots { get; set; }
    }

    public class Family
    {
        public string id { get; set; }
        public int afx_id { get; set; }
        public string name { get; set; }
        public int afx_type { get; set; }
        public string type { get; set; }
        public int sort_key { get; set; }
        public List<int> child_afx_ids { get; set; }
    }

    public class HardDependency
    {
        public string id { get; set; }
        public int afx_id { get; set; }
        public int afx_level { get; set; }
        public string name { get; set; }
        public int tier_number { get; set; }
        public string tier_name { get; set; }
        public int afx_type { get; set; }
        public string type { get; set; }
        public string icon_filename { get; set; }
        public int count { get; set; }
    }

    public class Ingredient
    {
        public string id { get; set; }
        public int afx_id { get; set; }
        public int afx_level { get; set; }
        public string name { get; set; }
        public int tier_number { get; set; }
        public string tier_name { get; set; }
        public int afx_type { get; set; }
        public string type { get; set; }
        public string icon_filename { get; set; }
        public int count { get; set; }
    }

    public class Recipe
    {
        public List<Ingredient> ingredients { get; set; }
        public CraftingPrice crafting_price { get; set; }
    }

    public class Root
    {
        [JsonProperty("$schema")]
        public string schema { get; set; }
        public List<ArtifactFamily> artifact_families { get; set; }
    }

    public class Tier
    {
        public Family family { get; set; }
        public string id { get; set; }
        public int afx_id { get; set; }
        public int afx_level { get; set; }
        public string name { get; set; }
        public int tier_number { get; set; }
        public string tier_name { get; set; }
        public int afx_type { get; set; }
        public string type { get; set; }
        public string icon_filename { get; set; }
        public double quality { get; set; }
        public bool craftable { get; set; }
        public List<double> base_crafting_prices { get; set; }
        public bool has_rarities { get; set; }
        public List<int> possible_afx_rarities { get; set; }
        public bool has_effects { get; set; }
        public bool available_from_missions { get; set; }
        public List<Effect> effects { get; set; }
        public Recipe? recipe { get; set; }
        public bool ingredients_available_from_missions { get; set; }
        public List<HardDependency> hard_dependencies { get; set; }
    }
    