# Perfecton Exclusions
## Contents
* [Usage](#usage)
  * [Example](#example)

## Usage<span id="usage" />
In order to exclude recipes from perfection, you need to use Content Patcher to edit the custom data asset `rokugin.perfectionexclusions/recipes`.<br>

`rokugin.perfectionexclusions/recipes` is a string → model lookup, where:
* The key is a unique ID for your excluded recipes.
* The value is a model with the following fields.

| field | purpose |
| :--- | :--- |
| CookingRecipes | *(Optional)* A list of strings matching the keys from `Data/CookingRecipes` that you want to exclude. |
| CraftingRecipes | *(Optional)* A list of strings matching the keys from `Data/CraftingRecipes` that you want to exclude. |
<br>

### Example<span id="example" />
An example of excluding two cooking and two crafting recipes from vanilla recipes:
```jsonc
{
    "Action": "EditData",
    "Target": "rokugin.perfectionexclusions/recipes",
    "Entries": {
        "{{ModId}}_Example": {
            "CookingRecipes": [
                "Cheese Cauli.",
                "Fried Calamari"
            ],
            "CraftingRecipes": [
                "Grass Starter",
                "Dark Sign"
            ]
        }
    }
}
```
In `Data/CookingRecipes` those entries look like:
```jsonc
"Cheese Cauli.": "190 1 424 1/5 5/197/f Pam 3/",
"Fried Calamari": "151 1 246 1 247 1/3 3/202/f Jodi 3/",
```
The important part being the keys, `"Cheese Cauli."` and `"Fried Calamari"`.<br><br>

Similarly, in `Data/CraftingRecipes`:
```jsonc
"Grass Starter": "771 10/Field/297/false/null/",
"Dark Sign": "767 5 881 5/Home/39/true/f Krobus 3/",
```
The important part again being the keys, `"Grass Starter"` and `"Dark Sign"`.<br><br>

Note: the key does not have to include your mod ID and be named whatever you want, but it should be unique to avoid conflicts.

## Console Commands<span id="console" />
Adds one new console command:
| command | purpose |
| :--- | :--- |
| rokugin.exclusions <recipeType> | *Syntax*: `rokugin.exclusions` `<S:recipe type>`<br> Prints a list of recipes still required to reach 100% in that area of perfection, taking into account recipes the player has already made.<br>`<S:recipe type>` takes either `cooking` or `crafting`. |
