import { Component } from '@angular/core';
import { AnalysisProductComponent } from '../analysis-product/analysis-product.component';
import { AnalysisReportComponent } from '../analysis-report/analysis-report.component';

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

}
