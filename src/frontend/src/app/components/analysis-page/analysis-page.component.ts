import { Component, EventEmitter, inject, signal } from '@angular/core';
import { AnalysisProductComponent } from '../analysis-product/analysis-product.component';
import { AnalysisReportComponent } from '../analysis-report/analysis-report.component';
import { AnalysisService } from '../../services/analysis.service';
import { AnalysisResult } from '../../models/analysis-result.model';

@Component({
  selector: 'app-analysis-page',
standalone: true,
  imports: [
    AnalysisProductComponent,
    AnalysisReportComponent
],
  templateUrl: './analysis-page.component.html',
  styleUrls: ['./analysis-page.component.scss']
})
export class AnalysisPageComponent {

  private analysisService = inject(AnalysisService);
  analysisResult = signal<AnalysisResult | undefined>(undefined);

  analyze(event: { file: File; allergens: string }): void {
    const { file, allergens } = event;
    const allergenList= allergens.split(',');

    this.analysisService.analyze(file, allergenList).subscribe(result => {
      this.analysisResult.set(result);
    });
  }

  onPhotoRemoved(): void {
    this.analysisResult.set(undefined);
  }
}
