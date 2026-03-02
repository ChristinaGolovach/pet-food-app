import { Component, input, Input } from '@angular/core';
import { AnalysisResult } from '../../models/analysis-result.model';

@Component({
  selector: 'app-analysis-report',
  standalone: true,
  imports: [],
  templateUrl: './analysis-report.component.html',
  styleUrl: './analysis-report.component.scss'
})
export class AnalysisReportComponent {
  analysisResult = input<AnalysisResult | undefined>(undefined);

}
