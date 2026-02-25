import { Component, EventEmitter, inject } from '@angular/core';
import { AnalysisProductComponent } from '../analysis-product/analysis-product.component';
import { AnalysisReportComponent } from '../analysis-report/analysis-report.component';
import { AnalysisService } from '../../services/analysis.service';

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

    analyze(event: { file: File; allergens: string }): void {
      const { file, allergens } = event;
      const allergenList= allergens.split(',');

      this.analysisService.analyze(file, allergenList).subscribe(result => {
        console.log('Analysis result:', result);
      });
  }
}
