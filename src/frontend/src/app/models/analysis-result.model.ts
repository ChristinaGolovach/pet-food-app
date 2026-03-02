import { Ingredient } from "./ingredient.model";

export interface AnalysisResult {
  ingredients: Ingredient[];
  ingredientCount: number;
  allergens: string[];
  allergenCount: number;
  allergenPercentage: number;
}
