export interface AnalysisResult {
  success: boolean;
  rawText?: string;
  ingredients: string[];
  overallRating?: string;
  warnings: string[];
  summary?: string;
}
